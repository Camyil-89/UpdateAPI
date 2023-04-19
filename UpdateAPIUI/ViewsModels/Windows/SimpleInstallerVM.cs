using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using UpdateAPIUI.Base.Command;
using UpdateAPIUI.Models;
using UpdateAPIUI.Service;
using UpdateAPIUI.Services;
using UpdateAPIUI.Views.Windows;

namespace UpdateAPIUI.ViewsModels.Windows
{

	class SimpleInstallerVM : Base.ViewModel.BaseViewModel
	{
		private static object _lock = new object();
		public SimpleInstallerVM()
		{
			BindingOperations.EnableCollectionSynchronization(DowloadFiles, _lock);
			#region Commands
			#endregion
		}

		public bool WaitEndDowload = false;
		#region Parametrs
		public SimpleInstallerWindow Window;

		public StatusDowload StatusDowload = StatusDowload.Error;

		#region Language: Description
		/// <summary>Description</summary>
		private Language _Language = new Models.Language();
		/// <summary>Description</summary>
		public Language Language { get => _Language; set => Set(ref _Language, value); }
		#endregion

		#region DowloadFiles: Description
		/// <summary>Description</summary>
		private ObservableCollection<string> _DowloadFiles = new ObservableCollection<string>();
		/// <summary>Description</summary>
		public ObservableCollection<string> DowloadFiles { get => _DowloadFiles; set => Set(ref _DowloadFiles, value); }
		#endregion

		#region NowProgress: Description
		/// <summary>Description</summary>
		private double _NowProgress = 0;
		/// <summary>Description</summary>
		public double NowProgress { get => _NowProgress; set => Set(ref _NowProgress, value); }
		#endregion


		#region TotalSizeFile: Description
		/// <summary>Description</summary>
		private long _TotalSizeFile;
		/// <summary>Description</summary>
		public long TotalSizeFile { get => _TotalSizeFile; set => Set(ref _TotalSizeFile, value); }
		#endregion


		#region TextSpeedDowload: Description
		/// <summary>Description</summary>
		private string _TextSpeedDowload;
		/// <summary>Description</summary>
		public string TextSpeedDowload { get => _TextSpeedDowload; set => Set(ref _TextSpeedDowload, value); }
		#endregion


		#region TextNowFile: Description
		/// <summary>Description</summary>
		private string _TextNowFile;
		/// <summary>Description</summary>
		public string TextNowFile { get => _TextNowFile; set => Set(ref _TextNowFile, value); }
		#endregion

		private UpdaterAPI.GitHub.Downloader Downloader;
		#endregion

		#region Commands

		#region CancelCommand: Description
		private ICommand _CancelCommand;
		public ICommand CancelCommand => _CancelCommand ??= new LambdaCommand(OnCancelCommandExecuted, CanCancelCommandExecute);
		private bool CanCancelCommandExecute(object e) => true;
		private void OnCancelCommandExecuted(object e)
		{
			AbortDownload();
		}
		#endregion
		#endregion

		#region Functions
		private void Download(UpdaterAPI.Models.VersionInfo version, string tmp_path_folder)
		{
			WaitEndDowload = true;
			try
			{
				foreach (var i in Downloader.UpdateFiles(version, tmp_path_folder))
				{
					TextNowFile = i.Path;
					NowProgress = i.PercentageDowload;
					TextSpeedDowload = $"{Utilities.RoundByte(i.TotalDowload, Language)} \\ {Utilities.RoundByte(i.SizeFile, Language)} ({Utilities.RoundByte(i.SpeedDowload, Language)} {Language.PerSecondText}) {Language.FileText}: {i.NowFileIndex} \\ {i.FilesCount}";
					if (i.Type == UpdaterAPI.GitHub.TypeInfoDowload.DowloadFileFinished)
					{
						DowloadFiles.Add(i.Path);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBoxHelper.ErrorShow("Произошла ошибка при скачивании!", ex);
				WaitEndDowload = false;
				return;
			}
			if (StatusDowload != StatusDowload.AbortDowload)
				StatusDowload = StatusDowload.OK;
			WaitEndDowload = false;
		}
		public void Start(UpdaterAPI.GitHub.Downloader downloader, UpdaterAPI.Models.VersionInfo version, string tmp_path_folder)
		{
			Downloader = downloader;
			Window.Closing += Window_Closing;
			Task.Run(() =>
			{
				Download(version, tmp_path_folder);
				Window.Dispatcher.Invoke(() => { Window.Close(); });
			});
		}
		private void AbortDownload()
		{
			Downloader.CancelDowload();
			if (StatusDowload != StatusDowload.OK)
			StatusDowload = StatusDowload.AbortDowload;
		}
		private void Window_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
		{
			AbortDownload();
		}
		#endregion
	}
}
