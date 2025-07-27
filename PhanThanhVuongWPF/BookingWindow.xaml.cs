using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Printing;
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
using Microsoft.EntityFrameworkCore.Infrastructure;
using Service;

namespace PhanThanhVuongWPF
{
    /// <summary>
    /// Interaction logic for BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window, INotifyPropertyChanged
    {
        private readonly RoomInformationService roomInformationService;
        private readonly RoomTypeService roomTypeService;
        private readonly CustomerService customerService;
        private readonly BookingReservationService bookingReservationService;
        private readonly BookingDetailService bookingDetailService;
        private int customerId;
        private RoomInformation roomSelect;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string des)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(des));

        }

        public BookingWindow(int id)
        {
            InitializeComponent();
            customerId = id;
            roomInformationService = new RoomInformationService();
            roomTypeService = new RoomTypeService();
            customerService = new CustomerService();
            bookingDetailService = new BookingDetailService();
            bookingReservationService = new BookingReservationService();

            loadData();
        }

        public void loadData()
        {
            try
            {
                Customer.Text = customerService.FindById(customerId).CustomerFullName;

                Debug.WriteLine(Customer.Text);
                cmbRoomType.ItemsSource = roomTypeService.GetAll().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine(ex);
                return;
            }


        }

        public List<RoomInformation> GetRoomFromSearch(object sender, RoutedEventArgs e)
        {
            DateTime checkInDate = (DateTime)dpCheckIn.SelectedDate;
            DateTime checkOutDate = (DateTime)dpCheckOut.SelectedDate;
            RoomType roomType = cmbRoomType.SelectedItem as RoomType;
            int numberOfPeople = int.Parse(txtNumberOfPerson.Text);
            if (checkInDate == null || checkOutDate == null || roomType == null)
            {
                MessageBox.Show("Vui long chon ngay check in, check out va loai phong !", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return new List<RoomInformation>();
            }
            var listRoomAvailable = roomInformationService.GetAvailableRooms(checkInDate, checkOutDate, roomType.RoomTypeId, numberOfPeople);
            return listRoomAvailable;
        }

        public void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (dpCheckIn.SelectedDate==null || dpCheckOut.SelectedDate==null || cmbRoomType.SelectedItem as RoomType ==null || txtNumberOfPerson.Text==null)
            {
                MessageBox.Show("Vui Long Chon Day du Thong Tin");
                return;
            }

            dataGridRooms.ItemsSource= GetRoomFromSearch(sender, e);
            dataGridRooms.Visibility = Visibility.Visible;
            btnBooking.Visibility = Visibility.Visible;
            btnSearch.Visibility = Visibility.Collapsed;

        }

        public void cmbRoomType_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                RoomType roomType = comboBox.SelectedItem as RoomType;

                txtTypeDescription.Text = roomType?.TypeDescription ?? string.Empty;

            }

        }

        public void dpCheckIn_Changed(object sender, SelectionChangedEventArgs e)
        {
            DateTime today = DateTime.Today;
            DatePicker datePicker = sender as DatePicker;

            DateTime checkInDate = (DateTime)dpCheckIn.SelectedDate;
            if (checkInDate != null)
            {
                if (checkInDate < today)
                {
                    MessageBox.Show("kHONG CHON QUA KHU ", "Invalid Date", MessageBoxButton.OK, MessageBoxImage.Warning);
                    dpCheckIn.SelectedDate = today;
                }
            }
        }
        public void dpCheckOut_Changed(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            DateTime checkOutDate = (DateTime)dpCheckOut.SelectedDate;

            DateTime checkInDate = (DateTime)dpCheckIn.SelectedDate;
            if (checkInDate == null) MessageBox.Show("Vui long chon ngay check in truoc!");
            if (checkInDate > checkOutDate)
            {
                MessageBox.Show("Chon checkout date khong hop le !","Invalid Date! ", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpCheckOut.SelectedDate = checkInDate.AddDays(1);
            }

        }

        public void btnCloseBooking_Click(object sender, RoutedEventArgs e)
        { 
            this.Close(); 
        }

        public void btnBooking_Click(object sender, RoutedEventArgs e)
        {
           
            
            DataGrid dataGrid = new DataGrid();
            RoomInformation room = roomSelect;
            BookingReservation bookingReservation=new BookingReservation();
            bookingReservation.CustomerId = customerId;
            if (dpCheckIn.SelectedDate != null)
            {
                bookingReservation.BookingDate=DateOnly.FromDateTime(dpCheckIn.SelectedDate.Value);
            }


          //  bookingReservation.BookingReservationId = 100;
            bookingReservation.TotalPrice = room.RoomPricePerDay;
            bookingReservation.BookingStatus = 1;
            bookingReservationService.Add(bookingReservation);
            

            BookingDetail bookingDetail = new BookingDetail();
            bookingDetail.BookingReservationId = bookingReservation.BookingReservationId;
            bookingDetail.StartDate=DateOnly.FromDateTime(dpCheckIn.SelectedDate.Value);
            bookingDetail.EndDate=DateOnly.FromDateTime(dpCheckOut.SelectedDate.Value);
            bookingDetail.ActualPrice = room.RoomPricePerDay;
            bookingDetail.RoomId= room.RoomId;
            bookingDetailService.Add(bookingDetail);
            MessageBox.Show("Booking Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();


        }
        public void chooseRoom(object sender, SelectionChangedEventArgs e)
        {
            
            roomSelect = dataGridRooms.SelectedItem as RoomInformation;
            

        }
    }
}
