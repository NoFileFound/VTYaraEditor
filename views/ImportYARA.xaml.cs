using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;

namespace VTYaraEditor.views
{
    /// <summary>
    /// Interaction logic for ImportYARA.xaml
    /// </summary>
    public partial class ImportYARA : Window
    {
        public ImportYARA()
        {
            InitializeComponent();
        }

        private async void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            string url = $"https://www.virustotal.com/api/v3/yara_rules/{idImportVTID.Text}";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-apikey", "API_KEY");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseBody);
                    JToken ruleToken = json.SelectToken("data.attributes.rule");
                    if (ruleToken != null)
                    {
                        MainWindow window = new MainWindow();
                        window.Editor.Document.Blocks.Clear();
                        window.Editor.Document.Blocks.Add(new Paragraph(new Run(ruleToken.ToString())));
                        window.Show();
                        window.UpdateLineNumbers();
                    }
                }
                else
                {
                    MainWindow window = new MainWindow();
                    window.Editor.Document.Blocks.Clear();
                    window.Show();
                    MessageBox.Show("Your api key is expired or you are not athorized to fetch from virustotal", "Notice", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                Close();
            }
        }
    }
}
