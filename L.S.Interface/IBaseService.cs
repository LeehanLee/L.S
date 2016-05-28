using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace L.S.Interface
{
    public interface IBaseService<T> : IDisposable where T : class
    {
        T Add(T t);
        IEnumerable<T> AddRange(List<T> list);
        T Remove(T t);
        IEnumerable<T> RemoveRange(List<T> list);
        void Update(T t);
        bool Exist(Expression<Func<T, bool>> exp);
        T Find(string id);
        T Find(Expression<Func<T, bool>> exp);
        List<T> GetList(Expression<Func<T, bool>> exp);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> exp);
        List<T> GetListForPaging(Expression<Func<T, bool>> exp, int page, int pagesize, out int totalcount, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);

        /// <summary>
        /// 直接使用PagedList从数据库加载分页数据
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="msg"></param>
        /// <param name="orderBy">如：modellist=>modellist.OrderByDescending(su=>su.ID)</param>
        /// <returns></returns>
        IPagedList<T> GetPagedList(Expression<Func<T, bool>> exp, int page, int pagesize, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        List<T> GetListWithSql(string sql, out string msg, params SqlParameter[] param);
        int ExecuteSql(string sql, out string msg, params SqlParameter[] param);
        int SaveChanges(out string msg);

    }

}
