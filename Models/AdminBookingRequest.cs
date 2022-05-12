using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartParking.Models
{
    public class AdminBookingRequest
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int SlotId { get; set; }
        [Required]
        public System.DateTime Date { get; set; }
        [Required]
        public string LicensePlate { get; set; }
        public string Status { get; set; }
        public bool? IsActive { get; set; }
        public IEnumerable<SelectListItem> FreeSpotList { get; set; }
        public IEnumerable<SelectListItem> ActiveUserList { get; set; }
    }
}