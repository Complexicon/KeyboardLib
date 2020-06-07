using System.Windows.Forms;

namespace KeyboardLib {
	class TranslationTable {

		string keymap = "de";

		public delegate void StringCatcher(string s);
		public event StringCatcher Stringer;

		public void TranslationHandler(KeyHookEventArgs args) {
			if(!args.KeyUpEvent) {
				bool bigChar = (args.IsKeyDown(Keys.LShiftKey) || args.IsKeyDown(Keys.RShiftKey)) ^ Control.IsKeyLocked(Keys.CapsLock);
				bool altGr = (args.IsKeyDown(Keys.LControlKey) || args.IsKeyDown(Keys.RControlKey)) && (args.IsKeyDown(Keys.LMenu) || args.IsKeyDown(Keys.RMenu));
				string dat = "";
				if(keymap == "de") {
					dat = TranslateDE(args.Key,bigChar,altGr);
				} else {
					//TODO
				}
				Stringer(dat);
			}
		}

		public static string TranslateDE(Keys key, bool bigChar, bool altGr) {
			switch(key) {
				case Keys.Return:
					return " <Return> ";
				case Keys.RWin:
				case Keys.LWin:
					return " <Win> ";
				case Keys.Capital:
				case Keys.RShiftKey:
				case Keys.LShiftKey:
					return "";
				case Keys.Space:
					return " ";
				case Keys.Tab:
					return " <TAB> ";
				case Keys.Back:
					return " <Rem> ";
				case Keys.OemPeriod:
					return bigChar ? ":" : ".";
				case Keys.Oemcomma:
					return bigChar ? ";" : ".";
				case Keys.OemMinus:
					return bigChar ? "_" : "-";
				case Keys.OemBackslash:
					if(altGr) return "|";
					return bigChar ? ">" : "<";
				case Keys.OemQuestion:
					return bigChar ? "'" : "#";
				case Keys.Oemplus:
					if(altGr) return "~";
					return bigChar ? "*" : "+";
				case Keys.D1:
					return bigChar ? "!" : "1";
				case Keys.D2:
					return bigChar ? '"' + "" : "2";
				case Keys.D3:
					return bigChar ? "§" : "3";
				case Keys.D4:
					return bigChar ? "$" : "4";
				case Keys.D5:
					return bigChar ? "%" : "5";					
				case Keys.D6:
					return bigChar ? "&" : "6";					
				case Keys.D7:
					if(altGr) return "{";
					return bigChar ? "/" : "7";					
				case Keys.D8:
					if(altGr) return "[";
					return bigChar ? "(" : "8";					
				case Keys.D9:
					if(altGr) return "]";
					return bigChar ? ")" : "9";					
				case Keys.D0:
					if(altGr) return "}";
					return bigChar ? "=" : "0";					
				case Keys.OemOpenBrackets:
					if(altGr) return @"\";
					return bigChar ? "?" : "ß";					
				case Keys.Oem6:
					return bigChar ? "`" : "´";
				case Keys.Oem5:
					return bigChar ? "°" : "^";
				case Keys.RControlKey:
				case Keys.LControlKey:
					return " <CTRL> ";
				case Keys.LMenu:
				case Keys.RMenu:
					return " <ALT> ";
				default:
					if(key.ToString().ToLower().StartsWith("numpad")) return key.ToString().Substring(6);
					return bigChar ? key.ToString().ToUpper() : key.ToString().ToLower();
			}
		}

	}

}
