using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UpdateAPIHelper.Base.Command;
using UpdateAPIHelper.Models;
using UpdateAPIHelper.Service;
using UpdaterAPI.Models;

namespace UpdateAPIHelper.ViewsModels
{

	class MainVM : Base.ViewModel.BaseViewModel
	{
		public MainVM()
		{
			#region Commands
			#endregion
			GenerateTree();
		}
		public Settings Settings => Settings.Instance;
		#region Parametrs

		#region TextUpdateInfo: Description
		/// <summary>Description</summary>
		private string _TextUpdateInfo;
		/// <summary>Description</summary>
		public string TextUpdateInfo { get => _TextUpdateInfo; set => Set(ref _TextUpdateInfo, value); }
		#endregion

		#region UpdateInfo: Description
		/// <summary>Description</summary>
		private UpdateInfo _UpdateInfo = new UpdateInfo();
		/// <summary>Description</summary>
		public UpdateInfo UpdateInfo { get => _UpdateInfo; set => Set(ref _UpdateInfo, value); }
		#endregion

		#region TreeViewList: Description
		/// <summary>Description</summary>
		private ObservableCollection<VersionItem> _TreeViewList = new ObservableCollection<VersionItem>();
		/// <summary>Description</summary>
		public ObservableCollection<VersionItem> TreeViewList { get => _TreeViewList; set => Set(ref _TreeViewList, value); }
		#endregion

		#region VisibilityNewVersionInfo: Description
		/// <summary>Description</summary>
		private Visibility _VisibilityNewVersionInfo = Visibility.Collapsed;
		/// <summary>Description</summary>
		public Visibility VisibilityNewVersionInfo { get => _VisibilityNewVersionInfo; set => Set(ref _VisibilityNewVersionInfo, value); }
		#endregion
		#region SelectedItem: Description
		/// <summary>Description</summary>
		private VersionItem _SelectedItem;
		/// <summary>Description</summary>
		public VersionItem SelectedItem
		{
			get => _SelectedItem; set
			{
				Set(ref _SelectedItem, value);
				ShowMenu();
			}
		}
		#endregion
		#endregion

		#region Commands

		#region DowloadUpdateInfoFromUrlCommand: Description
		private ICommand _DowloadUpdateInfoFromUrlCommand;
		public ICommand DowloadUpdateInfoFromUrlCommand => _DowloadUpdateInfoFromUrlCommand ??= new LambdaCommand(OnDowloadUpdateInfoFromUrlCommandExecuted, CanDowloadUpdateInfoFromUrlCommandExecute);
		private bool CanDowloadUpdateInfoFromUrlCommandExecute(object e) => true;
		private void OnDowloadUpdateInfoFromUrlCommandExecuted(object e)
		{
			TextUpdateInfo = Net.DowloadString(Settings.Parametrs.LastUrl);
		}
		#endregion


		#region ResetUpdateInfoCommand: Description
		private ICommand _ResetUpdateInfoCommand;
		public ICommand ResetUpdateInfoCommand => _ResetUpdateInfoCommand ??= new LambdaCommand(OnResetUpdateInfoCommandExecuted, CanResetUpdateInfoCommandExecute);
		private bool CanResetUpdateInfoCommandExecute(object e) => true;
		private void OnResetUpdateInfoCommandExecuted(object e)
		{
			UpdateInfo = new UpdateInfo();
			GenerateTree();
			MessageBoxHelper.InfoShow($"Информация о версиях сброшенна!");
		}
		#endregion

		#region UseTextUpdateInfoCommand: Description
		private ICommand _UseTextUpdateInfoCommand;
		public ICommand UseTextUpdateInfoCommand => _UseTextUpdateInfoCommand ??= new LambdaCommand(OnUseTextUpdateInfoCommandExecuted, CanUseTextUpdateInfoCommandExecute);
		private bool CanUseTextUpdateInfoCommandExecute(object e) => string.IsNullOrEmpty(TextUpdateInfo) == false;
		private void OnUseTextUpdateInfoCommandExecuted(object e)
		{
			try
			{
				UpdateInfo = UpdateInfo.Create(TextUpdateInfo);
				GenerateTree();
				MessageBoxHelper.InfoShow("Верный формат!");
			}
			catch (Exception ex) { MessageBoxHelper.ErrorShow("Неверный формат!", ex); }
		}
		#endregion
		#endregion

		#region Functions
		private void ShowMenu()
		{
			VisibilityNewVersionInfo = SelectedItem != null && SelectedItem.Type == TypeItem.Versions ? Visibility.Visible: Visibility.Collapsed;
		}
		private IEnumerable<VersionItem> GenerateFiles(IEnumerable<UpdaterAPI.Models.VersionInfo> files)
		{
			foreach (var i in files)
			{
				var version = new VersionItem() { Name = $"{i.Version}", Item = i, Type = TypeItem.VersionInfo };

				version.Items.Add(new VersionItem() { Name = $"Дата: {i.Date}", Item = i, Type = TypeItem.VersionInfo });
				version.Items.Add(new VersionItem() { Name = $"Тип: {i.Type}", Item = i, Type = TypeItem.VersionInfo });

				var info = new VersionItem() { Name = $"Файлы", Item = i, Type = TypeItem.Info };
				foreach (var file in i.Files)
				{
					var file_info = new VersionItem() { Name = $"{file.Path}", Item = i, Type = TypeItem.VersionInfo };
					file_info.Items.Add(new VersionItem() { Name = $"Url: {file.Url}", Item = i, Type = TypeItem.VersionInfo });
					file_info.Items.Add(new VersionItem() { Name = $"Hash: {file.Hash}", Item = i, Type = TypeItem.VersionInfo });
					info.Items.Add(file_info);
				}
				version.Items.Add(info);
				yield return version;
			}
		}
		private void GenerateTree()
		{
			TreeViewList.Clear();
			var main = new VersionItem() { Name = "UpdateInfo", Item = UpdateInfo, Type = TypeItem.UpdateInfo };
			TreeViewList.Add(main);

			var info_last_version = new VersionItem() { Name = "Последнии версии", Type = TypeItem.Info };
			foreach (var i in UpdateInfo.LastVersions)
			{
				var version = new VersionItem() { Name = $"{i.Version}", Item = i, Type = TypeItem.LastVersionInfo };

				version.Items.Add(new VersionItem() { Name = $"Дата: {i.Date}", Item = i, Type = TypeItem.LastVersionInfo });
				version.Items.Add(new VersionItem() { Name = $"Тип: {i.Type}", Item = i, Type = TypeItem.LastVersionInfo });
				if (string.IsNullOrEmpty(i.CustomType) == false)
					version.Items.Add(new VersionItem() { Name = $"Кастомный тип: {i.CustomType}", Item = i, Type = TypeItem.LastVersionInfo });
				info_last_version.Items.Add(version);
			}
			var info_version = new VersionItem() { Name = "Версии", Type = TypeItem.Versions };

			var info_Release = new VersionItem() { Name = "Release", Type = TypeItem.Info };
			foreach (var i in GenerateFiles(UpdateInfo.Versions.Where((i) => i.Type == TypeVersion.Release)))
				info_Release.Items.Add(i);

			var info_Beta = new VersionItem() { Name = "Beta", Type = TypeItem.Info };
			foreach (var i in GenerateFiles(UpdateInfo.Versions.Where((i) => i.Type == TypeVersion.Beta)))
				info_Beta.Items.Add(i);

			var info_Alpha = new VersionItem() { Name = "Alpha", Type = TypeItem.Info };
			foreach (var i in GenerateFiles(UpdateInfo.Versions.Where((i) => i.Type == TypeVersion.Alpha)))
				info_Alpha.Items.Add(i);


			var info_Custom = new VersionItem() { Name = $"Custom", Type = TypeItem.Info };
			foreach (var i in GenerateFiles(UpdateInfo.Versions.Where((i) => i.Type == TypeVersion.Custom)))
			{
				i.Name = $"{i.Name} | {(i.Item as VersionInfo).CustomType}";
				info_Custom.Items.Add(i);
			}



			info_version.Items.Add(info_Release);
			info_version.Items.Add(info_Beta);
			info_version.Items.Add(info_Alpha);
			info_version.Items.Add(info_Custom);

			main.Items.Add(info_last_version);
			main.Items.Add(info_version);
		}
		#endregion
	}
}
