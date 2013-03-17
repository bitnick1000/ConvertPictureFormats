using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConvertPictureFormats
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var folderBrowser = new CommonOpenFileDialog();
            folderBrowser.IsFolderPicker = true;
            folderBrowser.Title = "Select folder";
            folderBrowser.InitialDirectory = System.Environment.CurrentDirectory;
            var result = folderBrowser.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string filename = folderBrowser.FileName;
                List<FileInfo> files = GetFiles(filename, ".bmp");
                foreach (FileInfo f in files)
                {
                    BitmapFormat(f.FullName);
                }
            }
            folderBrowser.Dispose();
            MessageBox.Show("Done!");
            this.Close();
        }
        void BitmapFormat(string path)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(path);
            if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                return;
            System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            for (int y = 0; y < newBitmap.Height; y++)
                for (int x = 0; x < newBitmap.Width; x++)
                    newBitmap.SetPixel(x, y, bitmap.GetPixel(x, y));
            bitmap.Dispose();
            if (File.Exists(path))
                File.Delete(path);
            newBitmap.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
            newBitmap.Dispose();
        }
        List<FileInfo> GetFiles(string path, string extension)
        {
            List<FileInfo> files = new List<FileInfo>();
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (FileInfo f in directory.GetFiles())
            {
                if (f.Extension.ToLower() == extension.ToLower())
                    files.Add(f);
            }
            foreach (DirectoryInfo d in directory.GetDirectories())
            {
                files.AddRange(GetFiles(d.FullName, extension));
            }
            return files;
        }
    }
}
