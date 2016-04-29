using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace L.S.DAL.Interface
{
    public interface IBaseDBService<T> where T:class
    {
        bool Insert(T t,out string msg);
        bool AddRange(List<T> list, out string msg);
        bool Delete(T t, out string msg);
        bool RemoveRange(List<T> list, out string msg);
        bool Update(T t, out string msg);
        bool Exist(Expression<Func<T, bool>> exp, out string msg);
        T Find(Expression<Func<T, bool>> exp, out string msg);
        List<T> GetList(Expression<Func<T, bool>> exp, out string msg);
        List<T> GetListForPaging(Expression<Func<T, bool>> exp, int page, int pagesize, out int totalcount, out string msg, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);

        /// <summary>
        /// 直接使用PagedList从数据库加载分页数据
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="msg"></param>
        /// <param name="orderBy">如：modellist=>modellist.OrderByDescending(su=>su.ID)</param>
        /// <returns></returns>
        IPagedList<T> GetPagedList(Expression<Func<T, bool>> exp, int page, int pagesize, out string msg, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        List<T> GetListWithSql(string sql, out string msg, params SqlParameter[] param);
        int ExecuteSql(string sql, out string msg, params SqlParameter[] param);
        
    }
}
