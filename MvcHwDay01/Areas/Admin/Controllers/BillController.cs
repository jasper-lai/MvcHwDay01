using MvcHwDay01.Models;
using MvcHwDay01.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MvcHwDay01.Areas.Admin.Controllers
{
    public class BillController : Controller
    {

        private readonly BillingService _billingSvc = null;

        public BillController()
        {
            _billingSvc = new BillingService();
        }

        // GET: Admin/Bill
        public ActionResult Index()
        {
            //取得最近的 30 資料, 依日期降冪
            //呼叫 Service 層提供的資料查詢功能
            var bills = _billingSvc.GetTopN(30);

            return View(bills);
        }

        #region Delete 的功能

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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                _billingSvc.DeleteById(id);
                _billingSvc.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        #endregion

        #region Edit 的功能

        /// <summary>
        /// 編輯資料 (HttpGet)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(Guid? id)
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

            //定位在當初輸入資料的那個值
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", item.BillType);
            return View(item);
        }

        /// <summary>
        /// 編輯資料 (HttpPost)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BillingItemViewModel item)
        {
            //定位在當初輸入資料的那個值
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", item.BillType);

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            try
            {
                _billingSvc.Edit(item);
                _billingSvc.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(item);
            }
        }

        #endregion

    }
}