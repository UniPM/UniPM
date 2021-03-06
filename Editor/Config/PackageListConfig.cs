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

using System.IO;

namespace UniPM
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using QFramework;
    using UnityEditor;
    
    [System.Serializable]
    public class PackageListConfig 
    {

        private const string GIT_URL_KEY = "UniRxGitUrlKey";
        public static string GitUrl
        {
            get { return EditorPrefs.GetString(GIT_URL_KEY, string.Empty); }
            set { EditorPrefs.SetString(GIT_URL_KEY, value); }
        }

        public List<PackageConfig> PackageList = new List<PackageConfig>();

        public List<PackageConfig> InstalledPackageList { get; set; }

        
        public static PackageListConfig GetInstalledPackageList()
        {
            PackageListConfig localConfig = new PackageListConfig();
            localConfig.InstalledPackageList = new List<PackageConfig>();

            IOUtils.GetDirSubFilePathList(Application.dataPath, true, ".asset").ForEach(fileName =>
            {
                if (fileName.EndsWith(".asset") && Directory.Exists(fileName.Replace(".asset",string.Empty)) && !fileName.Contains("PackageListServer"))
                {
                    fileName = fileName.Replace(Application.dataPath, "Assets");
                    var installedPackageInfo = AssetDatabase.LoadAssetAtPath<PackageConfig>(fileName);
                    if (null != installedPackageInfo) 
                    {
                        localConfig.InstalledPackageList.Add(installedPackageInfo);
                    }
                }
            });

            return localConfig;
        }

        public void GetRemote(Action<PackageListConfig> onConfigReceived)
        {
            ObservableWWW.Get(GitUrl.Append("/raw/master/PackageList.json").ToString())
                .Subscribe(jsonCotnent =>
                {
                    onConfigReceived(SerializeHelper.FromJson<PackageListConfig>(jsonCotnent));
                }, err =>
                {
                    Log.E(err);
                });
        }

        /// <summary>
        /// 剔除掉服务端同步的插件，再进行合并
        /// TODO: 只剔除一个就可以
        /// </summary>
        /// <returns></returns>
        public static PackageListConfig MergeGitClonedPackageList(PackageConfig toUploadPackageConfig)
        {
            string clonedConfigFilePath = Application.dataPath.CombinePath(toUploadPackageConfig.PackageServerGitUrl.GetLastWord()).CombinePath("PackageList.json");

            Log.I(clonedConfigFilePath);
            
            PackageListConfig clonedConfig = SerializeHelper.LoadJson<PackageListConfig>(clonedConfigFilePath);
            
            clonedConfig.InstalledPackageList.RemoveAll(config => config.Name.Equals(toUploadPackageConfig.Name));
            
            clonedConfig.InstalledPackageList.Add(toUploadPackageConfig);

            return clonedConfig;
        }


        public void SaveLocal()
        {
            EditorPrefs.SetString(GIT_URL_KEY, GitUrl);
        }

        public PackageListConfig SaveExport(string gitServerUrl = null)
        {
            this.SaveJson(Application.dataPath.CombinePath(gitServerUrl.IsNullOrEmpty() ? GitUrl.GetLastWord() : gitServerUrl.GetLastWord()).CombinePath("PackageList.json"));
            return this;
        }
    }
}