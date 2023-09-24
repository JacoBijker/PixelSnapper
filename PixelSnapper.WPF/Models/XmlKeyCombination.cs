using PixelSnapper.Hotkey;

namespace PixelSnapper.WPF.Models
{
    public class XmlKeyCombination : KeyCombination
    {
        public string Tag { get; set; }

        public XmlKeyCombination()
            : base()
        { }

        public XmlKeyCombination(KeyCombination keyCombo, string tag)
            : base(keyCombo.Key, keyCombo.Win, keyCombo.Ctrl, keyCombo.Alt, keyCombo.Shift)
        {
            this.Tag = tag;
        }

        public KeyCombination GetHotkey()
        {
            return new KeyCombination(Key, Win, Ctrl, Alt, Shift);
        }
    }
}
