using System;
using System.Windows;
using BusinessObjects;
using Service;
using Repository;

namespace PhanThanhVuongWPF.AdminViewModel
{
    public partial class EditRoomWindow : Window
    {
        private readonly RoomTypeService roomTypeService = new RoomTypeService();
        private readonly RoomInformationRepository roomRepository = new RoomInformationRepository();
        private readonly RoomInformation originalRoom;

        public EditRoomWindow(RoomInformation room)
        {
            InitializeComponent();
            originalRoom = room;
            LoadRoomTypes();
            LoadRoomData();
        }

        private void LoadRoomTypes()
        {
            cmbRoomType.ItemsSource = roomTypeService.GetAll();
        }

        private void LoadRoomData()
        {
            txtRoomNumber.Text = originalRoom.RoomNumber;
            cmbRoomType.SelectedValue = originalRoom.RoomTypeId;
            txtRoomDescription.Text = originalRoom.RoomDetailDescription;
            txtRoomMaxCapacity.Text = originalRoom.RoomMaxCapacity?.ToString() ?? "";
            txtRoomPrice.Text = originalRoom.RoomPricePerDay?.ToString() ?? "";
        }

        private void btnSaveRoom_Click(object sender, RoutedEventArgs e)
        {
            string roomNumber = txtRoomNumber.Text.Trim();
            var roomType = cmbRoomType.SelectedItem as RoomType;
            string description = txtRoomDescription.Text.Trim();
            string maxCapacityText = txtRoomMaxCapacity.Text.Trim();
            string priceText = txtRoomPrice.Text.Trim();

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

            // Kiểm tra trùng số phòng (trừ phòng hiện tại)
            var existed = roomRepository.GetAll().Any(r => r.RoomNumber.Equals(roomNumber, StringComparison.OrdinalIgnoreCase) && r.RoomId != originalRoom.RoomId);
            if (existed)
            {
                MessageBox.Show("Số phòng đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Cập nhật dữ liệu
            originalRoom.RoomNumber = roomNumber;
            originalRoom.RoomTypeId = roomType.RoomTypeId;
            originalRoom.RoomDetailDescription = description;
            originalRoom.RoomMaxCapacity = maxCapacity;
            originalRoom.RoomPricePerDay = price;

            roomRepository.Update(originalRoom);
            roomRepository.Save();

            MessageBox.Show("Cập nhật phòng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

