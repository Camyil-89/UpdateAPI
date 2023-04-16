using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UpdaterAPI.GitHub
{
	public static class Downloader
	{
		private static WebClient WebClient = new WebClient();


		public static void SetToken(string token)
		{
			WebClient WebClient = new WebClient();
			WebClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");
			WebClient.Headers.Add(HttpRequestHeader.Authorization, $"token {token}");
			WebClient.Headers.Add(HttpRequestHeader.Accept, "application/octet-stream");
		}

		public static string GetLastVersion()
		{
			return "";
		}
	}
}
