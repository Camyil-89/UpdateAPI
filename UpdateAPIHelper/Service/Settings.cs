using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateAPIHelper.Models;

namespace UpdateAPIHelper.Service
{
    public class Settings: Base.ViewModel.BaseViewModel
    {
		public static Settings Instance;


		#region Parametrs: Description
		/// <summary>Description</summary>
		private Parametrs _Parametrs = new Parametrs();
		/// <summary>Description</summary>
		public Parametrs Parametrs { get => _Parametrs; set => Set(ref _Parametrs, value); }
		#endregion
	}
}
