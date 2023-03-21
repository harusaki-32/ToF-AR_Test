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
using MessagePack;
using MessagePack.Resolvers;
using TofAr.MessagePackResolver;
using UnityEngine;

namespace TofAr.V0.Color
{
    /// <summary>
    /// TODO+ C 不要クラス？
    /// </summary>
    [CreateAssetMenu(fileName = "colorMessagepackResolver", menuName = "TofAr/colorMessagepackResolver")]
    public class MessagePackResolverProvider : CustomResolverRegister
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        public override IFormatterResolver Resolver
        {
            get
            {
                return TofArColorResolver.Instance;
            }
        }
    }
}
