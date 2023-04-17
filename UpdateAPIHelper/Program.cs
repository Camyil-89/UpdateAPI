using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using UpdateAPIHelper.Service;

namespace UpdateAPIHelper
{
	public static class Program
	{
		public static Stopwatch Stopwatch = new Stopwatch();
		[STAThread]
		public static void Main(string[] args)
		{
			Directory.SetCurrentDirectory($"{new FileInfo(Process.GetCurrentProcess().MainModule.FileName).DirectoryName}");
			var app = new App();
			app.InitializeComponent();
			app.Exit += App_Exit;

			LoadSettings();
			app.Run();
		}
		private static void App_Exit(object sender, ExitEventArgs e)
		{
			SaveSettings();
		}

		private static void LoadSettings()
		{
			Settings.Instance = new Settings();
			try
			{
				Settings.Instance.Parametrs = XMLProvider.Load<Models.Parametrs>($"{Directory.GetCurrentDirectory()}\\Settings.xml");
			}
			catch { }
		}
		public static void SaveSettings()
		{
			XMLProvider.Save<Models.Parametrs>($"{Directory.GetCurrentDirectory()}\\Settings.xml", Settings.Instance.Parametrs);
		}
	}
}
