using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace UpdateAPIHelper.Service
{
	public class MessageBoxHelper
	{
		public static void BallonTip(string text, int timeout = 2000, System.Windows.Forms.ToolTipIcon tipIcon = System.Windows.Forms.ToolTipIcon.Info)
		{
			Task.Run(() =>
			{
				var notifyIcon = new System.Windows.Forms.NotifyIcon();
				notifyIcon.Visible = true;
				notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
				notifyIcon.ShowBalloonTip(timeout, "UpdateAPIHelper", text, tipIcon);
				Thread.Sleep(timeout);
				notifyIcon.Dispose();
			});
		}
		/// <summary>
		/// Окно сообщения содержит символ, состоящий из восклицательного знака в треугольнике
		///     с желтым фоном.
		/// </summary>
		/// <param name="text"></param>
		public static void WarningShow(string text)
		{
			MessageBox.Show(text, "UpdateAPIHelper Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
		/// <summary>
		/// Окно сообщения содержит символ, состоящий из белого X в кружке с красным фоном.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="er"></param>
		public static void ErrorShow(string text, Exception er)
		{
			MessageBox.Show($"{text}\n\n{er}", "UpdateAPIHelper Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		/// <summary>
		/// Окно сообщения содержит символ, состоящий из белого X в кружке с красным фоном.
		/// </summary>
		/// <param name="text"></param>
		public static void ErrorShow(string text)
		{
			MessageBox.Show(text, "UpdateAPIHelper Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		/// <summary>
		/// Окно сообщения содержит символ, состоящий из строчной буквы в кружке.
		/// </summary>
		/// <param name="text"></param>
		public static void InfoShow(string text)
		{
			MessageBox.Show(text, "UpdateAPIHelper Информация", MessageBoxButton.OK, MessageBoxImage.Information);
		}
		/// <summary>
		/// Окно сообщения содержит символ, состоящий из строчной буквы в кружке.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="title"></param>
		public static void InfoShow(string text, string title)
		{
			MessageBox.Show(text, $"UpdateAPIHelper {title}", MessageBoxButton.OK, MessageBoxImage.Information);
		}
		/// <summary>
		/// Окно сообщения содержит символ, состоящий из восклицательного знака в треугольнике
		///     с желтым фоном.
		/// </summary>
		/// <param name="text"></param>
		public static void ExclamationShow(string text)
		{
			MessageBox.Show(text, "UpdateAPIHelper Информация", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		/// <summary>
		/// Окно сообщения содержит символ, состоящий из вопросительного знака в кружке.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static MessageBoxResult QuestionShow(string text)
		{
			return MessageBox.Show(text, "UpdateAPIHelper Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
		}
		/// <summary>
		/// Окно сообщения содержит символ, состоящий из вопросительного знака в кружке.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static MessageBoxResult QuestionShowTopMost(string text)
		{
			return MessageBox.Show(text, "UpdateAPIHelper Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
		}

	}
}
