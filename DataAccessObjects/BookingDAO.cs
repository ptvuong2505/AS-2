using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
{

}

namespace DataAccessObjects
{
    public class BookingDAO
    {
        private readonly FuminiHotelManagementContext _context;
        public BookingDAO(FuminiHotelManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Context cannot be null");
        }
        public IEnumerable<BookingReservation> GetAll()
        {
                return _context.BookingReservations.ToList();
        }
        public BookingReservation GetById(int id)
        {
         
                return _context.BookingReservations.Find(id);
           
        }
        public void Add(BookingReservation booking)
        {
                _context.BookingReservations.Add(booking);
                _context.SaveChanges();
        }
        public void Update(BookingReservation booking)
        {
                _context.BookingReservations.Update(booking);
                _context.SaveChanges();
        }
        public void Delete(int id)
        {
                var booking = GetById(id);
                if (booking == null) throw new KeyNotFoundException($"Booking with ID {id} not found");
                _context.BookingReservations.Remove(booking);
                _context.SaveChanges();
            
        }
        public IEnumerable<BookingDetail> GetBookingDetailsByReservationId(int reservationId)
        {
           
                return _context.BookingDetails.Where(bd => bd.BookingReservationId == reservationId).ToList();
            
        }


    }
}
