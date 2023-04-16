using System;
using System.Collections.Generic;
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
		private string RootPath = "";
		/// <summary>
		/// Если репозиторий приватный
		/// </summary>
		/// <param name="token"></param>
		public void SetToken(string token)
		{
			WebClient WebClient = new WebClient();
			WebClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");
			WebClient.Headers.Add(HttpRequestHeader.Authorization, $"token {token}");
			WebClient.Headers.Add(HttpRequestHeader.Accept, "application/octet-stream");
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
		}
		/// <summary>
		/// путь к приложению которое будет обновляться
		/// </summary>
		/// <param name="path"></param>
		public void SetRootPath(string path)
		{
			RootPath = path;
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
		public void UpdateFiles(string version, TypeVersion type, string custon_type = null)
		{
			var info = GetUpdateInfo();
			var get_version = info.GetVersion(version, type, custon_type);
			foreach (var i in get_version.Files)
			{
				if (i.Hash != Checksum.GetMD5($"{RootPath}{i.Path}"))
					Console.WriteLine($"{i.Hash} | {Checksum.GetMD5($"{RootPath}{i.Path}")} | {i.Path} | {i.Url}");
			}
		}
	}
}
