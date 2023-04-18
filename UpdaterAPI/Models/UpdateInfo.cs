using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UpdaterAPI.Models
{
	public class UpdateInfo
	{
		public List<LastVersionInfo> LastVersions { get; set; } = new List<LastVersionInfo>();
		public List<VersionInfo> Versions { get; set; } = new List<VersionInfo>();

		public void SetLastVersion(string version, TypeVersion type, DateTime date, string custom_type = null)
		{
			if (Versions.FirstOrDefault((i) => i.Version == version && i.Type == type && i.CustomType == custom_type) == null)
				throw new Exception("Такой версии не существует!");

			var last_version = LastVersions.FirstOrDefault((i) => i.Type == type);
			if (last_version != null)
			{
				last_version.Version = version;
				last_version.Date = date;
				return;
			}
			LastVersions.Add(new LastVersionInfo() { Version = version, Date = date, Type = type, CustomType = custom_type });
		}
		/// <summary>
		/// Добавляет новую версию
		/// </summary>
		/// <param name="version"></param>
		public void AddNewVersion(VersionInfo version)
		{
			if (GetVersion(version.Version, version.Type, version.CustomType) == null)
				Versions.Add(version);
			else
				throw new Exception($"Такая версия уже существует! {version.Version} {version.Type} {version.CustomType}");
		}
		/// <summary>
		/// Получает версию
		/// </summary>
		/// <param name="version">Версия</param>
		/// <param name="type">Тип версии</param>
		/// <param name="custom_type">Кастомный тип версии</param>
		/// <returns></returns>
		public VersionInfo GetVersion(string version, TypeVersion type, string custom_type = null)
		{
			return Versions.FirstOrDefault((i) => i.Type == type && i.CustomType == custom_type && i.Version == version);
		}
		/// <summary>
		/// Получает информацию о последней версии
		/// </summary>
		/// <param name="type">Тип версии</param>
		/// <param name="custom_type">Кастомный тип версии</param>
		/// <returns></returns>
		public LastVersionInfo GetLastVersion(TypeVersion type, string custom_type = null)
		{
			return LastVersions.FirstOrDefault((i) => i.Type == type && i.CustomType == custom_type);
		}

		public static UpdateInfo Create(string data)
		{
			using (MemoryStream sw = new MemoryStream(Encoding.UTF8.GetBytes(data)))
			{
				XmlSerializer xmls = new XmlSerializer(typeof(UpdateInfo));
				return (UpdateInfo)xmls.Deserialize(sw);
			}
		}
	}
}
