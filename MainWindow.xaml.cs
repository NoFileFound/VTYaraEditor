using Microsoft.Win32;
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

namespace VTYaraEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LineNumbers.Text = "1";
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLineNumbers();
        }

        private void UpdateLineNumbers()
        {
            LineNumbers.Text = string.Empty;
            var rect = Editor.Document.ContentStart.GetCharacterRect(LogicalDirection.Forward);
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
                Filter = "YARA Files (*.yar;*.yara)|*.yar;*.yara",
                Title = "Open YARA File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Editor.Document.Blocks.Clear();
                Editor.Document.Blocks.Add(new Paragraph(new Run(File.ReadAllText(openFileDialog.FileName))));
                UpdateLineNumbers();
            }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "YARA Files (*.yar;*.yara)|*.yar;*.yara",
                Title = "Save YARA File",
                DefaultExt = ".yar"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd).Text);
            }
        }
    }
}