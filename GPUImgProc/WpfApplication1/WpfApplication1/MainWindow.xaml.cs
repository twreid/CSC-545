using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageEditing
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            int width = 300;
            int height = 300;

            // Create a writeable bitmap (which is a valid WPF Image Source
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            Bitmap bit = new Bitmap("knights");
            // Create an array of pixels to contain pixel color values
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

                    red = 0;
                    green = 255 * y / height;
                    blue = 255 * (width - x) / width;
                    alpha = 255;

                    pixels[i] = (uint)((blue << 24) + (green << 16) + (red << 8) + alpha);
                }
            }

            // apply pixels to bitmap
            bitmap.WritePixels(new Int32Rect(0, 0, 300, 300), pixels, width * 4, 0);

            // set image source to the new bitmap
            this.MainImage.Source = bitmap;
        }

        private uint[] greyScale(Bitmap bit) {
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
                    red = (int)(color.R * 0.30);
                    green = (int)(color.G * 0.59);
                    blue = (int)(color.B * 0.11);
                    alpha = color.A;

                    pixels[i] = (uint)((blue << 24) + (green << 16) + (red << 8) + alpha);
                }
            }
            return pixels;
        }

        private uint[] bw(Bitmap bit) {
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
                    else { 
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
    }
}