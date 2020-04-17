using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Button = System.Windows.Controls.Button;
using Label = System.Windows.Controls.Label;
using TextBox = System.Windows.Controls.TextBox;

namespace CrossbellTranslationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void PrepExtract(Object sender, RoutedEventArgs e)
        {
            ((Button)this.FindName("run_extraction")).Visibility = Visibility.Visible;
            ((Button)this.FindName("run_build")).Visibility = Visibility.Hidden;

            ((Label)this.FindName("label1")).Content = "Game directory:";
            ((Label)this.FindName("label2")).Content = "Directory to extract to:";

            ((Grid)this.FindName("paths")).Visibility = Visibility.Visible;
        }

        public void PrepBuild(Object sender, RoutedEventArgs e)
        {
            ((Button)this.FindName("run_extraction")).Visibility = Visibility.Hidden;
            ((Button)this.FindName("run_build")).Visibility = Visibility.Visible;

            ((Label)this.FindName("label1")).Content = "Modified files:";
            ((Label)this.FindName("label2")).Content = "Game directory:";

            ((Grid)this.FindName("paths")).Visibility = Visibility.Visible;
        }

        public void Extract(Object sender, RoutedEventArgs e)
        {
            String path = ((TextBox)this.FindName("path_1")).Text;
            String source = ((TextBox)this.FindName("path_2")).Text;

#if DEBUG
            Actions.Build.Run(path, source);
#else
            try
            {
                if (Directory.Exists(path) && Directory.Exists(source))
                {
                    Actions.Build.Run(path, source);
                }
            }
            catch (Exception ex)
            { 
                ((TextBlock)this.FindName("console")).Text = ex.Message + "\n\n" + ex.StackTrace;
                return;
            }
#endif

            ((TextBlock)this.FindName("console")).Text = "Extraction complete!";
        }

        public void Build(Object sender, RoutedEventArgs e)
        {
            String path = ((TextBox)this.FindName("path_1")).Text;
            String source = ((TextBox)this.FindName("path_2")).Text;

#if DEBUG
            Actions.Build.Run(source, path);
#else
            try
            {
                if (Directory.Exists(path) && Directory.Exists(source))
                {
                    Actions.Build.Run(source, path);
                }
            }
            catch (Exception ex)
            { 
                ((TextBlock)this.FindName("console")).Text = ex.Message + "\n\n" + ex.StackTrace;
                return;
            }
#endif
            ((TextBlock)this.FindName("console")).Text = "Build complete!";
        }

        public void OpenDialog1(Object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.ShowDialog();

            ((TextBox)this.FindName("path_1")).Text = folderDialog.SelectedPath;
            folderDialog.Dispose();
        }

        public void OpenDialog2(Object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.ShowDialog();

            ((TextBox)this.FindName("path_2")).Text = folderDialog.SelectedPath;
            folderDialog.Dispose();
        }
    }
}
