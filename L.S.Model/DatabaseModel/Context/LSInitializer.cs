using L.S.Model.DatabaseModel.Entity;
using L.Study.Common.Log;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.S.Model.DatabaseModel.Context
{
    public class LSInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<LSContext>
    {
        protected override void Seed(LSContext context)
        {
            string msg = "";
            try
            {
                SysDep root = new SysDep() { ID = "root", Name = "LSroot", AddBy = "init", AddByName = "init", AddDate = DateTime.Now, IsDel = false, IsAvailable = true, Parent = null, DepFullIDPath = "/root/", DepFullNamePath = "/LSroot/" };
                context.SysDeps.Add(root);
                context.SaveChanges();
                SysDep center = new SysDep { ID = "manage-center", Name = "平台中心", AddBy = "init", AddByName = "init", AddDate = DateTime.Now, IsDel = false, IsAvailable = true, ParentID = "root", ParentName = "LSroot", DepFullIDPath = "/root/manage-cente/", DepFullNamePath = "/LSroot/平台中心/" };
                context.SysDeps.Add(center);
                context.SaveChanges();
                var users = new List<SysUser>
                {
                new SysUser{ID="2016012921540001",Password="123456",Name="lihan",LoginName="lihan",AddBy="init",AddByName="init",AddDate=DateTime.Now, IsDel=false,IsAvailable=true,SysDepID="root",SysDepName="LSroot",Birthday=new DateTime(1995,5,5,5,5,5)},
                new SysUser{ID="2016012921540003",Password="123456",Name="b1551",LoginName="b1551",AddBy="init",AddByName="init",AddDate=DateTime.Now, IsDel=false,IsAvailable=true,SysDepID="root",SysDepName="LSroot",Birthday=new DateTime(1995,5,5,5,5,5)},
                new SysUser{ID="2016012921540004",Password="123456",Name="a1551",LoginName="a1551",AddBy="init",AddByName="init",AddDate=DateTime.Now, IsDel=false,IsAvailable=true,SysDepID="root",SysDepName="LSroot",Birthday=new DateTime(1995,5,5,5,5,5)},
                new SysUser{ID="2016012921540005",Password="123456",Name="t1551",LoginName="t1551",AddBy="init",AddByName="init",AddDate=DateTime.Now, IsDel=false,IsAvailable=true,SysDepID="root",SysDepName="LSroot",Birthday=new DateTime(1995,5,5,5,5,5)},
                new SysUser{ID="2016012921540006",Password="123456",Name="d1551",LoginName="d1551",AddBy="init",AddByName="init",AddDate=DateTime.Now, IsDel=false,IsAvailable=true,SysDepID="root",SysDepName="LSroot",Birthday=new DateTime(1995,5,5,5,5,5)},
                new SysUser{ID="2016012921540007",Password="123456",Name="z1551",LoginName="z1551",AddBy="init",AddByName="init",AddDate=DateTime.Now, IsDel=false,IsAvailable=true,SysDepID="root",SysDepName="LSroot",Birthday=new DateTime(1995,5,5,5,5,5)},
                new SysUser{ID="2016012921540002",Password="123456",Name="c1551",LoginName="c1551",AddBy="init",AddByName="init",AddDate=DateTime.Now, IsDel=false,IsAvailable=true,SysDepID="root",SysDepName="LSroot",Birthday=new DateTime(1995,5,5,5,5,5)},
                new SysUser{ID="2016012921540008",Password="123456",Name="super_man",LoginName="super_man",AddBy="init",AddByName="init",AddDate=DateTime.Now, IsDel=false,IsAvailable=true,SysDepID="root",SysDepName="LSroot"}
                };
                context.SysUsers.AddRange(users);
                context.SaveChanges();

                var super = new SysRole { Name = "超级管理员", DefaultHomePath = "/admin/sysuser/", Level = 0, ID = "superadmin", ParentID = null, ParentName = null, RoleIDPath = "/superadmin/", RoleNamePath = "/超级管理员/", AddBy = "init", AddByName = "init", AddDate = DateTime.Now, IsDel = false, IsAvailable = true, };
                context.SysRoles.Add(super);
                context.SaveChanges();
                var admin = new SysRole { Name = "系统管理员", DefaultHomePath = "/admin/sysuser/", Level = 1, ID = "admin", ParentID = "superadmin", ParentName = "超级管理员", RoleIDPath = "/superadmin/admin/", RoleNamePath = "/超级管理员/系统管理员/", AddBy = "init", AddByName = "init", AddDate = DateTime.Now.AddSeconds(1), IsDel = false, IsAvailable = true, };
                context.SysRoles.Add(admin);
                context.SaveChanges();
                var normaluser = new SysRole { Name = "普通用户", DefaultHomePath = "/admin/sysuser/", Level = 2, ID = "normaluser", ParentID = "admin", ParentName = "系统管理员", RoleIDPath = "/superadmin/admin/normaluser/", RoleNamePath = "/超级管理员/系统管理员/普通用户/", AddBy = "init", AddByName = "init", AddDate = DateTime.Now.AddSeconds(2), IsDel = false, IsAvailable = true, };
                context.SysRoles.Add(normaluser);
                context.SaveChanges();

                var userroles = new List<SysUserRole>
                    {
                    new SysUserRole{ID="20160129215400021",UserID="2016012921540001",RoleID="superadmin"},
                    new SysUserRole{ID="20160129215400022",UserID="2016012921540002",RoleID="superadmin"},
                    new SysUserRole{ID="20160129215400023",UserID="2016012921540003",RoleID="superadmin"},
                    new SysUserRole{ID="20160129215400024",UserID="2016012921540004",RoleID="superadmin"},
                    new SysUserRole{ID="20160129215400025",UserID="2016012921540005",RoleID="superadmin"},
                    new SysUserRole{ID="20160129215400026",UserID="2016012921540006",RoleID="superadmin"},
                    new SysUserRole{ID="20160129215400027",UserID="2016012921540007",RoleID="superadmin"},
                    new SysUserRole{ID="20160129215400028",UserID="2016012921540008",RoleID="superadmin"}
                    };
                userroles.ForEach(s => context.SysUserRoles.Add(s));
                context.SaveChanges();

                var SysManage = new SysRight { SortNo = 0, RightIDPath = "/SysManage/", ID = "SysManage", RightLevel = 0, Name = "系统管理", DisplayName = "系统管理", ParentID = null, ActionType = "View", MenuUrl = "/admin/sysuser/", AddBy = "init", AddByName = "init", AddDate = DateTime.Now, IsAvailable = true, IsDel = false };
                var InfoManage = new SysRight { SortNo = 1, RightIDPath = "/InfoManage/", ID = "InfoManage", RightLevel = 0, Name = "资讯管理", DisplayName = "资讯管理", ParentID = null, ActionType = "View", MenuUrl = "/admin/info/", AddBy = "init", AddByName = "init", AddDate = DateTime.Now, IsAvailable = true, IsDel = false };
                context.SysRights.Add(SysManage);
                context.SysRights.Add(InfoManage);
                context.SaveChanges();
                var rightList1 = new List<SysRight>
                {
                    new SysRight {SortNo=1, RightIDPath= "/SysManage/UsersManage/",RightLevel=1,ID="UsersManage",Name="用户管理",DisplayName="用户管理",ParentID="SysManage",ActionType="View",MenuUrl="/admin/sysuser",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=2, RightIDPath= "/SysManage/DepsManage/",RightLevel=1,ID="DepsManage",Name="部门管理",DisplayName="部门管理",ParentID="SysManage",ActionType="View",MenuUrl="/admin/sysdep/treeindex",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=3, RightIDPath= "/SysManage/RolesManage/",RightLevel=1,ID="RolesManage",Name="角色管理",DisplayName="角色管理",ParentID="SysManage",ActionType="View",MenuUrl="/admin/sysrole/treeindex",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=4, RightIDPath= "/SysManage/RightsManage/",RightLevel=1,ID="RightsManage",Name="权限管理",DisplayName="权限管理",ParentID="SysManage",ActionType="View",MenuUrl="/admin/sysright/treeindex",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=5, RightIDPath= "/SysManage/CategoryTypeManage/",RightLevel=1,ID="CategoryTypeManage",Name="分类类型管理",DisplayName="分类类型管理",ParentID="SysManage",ActionType="View",MenuUrl="/admin/categorytype/index",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=6, RightIDPath= "/SysManage/CategoryManage/",RightLevel=1,ID="CategoryManage",Name="分类管理",DisplayName="分类管理",ParentID="SysManage",ActionType="View",MenuUrl="/admin/category/index",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},

                    new SysRight {SortNo=2, RightIDPath= "/InfoManage/InfoList/",RightLevel=1,ID="InfoList",Name="资讯列表",DisplayName="资讯列表",ParentID="InfoManage",ActionType="View",MenuUrl="/admin/info",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                };
                context.SysRights.AddRange(rightList1);
                context.SaveChanges();
                var rightList2 = new List<SysRight>
                {
                    new SysRight {SortNo=1, RightIDPath= "/SysManage/UsersManage/UserCreate",RightLevel=2, ID="UserCreate",Name="用户添加",DisplayName="添加",ParentID="UsersManage",Position="ListTop",ActionType="View",MenuUrl="/admin/sysuser/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=2, RightIDPath= "/SysManage/UsersManage/UserEdit",RightLevel=2, ID="UserEdit",Name="用户编辑",DisplayName="编辑",ParentID="UsersManage",Position="ListRight",ActionType="View",MenuUrl="/admin/sysuser/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=3, RightIDPath= "/SysManage/UsersManage/UserDelete",RightLevel=2, ID="UserDelete",Name="用户删除",DisplayName="删除",ParentID="UsersManage",Position="ListTop",ActionType="Delete",MenuUrl="/admin/sysuser/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=4, RightIDPath= "/SysManage/UsersManage/UserAvailable",RightLevel=2, ID="UserAvailable",Name="用户启用",DisplayName="启用",ParentID="UsersManage",Position="ListTop",ActionType="Available",MenuUrl="/admin/sysuser/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=5, RightIDPath= "/SysManage/UsersManage/UserUnAvailable",RightLevel=2, ID="UserUnAvailable",Name="用户禁用",DisplayName="禁用",ParentID="UsersManage",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/sysuser/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},

                    new SysRight {SortNo=1, RightIDPath= "/SysManage/DepsManage/DepCreate/",RightLevel=2, ID="DepCreate",Name="部门添加",DisplayName="添加",ParentID="DepsManage",Position="ListTop",ActionType="View",MenuUrl="/admin/sysdep/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=2, RightIDPath= "/SysManage/DepsManage/DepEdit/",RightLevel=2, ID="DepEdit",Name="部门编辑",DisplayName="编辑",ParentID="DepsManage",Position="ListRight",ActionType="View",MenuUrl="/admin/sysdep/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=3, RightIDPath= "/SysManage/DepsManage/DepDelete/",RightLevel=2, ID="DepDelete",Name="部门删除",DisplayName="删除",ParentID="DepsManage",Position="ListTop",ActionType="Delete",MenuUrl="/admin/sysdep/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=4, RightIDPath= "/SysManage/DepsManage/DepAvailable/",RightLevel=2, ID="DepAvailable",Name="部门启用",DisplayName="启用",ParentID="DepsManage",Position="ListTop",ActionType="Available",MenuUrl="/admin/sysdep/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=5, RightIDPath= "/SysManage/DepsManage/DepUnAvailable/",RightLevel=2, ID="DepUnAvailable",Name="部门禁用",DisplayName="禁用",ParentID="DepsManage",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/sysdep/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                                                           
                    new SysRight {SortNo=1, RightIDPath= "/SysManage/RolesManage/RoleCreate/",RightLevel=2, ID="RoleCreate",Name="角色添加",DisplayName="添加",ParentID="RolesManage",Position="ListTop",ActionType="View",MenuUrl="/admin/sysrole/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=2, RightIDPath= "/SysManage/RolesManage/RoleEdit/",RightLevel=2, ID="RoleEdit",Name="角色编辑",DisplayName="编辑",ParentID="RolesManage",Position="ListRight",ActionType="View",MenuUrl="/admin/sysrole/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=3, RightIDPath= "/SysManage/RolesManage/RoleDelete/",RightLevel=2, ID="RoleDelete",Name="角色删除",DisplayName="删除",ParentID="RolesManage",Position="ListTop",ActionType="Delete",MenuUrl="/admin/sysrole/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=4, RightIDPath= "/SysManage/RolesManage/RoleAvailable/",RightLevel=2, ID="RoleAvailable",Name="角色启用",DisplayName="启用",ParentID="RolesManage",Position="ListTop",ActionType="Available",MenuUrl="/admin/sysrole/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=5, RightIDPath= "/SysManage/RolesManage/RoleUnAvailable/",RightLevel=2, ID="RoleUnAvailable",Name="角色禁用",DisplayName="禁用",ParentID="RolesManage",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/sysrole/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                                                           
                    new SysRight {SortNo=1, RightIDPath= "/SysManage/RightsManage/RightCreate/",RightLevel=2, ID="RightCreate",Name="权限添加",DisplayName="添加",ParentID="RightsManage",Position="ListTop",ActionType="View",MenuUrl="/admin/sysright/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=2, RightIDPath= "/SysManage/RightsManage/RightEdit/",RightLevel=2, ID="RightEdit",Name="权限编辑",DisplayName="编辑",ParentID="RightsManage",Position="ListRight",ActionType="View",MenuUrl="/admin/sysright/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=3, RightIDPath= "/SysManage/RightsManage/RightDelete/",RightLevel=2, ID="RightDelete",Name="权限删除",DisplayName="删除",ParentID="RightsManage",Position="ListTop",ActionType="Delete",MenuUrl="/admin/sysright/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=4, RightIDPath= "/SysManage/RightsManage/RightAvailable/",RightLevel=2, ID="RightAvailable",Name="权限启用",DisplayName="启用",ParentID="RightsManage",Position="ListTop",ActionType="Available",MenuUrl="/admin/sysright/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=5, RightIDPath= "/SysManage/RightsManage/RightUnAvailable/",RightLevel=2, ID="RightUnAvailable",Name="权限禁用",DisplayName="禁用",ParentID="RightsManage",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/sysright/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},

                    new SysRight {SortNo=1, RightIDPath= "/SysManage/CategoryTypeManage/CategoryTypeCreate/",RightLevel=2, ID="CategoryTypeCreate",Name="分类类型添加",DisplayName="添加",ParentID="CategoryTypeManage",Position="ListTop",ActionType="View",MenuUrl="/admin/categorytype/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=2, RightIDPath= "/SysManage/CategoryTypeManage/CategoryTypeEdit/",RightLevel=2, ID="CategoryTypeEdit",Name="分类类型编辑",DisplayName="编辑",ParentID="CategoryTypeManage",Position="ListRight",ActionType="View",MenuUrl="/admin/categorytype/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=3, RightIDPath= "/SysManage/CategoryTypeManage/CategoryTypeDelete/",RightLevel=2, ID="CategoryTypeDelete",Name="分类类型删除",DisplayName="删除",ParentID="CategoryTypeManage",Position="ListTop",ActionType="Delete",MenuUrl="/admin/categorytype/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=4, RightIDPath= "/SysManage/CategoryTypeManage/CategoryTypeAvailable/",RightLevel=2, ID="CategoryTypeAvailable",Name="分类类型启用",DisplayName="启用",ParentID="CategoryTypeManage",Position="ListTop",ActionType="Available",MenuUrl="/admin/categorytype/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=5, RightIDPath= "/SysManage/CategoryTypeManage/CategoryTypeUnAvailable/",RightLevel=2, ID="CategoryTypeUnAvailable",Name="分类类型禁用",DisplayName="禁用",ParentID="CategoryTypeManage",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/categorytype/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},

                    new SysRight {SortNo=1, RightIDPath= "/SysManage/CategoryManage/CategoryCreate/",RightLevel=2, ID="CategoryCreate",Name="分类添加",DisplayName="添加",ParentID="CategoryManage",Position="ListTop",ActionType="View",MenuUrl="/admin/category/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=2, RightIDPath= "/SysManage/CategoryManage/CategoryEdit/",RightLevel=2, ID="CategoryEdit",Name="分类编辑",DisplayName="编辑",ParentID="CategoryManage",Position="ListRight",ActionType="View",MenuUrl="/admin/category/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=3, RightIDPath= "/SysManage/CategoryManage/CategoryDelete/",RightLevel=2, ID="CategoryDelete",Name="分类删除",DisplayName="删除",ParentID="CategoryManage",Position="ListTop",ActionType="Delete",MenuUrl="/admin/category/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=4, RightIDPath= "/SysManage/CategoryManage/CategoryAvailable/",RightLevel=2, ID="CategoryAvailable",Name="分类启用",DisplayName="启用",ParentID="CategoryManage",Position="ListTop",ActionType="Available",MenuUrl="/admin/category/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=5, RightIDPath= "/SysManage/CategoryManage/CategoryUnAvailable/",RightLevel=2, ID="CategoryUnAvailable",Name="分类禁用",DisplayName="禁用",ParentID="CategoryManage",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/category/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},

                    new SysRight {SortNo=1, RightIDPath= "/InfoManage/InfoList/InfoCreate/",RightLevel=2, ID="InfoCreate",Name="资讯添加",DisplayName="添加",ParentID="InfoList",Position="ListTop",ActionType="View",MenuUrl="/admin/info/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=2, RightIDPath= "/InfoManage/InfoList/InfoEdit/",RightLevel=2, ID="InfoEdit",Name="资讯编辑",DisplayName="编辑",ParentID="InfoList",Position="ListRight",ActionType="View",MenuUrl="/admin/info/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=3, RightIDPath= "/InfoManage/InfoList/InfoDelete/",RightLevel=2, ID="InfoDelete",Name="资讯删除",DisplayName="删除",ParentID="InfoList",Position="ListTop",ActionType="Delete",MenuUrl="/admin/info/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},                    
                    new SysRight {SortNo=4, RightIDPath= "/InfoManage/InfoList/InfoAvailable/",RightLevel=2, ID="InfoAvailable",Name="资讯启用",DisplayName="启用",ParentID="InfoList",Position="ListTop",ActionType="Available",MenuUrl="/admin/info/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=5, RightIDPath= "/InfoManage/InfoList/InfoUnAvailable/",RightLevel=2, ID="InfoUnAvailable",Name="资讯禁用",DisplayName="禁用",ParentID="InfoList",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/info/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=6, RightIDPath= "/InfoManage/InfoList/InfoView/",RightLevel=2, ID="InfoView",Name="资讯预览",DisplayName="预览",ParentID="InfoList",Position="ListRight",ActionType="View",MenuUrl="/admin/info/infoview",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                };
                context.SysRights.AddRange(rightList2);
                context.SaveChanges();

                var rolerightlist = new List<SysRoleRight>
                {
                    new SysRoleRight {ID="20160314223000027",RoleID="superadmin",RightID="SysManage" },                    
                    new SysRoleRight {ID="20160314223000028",RoleID="superadmin",RightID="UsersManage" },
                    new SysRoleRight {ID="20160314223000029",RoleID="superadmin",RightID="DepsManage" },
                    new SysRoleRight {ID="20160314223000030",RoleID="superadmin",RightID="RolesManage" },
                    new SysRoleRight {ID="20160314223000031",RoleID="superadmin",RightID="RightsManage" },
                    new SysRoleRight {ID="20160315293000031",RoleID="superadmin",RightID="CategoryTypeManage" },
                    new SysRoleRight {ID="20160529173900047",RoleID="superadmin",RightID="CategoryManage" },
                    new SysRoleRight {ID="20160314223000032",RoleID="superadmin",RightID="UserCreate" },
                    new SysRoleRight {ID="20160314223000033",RoleID="superadmin",RightID="UserEdit" },
                    new SysRoleRight {ID="20160314223000034",RoleID="superadmin",RightID="UserDelete" },
                    new SysRoleRight {ID="20160314223000035",RoleID="superadmin",RightID="UserAvailable" },
                    new SysRoleRight {ID="20160314223000036",RoleID="superadmin",RightID="UserUnAvailable" },
                    new SysRoleRight {ID="20160314223000037",RoleID="superadmin",RightID="DepCreate" },
                    new SysRoleRight {ID="20160314223000038",RoleID="superadmin",RightID="DepEdit" },
                    new SysRoleRight {ID="20160314223000039",RoleID="superadmin",RightID="DepDelete" },
                    new SysRoleRight {ID="20160314223000040",RoleID="superadmin",RightID="DepAvailable" },
                    new SysRoleRight {ID="20160314223000041",RoleID="superadmin",RightID="DepUnAvailable" },
                    new SysRoleRight {ID="20160314223000042",RoleID="superadmin",RightID="RoleCreate" },
                    new SysRoleRight {ID="20160314223000043",RoleID="superadmin",RightID="RoleEdit" },
                    new SysRoleRight {ID="20160314223000044",RoleID="superadmin",RightID="RoleDelete" },
                    new SysRoleRight {ID="20160314223000045",RoleID="superadmin",RightID="RoleAvailable" },
                    new SysRoleRight {ID="20160314223000046",RoleID="superadmin",RightID="RoleUnAvailable" },
                    new SysRoleRight {ID="20160314223000047",RoleID="superadmin",RightID="RightCreate" },
                    new SysRoleRight {ID="20160314223000048",RoleID="superadmin",RightID="RightEdit" },
                    new SysRoleRight {ID="20160314223000049",RoleID="superadmin",RightID="RightDelete" },
                    new SysRoleRight {ID="20160314223000050",RoleID="superadmin",RightID="RightAvailable" },
                    new SysRoleRight {ID="20160314223000051",RoleID="superadmin",RightID="RightUnAvailable" },

                    new SysRoleRight {ID="20160529223000047",RoleID="superadmin",RightID="CategoryTypeCreate" },
                    new SysRoleRight {ID="20160529223000048",RoleID="superadmin",RightID="CategoryTypeEdit" },
                    new SysRoleRight {ID="20160529223000049",RoleID="superadmin",RightID="CategoryTypeDelete" },
                    new SysRoleRight {ID="20160529223000050",RoleID="superadmin",RightID="CategoryTypeAvailable" },
                    new SysRoleRight {ID="20160529223000051",RoleID="superadmin",RightID="CategoryTypeUnAvailable" },

                    new SysRoleRight {ID="20160529173000047",RoleID="superadmin",RightID="CategoryCreate" },
                    new SysRoleRight {ID="20160529173000048",RoleID="superadmin",RightID="CategoryEdit" },
                    new SysRoleRight {ID="20160529173000049",RoleID="superadmin",RightID="CategoryDelete" },
                    new SysRoleRight {ID="20160529173000050",RoleID="superadmin",RightID="CategoryAvailable" },
                    new SysRoleRight {ID="20160529173000051",RoleID="superadmin",RightID="CategoryUnAvailable" },

                    new SysRoleRight {ID="20160520223000027",RoleID="superadmin",RightID="InfoManage" },
                    new SysRoleRight {ID="20160520223000028",RoleID="superadmin",RightID="InfoList" },
                    new SysRoleRight {ID="20160520223000029",RoleID="superadmin",RightID="InfoCreate" },
                    new SysRoleRight {ID="20160520223000030",RoleID="superadmin",RightID="InfoEdit" },
                    new SysRoleRight {ID="20160520223000031",RoleID="superadmin",RightID="InfoDelete" },
                    new SysRoleRight {ID="20160520223000032",RoleID="superadmin",RightID="InfoAvailable" },
                    new SysRoleRight {ID="20160520223000033",RoleID="superadmin",RightID="InfoUnAvailable" },
                    new SysRoleRight {ID="20160520223000034",RoleID="superadmin",RightID="InfoView" },

                    new SysRoleRight {ID="20160314223000052",RoleID="admin",RightID="SysManage" },
                    new SysRoleRight {ID="20160314223000053",RoleID="admin",RightID="UsersManage" },
                    new SysRoleRight {ID="20160314223000054",RoleID="admin",RightID="DepsManage" },
                    new SysRoleRight {ID="20160314223000055",RoleID="admin",RightID="RolesManage" },
                    new SysRoleRight {ID="20160314223000056",RoleID="admin",RightID="RightsManage" },
                    new SysRoleRight {ID="20160314223000057",RoleID="admin",RightID="UserCreate" },
                    new SysRoleRight {ID="20160314223000058",RoleID="admin",RightID="UserEdit" },
                    new SysRoleRight {ID="20160314223000059",RoleID="admin",RightID="UserDelete" },
                    new SysRoleRight {ID="20160314223000060",RoleID="admin",RightID="UserAvailable" },
                    new SysRoleRight {ID="20160314223000061",RoleID="admin",RightID="UserUnAvailable" },

                    new SysRoleRight {ID="20160314223000062",RoleID="normaluser",RightID="SysManage" },
                    new SysRoleRight {ID="20160314223000063",RoleID="normaluser",RightID="UsersManage" },
                    new SysRoleRight {ID="20160314223000064",RoleID="normaluser",RightID="DepsManage" },
                    new SysRoleRight {ID="20160314223000065",RoleID="normaluser",RightID="RolesManage" },
                    new SysRoleRight {ID="20160314223000066",RoleID="normaluser",RightID="RightsManage" },
                };
                context.SysRoleRights.AddRange(rolerightlist);
                context.SaveChanges();

                var cateTypeList = new List<CategoryType>()
                {
                    new CategoryType() {ID="2016052916584281757704829ca37d9585f9",SortNo=0,AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false,Name="资讯分类" },
                    new CategoryType() {ID="201605291701073216304915224e159f537c",SortNo=1,AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false,Name="产品分类" },
                };
                context.CategoryTypes.AddRange(cateTypeList);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                var vex = ex as DbEntityValidationException;
                if (vex != null)
                {
                    foreach (var item1 in vex.EntityValidationErrors)
                    {
                        foreach (var item2 in item1.ValidationErrors)
                        {
                            msg += item2.ErrorMessage + "\r\n";
                        }
                    }
                }
                else
                {
                    msg += ex.Message + "\r\n" + ex.StackTrace;
                }
                Logger.AddLog(msg);
                return;
            }
        }

    }
}
