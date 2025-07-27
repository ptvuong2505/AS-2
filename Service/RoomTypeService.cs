using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repository;

namespace Service
{
    public class RoomTypeService
    {
        private readonly RoomTypeRepository repository;
        public RoomTypeService()
        {
            repository = new RoomTypeRepository();
        }

        public RoomType GetById(int id)
        {
            return repository.GetById(id);
        }
        public List<RoomType> GetAll()
        {
            return (List<RoomType>)repository.GetAll();
        }
    }
}
