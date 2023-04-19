using Microsoft.VisualBasic;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml.Serialization;
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
		private ObservableCollection<TreeViewItem> _TreeViewList = new ObservableCollection<TreeViewItem>();
		/// <summary>Description</summary>
		public ObservableCollection<TreeViewItem> TreeViewList { get => _TreeViewList; set => Set(ref _TreeViewList, value); }
		#endregion

		#region VisibilityNewVersionInfo: Description
		/// <summary>Description</summary>
		private Visibility _VisibilityNewVersionInfo = Visibility.Collapsed;
		/// <summary>Description</summary>
		public Visibility VisibilityNewVersionInfo { get => _VisibilityNewVersionInfo; set => Set(ref _VisibilityNewVersionInfo, value); }
		#endregion


		#region VisibilityAboutVersionInfo: Description
		/// <summary>Description</summary>
		private Visibility _VisibilityAboutVersionInfo = Visibility.Collapsed;
		/// <summary>Description</summary>
		public Visibility VisibilityAboutVersionInfo { get => _VisibilityAboutVersionInfo; set => Set(ref _VisibilityAboutVersionInfo, value); }
		#endregion


		#region VisibilityLastVersionInfo: Description
		/// <summary>Description</summary>
		private Visibility _VisibilityLastVersionInfo = Visibility.Collapsed;
		/// <summary>Description</summary>
		public Visibility VisibilityLastVersionInfo { get => _VisibilityLastVersionInfo; set => Set(ref _VisibilityLastVersionInfo, value); }
		#endregion


		#region SelectedVersionInfo: Description
		/// <summary>Description</summary>
		private UpdaterAPI.Models.FileInfo _SelectedVersionInfo;
		/// <summary>Description</summary>
		public UpdaterAPI.Models.FileInfo SelectedVersionInfo { get => _SelectedVersionInfo; set => Set(ref _SelectedVersionInfo, value); }
		#endregion

		#region ViewFilesInfo: Description
		/// <summary>Description</summary>
		private ObservableCollection<UpdaterAPI.Models.FileInfo> _ViewFilesInfo = new ObservableCollection<UpdaterAPI.Models.FileInfo>();
		/// <summary>Description</summary>
		public ObservableCollection<UpdaterAPI.Models.FileInfo> ViewFilesInfo { get => _ViewFilesInfo; set => Set(ref _ViewFilesInfo, value); }
		#endregion


		#region VersionInfoType: Description
		/// <summary>Description</summary>
		private TypeVersion _VersionInfoType = TypeVersion.Release;
		/// <summary>Description</summary>
		public TypeVersion VersionInfoType { get => _VersionInfoType; set => Set(ref _VersionInfoType, value); }
		#endregion


		#region VersionInfoVersion: Description
		/// <summary>Description</summary>
		private string _VersionInfoVersion;
		/// <summary>Description</summary>
		public string VersionInfoVersion { get => _VersionInfoVersion; set => Set(ref _VersionInfoVersion, value); }
		#endregion


		#region VersionInfoCustomType: Description
		/// <summary>Description</summary>
		private string _VersionInfoCustomType = null;
		/// <summary>Description</summary>
		public string VersionInfoCustomType { get => _VersionInfoCustomType; set => Set(ref _VersionInfoCustomType, value); }
		#endregion

		public IEnumerable<KeyValuePair<string, string>> VersionInfoTypes
		{
			get
			{
				return EnumHelper.GetAllValuesAndDescriptions<TypeVersion>();
			}
		}

		#region ExportTextUpdateInfo: Description
		/// <summary>Description</summary>
		private string _ExportTextUpdateInfo;
		/// <summary>Description</summary>
		public string ExportTextUpdateInfo { get => _ExportTextUpdateInfo; set => Set(ref _ExportTextUpdateInfo, value); }
		#endregion


		#region SelectedItemTreeViewItem: Description
		/// <summary>Description</summary>
		private TreeViewItem _SelectedItemTreeViewItem;
		/// <summary>Description</summary>
		public TreeViewItem SelectedItemTreeViewItem
		{
			get => _SelectedItemTreeViewItem; set
			{
				Set(ref _SelectedItemTreeViewItem, value);
				if (value == null)
				{
					SelectedItem = null;
					return;
				}
				SelectedItem = (VersionItem)value.Tag;
			}
		}
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


		#region AboutVersionInfoText: Description
		/// <summary>Description</summary>
		private string _AboutVersionInfoText;
		/// <summary>Description</summary>
		public string AboutVersionInfoText { get => _AboutVersionInfoText; set => Set(ref _AboutVersionInfoText, value); }
		#endregion


		#region VersionInfoDate: Description
		/// <summary>Description</summary>
		private DateTime _VersionInfoDate = DateTime.Now;
		/// <summary>Description</summary>
		public DateTime VersionInfoDate { get => _VersionInfoDate; set => Set(ref _VersionInfoDate, value); }
		#endregion

		#region NewVersionInfo: Description
		/// <summary>Description</summary>
		private VersionInfo _NewVersionInfo = new VersionInfo();
		/// <summary>Description</summary>
		public VersionInfo NewVersionInfo { get => _NewVersionInfo; set => Set(ref _NewVersionInfo, value); }
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
			MessageBoxHelper.BallonTip($"Информация о версиях сброшенна!");
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
				MessageBoxHelper.BallonTip("Верный формат!");
			}
			catch (Exception ex) { MessageBoxHelper.ErrorShow("Неверный формат!", ex); }
		}
		#endregion

		#region RemovePathFileCommand: Description
		private ICommand _RemovePathFileCommand;
		public ICommand RemovePathFileCommand => _RemovePathFileCommand ??= new LambdaCommand(OnRemovePathFileCommandExecuted, CanRemovePathFileCommandExecute);
		private bool CanRemovePathFileCommandExecute(object e) => SelectedVersionInfo != null;
		private void OnRemovePathFileCommandExecuted(object e)
		{
			NewVersionInfo.Files.Remove(SelectedVersionInfo);
			SelectedVersionInfo = null;
			CreateViewFilesInfo();
		}
		#endregion


		#region CopyToClipboardCommand: Description
		private ICommand _CopyToClipboardCommand;
		public ICommand CopyToClipboardCommand => _CopyToClipboardCommand ??= new LambdaCommand(OnCopyToClipboardCommandExecuted, CanCopyToClipboardCommandExecute);
		private bool CanCopyToClipboardCommandExecute(object e) => string.IsNullOrEmpty(ExportTextUpdateInfo) == false;
		private void OnCopyToClipboardCommandExecuted(object e)
		{
			Clipboard.SetText(ExportTextUpdateInfo);
			MessageBoxHelper.BallonTip("Текст скопирован!");
		}
		#endregion

		#region ExportUpdateInfoToTextCommand: Description
		private ICommand _ExportUpdateInfoToTextCommand;
		public ICommand ExportUpdateInfoToTextCommand => _ExportUpdateInfoToTextCommand ??= new LambdaCommand(OnExportUpdateInfoToTextCommandExecuted, CanExportUpdateInfoToTextCommandExecute);
		private bool CanExportUpdateInfoToTextCommandExecute(object e) => true;
		private void OnExportUpdateInfoToTextCommandExecuted(object e)
		{
			ExportTextUpdateInfo = UpdateInfo.ToString();
		}
		#endregion
		#region AddFilesCommand: Description
		private ICommand _AddFilesCommand;
		public ICommand AddFilesCommand => _AddFilesCommand ??= new LambdaCommand(OnAddFilesCommandExecuted, CanAddFilesCommandExecute);
		private bool CanAddFilesCommandExecute(object e) => true;
		private void OnAddFilesCommandExecuted(object e)
		{
			CommonOpenFileDialog dialog = new CommonOpenFileDialog();
			dialog.IsFolderPicker = true;
			dialog.Multiselect = false;
			if (dialog.ShowDialog() == CommonFileDialogResult.Ok && dialog.FileNames.Count() > 0)
			{
				try
				{
					NewVersionInfo.Files.Clear();
					foreach (var path in GetAllFilesInFolder(dialog.FileName))
					{
						var local_path = path.Replace($"{new DirectoryInfo(dialog.FileName).Parent.FullName}\\{dialog.FileName.Split("\\").Last()}", "");
						NewVersionInfo.AddFile(path, local_path, path.Replace($"{new DirectoryInfo(dialog.FileName).Parent.FullName}\\", ""));
					}
					CreateViewFilesInfo();
				}
				catch (Exception ex) { MessageBoxHelper.ErrorShow("Не используйте корневой каталог!", ex); }

			}
		}
		#endregion

		#region AddNewVerisonInfoCommand: Description
		private ICommand _AddNewVerisonInfoCommand;
		public ICommand AddNewVerisonInfoCommand => _AddNewVerisonInfoCommand ??= new LambdaCommand(OnAddNewVerisonInfoCommandExecuted, CanAddNewVerisonInfoCommandExecute);
		private bool CanAddNewVerisonInfoCommandExecute(object e)
		{
			if (NewVersionInfo.Files.Count == 0)
				return false;
			if (VersionInfoType == TypeVersion.Custom)
			{
				if (string.IsNullOrEmpty(VersionInfoCustomType) == false)
					return true;
				return false;
			}

			if (string.IsNullOrEmpty(VersionInfoVersion) == false)
				return true;

			return false;
		}
		private void OnAddNewVerisonInfoCommandExecuted(object e)
		{
			NewVersionInfo.Version = VersionInfoVersion;
			NewVersionInfo.Type = VersionInfoType;
			NewVersionInfo.CustomType = VersionInfoCustomType;
			NewVersionInfo.Date = VersionInfoDate;

			try
			{
				UpdateInfo.AddNewVersion(NewVersionInfo);
			}
			catch (Exception ex)
			{
				MessageBoxHelper.WarningShow(ex.Message);
				return;
			}
			GenerateTree();


			NewVersionInfo = new VersionInfo();
			CreateViewFilesInfo();
		}
		#endregion

		#region SaveAsUpdateInfoTextCommand: Description
		private ICommand _SaveAsUpdateInfoTextCommand;
		public ICommand SaveAsUpdateInfoTextCommand => _SaveAsUpdateInfoTextCommand ??= new LambdaCommand(OnSaveAsUpdateInfoTextCommandExecuted, CanSaveAsUpdateInfoTextCommandExecute);
		private bool CanSaveAsUpdateInfoTextCommandExecute(object e) => string.IsNullOrEmpty(ExportTextUpdateInfo) == false;
		private void OnSaveAsUpdateInfoTextCommandExecuted(object e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Text Files (*.xml)|*.xml|All Files (*.*)|*.*";
			saveFileDialog.FileName = "UpdateInfo.xml";

			if (saveFileDialog.ShowDialog() == true)
			{
				if (e.ToString() == "0")
					File.WriteAllText(saveFileDialog.FileName, ExportTextUpdateInfo);
				else
				{
					using (StreamWriter file_write = new StreamWriter(saveFileDialog.FileName))
					{
						using (var archive = new ZipArchive(file_write.BaseStream, ZipArchiveMode.Create, true))
						{
							var demoFile = archive.CreateEntry($"{saveFileDialog.FileName.Split("\\").Last()}");

							using (var entryStream = demoFile.Open())
							using (var sw = new StreamWriter(entryStream))
							{
								XmlSerializer xmls = new XmlSerializer(typeof(UpdateInfo));
								xmls.Serialize(sw, UpdateInfo);
							}
						}
					}
				}
			}
		}
		#endregion

		#region DeleteLastVersionInfoCommand: Description
		private ICommand _DeleteLastVersionInfoCommand;
		public ICommand DeleteLastVersionInfoCommand => _DeleteLastVersionInfoCommand ??= new LambdaCommand(OnDeleteLastVersionInfoCommandExecuted, CanDeleteLastVersionInfoCommandExecute);
		private bool CanDeleteLastVersionInfoCommandExecute(object e)
		{
			if (UpdateInfo.LastVersions.Count == 0)
				return false;
			if (VersionInfoType == TypeVersion.Custom)
			{
				if (string.IsNullOrEmpty(VersionInfoCustomType))
					return false;
				return true;
			}
			return true;
		}
		private void OnDeleteLastVersionInfoCommandExecuted(object e)
		{
			UpdateInfo.DeleteLastVersionInfo(VersionInfoType, VersionInfoCustomType);
			GenerateTree();
		}
		#endregion

		#region AddLastVersionInfoCommand: Description
		private ICommand _AddLastVersionInfoCommand;
		public ICommand AddLastVersionInfoCommand => _AddLastVersionInfoCommand ??= new LambdaCommand(OnAddLastVersionInfoCommandExecuted, CanAddLastVersionInfoCommandExecute);
		private bool CanAddLastVersionInfoCommandExecute(object e)
		{
			if (VersionInfoType == TypeVersion.Custom)
			{
				if (string.IsNullOrEmpty(VersionInfoCustomType) == false)
					return true;
				return false;
			}

			if (string.IsNullOrEmpty(VersionInfoVersion) == false)
				return true;

			return false;
		}
		private void OnAddLastVersionInfoCommandExecuted(object e)
		{
			try
			{
				UpdateInfo.SetLastVersion(VersionInfoVersion, VersionInfoType, VersionInfoDate, VersionInfoCustomType);
				GenerateTree();
			}
			catch (Exception ex) { MessageBoxHelper.WarningShow(ex.Message); }
		}
		#endregion
		#region DeleteVersionInfoCommand: Description
		private ICommand _DeleteVersionInfoCommand;
		public ICommand DeleteVersionInfoCommand => _DeleteVersionInfoCommand ??= new LambdaCommand(OnDeleteVersionInfoCommandExecuted, CanDeleteVersionInfoCommandExecute);
		private bool CanDeleteVersionInfoCommandExecute(object e) => true;
		private void OnDeleteVersionInfoCommandExecuted(object e)
		{
			UpdateInfo.DeleteVersionInfo((SelectedItem.Item as VersionInfo));
			GenerateTree();
		}
		#endregion
		#endregion

		private void CreateViewFilesInfo()
		{
			ViewFilesInfo.Clear();
			foreach (var i in NewVersionInfo.Files)
				ViewFilesInfo.Add(i);
		}

		#region Functions
		public List<string> GetAllFilesInFolder(string folderPath)
		{
			List<string> result = new List<string>();
			result.AddRange(Directory.GetFiles(folderPath));
			foreach (string subFolderPath in Directory.GetDirectories(folderPath))
				result.AddRange(GetAllFilesInFolder(subFolderPath));
			return result;
		}
		private void ShowMenu()
		{
			VisibilityNewVersionInfo = SelectedItem != null && SelectedItem.Type == TypeItem.Versions ? Visibility.Visible : Visibility.Collapsed;
			VisibilityAboutVersionInfo = SelectedItem != null && SelectedItem.Type == TypeItem.VersionInfo ? Visibility.Visible : Visibility.Collapsed;
			VisibilityLastVersionInfo = SelectedItem != null && SelectedItem.Type == TypeItem.LastVersionInfo ? Visibility.Visible : Visibility.Collapsed;

			if (VisibilityAboutVersionInfo == Visibility.Visible)
			{
				var info = (VersionInfo)SelectedItem.Item;
				AboutVersionInfoText = $"{info.Type} {info.CustomType} {info.Version} {info.Date}";
			}
		}
		private IEnumerable<VersionItem> GenerateFiles(IEnumerable<UpdaterAPI.Models.VersionInfo> files)
		{
			foreach (var i in files)
			{
				var version = new VersionItem() { Name = $"{i.Version}", Item = i, Type = TypeItem.VersionInfo };

				version.Items.Add(new VersionItem() { Name = $"Дата: {i.Date}", Item = i, Type = TypeItem.VersionInfo });
				version.Items.Add(new VersionItem() { Name = $"Тип: {i.Type}", Item = i, Type = TypeItem.VersionInfo });

				var info = new VersionItem() { Name = $"Файлы", Item = i, Type = TypeItem.FileInfo };
				foreach (var file in i.Files)
				{
					var file_info = new VersionItem() { Name = $"{file.Path}", Item = i, Type = TypeItem.FileInfo };
					file_info.Items.Add(new VersionItem() { Name = $"Url: {file.Url}", Item = i, Type = TypeItem.FileInfo });
					file_info.Items.Add(new VersionItem() { Name = $"Hash: {file.Hash}", Item = i, Type = TypeItem.FileInfo });
					info.Items.Add(file_info);
				}
				version.Items.Add(info);
				yield return version;
			}
		}
		private TreeViewItem CreateTreeViewItem(VersionItem node)
		{
			var item = new TreeViewItem { Header = node.Name, Tag = node };
			if (node.Type != TypeItem.FileInfo)
				item.ExpandSubtree();
			if (node.Items.Count != 0)
			{
				foreach (var child in node.Items)
				{
					item.Items.Add(CreateTreeViewItem(child));
				}
			}
			return item;
		}
		private void GenerateTree()
		{
			TreeViewList.Clear();
			var main = new VersionItem() { Name = "UpdateInfo", Item = UpdateInfo, Type = TypeItem.UpdateInfo };

			var info_last_version = new VersionItem() { Name = "Последнии версии", Type = TypeItem.LastVersionInfo };
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

			var info_Release = new VersionItem() { Name = "Release", Type = TypeItem.Versions };
			foreach (var i in GenerateFiles(UpdateInfo.Versions.Where((i) => i.Type == TypeVersion.Release)))
				info_Release.Items.Add(i);

			var info_Beta = new VersionItem() { Name = "Beta", Type = TypeItem.Versions };
			foreach (var i in GenerateFiles(UpdateInfo.Versions.Where((i) => i.Type == TypeVersion.Beta)))
				info_Beta.Items.Add(i);

			var info_Alpha = new VersionItem() { Name = "Alpha", Type = TypeItem.Versions };
			foreach (var i in GenerateFiles(UpdateInfo.Versions.Where((i) => i.Type == TypeVersion.Alpha)))
				info_Alpha.Items.Add(i);


			var info_Custom = new VersionItem() { Name = $"Custom", Type = TypeItem.Versions };
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

			TreeViewList.Add(CreateTreeViewItem(main));
		}
		#endregion
	}
}
