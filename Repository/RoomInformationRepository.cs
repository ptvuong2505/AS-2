using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repository
{
    public class RoomInformationRepository:GenericRepository<RoomInformation>, IRepository<RoomInformation>
    {
        public RoomInformationRepository()
        {
        }
        // You can add any specific methods for RoomInfomationRepository here if needed
    }
    
}
