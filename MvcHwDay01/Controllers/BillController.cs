using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcHwDay01.Models;

namespace MvcHwDay01.Controllers
{
    public class BillController : Controller
    {
        // GET: Bill
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var obj = new BillingItemViewModel()
            {
                BillDate = DateTime.Today
            };
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", -1);
            return View(obj);
        }

        [HttpPost]
        public ActionResult Create(BillingItemViewModel item)
        {
            Utilities.AddBillingDataByUser("Jasper", item);
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", item.BillType);
            return View();
        }

        [ChildActionOnly]
        public ActionResult ListCurrent()
        {
            //產生對應的中文名稱
            KeyValuePair<int, string> defaultObj = new KeyValuePair<int, string>(-1, "請選擇...");
            foreach (var item in BillingHistory.Data["Jasper"])
            {
                item.BillTypeName = GlobalCodeMappings.BillTypes.Where(o => o.Key == item.BillType).DefaultIfEmpty(defaultObj).First().Value;
            }
            return View(BillingHistory.Data["Jasper"]);
        }

    }
}