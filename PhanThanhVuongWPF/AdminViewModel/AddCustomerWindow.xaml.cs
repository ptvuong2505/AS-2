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
using BusinessObjects;
using Repository;
using Service;

namespace PhanThanhVuongWPF.AdminViewModel
{
    /// <summary>
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        private readonly CustomerService customerService = new CustomerService();
        private readonly CustomerRepository customerRepository = new CustomerRepository();

        public AddCustomerWindow()
        {
            InitializeComponent();
            cmbStatus.SelectedIndex = 0;
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            var birthday = dpBirthday.SelectedDate;
            string statusText = (cmbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || birthday == null || string.IsNullOrEmpty(statusText) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra email đã tồn tại chưa
            var existed = customerService.GetAll().Exists(c => c.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (existed)
            {
                MessageBox.Show("Email đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newCustomer = new Customer
            {
                CustomerFullName = fullName,
                EmailAddress = email,
                Telephone = phone,
                CustomerBirthday = DateOnly.FromDateTime(birthday.Value),
                CustomerStatus = statusText == "Active" ? (byte)1 : (byte)0,
                Password = password
            };

            customerRepository.Add(newCustomer);
            customerRepository.Save();

            MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
           
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
