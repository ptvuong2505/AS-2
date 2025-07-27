using System;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Repository;

namespace PhanThanhVuongWPF.AdminViewModel
{
    public partial class EditCustomerWindow : Window
    {
        private readonly CustomerRepository customerRepository = new CustomerRepository();
        private readonly Customer originalCustomer;

        public EditCustomerWindow(Customer customer)
        {
            InitializeComponent();
            originalCustomer = customer;

            // Gán dữ liệu lên form
            txtFullName.Text = customer.CustomerFullName;
            txtEmail.Text = customer.EmailAddress;
            txtPhone.Text = customer.Telephone;
            dpBirthday.SelectedDate = customer.CustomerBirthday?.ToDateTime(TimeOnly.MinValue);
            cmbStatus.SelectedIndex = customer.CustomerStatus == 1 ? 0 : 1;
            txtPassword.Password = customer.Password ?? "";
        }

        private void btnSaveCustomer_Click(object sender, RoutedEventArgs e)
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

            // Cập nhật dữ liệu
            originalCustomer.CustomerFullName = fullName;
            originalCustomer.EmailAddress = email;
            originalCustomer.Telephone = phone;
            originalCustomer.CustomerBirthday = DateOnly.FromDateTime(birthday.Value);
            originalCustomer.CustomerStatus = statusText == "Active" ? (byte)1 : (byte)0;
            originalCustomer.Password = password;

            customerRepository.Update(originalCustomer);
            customerRepository.Save();

            MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
