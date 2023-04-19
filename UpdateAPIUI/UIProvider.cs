using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UpdateAPIUI.Models;
using UpdateAPIUI.Views.Windows;
using UpdateAPIUI.ViewsModels.Windows;

namespace UpdateAPIUI
{
	public static class UIProvider
	{
		public static StatusDowload DowloadFilesWithInstaller(UpdaterAPI.GitHub.Downloader downloader, UpdaterAPI.Models.VersionInfo version, string tmp_path_folder, Language language = null)
		{
			SimpleInstallerWindow window = new SimpleInstallerWindow();

			SimpleInstallerVM vm = new SimpleInstallerVM();
			window.DataContext = vm;
			if (language != null)
				vm.Language = language;
			vm.Window = window;

			vm.Start(downloader, version, tmp_path_folder);

			window.ShowDialog();
			while (vm.WaitEndDowload)
			{
				Thread.Sleep(50);
			}
			return vm.StatusDowload;
		}
	}
}
