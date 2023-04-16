using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdaterAPI.GitHub
{
	public class InfoDowload
	{
		public long SpeedDowload { get; set; }
		public long TotalDowload { get; set; }
		public long SizeFile { get; set; }
		public string Path { get; set; }
		public string Name { get; set; }
	}
}
