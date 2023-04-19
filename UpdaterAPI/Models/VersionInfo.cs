using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UpdaterAPI.Service;

namespace UpdaterAPI.Models
{
	public enum TypeVersion : byte
	{
		Release = 0,
		Beta = 1,
		Alpha = 2,
		Custom = 3,
	}
	public class VersionInfo
	{
		private string _CustomType = null;
		public string CustomType
		{
			get => _CustomType; set
			{
				if (value == "")
					value = null;
				_CustomType = value;
			}
		}
		public TypeVersion Type { get; set; } = TypeVersion.Release;
		public string Version { get; set; }
		public DateTime Date { get; set; } = DateTime.Now;

		public List<FileInfo> Files { get; set; } = new List<FileInfo>();

		/// <summary>
		/// Добавляет файл в версию
		/// </summary>
		/// <param name="path">Путь до файла</param>
		/// <param name="local_path">Локальный путь в проекте (приложении)</param>
		/// <param name="url">Условный путь на гитхабе относительно UrlDowloadRoot. ОБЯЗАТЕЛЬНО УЧИТЫВАТЬ РЕГИСТР</param>
		public void AddFile(string path, string local_path, string url)
		{
			if (local_path.First() != '\\')
				local_path = $"\\{local_path}";
			var FileInfo = new FileInfo();
			FileInfo.Path = local_path;
			FileInfo.Url = url;
			FileInfo.Hash = Checksum.GetMD5(path);
			FileInfo.Size = new System.IO.FileInfo(path).Length;
			Files.Add(FileInfo);
		}
	}
}
