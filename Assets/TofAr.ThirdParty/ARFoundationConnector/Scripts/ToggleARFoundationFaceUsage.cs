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
using UnityEngine.UI;

namespace TofAr.ThirdParty.ARFoundationConnector
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleARFoundationFaceUsage : MonoBehaviour
    {
        [SerializeField]
        private ARFoundationConnectorManager connectorManager;

        void Start()
        {
            var toggle = this.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnARFoundationToggled);
            this.OnARFoundationToggled(toggle.isOn);
            //this is only used on the debug server, so make life easier
            connectorManager.AutoStart = true;
        }

        void OnARFoundationToggled(bool value)
        {
            connectorManager.UseARFoundationFace = value;
        }
    }
}
