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
        private SkillTreeHomeworkEntities db = new SkillTreeHomeworkEntities();

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

            //將 ViewModel 轉為 Model
            var acc = new AccountBook
            {
                Id = Guid.NewGuid(),
                Categoryyy = item.BillType,
                Dateee = item.BillDate,
                Amounttt = item.Aoumnt,
                Remarkkk = item.Memo
            };

            db.AccountBooks.Add(acc);
            db.SaveChanges();

            //定位在當初輸入資料的那個值
            ViewBag.BillTypes = new SelectList(GlobalCodeMappings.BillTypes, "Key", "Value", item.BillType);
            return View();
        }

        [ChildActionOnly]
        public ActionResult ListCurrent()
        {
            //取得最近的 20 筆資料, 依日期降冪
            var biils = db.AccountBooks
                .OrderByDescending(x => x.Dateee)
                .Take(20)
                //必須要加上這個, 以下 產生對應的中文名稱 的部份, 才不會出錯; 因為會連結至 DB 執行那段敍述, 而出現以下錯誤
                //LINQ to Entities 無法辨識方法 'System.String get_Item(Int32)' 方法，而且這個方法無法轉譯成存放區運算式。
                //為效能考量, 最好在所有的 Where, OrderBy, Take ... 都作完以後, 才進行 .AsEnumerable()
                .AsEnumerable()
                .Select(x =>
                   new BillingItemViewModel
                   {
                       BillType = x.Categoryyy,
                       BillDate = x.Dateee,
                       Aoumnt = x.Amounttt,
                       Memo = x.Remarkkk,
                       //產生對應的中文名稱
                       BillTypeName = GlobalCodeMappings.BillTypes[x.Categoryyy]
                   });

            return View(biils);

            #region 未加 AsEnumerable() 的 SQL 指令
            //// [說明]: 在 DB 就已作好 mapping (如果沒有 中文名稱轉換 的干擾, 且可以完整對應的話)
            //// C#
            //var biils = this.AccountBooks
            //.OrderByDescending(x => x.Dateee)
            //.Take(20)
            //.Select(x =>
            //   new
            //   {
            //       BillType = x.Categoryyy,
            //       BillDate = x.Dateee,
            //       Aoumnt = x.Amounttt,
            //       Memo = x.Remarkkk,
            //   }).Dump();
            //  -----------------------------------------
            //// SQL
            //SELECT[t1].[Categoryyy] AS[BillType], [t1].[Dateee]
            //AS[BillDate], [t1].[Amounttt]
            //AS[Aoumnt], [t1].[Remarkkk]
            //AS[Memo]
            //FROM(
            //SELECT TOP (20) [t0].[Categoryyy], [t0].[Amounttt], [t0].[Dateee], [t0].[Remarkkk]
            //        FROM[AccountBook] AS[t0]
            //    ORDER BY[t0].[Dateee]  DESC
            //    ) AS[t1]
            //ORDER BY[t1].[Dateee] DESC
            #endregion

            #region 有加 AsEnumerable() 的 SQL 指令
            //// [說明]: 在 AP Server 才作 mapping 的處理 (有中文名稱轉換 的干擾)
            //// C#
            //var biils = this.AccountBooks
            //.OrderByDescending(x => x.Dateee)
            //.Take(20)
            //.AsEnumerable()
            //.Select(x =>
            //   new
            //   {
            //       BillType = x.Categoryyy,
            //       BillDate = x.Dateee,
            //       Aoumnt = x.Amounttt,
            //       Memo = x.Remarkkk,
            //   }).Dump();
            //  -----------------------------------------
            //// SQL
            //SELECT TOP (20)[t0].[Id], [t0].[Categoryyy], [t0].[Amounttt], [t0].[Dateee], [t0].[Remarkkk]
            //FROM[AccountBook] AS[t0]
            //ORDER BY[t0].[Dateee] DESC

            #endregion

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}