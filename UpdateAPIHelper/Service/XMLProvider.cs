using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UpdateAPIHelper.Service
{
	public class XMLProvider
	{
		public static bool CompressZip = true;
		public static void Save<T>(string path, object obj)
		{
			if (CompressZip)
			{
				using (StreamWriter file_write = new StreamWriter(path))
				{
					using (var archive = new ZipArchive(file_write.BaseStream, ZipArchiveMode.Create, true))
					{
						var demoFile = archive.CreateEntry($"{path.Split("\\").Last()}");

						using (var entryStream = demoFile.Open())
						using (var sw = new StreamWriter(entryStream))
						{
							XmlSerializer xmls = new XmlSerializer(typeof(T));
							xmls.Serialize(sw, obj);
						}
					}
				}
				return;
			}
			using (StreamWriter sw = new StreamWriter(path))
			{
				XmlSerializer xmls = new XmlSerializer(typeof(T));
				xmls.Serialize(sw, obj);
			}
		}
		public static T Load<T>(string filename)
		{
			try
			{
				using (var archive = ZipFile.OpenRead(filename))
				{
					var file = archive.GetEntry($"{filename.Split('\\').Last()}");
					using (var stream = file.Open())
					{
						XmlSerializer xmls = new XmlSerializer(typeof(T));
						var x = (T)xmls.Deserialize(stream);
						return x;
					}
				}
			}
			catch { }

			using (StreamReader sw = new StreamReader(filename))
			{
				XmlSerializer xmls = new XmlSerializer(typeof(T));
				var x = (T)xmls.Deserialize(sw);
				return x;
			}
		}
	}
}
