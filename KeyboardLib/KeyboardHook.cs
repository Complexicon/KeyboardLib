using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyboardLib {
	public class KeyboardHook {

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, int wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		private const int WH_KEYBOARD_LL = 13;

		private const int WM_KEYDOWN = 0x0100;
		private const int KEYUP = 0x1;
		private const int SYSKEY = 0x4;

		private IntPtr _hookID = IntPtr.Zero;

		private static LowLevelKeyboardProc callback;
		private delegate IntPtr LowLevelKeyboardProc(int nCode, int wParam, IntPtr lParam);

		public delegate void KBHandler(KeyHookEventArgs args);
		public event KBHandler KeyHandler;
		public bool multipleCalls = true;

		private List<Keys> KeysDown = new List<Keys>();

		private IntPtr HookCallback(int nCode, int wParam, IntPtr lParam) {

			if(nCode >= 0) {
				Keys k = (Keys)Marshal.ReadInt32(lParam);
				if(wParam == WM_KEYDOWN || wParam == WM_KEYDOWN + SYSKEY) {
					if(!KeysDown.Contains(k)) {
						KeysDown.Add(k);
						KeyHandler(new KeyHookEventArgs(KeysDown, k, false));
					} else if(multipleCalls){
						KeyHandler(new KeyHookEventArgs(KeysDown, k, false));
					}
				}
				if(wParam == WM_KEYDOWN + KEYUP || wParam == WM_KEYDOWN + SYSKEY + KEYUP) {
					KeysDown.RemoveAll(item => item == k);
					KeyHandler(new KeyHookEventArgs(KeysDown, k, true));
				}
			}

			return CallNextHookEx(_hookID, nCode, wParam, lParam);

		}

		private IntPtr SetHook() => SetWindowsHookEx(WH_KEYBOARD_LL, callback, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);

		public KeyboardHook() {
			callback = HookCallback;
			_hookID = SetHook();
		}

		public void Unhook() => UnhookWindowsHookEx(_hookID);

	}
}
