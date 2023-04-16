using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
		public void SetUrlDowloadRoot(string url)
		{
			if (!url.Contains("https://raw.githubusercontent.com/"))
				url = $"https://raw.githubusercontent.com/{url}";

			if (url.Last() != '/')
				url += "/";
			UrlDowloadRoot = url;
		}
		public UpdateInfo GetUpdateInfo()
		{
			using (MemoryStream sw = new MemoryStream(Encoding.UTF8.GetBytes(WebClient.DownloadString(UrlUpdateInfo))))
			{
				XmlSerializer xmls = new XmlSerializer(typeof(UpdateInfo));
				return (UpdateInfo)xmls.Deserialize(sw);
			}
		}
		public LastVersionInfo GetLastVersionInfo(TypeVersion type, string custom_type = null)
		{
			return GetUpdateInfo().GetLastVersion(type, custom_type);
		}
		public VersionInfo GetLastVerison(TypeVersion type, string custom_type = null)
		{
			var info = GetUpdateInfo();
			return info.GetVersion(info.GetLastVersion(type, custom_type).Version, type);
		}
		public IEnumerable<InfoDowload> UpdateFiles(string version, TypeVersion type, string custon_type = null)
		{
			var info = GetUpdateInfo();
			var get_version = info.GetVersion(version, type, custon_type);
			foreach (var i in get_version.Files)
			{
				if (i.Hash != Checksum.GetMD5($"{RootPath}{i.Path}"))
				{
					string path_to_file = $"{RootPath}{i.Path}";
					Console.WriteLine($"{i.Hash} | {Checksum.GetMD5(path_to_file)} | {i.Path} | {i.Url}");
					IsDowloadFile = true;
					InfoDowload.SizeFile = i.Size;
					InfoDowload.Path = path_to_file;
					InfoDowload.Name = new System.IO.FileInfo(path_to_file).Name;
					ExceptionDowload = null;
					Stopwatch.Restart();
					WebClient.DownloadFileAsync(new Uri($"{UrlDowloadRoot}{i.Url}"), path_to_file);
					Console.WriteLine($"{UrlDowloadRoot}{i.Url}");
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
					yield return InfoDowload;
				}
			}
		}

		public void CancelDowload()
		{
			WebClient.CancelAsync();
		}
	}
}
