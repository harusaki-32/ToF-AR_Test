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
using System.Collections.Generic;
using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

namespace TofAr.MessagePackResolver
{
    public class ResolverRegister
    {
        static bool serializerRegistered = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            if (!serializerRegistered)
            {
                var resolvers = new List<IFormatterResolver>();

                // find pluged resolvers
                Resources.LoadAll<CustomResolverRegister>("Resolvers");
                var pluggedConnectors = Resources.FindObjectsOfTypeAll<CustomResolverRegister>();
                foreach (var pc in pluggedConnectors)
                {
                    resolvers.Add(pc.Resolver);
                }

                // finally, use builtin/primitive resolver(don't use StandardResolver, it includes dynamic generation)
                resolvers.Add(BuiltinResolver.Instance);
                resolvers.Add(AttributeFormatterResolver.Instance);
                resolvers.Add(PrimitiveObjectResolver.Instance);

                CompositeResolver.RegisterAndSetAsDefault(resolvers.ToArray());
                serializerRegistered = true;
            }
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void EditorInitialize()
        {
            Initialize();
        }
#endif
    }
}
