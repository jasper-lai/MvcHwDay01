using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Data.Entity;
using MvcHwDay01.Models.Globals;

namespace MvcHwDay01.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public IUnitOfWork UnitOfWork { get; set; }
        private DbSet<T> _Objectset;

        private DbSet<T> ObjectSet
        {
            get
            {
                if (_Objectset == null)
                {
                    _Objectset = UnitOfWork.Context.Set<T>();
                }
                return _Objectset;
            }
        }

        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IQueryable<T> GetAll()
        {
            return ObjectSet;
        }

        #region 以下看來不應這麼作, 因為包含了商業邏輯, 應該讓 Reqpositoy 單純化, 只處理資料庫存取

        //public IQueryable<T> GetTopN(int top, Expression<Func<T, bool>> orderby = null, SortOrder order = SortOrder.Ascending)
        //{
        //    var result = ObjectSet.AsQueryable();
        //    if ( null != orderby)
        //    {
        //        if (order == SortOrder.Ascending)
        //        {
        //            result = result.OrderBy(orderby).Take(top);
        //        }
        //        else
        //        {
        //            result = result.OrderByDescending(orderby).Take(top);
        //        }
        //    }
        //    else
        //    {
        //        result = result.Take(top);
        //    }
        //    return result;
        //}

        //public IQueryable<T> OrderBy(Expression<Func<T, bool>> orderby, SortOrder order = SortOrder.Ascending)
        //{
        //    var result = ObjectSet.AsQueryable();
        //    result = (order == SortOrder.Ascending) ?
        //                result.OrderBy(orderby) :
        //                result.OrderByDescending(orderby);
        //    return result;
        //}

        //public IQueryable<T> OrderByAndThenBy(Expression<Func<T, bool>> orderby, SortOrder order = SortOrder.Ascending,
        //                                Expression<Func<T, bool>> thenby, SortOrder then = SortOrder.Ascending)
        //{
        //    var result = ObjectSet.AsQueryable();
        //    if (order == SortOrder.Ascending)
        //    {
        //        result = (then == SortOrder.Ascending) ?
        //                   result.OrderBy(orderby).ThenBy(thenby) :
        //                   result.OrderBy(orderby).ThenByDescending(thenby);
        //    }
        //    else
        //    {
        //        result = (then == SortOrder.Ascending) ?
        //                   result.OrderByDescending(orderby).ThenBy(thenby) :
        //                   result.OrderByDescending(orderby).ThenByDescending(thenby);
        //    }

        //    return result;
        //}

        #endregion

        public IQueryable<T> Query(Expression<Func<T, bool>> filter)
        {
            return ObjectSet.Where(filter);
        }

        public T GetSingle(Expression<Func<T, bool>> filter)
        {
            return ObjectSet.SingleOrDefault(filter);
        }

        public void Create(T entity)
        {
            ObjectSet.Add(entity);
        }

        public void Remove(T entity)
        {
            ObjectSet.Remove(entity);
        }

        //將 Commit 的功能, 移到 UnitOfWork, 比較符合資料庫的操作
        //public void Commit()
        //{
        //    UnitOfWork.Save();
        //}
    }
}