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
using TensorFlowLite.Runtime;
using UnityEngine;

namespace TofAr.V0.MarkRecog
{
    internal class TFLiteEstimator
    {
        string sNetwork1 = "hand_mark.tflite";

        TFLiteRuntime tflite;

        string message;
        float[] input_data;
        float[] output_data;

        //keep textures to reducde reallocations
        Texture2D tex2_, tex2_resized;

        private string localFile = null;

        public int Init(TFLiteRuntime.ExecMode execMode, int threadsNum = 1)
        {
            this.RestoreTFLite();

            try
            {
                this.tflite = new TFLiteRuntime(localFile, execMode, threadsNum);
            }
            catch (Exception e)
            {
                this.tflite = null;
                TofArManager.Logger.WriteLog(LogLevel.Debug, $"Failed to initialize TFLite runtime: {e.Message}");
            }
            finally
            {
                if (this.localFile != null && System.IO.File.Exists(this.localFile))
                {
                    File.Delete(this.localFile);
                }
            }

            if (this.tflite == null)
            {
                return 2;
            }
            // get initial guess
            this.Exec(new Texture2D(200, 200, TextureFormat.ARGB32, false));


            return 0;
        }

        public void Free()
        {
            try
            {
                if (this.tflite != null)
                {
                    this.tflite.Dispose();
                    this.tflite = null;
                }
            }
            catch (DllNotFoundException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Error code: " + e);
            }
            catch (ArgumentException e)
            {
                TofArManager.Logger.WriteLog(LogLevel.Debug, "Error code: " + e);
            }

            CleanupTFLite();
        }

        private static Texture2D GetResized(Texture2D texture, int width, int height)
        {
            var rt = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(texture, rt);

            var preRT = RenderTexture.active;
            RenderTexture.active = rt;
            var ret = new Texture2D(width, height);
            ret.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            ret.Apply();
            RenderTexture.active = preRT;

            RenderTexture.ReleaseTemporary(rt);
            return ret;
        }

        public float[] Exec(Texture2D tex)
        {
            if (!ProcessInput(tex))
            {
                return null;
            }

            output_data = tflite.forward()[0];

            if (output_data == null)
            {
                return null;
            }
            int output_size = output_data.Length;
            float[] levels = new float[output_size];

            for (int dn = 0; dn < output_size; ++dn)
            {
                levels[dn] = output_data[dn];

            }

            return levels;
        }

        private bool ProcessInput(Texture2D tex)
        {
            if (tex == null)
            {
                return false;
            }

            int width = tex.width;
            int height = tex.height;


            // Processing algorithm  to cut out to input of DNN that 28x28.
            // 1. Examine the top and bottom edges and left and right edges of non-zero pixels on the image, and set width, height and center position.
            // 2. Cut out a square with the larger side of Width and height as one side.
            // 3. Resize the cropped image to 24x24.
            // 4. Add zero padding of 2 pixels each on the top, bottom, left, and right to make it 28x28.

            // 1.
            Rect boundaries = GetBoundaries(tex);
            if (boundaries.width == 0 || boundaries.height == 0)
            {
                return false;
            }

            int cx = (int)boundaries.x;
            int cy = (int)boundaries.y;
            int cw = (int)boundaries.width;
            int ch = (int)boundaries.height;

            int cutLength = Mathf.Max(cw, ch);

            if (tex2_ == null || tex2_.height != ch || tex2_.width != cw)
            {
                tex2_ = new Texture2D(cw, ch, TextureFormat.ARGB32, false);
            }
            var rt = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(tex, rt);
            var currentRT = RenderTexture.active;
            RenderTexture.active = rt;
            tex2_.ReadPixels(new Rect(cx, cy, cw, ch), 0, 0);
            tex2_.Apply();
            RenderTexture.active = currentRT;
            RenderTexture.ReleaseTemporary(rt);

            // 3.
            int ww = 24 * cw / cutLength;
            int hh = 24 * ch / cutLength;
            tex2_resized = GetResized(tex2_, ww, hh);

            input_data = tflite.getInputBuffer()[0];
            if (input_data == null)
            {
                return false;
            }
            if (input_data.Length < (28 * 28))
            {
                return false;
            }

            // 0 clear
            for (int i = 0; i < 28 * 28; i++)
            {
                input_data[i] = 0;
            }

            UnityEngine.Color[] colors2 = tex2_resized.GetPixels();
            int offsX = (28 - ww) / 2;
            int offsY = (28 - hh) / 2;
            try
            {
                for (int y = 0; y < hh; y++)
                {
                    for (int x = 0; x < ww; x++)
                    {
                        var color = colors2[(hh - y - 1) * ww + (ww - x - 1)];
                        input_data[(y + offsY) * 28 + (x + offsX)] = (color.r + color.g + color.b) > 0f ? 1f : 0f;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }

            return true;
        }

        private Rect GetBoundaries(Texture2D tex)
        {
            int width = tex.width;
            int height = tex.height;

            UnityEngine.Color[] colors = tex.GetPixels();

            int maxx = 0, maxy = 0, minx = width, miny = height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (colors[y * width + x].r != 0)
                    {
                        if (maxx < x)
                        {
                            maxx = x;
                        }
                        if (maxy < y)
                        {
                            maxy = y;
                        }
                        if (x < minx)
                        {
                            minx = x;
                        }
                        if (y < miny)
                        {
                            miny = y;
                        }
                    }
                }
            }
            float centerx = (minx + maxx) * 0.5f;
            float centery;
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2 || SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 ||
                SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Metal)
            {
                centery = (miny + maxy) * 0.5f;
            }
            else
            {
                centery = ((height - 1 - miny) + (height - 1 - maxy)) * 0.5f;
            }
            int cutWidth = maxx - minx + 1;
            int cutHeight = maxy - miny + 1;
            int cutLength = Mathf.Max(cutWidth, cutHeight);
            if (cutWidth <= 0 || cutHeight <= 0)
            {
                return new Rect();
            }

            // 2.
            int cx = (int)(centerx - cutLength * 0.5f);
            int cy = (int)(centery - cutLength * 0.5f);
            int cw = cutLength;
            if (cx < 0)
            {
                cw = cutLength + cx * 2;
                cx = 0;
            }
            int ch = cutLength;
            if (cy < 0)
            {
                ch = cutLength + cy * 2;
                cy = 0;
            }

            return new Rect(cx, cy, cw, ch);
        }

        public int getMaxResultFromLevels(float[] levels)
        {
            float max_score = (float)(-1.0e10);
            int prediction = 0;

            message = "Prediction scores: ";

            int output_size = output_data.Length;

            for (int dn = 0; dn < output_size; ++dn)
            {
                if (levels[dn] > max_score)
                {
                    prediction = dn;
                    max_score = levels[dn];
                }
                message = levels[dn] + " ";
                TofArManager.Logger.WriteLog(LogLevel.Debug, message);
            }

            message = "Mark Prediction: " + prediction;

            return prediction;
        }

        private string GetData(string file)
        {
            string toPath;
            string FileName = Path.GetFileNameWithoutExtension(file);

            TextAsset asset = Resources.Load(FileName) as TextAsset;
            if (asset != null)
            {
                Stream s = new MemoryStream(asset.bytes);
                BinaryReader br = new BinaryReader(s);
                toPath = Application.persistentDataPath + "/" + FileName + ".tflite";

                File.WriteAllBytes(toPath, br.ReadBytes(asset.bytes.Length));

                return toPath;
            }

            return null;
        }

        public void RestoreTFLite()
        {
            string pathToNetwork = Application.persistentDataPath + "/" + sNetwork1;
            if (!File.Exists(pathToNetwork) || Utils.PathIsSymlink(pathToNetwork))
            {
                this.localFile = GetData(sNetwork1);
            }
            else
            {
                this.localFile = pathToNetwork;
            }
        }

        public void CleanupTFLite()
        {
            if (this.localFile != null && File.Exists(this.localFile))
            {
                File.Delete(this.localFile);
            }
        }
    }
}
