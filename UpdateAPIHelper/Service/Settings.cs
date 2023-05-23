using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UpdateAPIHelper.Models;

namespace UpdateAPIHelper.Service
{
    public class Settings: Base.ViewModel.BaseViewModel
    {
		public static Settings Instance;

		#region Version: версия приложения
		/// <summary>версия приложения</summary>
		private string _Version = Assembly.GetEntryAssembly().GetName().Version.ToString().Replace(".0.0", "");
		/// <summary>версия приложения</summary>
		public string Version { get => _Version; set => Set(ref _Version, value); }
		#endregion
		#region Parametrs: Description
		/// <summary>Description</summary>
		private Parametrs _Parametrs = new Parametrs();
		/// <summary>Description</summary>
		public Parametrs Parametrs { get => _Parametrs; set => Set(ref _Parametrs, value); }
		#endregion
	}
}
