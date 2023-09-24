using PixelSnapper.WPF.Converters;
using PixelSnapper.WPF.Models;
using PixelSnapper.WPF.Services;
using PixelSnapper.WPF.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq;

namespace PixelSnapper.WPF.Pages
{
    public partial class ColorFormattingPage : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<BaseColorConverter> ColorConverters { get; set; }
        public DelegateCommand<BaseColorConverter> SetConverterCommand { get; set; }

        private BaseColorConverter selectedConverter;
        public BaseColorConverter SelectedConverter
        {
            get
            {
                return selectedConverter;
            }
            set
            {
                this.selectedConverter = value;
                ServiceLocator.Register(value);
                OnPropertyChanged("SelectedConverter");
            }
        }

        public ColorFormattingPage()
        {
            InitializeComponent();
            SetupConverters();
            SetConverterCommand = new DelegateCommand<BaseColorConverter>(SetConverter);

            var settings = ServiceLocator.Resolve<SettingsService>();
            var toFind = settings.Current.DefaultConverter ?? "Generic Rgb";
            SelectedConverter = ColorConverters.First(s => s.Title == toFind);
            SelectedConverter.Selected = true;
            this.DataContext = this;
        }


        private void SetupConverters()
        {
            ColorConverters = new List<BaseColorConverter>();

            ColorConverters.Add(new GenericRgb());
            ColorConverters.Add(new GenericRgbA());
            ColorConverters.Add(new GenericHex());
            ColorConverters.Add(new GenericHexA());
            ColorConverters.Add(new GenericHsl());
            ColorConverters.Add(new GenericHslA());
        }

        private void SetConverter(BaseColorConverter parameter)
        {
            SelectedConverter.Selected = false;
            SelectedConverter = parameter;
            SelectedConverter.Selected = true;

            var settings = ServiceLocator.Resolve<SettingsService>();
            settings.SetDefaultConverter(parameter.Title);
            settings.Save();
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
