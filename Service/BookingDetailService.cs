using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Service
{
    public class BookingDetailService
    {
        private readonly BookingDetailRepository repository;
        public BookingDetailService()
        {
            repository = new BookingDetailRepository();
        }

        public List<BookingDetail> GetAll()
        {
            using (var context = new FuminiHotelManagementContext(AppSettingsReader.GetString("ConnectionStrings", "DefaultConnection")))
            {
                return context.BookingDetails
                    .Include(b => b.Room)
                        .ThenInclude(r => r.RoomType)
                    .Include(b => b.BookingReservation)
                        .ThenInclude(br => br.Customer)
                    .ToList();
            }
        }
        public void Add(BookingDetail bookingDetail)
        {
            if (bookingDetail == null) throw new ArgumentNullException(nameof(bookingDetail));
            repository.Add(bookingDetail);
            repository.Save();
        }
    }
}
