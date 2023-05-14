using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UpdateAPIUI.Models
{
	public class WindowStyle: Base.ViewModel.BaseViewModel
	{

		#region Title: Description
		/// <summary>Description</summary>
		private string _Title = "UpdateAPI Installer";
		/// <summary>Description</summary>
		public string Title { get => _Title; set => Set(ref _Title, value); }
		#endregion
	}
}
