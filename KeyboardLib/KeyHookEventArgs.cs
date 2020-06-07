using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyboardLib {
	public class KeyHookEventArgs {

		private List<Keys> KeysDown;
		public readonly bool KeyUpEvent;
		public readonly Keys Key;

		public KeyHookEventArgs(List<Keys> KeysDown, Keys key, bool keyup) {
			this.KeysDown = KeysDown;
			Key = key;
			KeyUpEvent = keyup;
		}

		public bool IsKeyDown(Keys k) => KeysDown.Contains(k);

	}
}
