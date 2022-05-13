using SmartParking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartParking.Controllers
{
    public class AdminController : Controller
    {
        private SmartParkingDBEntities _entites = new SmartParkingDBEntities();

        #region Actions
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult ParkingSlots()
        {
            var model = new List<ParkingSlot>();
            model= FetchParkingSlots();
            ViewData["parkingSlots"] = model;
            return View();
        }

        public ActionResult ActiveBookings()
        {
            var model = new List<UserBooking>();
            model= FetchActiveBookings();
            ViewData["bookingHistory"] = model;
            return View();
        }

        public ActionResult BookingRequest(string message = null)
        {
            var freeSpots = new List<int>();
            var activeUsers = new List<int>();
            freeSpots= FetchFreeSpots();
            activeUsers = FetchActiveUserIds();

            var viewModel = new AdminBookingRequest
            {
                FreeSpotList = new SelectList(freeSpots),
                ActiveUserList = new SelectList(activeUsers)
            };

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }

            return View(viewModel);
        }
        
        public ActionResult UserManagment()
        {
            var model = new List<UserInformation>();
            model = FetchActiveUsers();
            ViewData["activeUsers"] = model;
            return View();
        }

        public ActionResult Mailbox()
        {
            var model = new List<ContactUsEmail>();
            model = FetchEmails();
            ViewData["emails"] = model;
            return View();
        }

        public ActionResult Email(int id)
        {
            var model = FetchEmail(id);
            return View(model);
        }
        #endregion


        #region HTTP Requests
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

            return RedirectToAction("ActiveBookings");
        }

        [HttpPost]
        public ActionResult BookingRequest(AdminBookingRequest form)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("BookingRequest", new { message = "General error" });
            }
            else if (form.Date < DateTime.Now)
            {
                return RedirectToAction("BookingRequest", new { message = "Not a valid date" });
            }
            else
            {
                if (SpotIsFree(form.SlotId)== 1)
                {
                    var userInfo = GetUserInformation(form.UserId);

                    var userBooking = new UserBooking
                    {
                        UserId = form.UserId,
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

                    return RedirectToAction("BookingRequest", new { message = "Booking went successfully! For further information, see active bookings" });
                }
                else
                {
                    return RedirectToAction("BookingRequest", new { message = "Spot is taken! Please, try again" });
                }

            }
        }
        #endregion


        #region Helper Functions
        private List<ParkingSlot> FetchParkingSlots()
        {
            return _entites.ParkingSlots.ToList();
        }

        private List<UserBooking> FetchActiveBookings()
        {
            return _entites.UserBookings.Where(a => a.IsActive == true).ToList();
        }

        private List<int> FetchFreeSpots()
        {
            return _entites.FreeSpots.AsNoTracking().Select(a => a.SlotId).ToList();
        }

        private List<int> FetchActiveUserIds()
        {
            return _entites.UserInformations.AsNoTracking().Where(a => a.UserType != "A").Select(a => a.UserId).ToList();
        }

        private List<UserInformation> FetchActiveUsers()
        {
            return _entites.UserInformations.AsNoTracking().Where(a => a.UserType != "A").ToList();
        }

        private int SpotIsFree(int spotId)
        {
            return _entites.FreeSpots.AsNoTracking().Count(a => a.SlotId == spotId);
        }

        private UserInformation GetUserInformation(int userId)
        {
            return _entites.UserInformations.Where(u => u.UserId == userId).FirstOrDefault();
        }

        private List<ContactUsEmail> FetchEmails()
        {
            return _entites.ContactUsEmails.ToList();
        }

        private ContactUsEmail FetchEmail(int id)
        {
            return _entites.ContactUsEmails.FirstOrDefault(e => e.EmailId == id);
        }
        #endregion
    }
}