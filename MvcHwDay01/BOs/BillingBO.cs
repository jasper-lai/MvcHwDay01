using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MvcHwDay01.Areas.Admin.ViewModels;
using MvcHwDay01.Repositories;
using MvcHwDay01.Models.Globals;
using MvcHwDay01.Models;

namespace MvcHwDay01.BOs
{

    public class BillingBO
    {
        private readonly IRepository<AccountBook> _billingRep;

        public BillingBO(IUnitOfWork unitOfWork) //: base(unitOfWork)
        {
            _billingRep = new Repository<AccountBook>(unitOfWork);
        }

        #region 新增資料

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

            _billingRep.Create(acc);
        }

        #endregion

        #region 修改資料

        public void Edit(BillingItemViewModel item)
        {
            //註: 因為是網頁線上程式, 不是批次, 最好確認一下前端輸入的資料, 確定資料是否存在, 才作刪除.
            //    以本Homework 的功能而言, 效能不是最大的考量因素, 安全性比較需要考慮
            //原理: 由資料庫再取一次舊資料, 確認資料存在後, 才作修改
            var oldItem = _billingRep.GetSingle(x => x.Id == item.Id);
            if (null == oldItem)
            {
                throw new Exception("您欲修改的資料不存在, 請檢查是否已被刪除.");
            }
            oldItem.Categoryyy = item.BillType;
            oldItem.Dateee = item.BillDate;
            oldItem.Amounttt = item.Amount;
            oldItem.Remarkkk = item.Memo;
        }

        #endregion

        #region 刪除資料

        public void Delete(BillingItemViewModel item)
        {
            //原理: 由資料庫再取一次舊資料, 確認資料存在後, 才作刪除
            var oldItem = _billingRep.GetSingle(x => x.Id == item.Id);
            if (null == oldItem)
            {
                throw new Exception("您欲刪除的資料不存在, 請檢查是否已被刪除.");
            }
            _billingRep.Remove(oldItem);
        }

        public void DeleteById(Guid id)
        {
            //原理: 由資料庫再取一次舊資料, 確認資料存在後, 才作刪除
            var oldItem = _billingRep.GetSingle(x => x.Id == id);
            if (null == oldItem)
            {
                throw new Exception("您欲刪除的資料不存在, 請檢查是否已被刪除.");
            }
            _billingRep.Remove(oldItem);
        }


        #endregion

        //將 Commit 的功能, 移到 UnitOfWork, 比較符合資料庫的操作
        //#region 確定寫入資料庫

        //public void Save()
        //{
        //    _billingRep.Commit();
        //}

        //#endregion

        #region 查詢資料 (GetAll / GetAllCount / GetTopN / GetSingle )

        /// <summary>
        /// 取回所有資料
        /// </summary>
        /// <returns></returns>
        public IQueryable<BillingItemViewModel> GetAll()
        {
            var bills = _billingRep.GetAll()
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
            result = _billingRep.GetAll().Count();
            return result;
        }

        /// <summary>
        /// 取回前 N 筆資料
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public IEnumerable<BillingItemViewModel> GetTopN(int top)
        {
            var bills = _billingRep.GetAll()
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

        /// <summary>
        /// 以 Id 作查詢, 取回一筆資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BillingItemViewModel GetSingle(Guid id)
        {
            var acc = _billingRep.GetSingle(x => x.Id == id);
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

        #endregion

        #region 查詢資料 ( GetSkipMTakeN / GetByQuery / GetByQueryYM)

        /// <summary>
        /// 取回 N 筆資料 (先跳開前 M 筆)
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<BillingItemViewModel> GetSkipMTakeN(int skip, int take)
        {
            var bills = _billingRep.GetAll()
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