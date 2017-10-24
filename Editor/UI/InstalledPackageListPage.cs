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

namespace PTGame.Framework
{
	using UnityEngine;
	using UniPM;
    /// <summary>
    /// 本地页面
    /// </summary>
    public class InstalledPackageListPage : EditorView
    {
	    public InstalledPackageListPage(PackageManagerConfig localConfig)
	    {
		    ScrollView scrollView = new ScrollView();
		    scrollView.Width = 800;
		    scrollView.Height = 600f;

		    VerticalView verticalView = new VerticalView();
		    verticalView.AddChild(new SpaceView());

		    HorizontalView horizontalView = new HorizontalView();

		    horizontalView.AddChild(UIFactory.CreateTitleLabel("package name"));
		    horizontalView.AddChild(UIFactory.CreateTitleLabel("version"));
		    horizontalView.AddChild(UIFactory.CreateTitleLabel("copyright"));
		    horizontalView.AddChild(UIFactory.CreateTitleLabel("folder"));
		    

		    verticalView.AddChild(horizontalView);
		    scrollView.AddChild(verticalView);

		    foreach (var localConfigPluginInfo in localConfig.PluginInfos)
		    {
			    var scrollItem = new HorizontalView();
			    
			    scrollItem.AddChild(UIFactory.CreateInstalledLabel(localConfigPluginInfo.Name));
			    scrollItem.AddChild(UIFactory.CreateInstalledLabel(string.Format("v{0}", localConfigPluginInfo.Version)));
			    scrollItem.AddChild(UIFactory.CreateInstalledLabel(localConfigPluginInfo.CopyRight));
			    scrollItem.AddChild(new Button("Download", 65, 25, () => UniPMWindow.DownloadZip(localConfigPluginInfo)));
//			    scrollItem.AddChild(UIFactory.CreateInstalledLabel(localConfigPluginInfo.PackagePath));

//			    scrollItem.AddChild(new Button("update", 65, 25, () => Application.OpenURL(localConfigPluginInfo.Url)));
			    scrollView.AddChild(scrollItem);
		    }

		    scrollView.AddChild(new FlexibaleSpaceView());

		    AddChild(scrollView);
	    }
    }
}