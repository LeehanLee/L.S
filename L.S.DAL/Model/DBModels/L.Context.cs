﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace L.S.DAL.Model.DBModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class L_Sys : DbContext
    {
        public L_Sys()
            : base("name=L_Sys")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Dep> Deps { get; set; }
        public virtual DbSet<DepUser> DepUsers { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuDep> MenuDeps { get; set; }
        public virtual DbSet<MenuRole> MenuRoles { get; set; }
        public virtual DbSet<MenuUser> MenuUsers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleUser> RoleUsers { get; set; }
        public virtual DbSet<SysUser> SysUsers { get; set; }
    }
}