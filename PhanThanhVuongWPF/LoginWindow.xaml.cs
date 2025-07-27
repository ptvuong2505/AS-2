using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace PhanThanhVuongWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        private readonly FuminiHotelManagementContext _context;
        private readonly CustomerService customerService=new CustomerService();
        public LoginWindow()
        {
            InitializeComponent();
            testfuction();
        }

        public void btnLogin_Click(object sender, RoutedEventArgs e)
        {
           testfuction();
           AppSettingsReader.display();
            var admin = new
            {
                username = AppSettingsReader.GetAccount( "Username"),
                password = AppSettingsReader.GetAccount( "Password")
            };
            Debug.WriteLine($"Admin Username: {admin.username}, Password: {admin.password}");
            if (txtUser.Text== admin.username & txtPass.Password==admin.password)
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                this.Close();
                return;
            }
            else
            {
                foreach (Customer customer in customerService.GetAll())
                {
                    if (customer.EmailAddress == txtUser.Text && customer.Password == txtPass.Password)
                    {
                        CustomerWindow customerWindow = new CustomerWindow(customer.CustomerId);
                        customerWindow.Show();

                        this.Close();
                        return;
                    }

                }
            }
                
            MessageBox.Show("Sai Email or Password !!!");
        }

        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void testfuction()
        {
            BookingReservationService bookingReservationService = new BookingReservationService();
            List<BookingReservation> bookingReservations = bookingReservationService.GetByCustomerId(3);
            foreach (BookingReservation bookingReservation in bookingReservations)
            {
                Debug.WriteLine($"Booking ID: {bookingReservation.BookingReservationId}, Customer ID: {bookingReservation.CustomerId}");
            }
        }

        
    }
}
