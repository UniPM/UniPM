﻿/****************************************************************************
 * Copyright (c) 2017 liangxieq
 * 
 * https://github.com/UniPM/UniPM
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using DG.Tweening.Plugins.Core.PathCore;

namespace UniPM
{
    using UnityEngine;
    using UniRx;
    using System;
    using System.Collections.Generic;
    using PTGame.Framework.Libs;
    using PTGame.Framework;

    [System.Serializable]
    public class PackageManagerConfig
    {
        public static PackageManagerConfig GetLocal()
        {
            PackageManagerConfig localConfig = new PackageManagerConfig();

            IOUtils.GetDirSubFilePathList(Application.dataPath + "/PTUGame", true, ".json").ForEach(fileName =>
            {
                if (fileName.EndsWith("Package.json") && !fileName.Contains("PTGamePluginServer"))
                {
                    var localPluginInfo = SerializeHelper.LoadJson<PackageConfig>(fileName);

                    localConfig.PackageList.Add(localPluginInfo);
                }
            });

            return localConfig;
        }

        public static void GetRemote(Action<PackageManagerConfig> onConfigReceived)
        {
            ObservableWWW.Get("http://code.putao.io/liqingyun/PTGamePluginServer/raw/master/PackageList.json")
                .Subscribe(jsonCotnent =>
                {
                    Log.I(jsonCotnent);
                    onConfigReceived(SerializeHelper.FromJson<PackageManagerConfig>(jsonCotnent));
                }, err =>
                {
                    err.ToString().Log();
                });
        }

        public List<PackageConfig> PackageList = new List<PackageConfig>();

        public void SaveLocal()
        {
            this.SaveJson(Application.dataPath + "/PTUGame/PackageList.json");
        }

        public void SaveExport()
        {
            this.SaveJson(Application.dataPath + "/PTUGame/PTGamePluginServer/PackageList.json");
        }
    }
}