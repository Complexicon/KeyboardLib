using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.Keys;

namespace KeyboardLib {
	class AsyncKeyboardHook {

		[DllImport("user32.dll")]
		public static extern int GetAsyncKeyState(int i);
		List<Keys> pressed = new List<Keys>();

		Thread t;
		public delegate void KBHandler(KeyHookEventArgs args);
		public event KBHandler KeyHandler;
		bool run = false;

		public AsyncKeyboardHook() {
			run = true;
			t = new Thread(ThreadFunc);
			t.Start();
		}

		public void Unhook() => run = false;

		private void ThreadFunc() {
			while(run) {
				Thread.Sleep(10);
				for(int i = 0x0; i < 0xFF; i++) {
					int state = GetAsyncKeyState(i);
					if(state > 0 && !pressed.Contains((Keys)i)) {
						Keys k =(Keys)i;
						switch(k) {
							case LButton:
							case RButton:
							case MButton:
							case ShiftKey:
							case Keys.Menu:
							case ControlKey:
								continue;
						}
						pressed.Add(k);
						KeyHandler(new KeyHookEventArgs(pressed, k, false));
					} else if(state == 0 && pressed.Contains((Keys)i)) {
						Keys k = (Keys)i;
						KeyHandler(new KeyHookEventArgs(pressed, k, true));
						pressed.RemoveAll(item => item == k);
					}
				}
			}
		}

	}
}
