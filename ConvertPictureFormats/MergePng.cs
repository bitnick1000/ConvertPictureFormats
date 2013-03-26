using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConvertPictureFormats
{
    public static class ColorExtension
    {
        public static Color Grayscale(this Color self)
        {
            int gray = (self.R + self.G + self.B) / 3;
            // curColor.R * 0.299 + curColor.G * 0.587 + curColor.B * 0.114);
            return Color.FromArgb(self.A, gray, gray, gray);
        }
    }
    public static class BitmapListExtension
    {
        public static List<Color> GetPixel(this List<Bitmap> self, int x, int y)
        {
            List<Color> colorList = new List<Color>();
            foreach (Bitmap bitmap in self)
            {
                colorList.Add(bitmap.GetPixel(x, y));
            }
            return colorList;
        }
    }
    public static class BitmapExtension
    {
        public static void Grayscale(this Bitmap self)
        {
            for (int y = 0; y < self.Height; y++)
            {
                for (int x = 0; x < self.Width; x++)
                {
                    self.SetPixel(x, y, self.GetPixel(x, y).Grayscale());
                }
            }
        }
    }
    class MergePng
    {
        public List<string> filenames = new List<string>();
        public List<Bitmap> bitmaps = new List<Bitmap>();
        int width;
        int height;
        Bitmap merged;
        public void AddFileName(string fileName)
        {
            filenames.Add(fileName);
        }
        public void Save(string fileName)
        {
            merged.Grayscale();
            merged.Save(fileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
        }
        public void Merge()
        {
            foreach (string filename in filenames)
            {
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(filename);
                bitmaps.Add(bitmap);
            }
            CheckSize();
            merged = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Color color = MergeColor(bitmaps.GetPixel(x, y));
                    merged.SetPixel(x, y, color);
                }
        }
        Color MergeColor(List<Color> colorList)
        {
            Color color = colorList[0];
            foreach (Color c in colorList)
            {
                if (!IsColorLike(c, color, 5))
                    return Color.FromArgb(0, 0, 0, 0);
            }
            return color;
        }
        bool IsColorLike(Color color1, Color color2, int range)
        {
            if (Math.Abs(color1.Grayscale().R - color2.Grayscale().R) < range)
                return true;
            return false;
        }
        void CheckSize()
        {
            this.width = bitmaps[0].Width;
            this.height = bitmaps[0].Height;
            foreach (Bitmap bitmap in bitmaps)
            {
                if (width != bitmap.Width || height != bitmap.Height)
                {
                    MessageBox.Show("not the same size");
                    throw new Exception();
                }
            }
        }
    }
}
