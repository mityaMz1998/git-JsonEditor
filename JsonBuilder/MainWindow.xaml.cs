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
using System.IO;
using System.Windows.Diagnostics;
using System.Diagnostics;
using Microsoft.Win32;
using WindowsFormsApp2;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;

namespace JsonBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AddOrEditData de;
        public List<Person> prs1 = new List<Person>();
        public MainWindow()
        {
            InitializeComponent();
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            personList1.ItemsSource = prs1;
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            de = new AddOrEditData(this, false);
            de.Owner = this;
            de.ShowDialog();
        }       
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog() { Filter = "JSON|*.json" };
            saveFile.ShowDialog();
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string jsonString = JsonSerializer.Serialize(prs1, options);
            File.WriteAllText(saveFile.FileName, jsonString);
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog() { Filter = "JSON|*.json" };
            openFile.ShowDialog();
            string json = File.ReadAllText(openFile.FileName);
            prs1 = JsonSerializer.Deserialize<List<Person>>(json);
            personList1.ItemsSource = prs1;
        }
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (personList1.Items.Count != 0)
            {
                de = new AddOrEditData(this, true);
                de.Owner = this;
                de.ShowDialog();
            }           
        }
        private void personList1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (personList1.SelectedItem != null)
            {
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnEdit.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (personList1.Items.Count != 0)
            {
                var res = MessageBox.Show("Do you really want to delete this person?", 
                                          "Delete object", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (res == MessageBoxResult.Yes)
                {                    
                    var line = personList1.SelectedItems[0];
                    prs1.Remove(line as Person);
                    personList1.Items.Refresh();
                }
            }
        }
    }
}