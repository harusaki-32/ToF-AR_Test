/*
 * Copyright 2018,2019,2020,2021,2022 Sony Semiconductor Solutions Corporation.
 *
 * This is UNPUBLISHED PROPRIETARY SOURCE CODE of Sony Semiconductor
 * Solutions Corporation.
 * No part of this file may be copied, modified, sold, and distributed in any
 * form or by any means without prior explicit permission in writing from
 * Sony Semiconductor Solutions Corporation.
 *
 */
using TofAr.V0;
using TofAr.V0.Tof;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//class to allow the configurations dropdown to be a prefab

/// <summary>
/// Tof Configドロップダウンリストクラス
/// </summary>
public class TofDepthConfigTool : MonoBehaviour
{
    /// <summary>
    /// Config選択時デリゲート
    /// </summary>
    [System.Serializable]
    public class OnDepthSelectedEvent : UnityEvent<CameraConfigurationProperty, bool> { }
    /// <summary>
    /// Config選択通知
    /// </summary>
    [SerializeField]
    public OnDepthSelectedEvent OnDepthSelected;

    /// <summary>
    /// Configドロップダウンリスト
    /// </summary>
    public Dropdown depthConfiguratinsDropDown;
    /// <summary>
    /// <para>true: テクスチャー処理を行う</para>
    /// <para>false: 処理を行わない</para>
    /// <para>デフォルト値: false</para>
    /// </summary>
    public bool isProcessTexture = false;
    private int currentIndex = -1;

    protected List<CameraConfigurationProperty> Configurations { get; set; }


    void Start()
    {
        UpdateList(TofArTofManager.Instance.GetProperty<CameraConfigurationsProperty>());
        TofArTofManager.OnAvailableConfigurationsChanged += UpdateList;
        TofArTofManager.OnStreamStarted += OnStreamStarted;
        TofArTofManager.OnStreamStopped += OnStreamStopped;
    }

    void OnDestroy()
    {
        TofArTofManager.OnAvailableConfigurationsChanged -= UpdateList;
        TofArTofManager.OnStreamStarted -= OnStreamStarted;
        TofArTofManager.OnStreamStopped -= OnStreamStopped;
    }

    protected virtual void LoadConfigurations(CameraConfigurationsProperty configs)
    {
        this.Configurations = configs.configurations.ToList<CameraConfigurationProperty>();
    }


    protected void UpdateList(CameraConfigurationsProperty configs)
    {
        LoadConfigurations(configs);
        var options = this.Configurations.Select(x => new Dropdown.OptionData(string.Format("{4}\u00A0{0}\u00A0{1}\u00A0{2}x{3}", (LensFacing)x.lensFacing, x.name, x.width, x.height, x.cameraId))).ToList();

        var defaultConfiguration = TofArTofManager.Instance.GetProperty<Camera2DefaultConfigurationProperty>();
        var index = Configurations.FindIndex((cfg) => cfg.uid == defaultConfiguration.uid);
        if (index != -1)
        {

            var def = options[index];
            //move the default to the top
            options.RemoveAt(index);
            options.Insert(0, def);

            var def_config = Configurations[index];
            Configurations.RemoveAt(index);
            Configurations.Insert(0, def_config);
        }

        //add a blank option as well
        Configurations.Insert(0, new CameraConfigurationProperty { uid = int.MinValue });
        options.Insert(0, new Dropdown.OptionData(""));

        this.depthConfiguratinsDropDown.options = options;
        StartCoroutine(UpdateIndex());
    }

    private IEnumerator UpdateIndex()
    {
        yield return new WaitForEndOfFrame();
        if (TofArTofManager.Instance.IsStreamActive)
        {
            var currentProp = TofArTofManager.Instance.GetProperty<CameraConfigurationProperty>();
            for (int i = 0; i < Configurations.Count; i++)
            {
                if (Configurations[i].uid == currentProp.uid)
                {
                    //make sure we dont call the callback here
                    currentIndex = i;
                    depthConfiguratinsDropDown.value = i;
                    break;
                }
            }
        }
        else
        {
            depthConfiguratinsDropDown.value = 0;
        }
    }

    private void OnStreamStarted(object sender, Texture2D depthTexture, Texture2D confidenceTexture, PointCloudData pointCloudData)
    {
        if (TofArTofManager.Instance.IsPlaying)
        {
            return;
        }//get the current config and check we are that value
        var conf = TofArTofManager.Instance.GetProperty<CameraConfigurationProperty>();
        if (conf == null || Configurations == null)
        {
            return;
        }
        for (int i = 1; i < Configurations.Count; i++)
        {
            if (Configurations[i].uid == conf.uid)
            {
                //set the dropdown to the current value
                if (depthConfiguratinsDropDown.value != i)
                {
                    currentIndex = i;
                    depthConfiguratinsDropDown.value = i;
                }
                break;
            }
        }
    }

    private void OnStreamStopped(object o)
    {
        if (TofArTofManager.Instance.IsPlaying)
        {
            return;
        }
        if (depthConfiguratinsDropDown.value != 0)
        {
            currentIndex = 0;
            depthConfiguratinsDropDown.value = 0;
        }
    }

    /// <summary>
    /// ドロップダウン選択時
    /// </summary>
    /// <param name="result">Tof configインデックス</param>
    public void DropdownSelectChanged(int result)
    {
        if (result == currentIndex)
        {
            return;
        }
        if (result >= this.Configurations.Count) { return; }
        currentIndex = result;

        bool hasPersistentTarget = false;
        for (int i = 0; i < OnDepthSelected.GetPersistentEventCount(); i++)
        {

            if (OnDepthSelected.GetPersistentTarget(i) != null)
            {
                hasPersistentTarget = true;
            }
        }

        TofArManager.Logger.WriteLog(LogLevel.Debug, "ToF mode " + this.Configurations[result].name + " has been selected.");
        if (hasPersistentTarget)
        {
            if (result == 0)
            {
                OnDepthSelected.Invoke(null, this.isProcessTexture);
            }
            else
            {
                OnDepthSelected.Invoke(this.Configurations[result], this.isProcessTexture);
            }
        }
        else
        {
            TofArTofManager.Instance.StopStream();
            //in case we have dynamic listeners
            if (result == 0)
            {
                OnDepthSelected.Invoke(null, this.isProcessTexture);
            }
            else
            {
                TofArTofManager.Instance.StartStream(this.Configurations[result], this.isProcessTexture);
                OnDepthSelected.Invoke(this.Configurations[result], this.isProcessTexture);
            }
        }
    }
}
