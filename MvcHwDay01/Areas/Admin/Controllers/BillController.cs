using MvcHwDay01.Models;
using MvcHwDay01.Services;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcHwDay01.Areas.Admin.Filters;
using MvcHwDay01.Areas.Admin.ViewModels;
using MvcHwDay01.Repositories;

namespace MvcHwDay01.Areas.Admin.Controllers
{

    [AuthorizeAdmin(Roles = "admin")]
    public class BillController : Controller
    {

        private readonly BillingService _billingSvc = null;
        private int pageSize = 10;      //每頁10筆資料

        public BillController()
        {
            _billingSvc = new BillingService();
        }

        #region Index 的功能 (查詢全部資料)

        public ActionResult Index(string year, string month, int? page = 1)
        {

            #region 檢核傳入參數, 並依傳入參數的狀況, 呼叫不同的 Service Method 作查詢

            int intYear = 0;
            int intMonth = 0;
            IEnumerable<BillingItemViewModel> bills = null;

            //如果 year 與 month 都有值, 那就傳 year / month 的參數作查詢
            //否則, 就查全部
            if (year != null && month != null)
            {
                //如果2者都不是, 那就呼叫服務層作查詢
                if (int.TryParse(year, out intYear) && int.TryParse(month, out intMonth))
                {
                    bills = _billingSvc.GetByQueryYM(intYear, intMonth);
                }
                else
                {
                    bills = new List<BillingItemViewModel>();   //回傳一個空物件
                }
            }
            else
            {
                bills = _billingSvc.GetAll();
            }

            #endregion

            #region 進行分頁

            int pageNumber = (!page.HasValue ? 1 : (page.Value < 1 ? 1 : page.Value));
            var onePage = bills.ToPagedList(pageNumber, pageSize);

            #endregion

            #region 將結果回傳

            return View(onePage);

            #endregion

        }

        #endregion

        #region Delete 的功能

        /// <summary>
        /// 刪除資料 (HttpGet)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(Guid? id)
        {
            #region 檢查參數

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            #endregion

            #region 確認欲刪除的資料是否存在

            //這裡必須用 id.Value, 不然編譯會出錯
            BillingItemViewModel item = _billingSvc.GetSingle(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }

            #endregion

            #region 其它處理

            item.BillTypeName = GlobalCodeMappings.BillTypes[item.BillType];

            #endregion

            #region 回傳結果

            return View(item);

            #endregion

        }


        /// <summary>
        /// 刪除資料 (HttpPost)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// 狀況: Delete 似乎只會 POST id 欄位, 而不是整個 ViewModel 物件
        /// 原因推測: Delete 的 HttpGet 頁面, 純粹顯示, 沒有 <input .../>; 所以不會回傳
        /// </remarks>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            #region 呼叫服務層

            try
            {
                _billingSvc.DeleteById(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }

            #endregion
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
            #region 檢查參數

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            #endregion

            #region 確認欲編輯的資料是否存在

            //這裡必須用 id.Value, 不然編譯會出錯
            BillingItemViewModel item = _billingSvc.GetSingle(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }

            #endregion

            #region 其它處理

            //定位在當初輸入資料的那個值
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", item.BillType);

            #endregion

            #region 回傳結果

            return View(item);

            #endregion

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

            #region Model 檢查

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            #endregion

            #region 呼叫服務層

            try
            {
                _billingSvc.Edit(item);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(item);
            }

            #endregion

        }

        #endregion

        #region Query 的功能

        [HttpGet]
        public ActionResult Query()
        {
            #region 準備初始資料

            var obj = new BillQueryViewModel()
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                PageIndex = 1
            };
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", -1);

            #endregion

            #region 回傳結果

            return View(obj);

            #endregion

        }

        [HttpPost]
        //public ActionResult Query(BillQueryViewModel query, int? page = 1)
        public ActionResult Query(BillQueryViewModel query)
        {
            //定位在當初輸入資料的那個值
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", query.BillType);

            #region Model 檢查

            //輸入資料驗證
            if (!ModelState.IsValid)
            {
                return View(query);
            }

            #endregion

            #region 呼叫服務層

            //已通過資料驗證
            int? page = query.PageIndex;    //原本由 HttpGet 的 QueryString 取得, 改由 Form 取得
            int pageNumber = (!page.HasValue ? 1 : (page.Value < 1 ? 1 : page.Value));
            var bills = _billingSvc.GetByQuery(query);
            var result = bills.ToPagedList(pageNumber, pageSize);
            query.QueryResult = result;

            #endregion

            #region 回傳結果

            return View(query);

            #endregion

        }

        #endregion

    }
}