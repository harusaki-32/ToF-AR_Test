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
using UnityEngine;

/// <summary>
/// 手モデルの透明度を管理するクラス
/// </summary>
public class handAlphaController2 : MonoBehaviour
{
    /// <summary>
    /// Material で Alpha Controll する Color の Name
    /// </summary>
    public string PropertyName = "_Color";

    private SkinnedMeshRenderer[] renderers;
    private Transform handRoot;
    private TofAr.V0.Hand.AbstractHandModel hbr;

    [SerializeField]
    private float alphaStartDistance = 0.3f;
    [SerializeField]
    private float alphaEndDistance = 0.1f;

    [SerializeField]
    private float dist;

    private Color[][] colorOne;
    private Color[][] colorZero;
    private int[] matCount;

    private int propID;
    private bool[] hasColorProperty;
    private UnityEngine.Rendering.ShadowCastingMode[] shadowModes;

    [SerializeField]
    private float currentAlpha;
    [SerializeField]
    private float currentMultiply;

    private int handCounter = 0;

    /// <summary>
    /// フェードアウトまでの時間
    /// </summary>
    public int fadeTerm = 15;
    private bool fade = true;

    // Start is called before the first frame update
    void Start()
    {
        hbr = GetComponent<TofAr.V0.Hand.AbstractHandModel>();
        handRoot = transform.GetChild(0);

        // common over renderers...
        propID = Shader.PropertyToID(PropertyName);

        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        matCount = new int[renderers.Length];
        colorOne = new Color[renderers.Length][];
        colorZero = new Color[renderers.Length][];
        hasColorProperty = new bool[renderers.Length];
        shadowModes = new UnityEngine.Rendering.ShadowCastingMode[renderers.Length];

        for (int j = 0; j < renderers.Length; j++)
        {
            matCount[j] = renderers[j].materials.Length;
            colorOne[j] = new Color[matCount[j]];
            colorZero[j] = new Color[matCount[j]];

            currentMultiply = 1;

            for (int i = 0; i < matCount[j]; i++)
            {
                SetupMaterialWithBlendMode(renderers[j].materials[i], true);
            }

            hasColorProperty[j] = true;
            shadowModes[j] = renderers[j].shadowCastingMode;

            for (int i = 0; i < matCount[j]; i++)
            {
                if (!renderers[j].materials[0].HasProperty(propID))
                {
                    hasColorProperty[j] = false;
                    break;
                }
                colorOne[j][i] = renderers[j].materials[i].GetColor(propID);
                colorZero[j][i] = new Color(colorOne[j][i].r, colorOne[j][i].g, colorOne[j][i].b, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hbr.IsHandDetected)
        {
            if (handCounter < fadeTerm)
            {
                handCounter++;
            }
        }
        else
        {
            if (handCounter > 0)
            {
                handCounter--;
            }
        }
        currentMultiply = (float)handCounter / fadeTerm;

        dist = Vector3.Distance(handRoot.position, transform.position);

        if (dist > alphaStartDistance)
        {
            currentAlpha = 1;
        }
        else if (dist < alphaEndDistance)
        {
            currentAlpha = 0;
        }
        else
        {
            currentAlpha = ratioInMinMax(dist, alphaEndDistance, alphaStartDistance);
        }
        setAlpha(currentAlpha * currentMultiply);
    }

    private static float ratioInMinMax(float value, float min, float max)
    {
        if (max > min)
        {
            return (value - min) / (max - min);
        }
        return (value - max) / (min - max);
    }

    private void setAlpha(float value)
    {
        bool isFade = (value < 1);

        bool fadeStatusChange = (fade != isFade);
        fade = isFade;

        for (int j = 0; j < renderers.Length; j++)
        {
            if (hasColorProperty[j])
            {
                for (int i = 0; i < matCount[j]; i++)
                {
                    if (fadeStatusChange)
                    {
                        SetupMaterialWithBlendMode(renderers[j].materials[i], isFade);
                    }
                    renderers[j].materials[i].SetColor(propID, Color.Lerp(colorZero[j][i], colorOne[j][i], value));
                }

            }
            else
            {
                renderers[j].gameObject.SetActive((value > 0.5f));
            }
            renderers[j].shadowCastingMode = value > 0.0f ? shadowModes[j] : UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }

    /// <summary>
    /// Material内の値を変更する
    /// </summary>
    public void SetupMaterialWithBlendMode(Material m, bool isFade)
    {
        if (isFade)
        {
            m.SetFloat("_Mode", 2);
            m.SetOverrideTag("RenderType", "Transparent");
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = 3000;
        }
        else
        {
            m.SetFloat("_Mode", 0);
            m.SetOverrideTag("RenderType", "");
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            m.SetInt("_ZWrite", 1);
            m.DisableKeyword("_ALPHATEST_ON");
            m.DisableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = -1;
        }
    }
}
