using IronOcr;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OcrJapanese
{
    public partial class MainWindow : Window
    {
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                FilePathTextBlock.Text = value;
            }
        }

        public BitmapImage Bitmap;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ReadCroppedImage(int x, int y, int width, int height, OcrLanguage language, TextBox textBox)
        {
            var CropArea = new Rectangle(x, y, width, height);

            var advancedOcr = new AdvancedOcr()
            {
                CleanBackgroundNoise = true,
                EnhanceContrast = true,
                EnhanceResolution = true,
                Strategy = AdvancedOcr.OcrStrategy.Advanced,
                ColorSpace = AdvancedOcr.OcrColorSpace.Color,
                DetectWhiteTextOnDarkBackgrounds = true,
                InputImageType = AdvancedOcr.InputTypes.AutoDetect,
                RotateAndStraighten = true,
                ReadBarCodes = false,
                ColorDepth = 4
            };

            if (language == OcrLanguage.English)
                advancedOcr.Language = IronOcr.Languages.English.OcrLanguagePack;

            if (language == OcrLanguage.Japanese)
                advancedOcr.Language = IronOcr.Languages.Japanese.OcrLanguagePack;

            var results = advancedOcr.Read(FilePath, CropArea);

            textBox.Text = results.Text;
        }

        private void GetFile()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                Bitmap = new BitmapImage();
                Bitmap.BeginInit();
                Bitmap.UriSource = new Uri(FilePath);
                Bitmap.EndInit();
            }
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            GetFile();
        }

        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {
            var bitmapWindow = new BitmapWindow();
            bitmapWindow.Init(Bitmap);
            bitmapWindow.Show();
        }

        private void ButtonScan_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FilePath))
                return;

            ReadCroppedImage(77, 74, 216, 216, OcrLanguage.Japanese, TextBlockMainKanji);

            ReadCroppedImage(547, 73, 1207, 76, OcrLanguage.English, TextBlockEnglish);
            ReadCroppedImage(560, 209, 1216, 76, OcrLanguage.Japanese, TextBlockJapanese);

            ReadCroppedImage(89, 334, 100, 41, OcrLanguage.Japanese, TextBlockSub1);
            ReadCroppedImage(205, 334, 335, 64, OcrLanguage.English, TextBlockSub2);

            ReadCroppedImage(1060, 444, 98, 54, OcrLanguage.Japanese, TextBlockTranslate1Japanese);
            ReadCroppedImage(1220, 443, 250, 50, OcrLanguage.Japanese, TextBlockTranslate1AJapanese);
            ReadCroppedImage(1463, 449, 250, 50, OcrLanguage.English, TextBlockTranslate1English);
        }
    }
}