using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcHwDay01.Models;
using MvcHwDay01.Models.Services;
using System.Net;
using System.Threading;

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
        [ValidateAntiForgeryToken]
        public ActionResult Create(BillingItemViewModel item)
        {

            //定位在當初輸入資料的那個值
            //不論 ModelState 是否為 Valid, 都要執行, 不然萬一 Model 驗證失敗, 就沒有 SelectList 可以用, 會造成例外 ...
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", item.BillType);
            Thread.Sleep(3 * 1000); //暫停一下, 看效果

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            //呼叫 Service 層提供的功能
            item.Id = Guid.NewGuid();
            _billingSvc.Add(item);
            _billingSvc.Save();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWithAjax(BillingItemViewModel item)
        {
            //定位在當初輸入資料的那個值
            //不論 ModelState 是否為 Valid, 都要執行, 不然萬一 Model 驗證失敗, 就沒有 SelectList 可以用, 會造成例外 ...
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", item.BillType);
            Thread.Sleep(3 * 1000); //暫停一下, 看效果

            if (!ModelState.IsValid)
            {
                //參考 https://gist.github.com/jpoehls/2230255 取得錯誤的作法
                var errors = ModelState
                                .Where(x => x.Value.Errors.Count > 0)
                                .Select(d => new AjaxErrorViewModel
                                {
                                    ClientId = d.Key,
                                    ErrorMessage = d.Value.Errors.FirstOrDefault().ErrorMessage
                                });
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(errors);

                #region 參考 91 老師的實作方式
                //var errorFields = ModelState.Where(d => d.Value.Errors.Any())
                //     .Select(x => new { x.Key, x.Value.Errors });
                //var errors = new List<AjaxErrorViewModel>();
                //foreach (var err in errorFields)
                //{
                //    errors.Add(err.Errors.Select(
                //        d => new AjaxErrorViewModel()
                //        {
                //            ClientId = err.Key,
                //            ErrorMessage = d.ErrorMessage
                //        }).FirstOrDefault());
                //}
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //return Json(errors);
                #endregion

                #region 最原始的作法
                ////不知應該如何處理錯誤訊息 (in AJAX Helper) ...
                ////有試過 return View("Create", item); 但發現畫面會亂掉
                ////
                //List<string> errors = new List<string>();
                //foreach (ModelState err in ModelState.Values)
                //{
                //    foreach (ModelError errmsg in err.Errors)
                //    {
                //        errors.Add(errmsg.ErrorMessage);
                //    }
                //}

                ////以下參考 ...
                ////http://stackoverflow.com/questions/18763993/send-exception-message-in-ajax-beginform-mvc-4-scenario
                //Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;    //強迫前端執行 OnFailure ...
                //return Json(errors);
                #endregion

            }

            //呼叫 Service 層提供的功能
            item.Id = Guid.NewGuid();
            _billingSvc.Add(item);
            _billingSvc.Save();

            var bills = _billingSvc.GetTopN(5);
            return PartialView("ListCurrent", bills);
        }

        /// <summary>
        /// 列出資料 (ChildActionOnly)
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult ListCurrent()
        {
            //取得最近的 5 筆資料, 依日期降冪
            //呼叫 Service 層提供的資料查詢功能
            var bills = _billingSvc.GetTopN(5);

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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ////方式一: 傳整個物件到 Service 層, 但這樣需要 3 次資料庫存取
            ////(1) _billingSvc.GetSingle(id)
            ////(2) in Service: var acc2 = _db.AccountBooks.Find(item.Id);
            ////(3) _billingSvc.Save()
            //var item = _billingSvc.GetSingle(id);
            //_billingSvc.Delete(item);

            //方式二: 只需傳 Id 到 Service 層, 
            //原本作法: 在 Servcie 層呼叫了 Find(id), 才 Remove(); 最後才到本程式下方的 billingSvc.Save(); 會有 2 次的資料庫存取.
            //新作法:   經由 91 老師的提示, 目前對資料庫的存取, 只需一次就 OK 了.
            try
            {
                _billingSvc.DeleteById(id);
                _billingSvc.Save();
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        #endregion

        #region 練習 Edit 的功能

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
                return RedirectToAction("Create");
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