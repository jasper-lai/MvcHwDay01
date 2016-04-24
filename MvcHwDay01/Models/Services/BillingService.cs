using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                               BillType = x.Categoryyy,
                               BillDate = x.Dateee,
                               Amount = x.Amounttt,
                               Memo = x.Remarkkk,
                           });
            return bills;
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
                               BillType = x.Categoryyy,
                               BillDate = x.Dateee,
                               Amount = x.Amounttt,
                               Memo = x.Remarkkk,
                           });
            return bills;
        }


        #endregion



    }
}