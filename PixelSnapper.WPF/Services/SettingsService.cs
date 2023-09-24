using PixelSnapper.Hotkey;
using PixelSnapper.WPF.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System;
using System.Text;

namespace PixelSnapper.WPF.Services
{
    public class SettingsService
    {
        private XmlSettings currentSettings;

        public XmlSettings Current
        {
            get
            {
                return currentSettings;
            }
        }

        public bool IsFirstRun { get; set; }

        public SettingsService()
        {
            this.currentSettings = new XmlSettings();
            this.currentSettings.Hotkeys = new List<XmlKeyCombination>();
        }

        public void Load()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(XmlSettings));

                using (var reader = new StringReader(Properties.Settings.Default["XML"].ToString()))
                {
                    currentSettings = (XmlSettings)serializer.Deserialize(reader);
                }
            }
            catch (System.Exception)
            {
                //Settings file won't exist in the first run.
                IsFirstRun = true;
            }

            AppendDefaultHotkeys();
        }

        private void AppendDefaultHotkeys()
        {
            if (!currentSettings.Hotkeys.Any(s => s.Tag == "StartCapture"))
                currentSettings.Hotkeys.Add(new XmlKeyCombination(new KeyCombination(System.Windows.Forms.Keys.C, true, false, false, false), "StartCapture"));

            if (!currentSettings.Hotkeys.Any(s => s.Tag == "CancelCapture"))
                currentSettings.Hotkeys.Add(new XmlKeyCombination(new KeyCombination(System.Windows.Forms.Keys.Escape, false, false, false, false), "CancelCapture"));
        }


        public void Save()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(XmlSettings));
                var sb = new StringBuilder();

                using (var SW = new StringWriter(sb))
                    serializer.Serialize(SW, currentSettings);

                Properties.Settings.Default["XML"] = sb.ToString();
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error updating configuration file. This can be fixed by running PixelSnapper as administrator");
            }
        }

        public void SetHokeys(Dictionary<string, KeyCombination> toSave)
        {
            currentSettings.Hotkeys = new List<XmlKeyCombination>();
            foreach (var item in toSave)
            {
                currentSettings.Hotkeys.Add(new XmlKeyCombination(item.Value, item.Key));
            }
        }

        internal void SetDefaultConverter(string converterTitle)
        {
            currentSettings.DefaultConverter = converterTitle;
        }
    }
}
