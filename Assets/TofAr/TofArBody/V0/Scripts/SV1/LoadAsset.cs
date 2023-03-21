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
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace TofAr.V0.Body.SV1
{
    /// <summary>
    /// *TODO+ B internalにできないか？
    /// Body推定用ファイルの管理
    /// </summary>
    internal static class LoadAsset
    {
        public delegate void LogOutputEventHandler(string message);
        public static event LogOutputEventHandler OnLogOutput;

        public static string LoadDirectoryFromResources(string dirPath)
        {
            string asset_path = null;
            string local_path = null;

            try
            {
                asset_path = dirPath;
                local_path = Application.persistentDataPath + "/" + asset_path;
                OnLogOutput?.Invoke($"Load directory from Resources:\n    asset_path = {asset_path}\n    local_path = {local_path}");

                var file_info = new FileInfo(local_path);
                file_info.Directory.Create();

                var objects = Resources.LoadAll(asset_path);
                foreach (var obj in objects)
                {
                    if (obj is TextAsset)
                    {
                        TextAsset asset = (TextAsset)obj;
                        var s = new MemoryStream(asset.bytes);
                        var br = new BinaryReader(s);
                        File.WriteAllBytes(local_path + Path.GetFileName(asset.name), br.ReadBytes(asset.bytes.Length));
                    }
                }
            }
            catch (IOException e)
            {
                OnLogOutput?.Invoke($"Failed to load directory from Resources:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }
            catch (ArgumentException e)
            {
                OnLogOutput?.Invoke($"Failed to load directory from Resources:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }

            return local_path;
        }

        public static string LoadFileFromResources(string filePath)
        {
            string asset_path = null;
            string local_path = null;

            try
            {
                asset_path = filePath;
                local_path = Application.persistentDataPath + "/" + asset_path;
                OnLogOutput?.Invoke($"Load file from Resources:\n    asset_path = {asset_path}\n    local_path = {local_path}");

                var file_info = new FileInfo(local_path);
                file_info.Directory.Create();

                var asset = Resources.Load(asset_path) as TextAsset;
                if (asset != null)
                {
                    var s = new MemoryStream(asset.bytes);
                    var br = new BinaryReader(s);
                    File.WriteAllBytes(local_path, br.ReadBytes(asset.bytes.Length));
                }
            }
            catch (IOException e)
            {
                OnLogOutput?.Invoke($"Failed to load directory from Resources:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }
            catch (ArgumentException e)
            {
                OnLogOutput?.Invoke($"Failed to load directory from Resources:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }

            return local_path;
        }

        // [TODO] Does not work on Android, because could not search files using GetFiles().
        public static string LoadDirectoryFromStreamingAssets(string dirPath)
        {
            string asset_path = null;
            string local_path = null;

            try
            {
                asset_path = Application.streamingAssetsPath + "/" + dirPath;
                local_path = Application.persistentDataPath + "/" + dirPath;
                OnLogOutput?.Invoke($"Load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}");

                var file_info = new FileInfo(local_path);
                file_info.Directory.Create();

                foreach (var asset in Directory.GetFiles(asset_path, "*.raw"))
                {
                    using (var request = UnityWebRequest.Get(asset))
                    {
                        // Set the communication timeout to 3.0 seconds.
                        var time_out = Time.realtimeSinceStartup + 3.0f;

                        request.SendWebRequest();
                        while (!request.isDone)
                        {
                            if (time_out < Time.realtimeSinceStartup)
                            {
                                var err_msg = "Comunication timeout, over than 3.0 seconds";
                                OnLogOutput?.Invoke(err_msg);
                                throw new TimeoutException(err_msg);
                            }
                            else if ((request.result == UnityWebRequest.Result.ConnectionError) || (request.result == UnityWebRequest.Result.ProtocolError))
                            {
                                OnLogOutput?.Invoke(request.error);
                                throw new System.Net.WebException(request.error);
                            }
                        }

                        // Copy to local, because can not access to StreamingAssets directly.
                        File.WriteAllBytes(local_path + Path.GetFileName(asset), request.downloadHandler.data);
                    }
                }
            }
            catch (ArgumentException e)
            {
                OnLogOutput?.Invoke($"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }
            catch (IOException e)
            {
                OnLogOutput?.Invoke($"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }
            catch (System.Net.WebException e)
            {
                OnLogOutput?.Invoke($"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }
            catch (TimeoutException e)
            {
                OnLogOutput?.Invoke($"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }

            return local_path;
        }

        public static string LoadFileFromStreamingAssets(string filePath)
        {
            string asset_path = null;
            string local_path = null;

            try
            {
                asset_path = Application.streamingAssetsPath + "/" + filePath;
                local_path = Application.persistentDataPath + "/" + filePath;
                OnLogOutput?.Invoke($"Load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}");

                var file_info = new FileInfo(local_path);
                file_info.Directory.Create();

                using (UnityWebRequest request = UnityWebRequest.Get(asset_path))
                {
                    // Set the communication timeout to 3.0 seconds.
                    var time_out = Time.realtimeSinceStartup + 3.0f;

                    request.SendWebRequest();
                    while (!request.isDone)
                    {
                        if (time_out < Time.realtimeSinceStartup)
                        {
                            var err_msg = "Comunication timeout, over than 3.0 seconds";
                            OnLogOutput?.Invoke(err_msg);
                            throw new TimeoutException(err_msg);
                        }
                        else if ((request.result == UnityWebRequest.Result.ConnectionError) || (request.result == UnityWebRequest.Result.ProtocolError))
                        {
                            OnLogOutput?.Invoke(request.error);
                            throw new System.Net.WebException(request.error);
                        }
                    }

                    // Copy to local, because can not access to StreamingAssets directly.
                    File.WriteAllBytes(local_path, request.downloadHandler.data);
                }
            }
            catch (ArgumentException e)
            {
                OnLogOutput?.Invoke($"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }
            catch (IOException e)
            {
                OnLogOutput?.Invoke($"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }
            catch (System.Net.WebException e)
            {
                OnLogOutput?.Invoke($"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }
            catch (TimeoutException e)
            {
                OnLogOutput?.Invoke($"Failed to load file from StreamingAssets:\n    asset_path = {asset_path}\n    local_path = {local_path}. Reason {e.Message}");
                throw;
            }

            return local_path;
        }

        public static void RemoveLocalFiles()
        {
            string local_path = Application.persistentDataPath + "/tflite";
            if (System.IO.Directory.Exists(local_path))
            {
                bool recursive = true;
                System.IO.Directory.Delete(local_path, recursive);
            }
        }
    }
}
