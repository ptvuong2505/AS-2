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
    /// Interaction logic for AddRoomWindow.xaml
    /// </summary>
    public partial class AddRoomWindow : Window
    {
        private readonly RoomTypeService roomTypeService = new RoomTypeService();
        private readonly RoomInformationRepository roomRepository = new RoomInformationRepository();

        public AddRoomWindow()
        {
            InitializeComponent();
            LoadRoomTypes();
        }

        private void LoadRoomTypes()
        {
            cmbRoomType.ItemsSource = roomTypeService.GetAll();
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu từ form
            string roomNumber = txtRoomNumber.Text.Trim();
            var roomType = cmbRoomType.SelectedItem as RoomType;
            string description = txtRoomDescription.Text.Trim();
            string maxCapacityText = txtRoomMaxCapacity.Text.Trim();
            string priceText = txtRoomPrice.Text.Trim();

            // Kiểm tra dữ liệu
            if (string.IsNullOrEmpty(roomNumber) || roomType == null ||
                string.IsNullOrEmpty(maxCapacityText) || string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(maxCapacityText, out int maxCapacity) || maxCapacity <= 0)
            {
                MessageBox.Show("Sức chứa phải là số nguyên dương!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price) || price <= 0)
            {
                MessageBox.Show("Giá phòng phải là số dương!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra trùng số phòng
            var existed = roomRepository.GetAll().Any(r => r.RoomNumber.Equals(roomNumber, StringComparison.OrdinalIgnoreCase));
            if (existed)
            {
                MessageBox.Show("Số phòng đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Tạo đối tượng phòng mới
            var newRoom = new RoomInformation
            {
                RoomNumber = roomNumber,
                RoomTypeId = roomType.RoomTypeId,
                RoomDetailDescription = description,
                RoomMaxCapacity = maxCapacity,
                RoomPricePerDay = price,
                RoomStatus = 1 // 1: available
            };

            // Thêm vào database
            roomRepository.Add(newRoom);
            roomRepository.Save();

            MessageBox.Show("Thêm phòng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
           
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
