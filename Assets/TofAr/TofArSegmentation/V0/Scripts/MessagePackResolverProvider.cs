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
using TofAr.MessagePackResolver;
using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

namespace TofAr.V0.Segmentation
{
    /// <summary>
    /// TODO+ C 不要クラス？
    /// </summary>
    [CreateAssetMenu(fileName = "segmentationMessagepackResolver", menuName = "TofAr/segmentationMessagepackResolver")]
    public class MessagePackResolverProvider : CustomResolverRegister
    {
        /// <summary>
        /// TODO+ C
        /// </summary>
        public override IFormatterResolver Resolver
        {
            get
            {
                return TofArSegmentationResolver.Instance;
            }
        }
    }
}
