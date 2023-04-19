using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateAPIHelper.Language
{
	public class LangProvider : Base.ViewModel.BaseViewModel
	{

		public static Dictionary<string, string> Language { get; set; } = new Dictionary<string, string>()
		{
			{"DowloadFromUrlBtn", "Скачать" },
			{"ResetBtn", "Сбросить" },
			{"UseThisBtn", "Использовать" },
			{"UpdateFileMenu", "Файл с обновлениями" },
			{"VersionsInfoMenu", "Информация о версиях" },
		};

		public static ObservableCollection<string> LanguagesAvailable { get; set; } = new ObservableCollection<string>();



		#region Test: Description
		/// <summary>Description</summary>
		private string _Test = "test";
		/// <summary>Description</summary>
		public string Test { get => _Test; set => Set(ref _Test, value); }
		#endregion

	}
}
