# UpdateAPI

Пример кода для получения обновлений с (https://github.com/Camyil-89/UpdateAPI-Publish)
```cs
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
```
