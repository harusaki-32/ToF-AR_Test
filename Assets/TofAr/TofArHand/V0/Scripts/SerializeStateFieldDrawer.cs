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

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

/// <summary>
/// ステータス用 カスタムプロパティ （描画）
/// </summary>
[CustomPropertyDrawer(typeof(SerializeStateFieldAttribute))]
public class SerializeStateFieldDrawer : PropertyDrawer
{
    private bool showField = true;

    /// <summary>
    /// GUIへの描画処理
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="property">プロパティ</param>
    /// <param name="label">ラベル</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializeStateFieldAttribute attribute = (SerializeStateFieldAttribute)this.attribute;
        if (attribute == null)
        {
            return;
        }
        SerializedProperty conditionField = property.serializedObject.FindProperty(attribute.fieldName);

        if (conditionField == null)
        {
            return;
        }

        if (conditionField.propertyType == SerializedPropertyType.Boolean)
        {
            try
            {
                bool comparationValue = attribute.value == null || (bool)attribute.value;
                showField = conditionField.boolValue == comparationValue;
            }
            catch (System.InvalidCastException)
            {
                return;
            }
        }

        if (showField)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    /// <summary>
    /// プロパティのGUI上の高さを取得
    /// </summary>
    /// <param name="property">プロパティ</param>
    /// <param name="label">ラベル</param>
    /// <returns>高さ</returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (showField)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }  
    }
}

#endif
