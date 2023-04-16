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
		public string CustomType { get; set; }
		public TypeVersion Type { get; set; } = TypeVersion.Release;
		public string Version { get; set; }
		public DateTime Date { get; set; } = DateTime.Now;

		public List<FileInfo> Files { get; set; } = new List<FileInfo>();
		public void AddFile(string path, string root_path, string url)
		{
			var FileInfo = new FileInfo();
			FileInfo.Path = path;
			FileInfo.Url = url;
			FileInfo.Hash = Checksum.GetMD5(path);
			FileInfo.Size = new System.IO.FileInfo(path).Length;
			Files.Add(FileInfo);
		}
	}
}
