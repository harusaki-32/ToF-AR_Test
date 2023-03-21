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
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// TODO+ C クラス名が変 
/// </summary>
public class colorfulDropdown : Dropdown
{
    /// <summary>
    /// ハイライト表示を行う項目
    /// </summary>
    [Serializable]
    public class IndexedColor
    {
        public int index;
        public Color color;

    }
    /// <summary>
    /// TODO+ C フィールド名微妙
    /// </summary>
    public IndexedColor[] m_cinds;

    private int dropdownInd = 0;

    protected override DropdownItem CreateItem(DropdownItem itemTemplate)
    {
        //add the color to the item if it's index is in the colorlist
        var newItem = (DropdownItem)Instantiate(itemTemplate);
        foreach (var c in m_cinds)
        {
            if (c.index == dropdownInd)
            {
                if (newItem.text != null)
                {
                    newItem.text.color = c.color;
                }//if(newItem.image != null) newItem.image.color = c.color;   
            }
        }
        dropdownInd++;
        return newItem;
    }

    /// <summary>
    /// PointerClick時処理
    /// </summary>
    /// <param name="eventData">PointerEventData</param>
    public override void OnPointerClick(PointerEventData eventData)
    {
        dropdownInd = 0;
        Show();
    }

    /// <summary>
    /// Submit時処理
    /// </summary>
    /// <param name="eventData">BaseEventData</param>
    public override void OnSubmit(BaseEventData eventData)
    {
        dropdownInd = 0;
        Show();
    }

}

#if UNITY_EDITOR
//to get variables to show in the editor
/// <summary>
/// TODO+ C
/// </summary>
[CustomEditor(typeof(colorfulDropdown))]
public class colorfulDropdownEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif
