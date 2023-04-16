using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

		private string GetFileChecksum(string filePath)
		{
			using (var md5 = MD5.Create())
			{
				using (var stream = File.OpenRead(filePath))
				{
					var hash = md5.ComputeHash(stream);
					return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
				}
			}
		}
		public void AddFile(string path, string root_path, string url)
		{
			Files.Add(new FileInfo() { Url = url, Path = path.Replace(root_path, ""), Hash = GetFileChecksum(path) });
		}
	}
}
