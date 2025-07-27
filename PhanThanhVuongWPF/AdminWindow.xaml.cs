using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PhanThanhVuongWPF.AdminViewModel;
using Repository;
using Service;

namespace PhanThanhVuongWPF
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private ObservableCollection<BookingDetail> BookingList;
        private ObservableCollection<RoomInformation> RoomList;
        private RoomInformation SelectedRoom;
        private readonly RoomInformationService roomService = new RoomInformationService();
        private readonly BookingDetailService bookingDetailService = new BookingDetailService();
        private ObservableCollection<Customer> CustomerList;
        private Customer SelectedCustomer;
        private readonly CustomerService customerService = new CustomerService();


        public AdminWindow()
        {
            InitializeComponent();
            LoadRooms();
            dataGridRooms.ItemsSource = RoomList;
            dataGridRooms.ItemsSource = RoomList;
            LoadBookings();
            LoadCustomers();
        }

        private void LoadRooms()
        {
            RoomList = new ObservableCollection<RoomInformation>(roomService.GetAll());
            if (dataGridRooms != null)
                dataGridRooms.ItemsSource = RoomList;
        }

        private void dataGridRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedRoom = dataGridRooms.SelectedItem as RoomInformation;
            btnEditRoom.Visibility = SelectedRoom != null ? Visibility.Visible : Visibility.Collapsed;
            btnDeleteRoom.Visibility = SelectedRoom != null ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnDeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRoom == null) return;
            var result = MessageBox.Show($"Bạn có chắc muốn xóa phòng {SelectedRoom.RoomNumber}?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var repo = new RoomInformationRepository();
                repo.Delete(SelectedRoom.RoomId);
                repo.Save();
                RoomList.Remove(SelectedRoom);
                SelectedRoom = null;
                btnEditRoom.Visibility = Visibility.Collapsed;
                btnDeleteRoom.Visibility = Visibility.Collapsed;
            }
        }

        private void btnEditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRoom == null) return;
            var editRoomWindow = new AdminViewModel.EditRoomWindow(SelectedRoom);
            if (editRoomWindow.ShowDialog() == true)
            {
                LoadRooms();
            }
        }



        private void addRoomWindow_Open(object sender, RoutedEventArgs e)
        {
            var addRoomWindow = new AdminViewModel.AddRoomWindow();
            if (addRoomWindow.ShowDialog() == true)
            {
                LoadRooms();
            }
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LoadBookings()
        {
            var bookings = bookingDetailService.GetAll();
            BookingList = new ObservableCollection<BookingDetail>(bookings);
            dataGridBookings.ItemsSource = BookingList;

            txtTotalBooking.Text = $"Tổng số booking: {BookingList.Count}";

            int availableRoomCount = roomService.GetAll().Count(r => r.RoomStatus == 1);
            txtAvailableRoom.Text = $"Số phòng trống hiện tại: {availableRoomCount}";
        }

        private void btnViewAvailableRoom_Click(object sender, RoutedEventArgs e)
        {
            var availableRooms = roomService.GetAll().Where(r => r.RoomStatus == 1).ToList();
            string message = availableRooms.Count == 0
                ? "Không có phòng trống hiện tại."
                : "Các phòng trống hiện tại:\n" + string.Join(", ", availableRooms.Select(r => r.RoomNumber));
            MessageBox.Show(message, "Phòng trống", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadCustomers()
        {
            CustomerList = new ObservableCollection<Customer>(customerService.GetAll());
            if (dataGridCustomers != null)
                dataGridCustomers.ItemsSource = CustomerList;
        }

        private void dataGridCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCustomer = dataGridCustomers.SelectedItem as Customer;
            btnEditCustomer.Visibility = SelectedCustomer != null ? Visibility.Visible : Visibility.Collapsed;
            btnDeleteCustomer.Visibility = SelectedCustomer != null ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCustomer == null) return;
            var result = MessageBox.Show($"Bạn có chắc muốn xóa khách hàng {SelectedCustomer.CustomerFullName}?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var repo = new CustomerRepository();
                repo.Delete(SelectedCustomer.CustomerId);
                repo.Save();
                CustomerList.Remove(SelectedCustomer);
                SelectedCustomer = null;
                btnEditCustomer.Visibility = Visibility.Collapsed;
                btnDeleteCustomer.Visibility = Visibility.Collapsed;
            }
        }
        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCustomer == null) return;
            var editCustomerWindow = new AdminViewModel.EditCustomerWindow(SelectedCustomer);
            if (editCustomerWindow.ShowDialog() == true)
            {
                LoadCustomers();
            }
        }

        private void addCustomerWindow_Open(object sender, RoutedEventArgs e)
        {
            var addCustomerWindow = new AdminViewModel.AddCustomerWindow();
            if (addCustomerWindow.ShowDialog() == true)
            {
                LoadCustomers();
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            // Đóng cửa sổ admin, có thể mở lại LoginWindow nếu cần
            this.Close();
            // Nếu muốn mở lại LoginWindow:
            
        }


    }
}
