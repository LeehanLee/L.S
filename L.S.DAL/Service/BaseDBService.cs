using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L.S.DAL.Interface;
using L.S.DAL.Model.DBModels;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Data.Entity.Validation;
using PagedList;

namespace L.S.DAL.Service
{
    public class BaseDBService<T> : IBaseDBService<T> where T : class
    {
        #region 针对数据库实体集L_Sys中表的通用增删改查的泛型实现
        public bool Insert(T t, out string msg)
        {
            msg = string.Empty;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    context.Set<T>().Add(t);
                    return context.SaveChanges() > 0;
                }
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
                return false;
            }
        }
        public bool AddRange(List<T> list, out string msg)
        {
            msg = string.Empty;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    context.Set<T>().AddRange(list);
                    return context.SaveChanges() > 0;
                }
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
                return false;
            }
        }
        public bool Delete(T t, out string msg)
        {
            msg = string.Empty;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    context.Set<T>().Remove(t);
                    return context.SaveChanges() > 0;
                }
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
                return false;
            }
        }
        public bool RemoveRange(List<T> list, out string msg)
        {
            msg = string.Empty;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    context.Set<T>().RemoveRange(list);
                    return context.SaveChanges() > 0;
                }
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
                return false;
            }
        }
        public bool Update(T entity, out string msg)
        {
            msg = string.Empty;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    context.Set<T>().Attach(entity);
                    context.Entry<T>(entity).State = EntityState.Modified;
                    return context.SaveChanges() > 0;
                }
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
                return false;
            }
        }
        public bool Exist(Expression<Func<T, bool>> exp, out string msg)
        {
            msg = string.Empty;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    var result = context.Set<T>().Any(exp);
                    string result_str = result.ToString();
                    return result;
                }
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
                return false;
            }
        }
        public List<T> GetList(Expression<Func<T, bool>> exp, out string msg)
        {
            msg = string.Empty;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    var result = context.Set<T>().Where(exp);
                    var result_list = result.ToList();
                    return result_list;
                }
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
                return null;
            }
        }
        public List<T> GetListForPaging(Expression<Func<T, bool>> exp, int page, int pagesize, out int totalcount, out string msg, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            if (page <= 0 || pagesize <= 0) { page = 1; pagesize = 10; }
            msg = string.Empty;
            totalcount = 0;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    var iquerableresult = context.Set<T>().Where(exp);
                    totalcount = iquerableresult.Count();
                    var result = orderBy(iquerableresult).Skip((page - 1) * pagesize).Take(pagesize);
                    var result_list = result.ToList();
                    return result_list;
                }
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
                return null;
            }
        }
        public IPagedList<T> GetPagedList(Expression<Func<T, bool>> exp, int page, int pagesize, out string msg, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            if (page <= 0 || pagesize <= 0) { page = 1; pagesize = 10; }
            msg = string.Empty;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    var iquerableresult = context.Set<T>().Where(exp);
                    var result = orderBy(iquerableresult).ToPagedList<T>(page, pagesize);
                    return result;
                }
            }
            catch (Exception ex)
            {
                msg += ex.Message + "<br />" + ex.StackTrace + "<br />";
                return null;
            }
        }
        public T Find(Expression<Func<T, bool>> exp, out string msg)
        {
            msg = string.Empty;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    var result = context.Set<T>().FirstOrDefault(exp);
                    return result;
                }
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
                return null;
            }
        }
        public List<T> GetListWithSql(string sql, out string msg, params SqlParameter[] param)
        {
            msg = string.Empty;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    var result = context.Database.SqlQuery<T>(sql, param).ToList<T>();
                    
                    return result;
                }
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
                return null;
            }
        }
        public int ExecuteSql(string sql, out string msg, params SqlParameter[] param)
        {
            msg = string.Empty;
            try
            {
                using (L_Sys context = new L_Sys())
                {
                    var result = context.Database.ExecuteSqlCommand(sql, param);
                    return result;
                }
            }
            catch (Exception ex)
            {
                msg += ex.Message+"\r\n"+ex.StackTrace;
                return 0;
            }
        }
        #endregion

    }
}
