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
                                               "laplace",
                                               "Chroma"
                                           };

        private bool _isDrawing = false;
        private bool _isChromaKey = false;

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
                    _isChromaKey = !_isChromaKey;
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

            if (_isChromaKey)
            {
                tempImg = ChromaKey(tempImg);
            }
            else
            {
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
                    case "laplace":
                        tempImg = laplace(tempImg);
                        break;
                    case "Chroma":
                        tempImg = ChromaKey(tempImg);
                        break;
                    default:
                        break;
                }
            }
            DisplayImage = tempImg;
            _isDrawing = false;

        }

        private Bitmap ChromaKey(Bitmap bit)
        {
            var newImg = new Bitmap(bit.Width, bit.Height);
            var temp = Image.FromFile("greenscreen.jpg");
            var foreground = new Bitmap(temp);

            for (int x = 0; x < bit.Width; ++x)
            {
                for (int y = 0; y < bit.Height; ++y)
                {
                    var backColor = bit.GetPixel(x, y);
                    var foreColor = foreground.GetPixel(x, y);

                    var col = (foreColor.G > (foreColor.R + foreColor.B)) ? backColor : foreColor;

                    newImg.SetPixel(x,y,col);

                }
            }
            return newImg;
            
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

        private Bitmap BlackWhite(Bitmap bit)
        {
            Bitmap ret = new Bitmap(bit.Width, bit.Height);
            for (int x = 0; x < bit.Width; x++)
            {
                for (int y = 0; y < bit.Height; y++)
                {
                    Color c = bit.GetPixel(x, y);
                    int r = (int)((c.R + c.G + c.B) / 3);
                    int g = 0;
                    int b = 0;
                    if (r > 127) { r = 255; g = 255; b = 255; }
                    else { r = 0; g = 0; b = 0; }


                    ret.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return ret;
        }

      /*  private Bitmap BlackWhite(Bitmap bit) {
           
            var tempBit = new Bitmap(bit.Width, bit.Height);
            for (int x = 0; x < bit.Width; ++x)
            {
                for (int y = 0; y < bit.Height; ++y)
                {
                    var color = bit.GetPixel(x, y);
                    var col = ((color.R*0.3 + color.G*0.59 + color.B*0.11) > 0.5f)
                                  ? Color.FromArgb(255, 255, 255)
                                  : Color.FromArgb(0, 0, 0);

                    tempBit.SetPixel(x,y,col);

                }
            }

            return tempBit;
        }*/

        private Bitmap laplace(Bitmap bit)
        {
            //double[] kernel = { -1.0, -1.0, -1.0 ,-1.0, 8.0, -1.0, -1.0, -1.0, -1.0 };
            double r, g, b, z;
            int midX = 1;//kernel.GetLength(1) / 2;
            int midY = 1;//kernel.Length / 2;
            Bitmap ret = new Bitmap(bit.Width, bit.Height);

            for (int y = 1; y < bit.Height - 1; y++)
            {
                for (int x = 1; x < bit.Width - 1; x++)
                {
                    r = 0; g = 0; b = 0;
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            Color col = bit.GetPixel(x + j, y + i);
                            if (((midX + j) * 3) + (midY + i) == 4) { z = 8.0; }
                            else { z = -1.0; }
                            r += col.R * z;
                            g += col.G * z;
                            b += col.B * z;
                        }
                    }
                    if (r > 255) r = 255;
                    else if (r < 0) r = 0;
                    if (g > 255) g = 255;
                    else if (g < 0) g = 0;
                    if (b > 255) b = 255;
                    else if (b < 0) b = 0;
                    ret.SetPixel(x, y, Color.FromArgb((int)r, (int)g, (int)b));
                }
            }
            return ret;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Draw(_techniques[_currentTechnique], _images[_currentImage]);
        }

        
    }
}