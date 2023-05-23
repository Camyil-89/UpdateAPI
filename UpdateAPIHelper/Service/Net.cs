using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UpdateAPIHelper.Service
{
    public static class Net
    {
		public static string DowloadString(string url)
		{
			try
			{
				return new WebClient().DownloadString(url);
			} catch (Exception ex) { MessageBoxHelper.ErrorShow($"Не удалось скачать данные!", ex); }
			return "";
		}
    }
}
