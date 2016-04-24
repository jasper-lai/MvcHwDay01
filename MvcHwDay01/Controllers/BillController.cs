using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcHwDay01.Models;
using MvcHwDay01.Models.Services;
using System.Net;

namespace MvcHwDay01.Controllers
{
    public class BillController : Controller
    {

        private readonly BillingService _billingSvc = null;

        public BillController()
        {
            _billingSvc = new BillingService();
        }

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
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            //呼叫 Service 層提供的功能
            item.Id = Guid.NewGuid();
            _billingSvc.Add(item);
            _billingSvc.Save();

            //定位在當初輸入資料的那個值
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", item.BillType);
            return View();
        }

        [ChildActionOnly]
        public ActionResult ListCurrent()
        {
            //取得最近的 20 筆資料, 依日期降冪
            //呼叫 Service 層提供的資料查詢功能
            var bills = _billingSvc.GetTopN(20);

            return View(bills);
        }

    }
}