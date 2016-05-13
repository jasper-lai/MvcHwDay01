using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using MvcHwDay01.Models.Globals;

namespace MvcHwDay01.Repositories
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// unit of work
        /// </summary>
        IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Lookups all.
        /// </summary>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        IQueryable<T> GetAll();

        #region 以下看來不應這麼作, 因為包含了商業邏輯, 應該讓 Reqpositoy 單純化, 只處理資料庫存取

        ///// <summary>
        ///// Get Top N
        ///// </summary>
        ///// <param name="top"></param>
        ///// <param name="orderby"></param>
        ///// <param name="order"></param>
        ///// <returns></returns>
        //IQueryable<T> GetTopN(int top, Expression<Func<T, bool>> orderby = null, SortOrder order = SortOrder.Ascending)

        ///// <summary>
        ///// 排序(OrderBy)
        ///// </summary>
        ///// <param name="orderby"></param>
        ///// <param name="order"></param>
        ///// <returns></returns>
        //IQueryable<T> OrderBy(Expression<Func<T, bool>> orderby, SortOrder order = SortOrder.Ascending);

        ///// <summary>
        ///// 排序(OrderByAndThenBy)
        ///// </summary>
        ///// <param name="orderby"></param>
        ///// <param name="order"></param>
        ///// <param name="thenby"></param>
        ///// <param name="then"></param>
        ///// <returns></returns>
        ///// <remarks>
        ///// 先作2個欄位的排序處理, 有時間再作多個欄位
        ///// </remarks>
        //IQueryable<T> OrderByAndThenBy(Expression<Func<T, bool>> orderby,SortOrder order = SortOrder.Ascending,
        //                                Expression<Func<T, bool>> thenby, SortOrder then = SortOrder.Ascending);

        #endregion

        /// <summary>
        /// 搜尋
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Query(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 取得單一 entity
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        T GetSingle(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 新增一個entity
        /// </summary>
        /// <param name="entity"></param>
        void Create(T entity);

        /// <summary>
        /// 刪除單一entity
        /// </summary>
        /// <param name="entity"></param>
        void Remove(T entity);

        /// <summary>
        /// save change
        /// </summary>
        void Commit();

    }
}