using System.Collections.Generic;
using System.Xml.Serialization;

namespace PixelSnapper.WPF.Models
{
    public class XmlSettings
    {
        [XmlArray("Hotkeys")]
        [XmlArrayItem("Hotkey")]
        public List<XmlKeyCombination> Hotkeys { get; set; }

        public string DefaultConverter { get; set; }
    }
}
