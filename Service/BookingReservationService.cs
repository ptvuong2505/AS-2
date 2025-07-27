using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repository;

namespace Service
{
    public class BookingReservationService
    {
           private readonly BookingReservationRepository repository;
           public BookingReservationService()
        {
            repository = new BookingReservationRepository();
        }
        public List<BookingReservation> GetAll()
        {
            return (List<BookingReservation>)repository.GetAll();
        }
        public List<BookingReservation> GetByCustomerId(int id)
        {
            return (List<BookingReservation>)repository.GetBookingReservationByCustomerId(id);
        }

        public void Add(BookingReservation reservation) { 
            repository.Add(reservation);
            repository.Save();   
        }

    }
}
