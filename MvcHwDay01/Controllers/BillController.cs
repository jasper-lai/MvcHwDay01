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

        /// <summary>
        /// 建立資料 (HttpGet)
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 建立資料 (HttpPost)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 列出資料 (ChildActionOnly)
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult ListCurrent()
        {
            //取得最近的 20 筆資料, 依日期降冪
            //呼叫 Service 層提供的資料查詢功能
            var bills = _billingSvc.GetTopN(20);

            return View(bills);
        }

        #region 練習 Delete 的功能

        /// <summary>
        /// 刪除資料 (HttpGet)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //這裡必須用 id.Value, 不然編譯會出錯
            BillingItemViewModel item = _billingSvc.GetSingle(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            item.BillTypeName = GlobalCodeMappings.BillTypes[item.BillType];
            return View(item);
        }


        /// <summary>
        /// 刪除資料 (HttpPost)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// 問題: Delete 似乎只會 POST id 欄位, 而不是整個 ViewModel 物件
        /// 原因推測: Delete 的 HttpGet 頁面, 純粹顯示, 沒有 <input .../>; 所以不會回傳
        /// </remarks>
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            //方式一: 傳整個物件到 Service 層, 但這樣需要 3 次資料庫存取
            //(1) _billingSvc.GetSingle(id)
            //(2) in Service: var acc2 = _db.AccountBooks.Find(item.Id);
            //(3) _billingSvc.Save()
            var item = _billingSvc.GetSingle(id);
            _billingSvc.Delete(item);

            ////方式二: 只需傳 Id 到 Service 層, 但這樣需要 2 次的資料庫存取
            ////(1) in Service: var acc = _db.AccountBooks.Find(id);
            ////(2) _billingSvc.Save()
            //_billingSvc.DeleteById(id);

            //------------------------------------------------------------
            //[問題] 有沒有辦法直接刪掉? 只要一次的資料庫存取, 類似 DELETE ... FROM ... WHERE ...
            //------------------------------------------------------------

            _billingSvc.Save();
            return RedirectToAction("Create");
        }

        #endregion



    }
}