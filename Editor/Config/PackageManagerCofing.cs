/****************************************************************************
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
                if (fileName.EndsWith("Config.json") && !fileName.Contains("PTGamePluginServer"))
                {
                    var localPluginInfo = SerializeHelper.LoadJson<PackageConfig>(fileName);

                    localConfig.PluginInfos.Add(localPluginInfo);
                }
            });

            return localConfig;
        }

        public static void GetRemote(Action<PackageManagerConfig> onConfigReceived)
        {
            ObservableWWW.Get(UniPMWindow.ServerURL + "raw/master/ServerConfigs.json").Subscribe(jsonCotnent =>
            {
                onConfigReceived(SerializeHelper.FromJson<PackageManagerConfig>(jsonCotnent));
            }, err =>
            {
                err.ToString().Log();
            });
        }

        public List<PackageConfig> PluginInfos = new List<PackageConfig>();

        public void SaveLocal()
        {
            this.SaveJson(Application.dataPath + "/PTUGame/ServerConfigs.json");
        }

        public void SaveExport()
        {
            this.SaveJson(Application.dataPath + "/PTUGame/PTGamePluginServer/ServerConfigs.json");
        }
    }
}