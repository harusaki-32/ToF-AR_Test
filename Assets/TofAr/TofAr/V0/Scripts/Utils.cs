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
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace TofAr.V0
{
    /// <summary>
    /// ユーティリティ関数
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Exceptionをフォーマットしたメッセージを取得する
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>メッセージ</returns>
        public static string FormatException(Exception exception)
        {
            return $"[HANDLED] {exception.GetType().Name} : {exception.Message}\n{exception.StackTrace}";
        }

        /// <summary>
        /// -180°～ +180°のオイラー角を 0°～360°へ変換する
        /// </summary>
        /// <param name="input">-180°～ +180°のオイラー角</param>
        /// <returns>0°～ 360°へ変換されたオイラー角</returns>
        public static Vector3 MapEuler(Vector3 input)
        {
            return new Vector3
            {
                x = input.x > 180 ? input.x - 360 : input.x,
                y = input.y > 180 ? input.y - 360 : input.y,
                z = input.z > 180 ? input.z - 360 : input.z,
            };
        }

        /// <summary>
        /// ディレクトリがシンボリックリンクを含んでいないことを確認する
        /// </summary>
        /// <param name="folder">ディレクトリパス文字列</param>
        /// <returns>trueの場合、ディレクトリはシンボリックリンクを含んでいない</returns>
        public static bool DirectoryContainsNoSymlinks(string folder)
        {
            if (!Directory.Exists(folder))
            {
                return false;
            }
            var files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                if (File.GetAttributes(file).HasFlag(FileAttributes.ReparsePoint))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// パスがシンボリックリンクであるか確認する
        /// </summary>
        /// <param name="path">確認するパス文字列</param>
        /// <returns>trueの場合、パスはシンボリックリンクである</returns>
        public static bool PathIsSymlink(string path)
        {
            if (path == null)
            {
                return false;
            }
            try
            {
                return File.GetAttributes(path).HasFlag(FileAttributes.ReparsePoint);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        internal static T FindFirstGameObjectThatImplements<T>(bool includeInactive = false) where T : class
        {
            //this is very slow, but we cant find objects marked with dont destroy on load in the hierarchy
            var gameObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (var go in gameObjects)
            {
                var implementer = go?.GetComponent<T>() ?? go?.GetComponentInChildren<T>();
                if (implementer != null)
                {
                    var implComp = implementer as MonoBehaviour;
                    if (implComp != null && (includeInactive || implComp.isActiveAndEnabled))
                    {
                        return implementer;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// シーン内で型Tのコンポーネントを実装するGameObjectを探索し、最初に発見したコンポーネントを取得する
        /// </summary>
        /// <typeparam name="T">探索するコンポーネントのクラス</typeparam>
        /// <returns>発見したコンポーネント、発見できなかった場合はnull</returns>
        public static T FindFirstActiveGameObjectThatImplements<T>() where T : class
        {
            return FindFirstGameObjectThatImplements<T>();
        }

        private static readonly DateTime unixStartDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 1970/1/1 00:00:00（UnixのStartDate）から現在までの経過時間を取得する
        /// </summary>
        /// <returns>現在までの経過時間</returns>
        public static long GetUnixTimestampAsNanoSeconds()
        {
            return (DateTime.UtcNow - unixStartDate).Ticks * 100;
        }

        /// <summary>
        /// 配列の全要素にfillValueをセットする
        /// </summary>
        /// <typeparam name="T">配列要素のデータ型</typeparam>
        /// <param name="array">配列</param>
        /// <param name="fillValue">フィルする値</param>
        /// <returns>成功した場合true</returns>
        public static bool FillArray<T>(T[] array, T fillValue)
        {
            if (array.Length == 0)
            {
                return false;
            }
            array[0] = fillValue;
            int count;
            for (count = 1; count <= array.Length / 2; count *= 2)
            {
                Array.Copy(array, 0, array, count, count);
            }
            Array.Copy(array, 0, array, count, array.Length - count);
            return true;
        }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        /// <summary>
        /// adbコマンドを実行する
        /// </summary>
        /// <param name="adbPath">adbへのフルパス</param>
        /// <param name="command">adbコマンド</param>
        /// <param name="arguments">パラメータリスト</param>
        public static void InvokeAdbCommand(string adbPath, TofArManager.AdbCommand command, params string[] arguments)
        {
            if (!string.IsNullOrEmpty(adbPath) && System.IO.Path.IsPathRooted(adbPath))
            {
                var psi = new ProcessStartInfo();

                psi.FileName = adbPath;
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                string psiArgs = CheckArguments(command, arguments);
                psi.Arguments = psiArgs;
                var p = Process.Start(psi);

                string strOutput = ReadOutput(p.StandardOutput);
                p.WaitForExit();

                if (strOutput.StartsWith("adb: error:"))
                {
                    throw new InvalidOperationException(strOutput);
                }
                if (p.ExitCode != 0)
                {
                    var stdEtror = ReadOutput(p.StandardError);
                    throw new InvalidOperationException(stdEtror);
                }
            }
            else
            {
                throw new InvalidOperationException("AndroidSdk not found.");
            }
        }

        private static string CheckArguments(TofArManager.AdbCommand command, params string[] arguments)
        {
            switch (command)
            {
                case TofArManager.AdbCommand.Push:
                    if (arguments.Length != 2)
                    {
                        throw new ArgumentException("adb push needs two arguments (src, dst)");
                    }
                    // check arguments for absolute path
                    if (!System.IO.Path.IsPathRooted(arguments[0]))
                    {
                        throw new ArgumentException($"src {arguments[0]} must be absolute path");
                    }
                    if (!System.IO.Path.IsPathRooted(arguments[1]))
                    {
                        throw new ArgumentException($"dst {arguments[1]} must be absolute path");
                    }
                    return $"push {arguments[0]} {arguments[1]}";
                case TofArManager.AdbCommand.Pull:
                    if (arguments.Length != 2)
                    {
                        throw new ArgumentException("adb pull needs two arguments (src, dst)");
                    }
                    // check arguments for absolute path
                    if (!System.IO.Path.IsPathRooted(arguments[0]))
                    {
                        throw new ArgumentException($"src {arguments[0]} must be absolute path");
                    }
                    if (!System.IO.Path.IsPathRooted(arguments[1]))
                    {
                        throw new ArgumentException($"dst {arguments[1]} must be absolute path");
                    }
                    return $"pull {arguments[0]} {arguments[1]}";
                case TofArManager.AdbCommand.Remove:
                    if (arguments.Length != 1)
                    {
                        throw new ArgumentException("adb shell rm needs one argument");
                    }
                    if (!System.IO.Path.IsPathRooted(arguments[0]))
                    {
                        throw new ArgumentException("path must be absolute");
                    }
                    return $"shell \"rm {arguments[0]}\"";
                case TofArManager.AdbCommand.Forward:
                    if (arguments.Length != 2)
                    {
                        throw new ArgumentException("adb forward needs two arguments (src protocol:port, dst protocol:port)");
                    }
                    if (!System.Text.RegularExpressions.Regex.Match(arguments[0], @"^\b(tcp|udp|local)\b:\d{1,5}").Success ||
                        !System.Text.RegularExpressions.Regex.Match(arguments[1], @"^\b(tcp|udp|local)\b:\d{1,5}").Success)
                    {
                        throw new ArgumentException("Arguments must be like this: [tcp|udp|local]:[PORT_NUMBER]");
                    }
                    return $"forward {arguments[0]} {arguments[1]}";
                default:
                    throw new ArgumentException("Invalid adb command");
            }
        }

        private const int maxStrLen = 1024;

        /// <summary>
        /// readerから文字列を読む 
        /// </summary>
        /// <param name="reader">reader</param>
        /// <returns>読んだ文字列</returns>
        public static string ReadOutput(System.IO.StreamReader reader)
        {
            var sb = new StringBuilder();

            int cInt;
            while ((cInt = reader.Read()) != -1)
            {
                char c = (char)cInt;
                if (c == '\n' && sb.Length >= maxStrLen)
                {
                    break;
                }
                sb.Append(c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// adb の TCP forward コマンドを実行する 
        /// </summary>
        /// <param name="adbPath">adbへのフルパス</param>
        public static void TcpForward(string adbPath)
        {
            var src = "tcp:9999";
            var dst = "tcp:8080";
            try
            {
                Utils.InvokeAdbCommand(adbPath, TofArManager.AdbCommand.Forward, src, dst);
            }
            catch (InvalidOperationException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to tcp forward. {e.Message}");
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to tcp forward. {e.Message}");
            }
        }
#endif
    }
}
