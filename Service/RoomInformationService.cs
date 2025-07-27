using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RoomInformationService
    {
        private readonly RoomInformationRepository repository;
        private readonly BookingDetailRepository bookingDetailRepository;
        public RoomInformationService()
        {
            repository = new RoomInformationRepository();
            bookingDetailRepository = new BookingDetailRepository();
        }
        public List<RoomInformation> GetAll()
        {
            return (List<RoomInformation>)repository.GetAll();
        }

        public RoomInformation FindById(int id)
        {
            return repository.GetById(id);
        }

        public List<RoomInformation> GetAvailableRooms(DateTime checkInDate, DateTime checkOutDate, int roomTypeId, int numberOfPeople)
        {
            List<RoomInformation> roomrt=new List<RoomInformation>();
            List < RoomInformation > r2 = new List<RoomInformation>();
            List<RoomInformation> allRooms = GetAll();
            foreach(RoomInformation room in allRooms)
            {
                if (room.RoomTypeId != roomTypeId ||
                    room.RoomMaxCapacity < numberOfPeople ||
                    room.RoomStatus != 1) // Assuming 1 means available
                {

                }
                else roomrt.Add(room);
            }
            List<BookingDetail> bookedRooms = bookingDetailRepository.GetAll().ToList();

            foreach (BookingDetail booking in bookedRooms)
            {
               
                RoomInformation room = roomrt.FirstOrDefault(r => r.RoomId == booking.RoomId);
               

                if ( room !=null)
                {
                    if ( booking.StartDate > DateOnly.FromDateTime(checkOutDate) || booking.EndDate > DateOnly.FromDateTime(checkInDate))
                    {
                        roomrt.Remove(room);
                    }
                }
            }

                return roomrt;
            throw new NotImplementedException();
        }

        public RoomInformation GetById(int id) { 
            return repository.GetById(id);
        }

       

    }
}
