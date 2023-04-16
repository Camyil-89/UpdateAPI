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

namespace UpdaterAPI.GitHub
{
	public static class Downloader
	{
		private static WebClient WebClient = new WebClient();

		/// <summary>
		/// Если репозиторий приватный
		/// </summary>
		/// <param name="token"></param>
		public static void SetToken(string token)
		{
			WebClient WebClient = new WebClient();
			WebClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");
			WebClient.Headers.Add(HttpRequestHeader.Authorization, $"token {token}");
			WebClient.Headers.Add(HttpRequestHeader.Accept, "application/octet-stream");
		}
		public static UpdateInfo GetUpdateInfo(string url)
		{
			UpdateInfo info = new UpdateInfo();
			using (StreamWriter sw = new StreamWriter(WebClient.DownloadString(url)))
			{
				XmlSerializer xmls = new XmlSerializer(typeof(UpdateInfo));
				xmls.Serialize(sw, info);
			}
			return info;
		}
		public static LastVersionInfo GetLastVersion(string url, TypeVersion type, string custom_type = null)
		{
			return GetUpdateInfo(url).GetLastVersion(type, custom_type);
		}
	}
}
