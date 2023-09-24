using System.Windows.Forms;
using System.Linq;
using System;

namespace PixelSnapper.Hotkey
{
    public class KeyCombination : IEquatable<KeyCombination>
    {
        protected static Keys[] modifierKeys = new Keys[] { Keys.LWin, Keys.RWin, Keys.LControlKey, Keys.RControlKey, Keys.LShiftKey, Keys.RShiftKey, Keys.Alt };

        public Keys Key { get; set; }
        public bool Win { get; set; }
        public bool Ctrl { get; set; }
        public bool Shift { get; set; }
        public bool Alt { get; set; }

        public KeyCombination()
        { }

        public KeyCombination(Keys key, bool win, bool ctrl, bool alt, bool shift)
        {
            this.Key = key;
            this.Win = win;
            this.Ctrl = ctrl;
            this.Shift = shift;
            this.Alt = alt;
        }



        public override string ToString()
        {
            string modifiers = string.Empty;
            modifiers += (Win ? "Win + " : "");
            modifiers += (Ctrl ? "Ctrl + " : "");
            modifiers += (Shift ? "Shift + " : "");
            modifiers += (Alt ? "Alt + " : "");

            if (modifierKeys.Contains(Key))
                return modifiers;

            return modifiers + Key.ToString();
        }

        public bool Equals(KeyCombination other)
        {
            return this.Key == other.Key && this.Win == other.Win && this.Ctrl == other.Ctrl && this.Alt == other.Alt && this.Shift == other.Shift;
        }
    }
}
