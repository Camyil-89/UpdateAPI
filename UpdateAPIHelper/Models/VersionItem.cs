using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateAPIHelper.Models
{
	public enum TypeItem: byte
	{
		UpdateInfo = 0,
		LastVersionInfo = 1,
		FileInfo = 2,
		VersionInfo = 3,
		Info = 4,
		Versions = 5,
	}
    public class VersionItem: Base.ViewModel.BaseViewModel
    {

		#region Type: Description
		/// <summary>Description</summary>
		private TypeItem _Type;
		/// <summary>Description</summary>
		public TypeItem Type { get => _Type; set => Set(ref _Type, value); }
		#endregion


		#region Name: Description
		/// <summary>Description</summary>
		private string _Name;
		/// <summary>Description</summary>
		public string Name { get => _Name; set => Set(ref _Name, value); }
		#endregion


		#region Item: Description
		/// <summary>Description</summary>
		private object _Item;
		/// <summary>Description</summary>
		public object Item { get => _Item; set => Set(ref _Item, value); }
		#endregion


		#region Items: Description
		/// <summary>Description</summary>
		private ObservableCollection<VersionItem> _Items = new ObservableCollection<VersionItem>();
		/// <summary>Description</summary>
		public ObservableCollection<VersionItem> Items { get => _Items; set => Set(ref _Items, value); }
		#endregion
	}
}
