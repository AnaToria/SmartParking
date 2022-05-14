using SmartParking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartParking.Controllers
{
    public class UserController : Controller
    {
        private SmartParkingDBEntities _entites = new SmartParkingDBEntities();

        #region Actions
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult BookingHistory() 
        {
            var model = new List<UserBooking>();
            model= FetchBookingData(Session["userId"].ToString());
            ViewData["bookingHistory"] = model;
            return View();
        }
        public ActionResult BookingRequest(string message = null)
        {
            var freeSpots = new List<int>();
            freeSpots= FetchFreeSpots().Select(r => r.SlotId).ToList();
            var viewModel = new BookingRequest
            {
                FreeSpotList = new SelectList(freeSpots)
            };

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }

            return View(viewModel);
        }

        public ActionResult UserProfile()
        {
            ViewBag.Message = Session["userId"].ToString();
            return View();
        }

        #endregion

        #region Http Requests
        [HttpPost]
        public ActionResult BookingRequest(BookingRequest form)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("BookingRequest", new { message = "General error" });
            }
            else if(form.Date < DateTime.Now)
            {
                return RedirectToAction("BookingRequest", new { message = "Not a valid date" });
            }
            else
            {
                if (SpotIsFree(form.SlotId )== 1)
                {
                    var userInfo = GetUserInformation(Session["userId"].ToString());

                    var userBooking = new UserBooking
                    {
                        UserId = userInfo.UserId,
                        SlotId = form.SlotId,
                        Date = form.Date,
                        LicensePlate = form.LicensePlate,
                        Status = "B",
                        IsActive = true
                    };

                    _entites.UserBookings.Add(userBooking);

                    var parkingSlot = _entites.ParkingSlots.Where(p => p.SlotId == form.SlotId).FirstOrDefault();
                    parkingSlot.Status = "B";
                    parkingSlot.LicensePlate = form.LicensePlate;
                    parkingSlot.Person = userInfo.FirstName + " " + userInfo.LastName;
                    parkingSlot.ContactInfo = userInfo.Phone;

                    _entites.SaveChanges();

                    return RedirectToAction("BookingRequest", new { message = "Booking went successfully! For further information, see booking history" });
                }
                else
                {
                    return RedirectToAction("BookingRequest", new { message = "Spot is taken! Please, try again" });
                }
               
            }
        }

        [HttpPost]
        public ActionResult DeleteRecord(int id)
        {            
            var record = _entites.UserBookings.Where(b => b.Id == id).FirstOrDefault();
            record.IsActive = false;
            
            var spot = _entites.ParkingSlots.Where(s => s.SlotId == record.SlotId).FirstOrDefault();
            spot.Status="F";
            spot.LicensePlate = string.Empty;
            spot.Person = string.Empty;
            spot.ContactInfo = string.Empty;
            
            _entites.SaveChanges();

            return RedirectToAction("BookingHistory");
        }
        
        [HttpPost]
        public ActionResult UpdateRecord(int id)
        {
            var record = _entites.UserBookings.Where(b => b.Id == id).FirstOrDefault();
            record.Status = "I";

            var spot = _entites.ParkingSlots.Where(s => s.Id == record.SlotId).FirstOrDefault();
            spot.Status = "I";
            _entites.SaveChanges();

            return RedirectToAction("BookingHistory");
        }
        #endregion

        #region Helper Functions
        private List<UserBooking> FetchBookingData(string userId)
        {
            return _entites.UserBookings.AsNoTracking().Where(b => b.UserId.ToString() == userId).ToList();
        }

        private List<FreeSpot> FetchFreeSpots()
        {
            return _entites.FreeSpots.AsNoTracking().ToList();
        }

        private UserInformation GetUserInformation(string userId)
        {
            return _entites.UserInformations.Where(u => u.UserId.ToString() == userId).FirstOrDefault();
        }

        private int SpotIsFree(int spotId)
        {
            return _entites.FreeSpots.AsNoTracking().Count(a => a.SlotId == spotId);
        }
        #endregion

    }
}