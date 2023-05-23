using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdaterAPI.Models
{
	public class FileInfo
	{
		public string Path { get; set; }
		public string Url { get; set; }
		public string Hash { get; set; }
		public long Size { get; set; }
	}
}
