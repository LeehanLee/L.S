using L.S.Model.DatabaseModel.Context;
using L.S.Interface;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        public DbContext context;
        public BaseService(DbContext _context)
        {
            //if (context == null)
            //{
            //    context = new LSContext();                
            //}
            context = _context;
        }

        #region 针对数据库实体集LSContext中表的通用增删改查的泛型实现
        public T Add(T t)
        {

            return context.Set<T>().Add(t);

        }
        public IEnumerable<T> AddRange(List<T> list)
        {
            return context.Set<T>().AddRange(list);
        }
        public T Remove(T t)
        {
            return context.Set<T>().Remove(t);
        }
        public IEnumerable<T> RemoveRange(List<T> list)
        {
            return context.Set<T>().RemoveRange(list);

        }
        public void Update(T entity)
        {
            context.Set<T>().Attach(entity);
            context.Entry<T>(entity).State = EntityState.Modified;
        }
        public bool Exist(Expression<Func<T, bool>> exp)
        {
            var result = context.Set<T>().Any(exp);
            string result_str = result.ToString();
            return result;
        }
        public List<T> GetList(Expression<Func<T, bool>> exp)
        {
            var result = context.Set<T>().Where(exp);
            var result_list = result.ToList();
            return result_list;
        }
        public List<T> GetListForPaging(Expression<Func<T, bool>> exp, int page, int pagesize, out int totalcount, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            if (page <= 0 || pagesize <= 0) { page = 1; pagesize = 10; }
            totalcount = 0;

            var iquerableresult = context.Set<T>().Where(exp);
            totalcount = iquerableresult.Count();
            var result = orderBy(iquerableresult).Skip((page - 1) * pagesize).Take(pagesize);
            var result_list = result.ToList();
            return result_list;
        }
        public IPagedList<T> GetPagedList(Expression<Func<T, bool>> exp, int page, int pagesize, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            if (page <= 0 || pagesize <= 0) { page = 1; pagesize = 10; }

            var iquerableresult = context.Set<T>().Where(exp);
            var result = orderBy(iquerableresult).ToPagedList<T>(page, pagesize);
            return result;
        }
        public T Find(Expression<Func<T, bool>> exp)
        {
            var result = context.Set<T>().FirstOrDefault(exp);
            return result;
        }
        public T Find(string id)
        {

            var result = context.Set<T>().Find(id);
            return result;

        }
        public List<T> GetListWithSql(string sql, out string msg, params SqlParameter[] param)
        {
            msg = string.Empty;
            try
            {
                var result = context.Database.SqlQuery<T>(sql, param).ToList<T>();
                return result;
            }
            catch (Exception ex)
            {
                msg += ex.Message + "\r\n" + ex.StackTrace;
                return null;
            }
        }
        public int ExecuteSql(string sql, out string msg, params SqlParameter[] param)
        {
            msg = string.Empty;
            try
            {
                var result = context.Database.ExecuteSqlCommand(sql, param);
                return result;
            }
            catch (Exception ex)
            {
                msg += ex.Message + "\r\n" + ex.StackTrace;
                return 0;
            }
        }
        public int SaveChanges(out string msg)
        {
            msg = string.Empty;
            try
            {
                return context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var item1 in ex.EntityValidationErrors)
                {
                    foreach (var item2 in item1.ValidationErrors)
                    {
                        msg += item2.ErrorMessage + "<br />";
                    }
                }
                return 0;
            }
        }

        #endregion

        #region Dispose
        /// <summary>
        /// Dispose模式设计的思路基于：如果调用者显式调用了Dispose方法，那么类型就该按部就班为自己的所以资源全部释放掉。如果调用者忘记调用 Dispose方法，
        /// 那么类型就假定自己的所有托管资源（哪怕是那些上段中阐述的非普通类型）全部交给垃圾回收器去回收，而不进行手工清理。理解了这一 点，我们就理解了为什么Dispose方法中，
        /// 虚方法传入的参数是true，而终结器中，虚方法传入的参数是false。
        /// </summary>
        private bool IsDisposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Disposing若为 true，则同时释放托管资源和非托管资源；若为 false，则仅释放非托管资源。
        /// </summary>
        /// <param name="Disposing"></param>
        protected virtual void Dispose(bool Disposing)
        {
            if (!IsDisposed)
            {
                if (Disposing)
                {
                    //清理托管资源
                }
                context.Dispose();//回收非托管资源

            }
            IsDisposed = true;
        }
        /// <summary>
        /// 当使用者没有显示调用Dispose()时由GC完成资源回收功能，此方法即是Finalize()方法，
        /// 当GC认为此对象可回收时将会调用此方法（如果定义了此方法），并自动隐式地回收此对象使用的内存空间；
        /// 如果没有定义此方法，GC将会隐式回收此对象使用的内存空间；
        /// 这意味着此方法由开发人员显式的定义在这里的原因是在这里手动释放外部资源（即非应用程序池内的资源，如数据库连接）
        /// </summary>
        ~BaseService()
        {
            Dispose(false);
        }
        #endregion
    }
}
