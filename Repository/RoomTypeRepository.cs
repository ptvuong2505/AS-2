using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
namespace Repository
{
    public class RoomTypeRepository : GenericRepository<RoomType>, IRepository<RoomType>
    {
        public RoomTypeRepository()
        {
        }
        // You can add any specific methods for RoomTypeRepository here if needed
    }
   
}
