using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace UpdaterAPI.GitHub
{
	public enum TypeInfoDowload: byte
	{
		DowloadFile = 0,
		DowloadFileFinished = 2,
		FinishedDownloading = 1,
		AbortDowload = 3,
	}
	public class InfoDowload
	{
		public long SpeedDowload { get; set; }
		public long TotalDowload { get; set; }
		public long SizeFile { get; set; }
		public string Path { get; set; }
		public string Name { get; set; }
		public double PercentageDowload { get; set; }
		public TypeInfoDowload Type { get; set; } = TypeInfoDowload.DowloadFile;
		public int FilesCount { get; set; }
		public int NowFileIndex { get; set; }
	}
}
