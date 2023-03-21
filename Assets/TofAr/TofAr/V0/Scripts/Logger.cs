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

using System.IO;
using UnityEngine;

namespace TofAr.V0
{
    /// <summary>
    /// ログレベル
    /// </summary>
    public enum LogLevel : byte
    {
        /// <summary>
        /// Silent
        /// </summary>
        Silent = 8,  //ANDROID_LOG_SILENT

        /// <summary>
        /// Verbose
        /// </summary>
        Verbose = 2, //ANDROID_LOG_VERBOSE

        /// <summary>
        /// Debug
        /// </summary>
        Debug = 3,   //ANDROID_LOG_DEBUG,

        /// <summary>
        /// Info
        /// </summary>
        Info = 4,    //ANDROID_LOG_INFO,

        /// <summary>
        /// Warn
        /// </summary>
        Warn = 5,    //ANDROID_LOG_WARN,

        /// <summary>
        /// Error
        /// </summary>
        Error = 6,   //ANDROID_LOG_ERROR
    };

    /// <summary>
    /// ログ出力機能
    /// </summary>
    public class Logger
    {
#if UNITY_IOS
        /// <summary>
        /// *TODO+ B internalにしたい
        /// ログ設定ファイルのパス
        /// </summary>
        internal static string SettingsFilePath = "tofar_log_settings";
#else
        /// <summary>
        /// *TODO+ B internalにしたい
        /// ログ設定ファイルのパス
        /// </summary>
        internal static string SettingsFilePath = "/data/local/tmp/tofar/config/tofar_log_settings";
#endif

        private LogLevel logLevel = LogLevel.Silent;
        /// <summary>
        /// ログレベル
        /// </summary>
        public LogLevel LogLevel
        {
            get => this.logLevel;
            set
            {
                if (this.logLevel != value)
                {
                    this.logLevel = value;
                    this.OnLogLevelChanged?.Invoke(this.logLevel);
                }
            }
        }

        /// <summary>
        /// ログレベル変更時デリゲート
        /// </summary>
        /// <param name="newLogLevel">新しく指定されたログレベル</param>
        public delegate void OnLogLevelChangedEvent(LogLevel newLogLevel);
        /// <summary>
        /// ログレベル変更時イベント
        /// </summary>
        public event OnLogLevelChangedEvent OnLogLevelChanged;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Logger()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            if ((Application.platform == RuntimePlatform.WindowsEditor) || (Application.platform == RuntimePlatform.WindowsPlayer)
                || (Application.platform == RuntimePlatform.OSXEditor) || (Application.platform == RuntimePlatform.OSXPlayer))
            {
                this.logLevel = LogLevel.Verbose;
                return;
            }
            this.logLevel = LogLevel.Silent;

            var filePath = SettingsFilePath;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                filePath = $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}{SettingsFilePath}";
            }

            if (File.Exists(filePath))
            {
                if (Utils.PathIsSymlink(filePath))
                {
                    TofArManager.Logger.WriteLog(LogLevel.Debug, "settings file " + SettingsFilePath + "was a symlink");
                }
                else
                {
                    var lines = File.ReadAllLines(filePath);
                    if (lines.Length > 0)
                    {
                        switch (lines[0].ToLower().Trim())
                        {
                            case "loglevel=silent":
                                this.logLevel = LogLevel.Silent;
                                break;
                            case "loglevel=verbose":
                                this.logLevel = LogLevel.Verbose;
                                break;
                            case "loglevel=debug":
                                this.logLevel = LogLevel.Debug;
                                break;
                            case "loglevel=info":
                                this.logLevel = LogLevel.Info;
                                break;
                            case "loglevel=warn":
                                this.logLevel = LogLevel.Warn;
                                break;
                            case "loglevel=error":
                                this.logLevel = LogLevel.Error;
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ログを出力する
        /// </summary>
        /// <param name="logLevel">ログレベル</param>
        /// <param name="message">メッセージ</param>
        /// <returns>trueの場合、処理成功</returns>
        public bool WriteLog(LogLevel logLevel, string message)
        {
            if ((LogLevel.Verbose <= logLevel) && (this.LogLevel <= logLevel) && (logLevel <= LogLevel.Error))
            {
                switch (logLevel)
                {
                    case LogLevel.Silent:
                        //still don't say anything - this level shouldn't be used as a log, use ForceWriteLog if you always want to log it
                        break;
                    case LogLevel.Verbose:
                    case LogLevel.Debug:
                    case LogLevel.Info:
                        Debug.Log(message);
                        break;
                    case LogLevel.Warn:
                        Debug.LogWarning(message);
                        break;
                    case LogLevel.Error:
                        Debug.LogError(message);
                        break;
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// 強制的にログを出力する
        /// </summary>
        /// <param name="logLevel">ログレベル</param>
        /// <param name="message">メッセージ</param>
        /// <returns>trueの場合、処理成功</returns>
        public bool ForceWriteLog(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Silent:
                case LogLevel.Verbose:
                case LogLevel.Debug:
                case LogLevel.Info:
                    Debug.Log(message);
                    break;
                case LogLevel.Warn:
                    Debug.LogWarning(message);
                    break;
                case LogLevel.Error:
                    Debug.LogError(message);
                    break;
            }
            return true;
        }
    }
}
