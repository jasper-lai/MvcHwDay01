﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var acc = new AccountBook
            {
                Id = item.Id,
                Categoryyy = item.BillType,
                Dateee = item.BillDate,
                Amounttt = item.Amount,
                Remarkkk = item.Memo
            };

            ////狀況一: 
            ////錯誤訊息: The object cannot be deleted because it was not found in the ObjectStateManager 
            //_db.AccountBooks.Remove(acc);

            ////狀況二:
            ////作法參考: http://stackoverflow.com/questions/15945172/the-object-cannot-be-deleted-because-it-was-not-found-in-the-objectstatemanager
            ////例外發生點: _db.AccountBooks.Attach(acc);
            ////錯誤訊息: {"附加類型 'MvcHwDay01.Models.AccountBook' 的實體失敗，因為另一個相同類型的實體已經有相同的主索引鍵值。如果圖形中的任何實體有衝突的索引值鍵，在使用 'Attach' 方法或將實體的狀態設為 'Unchanged' 或 'Modified' 時，就可能發生這種狀況。
            ////          原因可能是有些實體是新增的，尚未收到資料庫產生的索引鍵值。如有這種狀況，請使用 'Add' 方法或 'Added' 實體狀態來追蹤圖形，然後再視需要將非新增實體的狀態設為 'Unchanged' 或 'Modified'。"}
            ////
            ////原因推測: 在 Controller 的 DeleteConfirm() 裡, 有呼叫 _billingSvc.GetSingle(), 在整個 dbcontext 裡, 已存在一筆資料...
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
            var acc2 = _db.AccountBooks.Find(item.Id);
            _db.AccountBooks.Remove(acc2);

            ////[問題]: 怎麼作才是比較好的方式, 可以避開再由資料庫讀一次的問題?
        }


        public void DeleteById(Guid id)
        {
            var acc = _db.AccountBooks.Find(id);
            _db.AccountBooks.Remove(acc);
        }

        #endregion

    }
}