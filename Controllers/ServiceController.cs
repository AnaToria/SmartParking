using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartParking.Controllers
{
    public class ServiceController : IServiceController, IDisposable
    {
        private SmartParkingDBEntities _entites = new SmartParkingDBEntities();

        public int SpotCount()
        {
            return _entites.ParkingSlots.Count(a => a.Status == "I" || a.Status == "F");

        }

        public void TakeSlot(int id)
        {
            var slot = _entites.ParkingSlots.Where(p => p.SlotId == id).FirstOrDefault();
            var booked = _entites.UserBookings.Where(p => p.SlotId == id && p.IsActive == true).FirstOrDefault();


            if (booked != null)
            {
                booked.Status = "P";
                slot.Status = "P";
            }
            else
            {
                slot.Status = "P";
            }

            _entites.SaveChanges();
        }

        public void LeaveSlot(int id)
        {
            var slot = _entites.ParkingSlots.Where(p => p.SlotId == id).FirstOrDefault();
            slot.Status = "F";

            _entites.SaveChanges();
        }

        public void AddIn()
        {
            var inOut = _entites.ParkInOuts.FirstOrDefault();
            inOut.ParkIn = inOut.ParkIn + 1;

            _entites.SaveChanges();
        }

        public void AddOut()
        {
            var inOut = _entites.ParkInOuts.FirstOrDefault();
            inOut.ParkOut = inOut.ParkOut + 1;

            _entites.SaveChanges();
        }

        public int Difference()
        {
            var inOut = _entites.ParkInOuts.FirstOrDefault();

            return inOut.ParkIn - inOut.ParkOut;
        }

        public void Dispose()
        {
            _entites.Dispose();
        }

    }
}