using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
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
			app.Startup += App_Startup; ;

			LoadSettings();
			app.Run();
		}

		private static void App_Startup(object sender, StartupEventArgs e)
		{
			Task.Run(() =>
			{
				try
				{
					UpdaterAPI.GitHub.Downloader downloader = new UpdaterAPI.GitHub.Downloader();
					downloader.SetRootPath(Directory.GetCurrentDirectory());
					downloader.SetUrlUpdateInfo("Camyil-89/UpdateAPI-Publish/main/UpdateInfo.xml");
					downloader.SetUrlDowloadRoot("Camyil-89/UpdateAPI-Publish/main/versions");
					var tmp_path = $"{Directory.GetCurrentDirectory()}\\update";
					var last_version = downloader.GetLastVerison(UpdaterAPI.Models.TypeVersion.Release);

					if (last_version.Version != Settings.Instance.Version && MessageBoxHelper.QuestionShow($"Доступна новая версия {last_version.Version}\nСкачать?") == MessageBoxResult.Yes)
					{
						App.Current.Dispatcher.Invoke(() =>
						{
							var status = UpdateAPIUI.UIProvider.DowloadFilesWithInstaller(downloader, last_version, tmp_path);
							if (status == UpdateAPIUI.Models.StatusDowload.OK)
							{
								downloader.CopyFilesFromTempDirectory(tmp_path, $"taskkill /pid {Process.GetCurrentProcess().Id} &&", $"&& rmdir /s /q \"{Directory.GetCurrentDirectory()}\\update\" && \"{Process.GetCurrentProcess().MainModule.FileName.Split("\\").Last()}\"");
							}
							else
							{
								Directory.Delete(tmp_path, true);
							}
						});
					}
				}
				catch (Exception ex) { Console.WriteLine(ex); }
			});
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
