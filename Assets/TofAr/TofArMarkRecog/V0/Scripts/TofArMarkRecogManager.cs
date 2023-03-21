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
using TensorFlowLite.Runtime;
using UnityEngine.Events;

namespace TofAr.V0.MarkRecog
{
    /// <summary>
    /// マーク認識モデル初期化時イベント
    /// </summary>
    public class OnInitNnpEvent : UnityEvent<string> { }

    /// <summary>
    /// 下記機能を有する
    /// <list type="bullet">
    /// <item><description>GetPropertyによるマーク認識結果の取得</description></item>
    /// </list>
    /// </summary>
    public class TofArMarkRecogManager : Singleton<TofArMarkRecogManager>, IDisposable
    {
        /// <summary>
        /// コンポーネントのバージョン番号
        /// </summary>
        public string Version
        {
            get
            {
                return ComponentVersion.version;
            }
        }

        private TFLiteEstimator runtime = null;

        /// <summary>
        /// マーク認識モデル初期化時デリゲート
        /// </summary>
        /// <param name="msg"></param>
        public delegate void OnInitNnpEventEventHandler(string msg);

        /// <summary>
        /// マーク認識モデル初期化通知
        /// </summary>
        public static event OnInitNnpEventEventHandler OnInitNnpEvent;

        /// <summary>
        /// TFLiteのExecMode
        /// <para>デフォルト値: EXEC_MODE_CPU</para>
        /// </summary>
        public TFLiteRuntime.ExecMode execMode = TFLiteRuntime.ExecMode.EXEC_MODE_CPU;

        /// <summary>
        /// TFLiteが使用するスレッド数
        /// </summary>
        public int threadsNum = 1;

        /// <summary>
        /// アプリケーション一時停止開始時デリゲート
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        public delegate void ApplicationPausingEventHandler(object sender);
        /// <summary>
        /// アプリケーション一時停止開始時
        /// </summary>
        public static event ApplicationPausingEventHandler OnApplicationPausing;

        /// <summary>
        /// アプリケーション復帰開始時デリゲート
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        public delegate void ApplicationResumingEventHandler(object sender);
        /// <summary>
        /// アプリケーション復帰開始時
        /// </summary>
        public static event ApplicationResumingEventHandler OnApplicationResuming;

        private void Start()
        {
            this.runtime = new TFLiteEstimator();
            int error = this.runtime.Init(this.execMode, this.threadsNum);

            if (error != 0)
            {
                this.runtime = null;

                if (OnInitNnpEvent != null)
                {
                    OnInitNnpEvent.Invoke(error.ToString());
                }
            }
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            TofArManager.Logger.WriteLog(LogLevel.Debug, "TofArMarkRecogManager.Dispose()");

            this.runtime.Free();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                OnApplicationPausing?.Invoke(this);
            }
            else
            {
                OnApplicationResuming?.Invoke(this);
            }
        }

        /// <summary>
        /// コンポーネントプロパティを取得する。入力パラメータvalueを指定可能。
        /// </summary>
        /// <typeparam name="T">IBaseProperty継承クラス</typeparam>
        /// <param name="value">入力パラメータ</param>
        /// <returns>プロパティクラス</returns>
        public T GetProperty<T>(T value) where T : ResultProperty
        {
            if (this.runtime != null && value != null && value.image != null)
            {
                // 認識処理を実施
                value.levels = this.runtime.Exec(value.image);
            }
            return value;
        }
    }
}


