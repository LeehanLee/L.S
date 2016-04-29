
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L.S.Home.Models
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using L.S.Model.DatabaseModel.Entity;
    using L.S.Service;
    using L.S.Interface;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.Mvc;
    using Study.Common.Cache;
    using Autofac.Configuration;
    using BLL;
    public class IocConfig
    {
        public static IContainer Container;
        public static void RegisterDependencies()
        {
            var ContainerBuilder = new ContainerBuilder();
            ContainerBuilder.RegisterControllers(typeof(MvcApplication).Assembly);
            ContainerBuilder.RegisterType<RoleBLL>();
            //builder.Register(us => new UserService()).As<IUserService>();
            //builder.Register(us => new DepService()).As<IDepService>();
            //builder.Register(us => new RoleService()).As<IRoleService>();
            //builder.Register(us => new RightService()).As<IRightService>();
            ContainerBuilder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            #region 如果为Winform类型，请使用以下获取Assembly方法
            //builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).Where(t => t.Name.EndsWith("Services")).AsImplementedInterfaces();
            ////查找程序集中以services结尾的类型
            //builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();
            ////查找程序集中以Repository结尾的类型 
            #endregion

            #region 如果有web类型，请使用如下获取Assenbly方法
            //var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            //builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
            //泛型接口这里好像resove不了？？-_ -||，B了狗
            //查找程序集中以Service结尾的类型
            #endregion

            //autofac 注册依赖
            Container = ContainerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }
    }
}