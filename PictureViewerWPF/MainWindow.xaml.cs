using System;
using System.Collections.Generic;
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

namespace PictureViewerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected Microsoft.Win32.OpenFileDialog openDialog;

        public MainWindow()
        {
            InitializeComponent();

            openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.FileName = "image";
            openDialog.DefaultExt = "jpg";
            openDialog.Filter = "JPEG Files (*.jpg)|*.jpg;*.jpeg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|All files (*.*)|*.*";
            openDialog.Title = "Select a picture file";
 
            image.Stretch = Stretch.None;
        }

        private void showButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool) openDialog.ShowDialog())
            {
                image.Source = new BitmapImage(new Uri(openDialog.FileName));
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            image.Source = null;
        }

        private void backgroundButton_Click(object sender, RoutedEventArgs e)
        {
            // Not implemented in default WPF
            // Apparently needs an additional library to actually work

            // WinForms code:
            //if (colorDialog1.ShowDialog() == DialogResult.OK)
            //    image.BackColor = colorDialog1.Color;

            // ?
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void checkBox_Changed(object sender, RoutedEventArgs e)
        {
            if ((bool) checkBox.IsChecked)
                image.Stretch = Stretch.Uniform;
            else
                image.Stretch = Stretch.None;
        }
    }
}
