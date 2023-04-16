using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdaterAPI.Models
{
	public class UpdateInfo
	{
		public List<LastVersionInfo> LastVersions { get; set; } = new List<LastVersionInfo>();
		public List<VersionInfo> Versions { get; set; } = new List<VersionInfo>();

		public void SetLastVersion(string version, TypeVersion type, DateTime date, string custom_type = null)
		{
			var last_version = LastVersions.FirstOrDefault((i) => i.Type == type);
			if (last_version != null)
			{
				last_version.Version = version;
				last_version.Date = date;
				return;
			}
			LastVersions.Add(new LastVersionInfo() { Version = version, Date = date, Type = type, CustomType = custom_type });
		}
		public void AddNewVersion(VersionInfo version)
		{
			Versions.Add(version);
		}
		public VersionInfo GetVersion(string version, TypeVersion type)
		{
			return Versions.FirstOrDefault((i) => i.Type == type && i.Version == version);
		}
		public LastVersionInfo GetLastVersion(TypeVersion type, string custom_type = null)
		{
			return LastVersions.FirstOrDefault((i) => i.Type == type && i.CustomType == custom_type);
		}
	}
}
