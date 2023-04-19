using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateAPIUI.Models
{
    public class Language: Base.ViewModel.BaseViewModel
    {

		#region CancelButton: Description
		/// <summary>Description</summary>
		private string _CancelButton = "Отменить";
		/// <summary>Description</summary>
		public string CancelButton { get => _CancelButton; set => Set(ref _CancelButton, value); }
		#endregion

		#region ByteText: Description
		/// <summary>Description</summary>
		private string _ByteText = "Байт";
		/// <summary>Description</summary>
		public string ByteText { get => _ByteText; set => Set(ref _ByteText, value); }
		#endregion


		#region KbyteText: Description
		/// <summary>Description</summary>
		private string _KbyteText = "КБайт";
		/// <summary>Description</summary>
		public string KbyteText { get => _KbyteText; set => Set(ref _KbyteText, value); }
		#endregion


		#region MByte: Description
		/// <summary>Description</summary>
		private string _MByte = "Мбайт";
		/// <summary>Description</summary>
		public string MByte { get => _MByte; set => Set(ref _MByte, value); }
		#endregion


		#region GByte: Description
		/// <summary>Description</summary>
		private string _GByte = "ГБайт";
		/// <summary>Description</summary>
		public string GByte { get => _GByte; set => Set(ref _GByte, value); }
		#endregion


		#region PerSecondText: Description
		/// <summary>Description</summary>
		private string _PerSecondText = "сек.";
		/// <summary>Description</summary>
		public string PerSecondText { get => _PerSecondText; set => Set(ref _PerSecondText, value); }
		#endregion


		#region FileText: Description
		/// <summary>Description</summary>
		private string _FileText = "файл";
		/// <summary>Description</summary>
		public string FileText { get => _FileText; set => Set(ref _FileText, value); }
		#endregion
	}
}
