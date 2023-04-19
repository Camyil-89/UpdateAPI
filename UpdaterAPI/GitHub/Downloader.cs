using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UpdaterAPI.Models;
using UpdaterAPI.Service;

namespace UpdaterAPI.GitHub
{
	public class Downloader
	{
		private WebClient WebClient = new WebClient();
		private string UrlUpdateInfo = "";
		private string UrlDowloadRoot = "";

		private string RootPath = "";

		private Exception ExceptionDowload;

		private bool IsDowloadFile = false;
		private Stopwatch Stopwatch = new Stopwatch();
		private bool IsCancel = false;
		private int BlockTimeout = 50;

		private List<string> DowloadFiles = new List<string>();
		private InfoDowload InfoDowload = new InfoDowload();

		/// <summary>
		/// Если репозиторий приватный
		/// </summary>
		/// <param name="token"></param>
		public void SetToken(string token)
		{
			WebClient = new WebClient();
			WebClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");
			WebClient.Headers.Add(HttpRequestHeader.Authorization, $"token {token}");
			WebClient.Headers.Add(HttpRequestHeader.Accept, "application/octet-stream");
			Init();
		}
		private void Init()
		{
			WebClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
			WebClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
		}

		private void WebClient_DownloadFileCompleted(object? sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			ExceptionDowload = e.Error;
			IsDowloadFile = false;
			Stopwatch.Stop();
		}

		private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			InfoDowload.SpeedDowload = (long)((double)e.BytesReceived / Stopwatch.Elapsed.TotalSeconds);
			InfoDowload.PercentageDowload = e.ProgressPercentage;
			InfoDowload.TotalDowload = e.BytesReceived;
			InfoDowload.SizeFile = e.TotalBytesToReceive;
		}

		/// <summary>
		/// url до файла на github в котором лежит xml
		/// </summary>
		/// <param name="url"></param>
		public void SetUrlUpdateInfo(string url)
		{
			if (!url.Contains("https://raw.githubusercontent.com/"))
				url = $"https://raw.githubusercontent.com/{url}";
			UrlUpdateInfo = url;
			Init();
		}
		/// <summary>
		/// путь к приложению которое будет обновляться
		/// </summary>
		/// <param name="path"></param>
		public void SetRootPath(string path)
		{
			RootPath = path;
		}
		/// <summary>
		/// Путь до папки на гитхабе где лежат все версии.
		/// </summary>
		/// <param name="url"></param>
		public void SetUrlDowloadRoot(string url)
		{
			if (!url.Contains("https://raw.githubusercontent.com/"))
				url = $"https://raw.githubusercontent.com/{url}";

			if (url.Last() != '/')
				url += "/";
			UrlDowloadRoot = url;
		}
		/// <summary>
		/// Получает информацию об версиях
		/// </summary>
		/// <returns></returns>
		public UpdateInfo GetUpdateInfo()
		{
			try
			{
				using (var archive = new ZipArchive(new MemoryStream(WebClient.DownloadData(UrlUpdateInfo))))
				{
					var file = archive.GetEntry($"{UrlUpdateInfo.Split('/').Last()}");
					using (var stream = file.Open())
					{
						XmlSerializer xmls = new XmlSerializer(typeof(UpdateInfo));
						var x = (UpdateInfo)xmls.Deserialize(stream);
						return x;
					}
				}
			}
			catch { }

			using (MemoryStream sw = new MemoryStream(Encoding.UTF8.GetBytes(WebClient.DownloadString(UrlUpdateInfo))))
			{
				XmlSerializer xmls = new XmlSerializer(typeof(UpdateInfo));
				return (UpdateInfo)xmls.Deserialize(sw);
			}
		}
		/// <summary>
		/// Получает информацию о последней версии выбранного типа
		/// </summary>
		/// <param name="type"></param>
		/// <param name="custom_type"></param>
		/// <returns></returns>
		public LastVersionInfo GetLastVersionInfo(TypeVersion type, string custom_type = null)
		{
			return GetUpdateInfo().GetLastVersion(type, custom_type);
		}
		/// <summary>
		/// Получает последнию версию программы выбранного типа
		/// </summary>
		/// <param name="type"></param>
		/// <param name="custom_type"></param>
		/// <returns></returns>
		public VersionInfo GetLastVerison(TypeVersion type, string custom_type = null)
		{
			var info = GetUpdateInfo();
			return info.GetVersion(info.GetLastVersion(type, custom_type).Version, type);
		}
		/// <summary>
		/// Скачивает файлы с гитхаба
		/// </summary>
		/// <param name="version">Версия которую необходимо скачать</param>
		/// <param name="type">Тип версии которую качаем</param>
		/// <param name="path_tmp_folder">Временная папка куда скачиваются все файлы, она нужно чтобы в случаи ошибки при скачивании ее удалить.</param>
		/// <param name="custon_type">Кастомный тип версии</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public IEnumerable<InfoDowload> UpdateFiles(string version, TypeVersion type, string path_tmp_folder, string custon_type = null)
		{
			DowloadFiles.Clear();
			Directory.CreateDirectory(path_tmp_folder);
			var info = GetUpdateInfo();
			var get_version = info.GetVersion(version, type, custon_type);
			foreach (var i in get_version.Files)
			{
				if (!File.Exists($"{RootPath}{i.Path}") || i.Hash != Checksum.GetMD5($"{RootPath}{i.Path}"))
				{
					string path_to_file = $"{path_tmp_folder}{i.Path}";
					Directory.CreateDirectory(Path.GetDirectoryName(path_to_file));
					IsDowloadFile = true;
					InfoDowload.IsDowload = false;
					InfoDowload.SizeFile = i.Size;
					InfoDowload.Path = path_to_file;
					InfoDowload.Name = new System.IO.FileInfo(path_to_file).Name;
					ExceptionDowload = null;
					Stopwatch.Restart();
					WebClient.DownloadFileAsync(new Uri($"{UrlDowloadRoot}{i.Url}"), path_to_file);
					while (IsDowloadFile)
					{
						Thread.Sleep(BlockTimeout);
						if (IsCancel)
							break;
						yield return InfoDowload;
					}

					if (IsCancel)
						break;
					if (ExceptionDowload != null)
					{
						throw new Exception($"Error dowload: {ExceptionDowload}");
					}
					InfoDowload.IsDowload = true;
					DowloadFiles.Add(path_to_file);
					yield return InfoDowload;
				}
			}
		}
		/// <summary>
		/// Копирует файлы с временной папки. Копируется вся структура в этой папке.
		/// </summary>
		/// <param name="path_folder_update">Путь до временной папки</param>
		/// <param name="before_command">Команды которые выполняться в cmd до копирования. В начале обязательно нужно написать "&&"</param>
		/// <param name="after_command">Команды которые выполняться в cmd после копирования. В начале обязательно нужно написать "&&"</param>
		public void CopyFilesFromTempDirectory(string path_folder_update, string before_command = "", string after_command = "")
		{
			Process cmd = new Process();
			cmd.StartInfo.FileName = "cmd.exe";
			cmd.StartInfo.Arguments = $"/c {before_command} xcopy /y /s \"{path_folder_update}\" \"{RootPath}\" {after_command}";
			cmd.StartInfo.CreateNoWindow = true;
			cmd.Start();
		}
		/// <summary>
		/// Отменяет скачивания
		/// </summary>
		public void CancelDowload()
		{
			WebClient.CancelAsync();
		}
	}
}
