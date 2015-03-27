using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Common.WPF
{
    /// <summary>
    /// Interaction logic for ExceptionMessageBox.xaml
    /// </summary>
    public partial class ExceptionMessageBox : Window, INotifyPropertyChanged
    {
        private string _message = string.Empty;
        private List<string> _additionalMessages = new List<string>();
        private Visibility _additionalInfoVisible = Visibility.Collapsed;
        private BitmapSource _errorImage = null;

        public Exception Exception { get; set; }

        public string Message
        {
            get { return _message; }
            set { ChangeAndNotify(ref _message, value); }
        }

        public List<string> AdditionalMessages
        {
            get { return _additionalMessages; }
            set { ChangeAndNotify(ref _additionalMessages, value); }
        }

        public Visibility AdditionalInfoVisible
        {
            get { return _additionalInfoVisible; }
            set { ChangeAndNotify(ref _additionalInfoVisible, value); }
        }

        public BitmapSource ErrorImage
        {
            get { return _errorImage; }
            set { ChangeAndNotify(ref _errorImage, value); }
        }

        public ExceptionMessageBox(Exception ex, string message, string title)
        {
            InitializeComponent();
            DataContext = this;

            Exception = ex;
            Title = title;
            Message = message;

            if (String.IsNullOrWhiteSpace(Message))
                Message = ex.Message;
            if (String.IsNullOrWhiteSpace(Title))
                Title = Exception.Source;

            if(String.IsNullOrWhiteSpace(message))
                AdditionalMessages = GetAdditionalMessages();

            if (AdditionalMessages.Any())
                AdditionalInfoVisible = Visibility.Visible;

            var error = SystemIcons.Error;
            // You can adjust the parameters of BitmapSizeOptions to suite your specific needs
            var image = Imaging.CreateBitmapSourceFromHIcon(error.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            // icon has been declared as Image
            ErrorImage = image;
        }

        private List<string> GetAdditionalMessages()
        {
            var additionalMessages = new List<string>();

            int count = 0;
            for (var ex = Exception.InnerException; ex != null; ex = ex.InnerException)
            {
                additionalMessages.Add(string.Format("{0}{1}", new string(' ', count * 5), ex.Message));
                count++;
            }
            return additionalMessages;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            var window = new ExceptionDetails(Exception);
            window.ShowDialog();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            
            Clipboard.SetText(Exception.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void ChangeAndNotify<T>(ref T storage, T value,
    [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return;

            storage = value;

            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
