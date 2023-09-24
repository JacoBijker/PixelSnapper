using System.ComponentModel;
using System.Drawing;

namespace PixelSnapper.WPF.Converters
{
    public abstract class BaseColorConverter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public abstract string Title { get; }
        public string Example => Convert(System.Drawing.Color.FromArgb(120, 60, 240));

        private bool selected;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
            }
        }

        public abstract string Convert(System.Drawing.Color color);

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
