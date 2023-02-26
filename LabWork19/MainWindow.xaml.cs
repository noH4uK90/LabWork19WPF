using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Forms;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace LabWork19
{
    public partial class MainWindow : Window
    {
        private string _directoryName = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateArchiveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            
            var saveDialog = new SaveFileDialog
            {
                Filter = "Zip (.zip)|*.zip"
            };
            if (saveDialog.ShowDialog() != true) return;

            _directoryName = folderBrowserDialog.SelectedPath;
            var destination = saveDialog.FileName;
            
            ZipFile.CreateFromDirectory(_directoryName, destination);
        }

        private void UnzipButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Zip (.zip)|*.zip"
            };
            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            var source = openFileDialog.FileName;
            var destination = folderBrowserDialog.SelectedPath;

            ZipFile.ExtractToDirectory(source, destination);
        }

        private void OpenArchiveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Zip (.zip)|*.zip"
            };
            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            var archive = ZipFile.OpenRead(openFileDialog.FileName);
            FilesListBox.ItemsSource = archive.Entries;
        }

        private void AddToArchiveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Zip (.zip)|*.zip"
            };
            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            var addOpenFileDialog = new OpenFileDialog();
            if (addOpenFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            var fileInfo = new FileInfo(addOpenFileDialog.FileName);

            using var archive = ZipFile.Open(openFileDialog.FileName, ZipArchiveMode.Update);
            archive.CreateEntryFromFile(fileInfo.FullName, fileInfo.Name);
        }
    }
}