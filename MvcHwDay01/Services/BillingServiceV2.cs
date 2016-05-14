using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MvcHwDay01.Areas.Admin.ViewModels;
using MvcHwDay01.Repositories;
using MvcHwDay01.Models.Globals;
using MvcHwDay01.Models;
using MvcHwDay01.BOs;

namespace MvcHwDay01.Services
{

    public class BillingService : IDisposable
    {
        EFUnitOfWork uow = null;
        private readonly BillingBO _billingBo = null;

        public BillingService()
        {
            //共用同一條連線 (in Service constructor)
            uow = new EFUnitOfWork();
            _billingBo = new BillingBO(uow);
        }

        #region 新增資料

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="item"></param>
        public void Add(BillingItemViewModel item)
        {
            //不可在這裡作 using, 萬一新增完後, 要作查詢, 那個 uow 就不見了
            //using (uow)
            //{
            //    _billingBo.Add(item);
            //    uow.Commit();
            //}

            _billingBo.Add(item);
            uow.Commit();
        }

        #endregion

        #region 修改資料

        public void Edit(BillingItemViewModel item)
        {
            _billingBo.Edit(item);
            uow.Commit();
        }

        #endregion

        #region 刪除資料

        public void Delete(BillingItemViewModel item)
        {
            _billingBo.Delete(item);
            uow.Commit();
        }

        public void DeleteById(Guid id)
        {
            _billingBo.DeleteById(id);
            uow.Commit();
        }


        #endregion

        #region 查詢資料 (GetAll / GetAllCount / GetTopN / GetSingle )

        /// <summary>
        /// 取回所有資料
        /// </summary>
        /// <returns></returns>
        public IQueryable<BillingItemViewModel> GetAll()
        {
            var bills = _billingBo.GetAll();
            return bills;
        }

        /// <summary>
        /// 取回總筆數, 以供分頁參考之用
        /// </summary>
        /// <returns></returns>
        public int GetAllCount()
        {
            int result = 0;
            result = _billingBo.GetAllCount();
            return result;
        }

        /// <summary>
        /// 取回前 N 筆資料
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public IEnumerable<BillingItemViewModel> GetTopN(int top)
        {
            var bills = _billingBo.GetTopN(top);
            return bills;
        }

        /// <summary>
        /// 以 Id 作查詢, 取回一筆資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BillingItemViewModel GetSingle(Guid id)
        {
            var bill = _billingBo.GetSingle(id);
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
            var bills = _billingBo.GetSkipMTakeN(skip, take);
            return bills;
        }

        public IEnumerable<BillingItemViewModel> GetByQuery(BillQueryViewModel query)
        {
            var bills = _billingBo.GetByQuery(query);
            return bills;
        }

        public IEnumerable<BillingItemViewModel> GetByQueryYM(int year, int month)
        {
            var bills = _billingBo.GetByQueryYM(year, month);
            return bills;
        }

        #endregion

        #region 實作 IDisposable

        public void Dispose()
        {
            uow.Dispose();
        }

        #endregion

    }

}