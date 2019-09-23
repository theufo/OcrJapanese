using System.Windows;
using System.Windows.Media.Imaging;

namespace OcrJapanese
{
    public partial class BitmapWindow : Window
    {
        public BitmapWindow()
        {
            InitializeComponent();
        }

        public void Init(BitmapImage bitmapImage)
        {
            ImageViewer1.Source = bitmapImage;
        }
    }
}