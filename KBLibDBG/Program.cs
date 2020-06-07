using KeyboardLib;
using System;
using System.IO;
using System.Windows.Forms;

namespace KBLibDBG {
	class Program {

		static KeyboardHook hook;
		static TranslationTable table;

		static void Main(string[] args) {
			hook = new KeyboardHook();
			hook.multipleCalls = false;
			table = new TranslationTable();
			hook.KeyHandler += table.TranslationHandler;
			hook.KeyHandler += Hook_KeyHandler;
			table.Stringer += Handler;
			Application.Run();
			hook.Unhook();
		}

		private static void Hook_KeyHandler(KeyHookEventArgs args) {
			if(!args.KeyUpEvent) Console.WriteLine(args.Key.ToString());
		}

		public static void Handler(String s) {
			var w = File.AppendText("log.txt");
			w.Write(s);
			w.Close();
		}

	}
}
