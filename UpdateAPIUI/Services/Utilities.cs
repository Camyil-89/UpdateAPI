using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateAPIUI.Models;

namespace UpdateAPIUI.Services
{
    public static class Utilities
    {
		public static string RoundByte(long Bytes, Language lang)
		{
			if (Bytes < Math.Pow(1024, 1)) return $"{Bytes} {lang.ByteText}";
			else if (Bytes < Math.Pow(1024, 2)) return $"{Math.Round((float)Bytes / 1024, 2)} {lang.KbyteText}";
			else if (Bytes < Math.Pow(1024, 3)) return $"{Math.Round((float)Bytes / Math.Pow(1024, 2), 2)} {lang.MByte}";
			else if (Bytes < Math.Pow(1024, 4)) return $"{Math.Round((float)Bytes / Math.Pow(1024, 3), 2)} {lang.GByte}";
			return "";
		}
	}
}
