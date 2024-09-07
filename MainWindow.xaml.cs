using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VTYaraEditor.views;

namespace VTYaraEditor
{
    public partial class MainWindow : Window
    {
        private ImportYARA? importYaraWindow;

        public MainWindow()
        {
            InitializeComponent();
            LineNumbers.Text = "1";
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(clearTextItemName != null) 
                clearTextItemName.IsEnabled = (getEditorText().Length > 0);

            if(saveFileItemName != null)
                saveFileItemName.IsEnabled = (getEditorText().Length > 0);

            UpdateLineNumbers();
        }

        public void UpdateLineNumbers()
        {
            LineNumbers.Text = string.Empty;
            double lineHeight = Editor.FontSize * 1.2;
            double contentHeight = Editor.ExtentHeight;
            int totalLines = (int)(contentHeight / lineHeight);
            for (int i = 1; i <= totalLines; i++)
            {
                LineNumbers.Text += i + Environment.NewLine;
            }
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Yara Rules (*.yar;*.yara)|*.yar;*.yara",
                Title = "Open YARA File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Editor.Document.Blocks.Clear();
                Editor.Document.Blocks.Add(new Paragraph(new Run(File.ReadAllText(openFileDialog.FileName))));
                //UpdateLineNumbers();
                /// FixMe: Line count does not appear when open a file.
            }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Yara Rules (*.yar;*.yara)|*.yar;*.yara",
                Title = "Save YARA File",
                DefaultExt = ".yar",
                FileName = "Untitled01.yara"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, getEditorText());
            }
        }

        private void ExitMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ImportVTYaraButton_Click(Object sender, RoutedEventArgs e)
        {
            if(importYaraWindow == null)
            {
                importYaraWindow = new ImportYARA();
                importYaraWindow.Closed += (s, args) => importYaraWindow = null;
                importYaraWindow.Show();
                Close();
            }
        }

        private void ClearTextButton_Click(Object sender, RoutedEventArgs e)
        {
            if(Editor != null)
                Editor.Document.Blocks.Clear();
        }


        private String getEditorText()
        {
            if (Editor != null)
            {
                TextRange textRange = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
                return textRange.Text;
            }
            return "";
        }
    }
}