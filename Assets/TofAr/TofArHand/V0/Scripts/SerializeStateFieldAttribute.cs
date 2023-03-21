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

/// <summary>
/// ステータス用 カスタムプロパティ
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class SerializeStateFieldAttribute : PropertyAttribute
{
    public readonly string fieldName;
    public readonly object value;

    /// <summary>
    /// 特定の条件によってFieldを表示/非表示にする
    /// </summary>
    /// <param name="conditionFieldName">名前</param>
    /// <param name="comparationValue">値</param>
    public SerializeStateFieldAttribute(string fieldName, object value = null)
    {
        this.fieldName = fieldName;
        this.value = value;
    }
}
