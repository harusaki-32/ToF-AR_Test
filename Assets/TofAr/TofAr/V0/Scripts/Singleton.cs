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
using UnityEditor;
#endif
using UnityEngine;

namespace TofAr.V0
{
    /// <summary>
    /// シングルトンクラス
    /// </summary>
    /// <typeparam name="T">クラス</typeparam>
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        public static T Instance
        {
            get
            {
                //find the object if we want to call a reference to it before it's awake has been called
                if (instance == null)
                {
                    Instance = (T)GameObject.FindObjectOfType<T>();
                }
                return instance;
            }

            private set
            {
                instance = value;
                if (instance != null)
                {
#if UNITY_EDITOR
                    if (EditorApplication.isPlaying)
                    {
                        DontDestroyOnLoad(value.gameObject);
                    }
#else
                    DontDestroyOnLoad(value.gameObject);
#endif
                }
            }
        }

        /// <summary>
        /// スクリプトのインスタンスがロードされたときに呼び出されます
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                Instance = (T)this;
#if UNITY_EDITOR
                if (EditorApplication.isPlaying)
                {
                    DontDestroyOnLoad(gameObject);
                }
#else
                DontDestroyOnLoad(gameObject);
#endif
            }

            if (instance != this)
            {
                DestroyImmediate(this);
            }
        }

        /// <summary>
        /// trueの場合、インスタンス化されている
        /// </summary>
        public static bool Instantiated
        {
            get
            {
                return (instance != null);
            }
        }

        //when you want to destroy it manually 
        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
            //call the dispose function if it exists
            var disposable = this as System.IDisposable;
            if (disposable != null)
            {
                lock (stopLock)
                {
                    disposable.Dispose();
                }
            }
        }

        protected object stopLock = new object();
    }
}
