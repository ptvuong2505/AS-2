using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
namespace Repository
{
    public class BookingDetailRepository : GenericRepository<BookingDetail>, IRepository<BookingDetail>
    {
        public BookingDetailRepository()
        {
        }
        // You can add any specific methods for BookingDetailRepository here if needed
    }
    
}
