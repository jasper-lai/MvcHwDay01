using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcHwDay01.Models;
using MvcHwDay01.Services;
using System.Net;
using System.Threading;
using MvcHwDay01.Filters;
using PagedList;
using MvcHwDay01.Repositories;

namespace MvcHwDay01.Controllers
{
    public class BillController : Controller
    {

        private readonly BillingService _billingSvc = null;
        private int pageSize = 10;      //每頁10筆資料

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
        public ActionResult Create(int? page)
        {
            #region 建立初始資料

            var obj = new BillingItemViewModel()
            {
                BillDate = DateTime.Today
            };
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", -1);

            #endregion

            #region 回傳結果

            return View(obj);

            #endregion
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
            //Thread.Sleep(3 * 1000); //暫停一下, 看效果

            #region Model 檢查

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            #endregion

            #region 呼叫服務層

            item.Id = Guid.NewGuid();
            _billingSvc.Add(item);

            #endregion

            #region 回傳結果

            return View();

            #endregion

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizePlus] 
        public ActionResult CreateWithAjax(BillingItemViewModel item, int? page)
        {
            //定位在當初輸入資料的那個值
            //不論 ModelState 是否為 Valid, 都要執行, 不然萬一 Model 驗證失敗, 就沒有 SelectList 可以用, 會造成例外 ...
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", item.BillType);
            Thread.Sleep(3 * 1000); //暫停一下, 看效果

            #region Model 檢查

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

            #endregion

            #region 呼叫服務層

            //呼叫 Service 層提供的功能
            item.Id = Guid.NewGuid();
            _billingSvc.Add(item);

            #endregion

            #region 分頁處理

            int pageNumber = (!page.HasValue ? 1 : (page.Value < 1 ? 1 : page.Value));
            var bills = _billingSvc.GetAll();
            var onePage = bills.ToPagedList(pageNumber, pageSize);

            #endregion

            #region 回傳結果

            return PartialView("ListCurrent", onePage);
            //return PartialView("ListCurrent", bills);

            #endregion

        }

        /// <summary>
        /// 列出資料 (ChildActionOnly)
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult ListCurrent(int? page)
        {
            //取得最近的 5 筆資料, 依日期降冪
            //呼叫 Service 層提供的資料查詢功能
            //var bills = _billingSvc.GetTopN(5);

            #region 呼叫服務層

            var bills = _billingSvc.GetAll();

            #endregion

            #region 分頁處理

            int pageNumber = (!page.HasValue ? 1 : (page.Value < 1 ? 1 : page.Value));
            var onePage = bills.ToPagedList(pageNumber, pageSize);

            #endregion

            #region 回傳結果

            return PartialView(onePage);

            #endregion

        }

    }
}