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
using Microsoft.EntityFrameworkCore.Metadata;
using Service;

namespace PhanThanhVuongWPF
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private int customerID;
        private BookingReservationService bookingReservationService;
        private BookingDetailService bookingDetailService;
        private RoomInformationService roomInformationService;
        private RoomTypeService roomTypeService;
        private CustomerService customerService;
        public CustomerWindow(int id)
        {
            InitializeComponent();
            customerID = id;
            bookingReservationService=new BookingReservationService();
            bookingDetailService=new BookingDetailService();
            roomInformationService = new RoomInformationService();
            roomTypeService = new RoomTypeService();
            customerService = new CustomerService();
            LoadDataBookings();
            loadCustomerData();

        }

        private void LoadDataBookings()
        {
            List<BookingReservation> bookings =bookingReservationService.GetByCustomerId(customerID);
            
            var bookingsforload = bookings.Select( b => new
            {
                b.BookingReservationId,
                BookingDate = b.BookingDate!=null ? b.BookingDate.ToString("dd/MM/yyyy") : "N/A",
                TotalPrice = b.TotalPrice.HasValue ? b.TotalPrice.Value.ToString("C") : "N/A",
                BookingStatus = b.BookingStatus.HasValue ? (b.BookingStatus.Value).ToString() : "N/A",
                BookingDetails= bookingDetailService.GetAll()
                    .Where(bd => bd.BookingReservationId == b.BookingReservationId)
                    .Select(bd => new
                    {

                        RoomNumber = roomInformationService.GetById(bd.RoomId).RoomNumber,
                        RoomType = roomTypeService.GetById(bd.RoomId).RoomTypeName,
                        ActualPrice = bd.ActualPrice,
                        CheckInDate = bd.StartDate.ToString("dd/MM/yyyy"),
                        CheckOutDate = bd.EndDate.ToString("dd/MM/yyyy")
                    })
                    .ToList()


            }).ToList();

            dataGridBookings.ItemsSource = bookingsforload;
        }

        public void loadCustomerData()
        {
            Customer customer = customerService.FindById(customerID);
            Profile.DataContext = customer;
        }

        public void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            
            
            Profile.Visibility=Visibility.Visible;
            dataGridBookings.Visibility = Visibility.Collapsed;
        }
        public void btnCloseProfile_Click(object sender, RoutedEventArgs e)
        {
            
            Profile.Visibility = Visibility.Collapsed;
            dataGridBookings.Visibility = Visibility.Visible;
        }

        public void btnBooking_Click(object sender, RoutedEventArgs e)
        {

            BookingWindow bookingWindow = new BookingWindow(customerID);
            bookingWindow.Show();
           
        }

        private void dataGridBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
             loginWindow.Show();
            this.Close();
           
            
        }

    }
}
