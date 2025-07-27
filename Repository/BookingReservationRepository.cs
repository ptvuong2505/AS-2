using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repository
{
    public class BookingReservationRepository : GenericRepository<BookingReservation>, IRepository<BookingReservation>
    {
        public BookingReservationRepository()
        {
        }
        public List<BookingReservation> GetBookingReservationByCustomerId(int customerId)
        {
            // Assuming you have a DbContext or similar to access the database
            using (var context = new FuminiHotelManagementContext(AppSettingsReader.GetConnectionString()))
            {
                return context.BookingReservations.Where(br => br.CustomerId == customerId).ToList();
            }
        }

        

       
    }
    
}
