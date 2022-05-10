using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartParking.Controllers
{
    public class ApiController : Controller
    {
        private readonly IServiceController _serviceController = new ServiceController();

        public JsonResult AllowCar()
        {
            var num = _serviceController.SpotCount();
            try
            {
                if(num == 0)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else if(num == 1 && _serviceController.Difference() != 0)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _serviceController.AddIn();
                    return Json(num, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CarIn(int id)
        {
            try
            {
                _serviceController.TakeSlot(id);
                return Json(id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }         
        }

        public JsonResult CarOut(int id)
        {
            try
            {
                _serviceController.LeaveSlot(id);
                _serviceController.AddOut();
                return Json(id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }        
        }

        public JsonResult Park(int id)
        {
            try
            {
                var temp = id;
                var info = new { result = "S" };
                return Json(info, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }            
        }

        public JsonResult CarParked(int id)
        {
            try
            {
                return Json(id*3, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult CarLeft(int id)
        {
            try
            {
                return Json(id, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        
    }
}