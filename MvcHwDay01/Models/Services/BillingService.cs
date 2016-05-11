using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MvcHwDay01.Areas.Admin.ViewModels;

namespace MvcHwDay01.Models.Services
{
    public class BillingService
    {
        private SkillTreeHomeworkEntities _db = null;

        public BillingService()
        {
            _db = new SkillTreeHomeworkEntities();
        }

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="item"></param>
        public void Add(BillingItemViewModel item)
        {
            var acc = new AccountBook
            {
                Id = item.Id,
                Categoryyy = item.BillType,
                Dateee = item.BillDate,
                Amounttt = item.Amount,
                Remarkkk = item.Memo
            };

            _db.AccountBooks.Add(acc);
        }

        /// <summary>
        /// 取回前 N 筆資料
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public IEnumerable<BillingItemViewModel> GetTopN(int top)
        {
            var bills = _db.AccountBooks
                        .OrderByDescending(x => x.Dateee)
                        .Take(top)
                        .Select(x =>
                           new BillingItemViewModel
                           {
                               Id = x.Id,
                               BillType = x.Categoryyy,
                               BillDate = x.Dateee,
                               Amount = x.Amounttt,
                               Memo = x.Remarkkk,
                           });
            return bills;
        }


        public void Save()
        {
            _db.SaveChanges();
        }


        #region 以下目前作業用不到, 只是自己留作練習參考用, 如果是真實專案, 就不應保留這個部份

        /// <summary>
        /// 以 Id 作查詢, 取回一筆資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BillingItemViewModel GetSingle(Guid id)
        {
            var acc = _db.AccountBooks.Find(id);
            BillingItemViewModel bill = null;
            if (null != acc)
            {
                //將 Model 轉換為 ViewModel
                bill = new BillingItemViewModel
                {
                    Id = acc.Id,
                    BillType = acc.Categoryyy,
                    BillDate = acc.Dateee,
                    Amount = acc.Amounttt,
                    Memo = acc.Remarkkk
                };
            }

            return bill;
        }

        /// <summary>
        /// 取回所有資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BillingItemViewModel> GetAll()
        {
            var bills = _db.AccountBooks
                        .OrderByDescending(x => x.Dateee)
                        .Select(x =>
                           new BillingItemViewModel
                           {
                               Id = x.Id,
                               BillType = x.Categoryyy,
                               BillDate = x.Dateee,
                               Amount = x.Amounttt,
                               Memo = x.Remarkkk,
                           });
            return bills;
        }

        /// <summary>
        /// 取回總筆數, 以供分頁參考之用
        /// </summary>
        /// <returns></returns>
        public int GetAllCount()
        {
            int result = 0;
            result = _db.AccountBooks.Count();
            return result;
        }

        /// <summary>
        /// 取回 N 筆資料 (先跳開前 M 筆)
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<BillingItemViewModel> GetSkipMTakeN(int skip, int take)
        {
            var bills = _db.AccountBooks
                        .OrderByDescending(x => x.Dateee)
                        .Skip(skip)
                        .Take(take)
                        .Select(x =>
                           new BillingItemViewModel
                           {
                               Id = x.Id,
                               BillType = x.Categoryyy,
                               BillDate = x.Dateee,
                               Amount = x.Amounttt,
                               Memo = x.Remarkkk,
                           });
            return bills;
        }


        #endregion


        #region 練習 Delete 的功能

        public void Delete(BillingItemViewModel item)
        {

            ////狀況一: 
            ////錯誤訊息: The object cannot be deleted because it was not found in the ObjectStateManager 
            ////例外發生點: _db.AccountBooks.Remove(acc);
            ////推測原因: 這裡 new 的是一般的物件, 沒有被加入 dbcontext; 但即使包裝成 Entry 物件 (狀況二, 狀況三), 還是有另外的問題
            //var acc = new AccountBook
            //{
            //    Id = item.Id,
            //    Categoryyy = item.BillType,
            //    Dateee = item.BillDate,
            //    Amounttt = item.Amount,
            //    Remarkkk = item.Memo
            //};
            //_db.AccountBooks.Remove(acc);

            ////狀況二:
            ////作法參考: http://stackoverflow.com/questions/15945172/the-object-cannot-be-deleted-because-it-was-not-found-in-the-objectstatemanager
            ////例外發生點: _db.AccountBooks.Attach(acc);
            ////錯誤訊息: {"附加類型 'MvcHwDay01.Models.AccountBook' 的實體失敗，因為另一個相同類型的實體已經有相同的主索引鍵值。如果圖形中的任何實體有衝突的索引值鍵，在使用 'Attach' 方法或將實體的狀態設為 'Unchanged' 或 'Modified' 時，就可能發生這種狀況。
            ////          原因可能是有些實體是新增的，尚未收到資料庫產生的索引鍵值。如有這種狀況，請使用 'Add' 方法或 'Added' 實體狀態來追蹤圖形，然後再視需要將非新增實體的狀態設為 'Unchanged' 或 'Modified'。"}
            ////
            ////推測原因: 在 Controller 的 DeleteConfirm() 裡, 有呼叫 _billingSvc.GetSingle(), 在整個 dbcontext 裡, 已存在一筆資料...
            //var acc = new AccountBook
            //{
            //    Id = item.Id,
            //    Categoryyy = item.BillType,
            //    Dateee = item.BillDate,
            //    Amounttt = item.Amount,
            //    Remarkkk = item.Memo
            //};
            //var entry = _db.Entry<AccountBook>(acc);
            //if (entry.State == EntityState.Detached)
            //{
            //    _db.AccountBooks.Attach(acc);
            //}
            //_db.AccountBooks.Remove(acc);

            ////狀況三: 查了 stack overflow
            ////作法參考: http://stackoverflow.com/questions/15637965/the-object-cannot-be-deleted-because-it-was-not-found-in-the-objectstatemanager
            ////作法原理: 強迫 Entity Framework 不要檢查 ... 副作用?
            ////例外發生點: _db.AccountBooks.Attach(acc);
            ////錯誤訊息: {"附加類型 'MvcHwDay01.Models.AccountBook' 的實體失敗，因為另一個相同類型的實體已經有相同的主索引鍵值。如果圖形中的任何實體有衝突的索引值鍵，在使用 'Attach' 方法或將實體的狀態設為 'Unchanged' 或 'Modified' 時，就可能發生這種狀況。
            ////          原因可能是有些實體是新增的，尚未收到資料庫產生的索引鍵值。如有這種狀況，請使用 'Add' 方法或 'Added' 實體狀態來追蹤圖形，然後再視需要將非新增實體的狀態設為 'Unchanged' 或 'Modified'。"}
            ////推測原因: 在 Controller 的 DeleteConfirm() 裡, 有呼叫 _billingSvc.GetSingle(), 在整個 dbcontext 裡, 已存在一筆資料...
            //var acc = new AccountBook
            //{
            //    Id = item.Id,
            //    Categoryyy = item.BillType,
            //    Dateee = item.BillDate,
            //    Amounttt = item.Amount,
            //    Remarkkk = item.Memo
            //};
            //bool oldValidateOnSaveEnabled = _db.Configuration.ValidateOnSaveEnabled;
            //try
            //{
            //    _db.Configuration.ValidateOnSaveEnabled = false;
            //    var entry = _db.Entry<AccountBook>(acc);
            //    if (entry.State == EntityState.Detached)
            //    {
            //        _db.AccountBooks.Attach(acc);
            //    }
            //    _db.Entry(acc).State = EntityState.Deleted;
            //}
            //finally
            //{
            //    _db.Configuration.ValidateOnSaveEnabled = oldValidateOnSaveEnabled;
            //}

            //狀況四: 看來是可以運作的方式
            //作法原理: 重新由 DB 讀一次後, 再作 Remove
            //副作用: 由 DB 再讀取一次
            var acc = _db.AccountBooks.Find(item.Id);
            _db.AccountBooks.Remove(acc);

            ////[問題]: 怎麼作才是比較好的方式, 因為手上已經有那筆需被移除的資料了, 實在不需再由 DB 再取一次
        }

        public void DeleteById(Guid id)
        {
            //註: 因為是網頁線上程式, 不是批次, 最好確認一下前端輸入的資料, 確定資料是否存在, 才作刪除.
            //    以本Homework 的功能而言, 效能不是最大的考量因素, 安全性比較需要考慮
            //方式一
            var acc = _db.AccountBooks.Find(id);    //萬一找不到時, acc 會是 null
            if (null == acc)
            {
                throw new Exception("您欲刪除的資料不存在, 請檢查是否已被刪除.");
            }
            _db.AccountBooks.Remove(acc);

            ////方式二
            //var acc = new AccountBook { Id = id };
            //_db.AccountBooks.Attach(acc);
            //_db.AccountBooks.Remove(acc);
        }

        #endregion

        #region 練習 Edit 的功能

        public void Edit(BillingItemViewModel item)
        {
            //註: 因為是網頁線上程式, 不是批次, 最好確認一下前端輸入的資料, 確定資料是否存在, 才作刪除.
            //    以本Homework 的功能而言, 效能不是最大的考量因素, 安全性比較需要考慮
            //方式一: 由資料庫再取一次舊資料
            var oldItem = _db.AccountBooks.Find(item.Id);
            if (null == oldItem)
            {
                throw new Exception("您欲修改的資料不存在, 請檢查是否已被刪除.");
            }
            oldItem.Categoryyy = item.BillType;
            oldItem.Dateee = item.BillDate;
            oldItem.Amounttt = item.Amount;
            oldItem.Remarkkk = item.Memo;

            ////方式二: 採用 Attach 的方式作處理, 不用再先到資料庫取一次舊資料了
            //var acc = new AccountBook
            //{
            //    Id = item.Id,
            //    Categoryyy = item.BillType,
            //    Dateee = item.BillDate,
            //    Amounttt = item.Amount,
            //    Remarkkk = item.Memo
            //};

            //var entry = _db.Entry<AccountBook>(acc);
            //if (entry.State == EntityState.Detached)
            //{
            //    _db.AccountBooks.Attach(acc);
            //}
            //entry.State = EntityState.Modified;

            //[問題]: 
            //1. 雖然說, 用 ViewModel 作隔離, 完全由自己掌控要異動的欄位, 可能不需用到 TryUpdateModel(...) + includes + excludes ?
            //2. 如果想要用的話, 應該要作在那一層? Controller or Service?
            //(1) 如果作在 Controller 層, 因為是 TryUpdateModel() 的對象應該是 ViewModel, 如何反映到 Service 層的 Entity Framework?
            //(2) 如果作在 Service 層, 因為 TryUpdateModel() 會將 POST 上來的資料 (ViewModel),
            //      與由 DB 取出的資料 (Model) 作比對 //本方法裡的第一列敍述  var oldItem = _db.AccountBooks.Find(item.Id);
            //      但因為 property name 不同, TryUpdateModel() 要如何作比對?
        }

        #endregion

        #region 練習 Query 的功能

        public IEnumerable<BillingItemViewModel> GetByQuery(BillQueryViewModel query)
        {
            //建立取全部資料的 SQL
            //註: 以下這些敍述, 都還沒有到 DB 實際存取; 只有在 View 的 foreach, 才會真正取資料
            var bills = this.GetAll();

            //再逐步下 WHERE
            //參考: http://stackoverflow.com/questions/6353350/multiple-where-conditions-in-ef
            if (query.BillType != null && query.BillType > -1)
            {
                bills = bills.Where(x => x.BillType == query.BillType);
            }

            //假設在呼叫之前, 都已驗證過
            if (query.StartDate.HasValue)
            {
                bills = bills.Where(x => x.BillDate >= query.StartDate);
            }
            if (query.EndDate.HasValue)
            {
                bills = bills.Where(x => x.BillDate <= query.EndDate);
            }

            bills = bills.OrderByDescending(x => x.BillDate);
            return bills;

            //===========================================
            //註: 送到 SQL Server 的查詢語句範例
            //===========================================
            //--Region Parameters
            //DECLARE @p0 DateTime = '2016-05-10 00:00:00.000'
            //DECLARE @p1 DateTime = '2016-04-01 00:00:00.000'
            //DECLARE @p2 Int = 1
            //-- EndRegion
            //SELECT[t0].[Id], [t0].[Categoryyy]
            //        AS[BillType], [t0].[Dateee]
            //        AS[BillDate], [t0].[Amounttt]
            //        AS[Amount], [t0].[Remarkkk]
            //        AS[Memo]
            //FROM[AccountBook] AS[t0]
            //WHERE([t0].[Dateee] <= @p0) AND([t0].[Dateee] >= @p1) AND([t0].[Categoryyy] = @p2)
            //ORDER BY[t0].[Dateee]
            //        DESC

        }

        #endregion


        #region 練習 Query 的功能 (by 年月)

        public IEnumerable<BillingItemViewModel> GetByQueryYM(int year, int month)
        {
            //建立取全部資料的 SQL
            //註: 以下這些敍述, 都還沒有到 DB 實際存取; 只有在 View 的 foreach, 才會真正取資料
            var bills = this.GetAll();
            DateTime start = new DateTime(year, month, 1);
            DateTime end = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            //串接 WHERE 條件
            bills = bills.Where(x => x.BillDate >= start);
            bills = bills.Where(x => x.BillDate <= end);

            bills = bills.OrderByDescending(x => x.BillDate);
            return bills;

        }


        #endregion
    }
}