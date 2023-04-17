using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateAPIHelper.Models
{
    public class Parametrs: Base.ViewModel.BaseViewModel
    {

		#region LastUrl: Description
		/// <summary>Description</summary>
		private string _LastUrl;
		/// <summary>Description</summary>
		public string LastUrl { get => _LastUrl; set => Set(ref _LastUrl, value); }
		#endregion
	}
}
