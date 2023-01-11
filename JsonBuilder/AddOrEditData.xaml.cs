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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using Xceed.Wpf.Toolkit;

namespace JsonBuilder
{
    /// <summary>
    /// Interaction logic for DataEntry
    /// </summary>
    public partial class AddOrEditData : Window
    {
        MainWindow mw;
        public List<Card> lstCard = new List<Card>();
        public bool flag = false;
        bool flgFullCardNumber = false;
        public AddOrEditData(MainWindow mw, bool flg)
        {
            InitializeComponent();
            this.mw = mw;
            this.flag = flg;
            btnCardAddOrEdit.IsEnabled = false;
            btnCardDelete.IsEnabled = false;
            listCardAdd.ItemsSource = lstCard;
            if (flag == false)
            {
                btnSave.IsEnabled = false;
                txtFIO.LostFocus += Calendar1_LostFocus;
                calendar1.LostFocus += Calendar1_LostFocus;
                rbtnMan.LostFocus += Calendar1_LostFocus;
                rbtnWoman.LostFocus += Calendar1_LostFocus;
                txtCntChild.LostFocus += Calendar1_LostFocus;
                cmbBox1.LostFocus += Calendar1_LostFocus;
                txtInputCard.LostFocus += Calendar1_LostFocus;
                txtInputDateCard.LostFocus += Calendar1_LostFocus;
            }
            else
            {
                Edit();
            }
        }
        object line;
        private void Edit() // Method for displaying editable data
        {
            line = mw.personList1.SelectedItems[0];
            txtFIO.Text = (line as Person).FIO;
            calendar1.SelectedDate = (line as Person).DateOfBirth;
            bool male = (line as Person).Male;
            if (male == true)
                rbtnMan.IsChecked = true;
            else if (male == false)
                rbtnWoman.IsChecked = true;
            cmbBox1.Text = (line as Person).MaritalStatus;
            txtCntChild.Text = Convert.ToString((line as Person).CountOfChildren);
            lstCard = (line as Person).ListCard;
            listCardAdd.ItemsSource = lstCard;
            listCardAdd.Items.Refresh();
        }
        private void calendar1_SelectedDatesChanged(object sender, SelectionChangedEventArgs e) // Method where a date is selected in the calendar
        {
            DateTime? selectedDate = calendar1.SelectedDate;
        }
        private void Calendar1_LostFocus(object sender, RoutedEventArgs e) // A method where access is given to save data when focus is lost if all data is entered
        {
            if (txtFIO.Text != null && calendar1.SelectedDate != null && (rbtnMan.IsChecked == true || rbtnWoman.IsChecked == true) &&
                cmbBox1.Text != null && lstCard.Count != 0)
                btnSave.IsEnabled = true;
            else
                btnSave.IsEnabled = false;
        }
        private int GetID() // Method for determining the auto-increment 
        {
            return mw.prs1.Count() > 0 ? mw.prs1.Max<Person>(x => x.ID) + 1 : 1;
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e) // Button for save the entered data
        {
            if (flag == false)
            {
                Person p = new Person(GetID(), txtFIO.Text, (DateTime)calendar1.SelectedDate, rbtnMan.IsChecked.Value, cmbBox1.Text,
                                      txtCntChild.Text.Trim().Length == 0 ? null : Convert.ToInt32(txtCntChild.Text), lstCard);
                mw.prs1.Add(p);
                mw.personList1.Items.Refresh();
            }
            else
            {
                int ID = (mw.personList1.SelectedItems[0] as Person).ID;
                (line as Person).ID = ID;
                (line as Person).FIO = txtFIO.Text;
                (line as Person).DateOfBirth = (DateTime)calendar1.SelectedDate;
                (line as Person).Male = rbtnMan.IsChecked.Value;
                (line as Person).MaritalStatus = cmbBox1.Text;
                (line as Person).CountOfChildren = String.IsNullOrWhiteSpace(txtCntChild.Text) ? null : Convert.ToInt32(txtCntChild.Text);
                (line as Person).ListCard = lstCard;
                Person p = mw.prs1.Find(x => x.ID == ID);
                p = line as Person;
                mw.personList1.Items.Refresh();
            }
            txtFIO.Clear();
            txtCntChild.Clear();
            calendar1.SelectedDate = null;
            cmbBox1.Items.Clear();
            rbtnMan.IsChecked = false;
            rbtnWoman.IsChecked = false;
            txtInputCard.Clear();
            txtInputDateCard.Clear();
            Close();
        }
        private void RadioButtonMan_Checked(object sender, RoutedEventArgs e) // Radio button that allows you to select a male gender
        {
            if (rbtnMan.IsChecked == true && rbtnWoman.IsChecked == false)
                ManMaritalStatus();
            else
                WomanMaritalStatus();

        }
        private void RadioButtonWoman_Checked(object sender, RoutedEventArgs e) // Radio button that allows you to select a female gender
        {
            if (rbtnWoman.IsChecked == true)
                WomanMaritalStatus();
            else
                ManMaritalStatus();
        }
        private void ManMaritalStatus() // Method for choosing the status (man)
        {
            cmbBox1.Items.Clear();
            cmbBox1.Items.Add("Single");
            cmbBox1.Items.Add("Married");
            cmbBox1.Items.Add("Divorced");
        }
        private void WomanMaritalStatus() // Method for choosing the status (woman)
        {
            cmbBox1.Items.Clear();
            cmbBox1.Items.Add("Married");
            cmbBox1.Items.Add("Not married");
            cmbBox1.Items.Add("Divorced");
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e) // Method for closing the form
        {
            Close();
        }
        private void txtCntChild_LostFocus(object sender, RoutedEventArgs e) // Method for entering the count of children 
        {
            int count;
            string vvod = txtCntChild.Text;
            if (!String.IsNullOrWhiteSpace(vvod) && !int.TryParse(vvod, out count))
            {
                txtCntChild.BorderBrush = new SolidColorBrush(Colors.Red);
                lbError.Visibility = Visibility.Visible;
                btnSave.IsEnabled = false;
            }
            else
            {
                txtCntChild.BorderBrush = new SolidColorBrush(Colors.White);
                lbError.Visibility = Visibility.Hidden;
                btnSave.IsEnabled = true;
            }
        }
        private void btnCardAddOrEdit_Click(object sender, RoutedEventArgs e) // Method for adding or editing a card
        {
            Card card = new Card(long.Parse(txtInputCard.Text.Replace(" ", "")), txtInputDateCard.Text);
            lstCard.Add(card);
            listCardAdd.Items.Refresh();
            txtInputCard.Clear();
            txtInputDateCard.Clear();
            btnCardAddOrEdit.IsEnabled = false;
        }
        private void btnCardDelete_Click(object sender, RoutedEventArgs e) // Method for deleting the card
        {
            if (listCardAdd.Items.Count != 0)
            {
                var res = System.Windows.MessageBox.Show("Do you really want to delete this card number?",
                                          "Delete number", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    var line = listCardAdd.SelectedItems[0];
                    lstCard.Remove((Card)line);
                    listCardAdd.Items.Refresh();
                }
            }
        }
        private void txtInputCard_LostFocus(object sender, RoutedEventArgs e) // Method for entering the card number
        {
            long number;
            string inputCard = txtInputCard.Text.Replace(" ", "");
            if (String.IsNullOrWhiteSpace(inputCard) || inputCard.Length < 16 || !long.TryParse(inputCard, out number))
            {
                txtInputCard.BorderBrush = new SolidColorBrush(Colors.Red);
                lbErrorCard.Visibility = Visibility.Visible;
                btnCardAddOrEdit.IsEnabled = false;                
            }
            else
            {
                txtInputCard.BorderBrush = new SolidColorBrush(Colors.White);
                lbErrorCard.Visibility = Visibility.Hidden;
                flgFullCardNumber = true;
            }
        }
        private void listCardAdd_SelectionChanged(object sender, SelectionChangedEventArgs e) // Method for removing a card from the list (if the card was selected)
        {
            if (listCardAdd.SelectedItem != null)
                btnCardDelete.IsEnabled = true;
            else
                btnCardDelete.IsEnabled = false;
        }
        private void txtInputDateCard_LostFocus(object sender, RoutedEventArgs e) // Method for entering the card expiration date
        {
            int date;
            string inputDateCard = txtInputDateCard.Text.Replace("/","");
            if (!int.TryParse(inputDateCard, out date) || date > 1299) // 12/99 (12 months, 99-th year max)
            {
                txtInputDateCard.BorderBrush = new SolidColorBrush(Colors.Red);
                lbErrorDate.Visibility = Visibility.Visible;
                btnCardAddOrEdit.IsEnabled = false;
            }
            else
            {
                txtInputDateCard.BorderBrush = new SolidColorBrush(Colors.White);
                lbErrorDate.Visibility = Visibility.Hidden;
                if (flgFullCardNumber == true)
                    btnCardAddOrEdit.IsEnabled = true;
            }
        }
    }
}
