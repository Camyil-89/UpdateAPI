using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdaterAPI.Models
{
	public class LastVersionInfo
	{
		public string CustomType { get; set; }
		public string Version { get; set; }
		public DateTime Date { get; set; } = DateTime.Now;
		public TypeVersion Type { get; set; }
		public TypeSystem TypeSystem { get; set; } = TypeSystem.x64;
	}
}
