using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.IO;
using System.Drawing.Imaging;

namespace WpfApplication1
{
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly string[] _images = new string[]
                                       {
                                           @"bathtub.jpg",
                                           @"knights.jpg",
                                           @"vampire.jpg",
                                           @"Batman Edge Detect Test.jpg"
                                       };

        private readonly string[] _techniques = new string[]
                                           {
                                               "normal",
                                               "greyscale",
                                               "blackandwhite",
                                               "laplace"
                                           };

        private bool _isDrawing = false;

        private int _currentTechnique = 0;
        private int _currentImage = 0;

        private Bitmap _image;

        public Bitmap DisplayImage
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged("DisplayImage");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        private new void KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                    
                    case Key.Up:
                    if(!_isDrawing)
                        _currentTechnique = (_currentTechnique + _techniques.Length - 1)%_techniques.Length;
                    break;
                    case Key.Down:
                    if (!_isDrawing)
                        _currentTechnique = (_currentTechnique + _techniques.Length + 1) % _techniques.Length;
                    break;
                    case Key.Left:
                    if (!_isDrawing)
                        _currentImage = (_currentImage + _images.Length - 1) % _images.Length;
                    break;
                    case Key.Right:
                    if (!_isDrawing)
                        _currentImage = (_currentImage + _images.Length + 1) % _images.Length;
                    break;
                    case Key.C:
                    break;
                    case Key.Escape:
                        this.Close();
                    break;
            }

            Draw(_techniques[_currentTechnique], _images[_currentImage]);
        }

        public Window1()
        {
            InitializeComponent();

            EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyUpEvent, new KeyEventHandler(KeyUp), true);
            MainImage.DataContext = this;
       }

        private void Draw(string tech, string img)
        {
            _isDrawing = true;
            var image = Image.FromFile(img);
            var tempImg = new Bitmap(img);

            switch (tech)
            {
                case "normal":
                    break;
                case "greyscale":
                    tempImg = GreyScale(tempImg);
                    break;
                case "blackandwhite":
                    tempImg = BlackWhite(tempImg);
                    break;
                default:
                    break;
            }
            DisplayImage = tempImg;
            _isDrawing = false;

        }

        private Bitmap GreyScale(Bitmap bit) {

            var temp = new Bitmap(bit.Width, bit.Height);

            for (int x = 0; x < bit.Width; ++x)
            {
                for (int y = 0; y < bit.Height; ++y)
                {
                   var color = bit.GetPixel(x, y);

                    int luma = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    
                    temp.SetPixel(x, y, Color.FromArgb(luma, luma, luma));
                }
            }
            return temp;
        }

        private Bitmap BlackWhite(Bitmap bit) {
           
            var tempBit = new Bitmap(bit.Width, bit.Height);
            for (int x = 0; x < bit.Width; ++x)
            {
                for (int y = 0; y < bit.Height; ++y)
                {
                    var color = bit.GetPixel(x, y);
                    if (color.R + color.G + color.B > 127)
                    {
                        tempBit.SetPixel(x, y, Color.FromArgb(255,255,255));
                    }
                    else {

                        tempBit.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                   
                }
            }

            return tempBit;
        }

        private uint[] laPlace(Bitmap bit)
        {
            int height = bit.Height;
            int width = bit.Width;

            uint[] pixels = new uint[width * height];

            int red;
            int green;
            int blue;
            int alpha;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    int i = width * y + x;
                    System.Drawing.Color color = bit.GetPixel(x, y);
                    red = (int)(color.R);
                    green = (int)(color.G);
                    blue = (int)(color.B);
                    alpha = color.A;
                    if (color.R + color.G + color.B > 127)
                    {
                        red = 255;
                        green = 255;
                        blue = 255;
                        alpha = color.A;
                        pixels[i] = (uint)((blue << 24) + (green << 16) + (red << 8) + alpha);
                    }
                    else
                    {
                        red = 0;
                        green = 0;
                        blue = 0;
                        alpha = color.A;
                        pixels[i] = (uint)((blue << 24) + (green << 16) + (red << 8) + alpha);
                    }

                }
            }
            return pixels;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Draw(_techniques[_currentTechnique], _images[_currentImage]);
        }

        
    }
}