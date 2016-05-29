namespace L.S.Model.DatabaseModel.Context
{
    using L.S.Model.DatabaseModel.Entity;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class LSContext : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“LSContext”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“L.S.Model.DatabaseModel.Context.LSContext”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“LSContext”
        //连接字符串。
        public LSContext(): base("name=LSContext")
        {
        }

        public virtual DbSet<SysUser> SysUsers { get; set; }
        public virtual DbSet<SysDep> SysDeps { get; set; }
        public virtual DbSet<SysRole> SysRoles { get; set; }
        public virtual DbSet<SysUserRole> SysUserRoles { get; set; }
        public virtual DbSet<SysRight> SysRights { get; set; }
        public virtual DbSet<SysRoleRight> SysRoleRights { get; set; }
        public virtual DbSet<CategoryType> CategoryTypes { get; set; }

        public virtual DbSet<Info> Infoes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}