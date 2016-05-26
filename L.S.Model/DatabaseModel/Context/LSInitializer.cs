﻿using L.S.Model.DatabaseModel.Entity;
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
                SysDep root = new SysDep() { ID = "root", Name = "LSroot", AddBy = "init", AddByName = "init", AddDate = DateTime.Now, IsDel = false, IsAvailable = true, Parent = null, DepFullIDPath = "root", DepFullNamePath = "LSroot" };
                context.SysDeps.Add(root);
                context.SaveChanges();
                SysDep center = new SysDep { ID = "manage-center", Name = "平台中心", AddBy = "init", AddByName = "init", AddDate = DateTime.Now, IsDel = false, IsAvailable = true, ParentID = "root", ParentName = "LSroot", DepFullIDPath = "root/manage-center", DepFullNamePath = "LSroot/平台中心" };
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

                var super = new SysRole { Name = "超级管理员", DefaultHomePath = "/admin/sysuser/", Level = 0, ID = "superadmin", ParentID = null, ParentName = null, RoleIDPath = "superadmin", RoleNamePath = "超级管理员", AddBy = "init", AddByName = "init", AddDate = DateTime.Now, IsDel = false, IsAvailable = true, };
                context.SysRoles.Add(super);
                context.SaveChanges();
                var admin = new SysRole { Name = "系统管理员", DefaultHomePath = "/admin/sysuser/", Level = 1, ID = "admin", ParentID = "superadmin", ParentName = "超级管理员", RoleIDPath = "superadmin/admin", RoleNamePath = "超级管理员/系统管理员", AddBy = "init", AddByName = "init", AddDate = DateTime.Now.AddSeconds(1), IsDel = false, IsAvailable = true, };
                context.SysRoles.Add(admin);
                context.SaveChanges();
                var normaluser = new SysRole { Name = "普通用户", DefaultHomePath = "/admin/sysuser/", Level = 2, ID = "normaluser", ParentID = "admin", ParentName = "系统管理员", RoleIDPath = "superadmin/admin/normaluser", RoleNamePath = "超级管理员/系统管理员/普通用户", AddBy = "init", AddByName = "init", AddDate = DateTime.Now.AddSeconds(2), IsDel = false, IsAvailable = true, };
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

                var SysManage = new SysRight { SortNo = 0, RightIDPath = "SysManage", ID = "SysManage", RightLevel = 0, Name = "系统管理", ParentID = null, ActionType = "View", MenuUrl = "/admin/sysuser", AddBy = "init", AddByName = "init", AddDate = DateTime.Now, IsAvailable = true, IsDel = false };
                var InfoManage = new SysRight { SortNo = 1, RightIDPath = "InfoManage", ID = "InfoManage", RightLevel = 0, Name = "资讯管理", ParentID = null, ActionType = "View", MenuUrl = "/admin/info", AddBy = "init", AddByName = "init", AddDate = DateTime.Now, IsAvailable = true, IsDel = false };
                context.SysRights.Add(SysManage);
                context.SysRights.Add(InfoManage);
                context.SaveChanges();
                var rightList1 = new List<SysRight>
                {
                    new SysRight {SortNo=10, RightIDPath= "SysManage/UsersManage",RightLevel=1,ID="UsersManage",Name="用户管理",ParentID="SysManage",ActionType="View",MenuUrl="/admin/sysuser",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=20, RightIDPath= "SysManage/DepsManage",RightLevel=1,ID="DepsManage",Name="部门管理",ParentID="SysManage",ActionType="View",MenuUrl="/admin/sysdep",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=30, RightIDPath= "SysManage/RolesManage",RightLevel=1,ID="RolesManage",Name="角色管理",ParentID="SysManage",ActionType="View",MenuUrl="/admin/sysrole",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=40, RightIDPath= "SysManage/RightsManage",RightLevel=1,ID="RightsManage",Name="权限管理",ParentID="SysManage",ActionType="View",MenuUrl="/admin/sysright",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},

                    new SysRight {SortNo=50, RightIDPath= "InfoManage/InfoList",RightLevel=1,ID="InfoList",Name="资讯列表",ParentID="InfoManage",ActionType="View",MenuUrl="/admin/info",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                };
                context.SysRights.AddRange(rightList1);
                context.SaveChanges();
                var rightList2 = new List<SysRight>
                {
                    new SysRight {SortNo=11, RightIDPath= "SysManage/UsersManage/UserCreate",RightLevel=2, ID="UserCreate",Name="用户添加",ParentID="UsersManage",Position="ListTop",ActionType="View",MenuUrl="/admin/sysuser/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=12, RightIDPath= "SysManage/UsersManage/UserEdit",RightLevel=2, ID="UserEdit",Name="用户编辑",ParentID="UsersManage",Position="ListRight",ActionType="View",MenuUrl="/admin/sysuser/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=13, RightIDPath= "SysManage/UsersManage/UserDelete",RightLevel=2, ID="UserDelete",Name="用户删除",ParentID="UsersManage",Position="ListTop",ActionType="Delete",MenuUrl="/admin/sysuser/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=14, RightIDPath= "SysManage/UsersManage/UserAvailable",RightLevel=2, ID="UserAvailable",Name="用户启用",ParentID="UsersManage",Position="ListTop",ActionType="Available",MenuUrl="/admin/sysuser/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=15, RightIDPath= "SysManage/UsersManage/UserUnAvailable",RightLevel=2, ID="UserUnAvailable",Name="用户禁用",ParentID="UsersManage",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/sysuser/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},

                    new SysRight {SortNo=21, RightIDPath= "SysManage/DepsManage/DepCreate",RightLevel=2, ID="DepCreate",Name="部门添加",ParentID="DepsManage",Position="ListTop",ActionType="View",MenuUrl="/admin/sysdep/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=22, RightIDPath= "SysManage/DepsManage/DepEdit",RightLevel=2, ID="DepEdit",Name="部门编辑",ParentID="DepsManage",Position="ListRight",ActionType="View",MenuUrl="/admin/sysdep/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=23, RightIDPath= "SysManage/DepsManage/DepDelete",RightLevel=2, ID="DepDelete",Name="部门删除",ParentID="DepsManage",Position="ListTop",ActionType="Delete",MenuUrl="/admin/sysdep/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=24, RightIDPath= "SysManage/DepsManage/DepAvailable",RightLevel=2, ID="DepAvailable",Name="部门启用",ParentID="DepsManage",Position="ListTop",ActionType="Available",MenuUrl="/admin/sysdep/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=25, RightIDPath= "SysManage/DepsManage/DepUnAvailable",RightLevel=2, ID="DepUnAvailable",Name="部门禁用",ParentID="DepsManage",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/sysdep/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},

                    new SysRight {SortNo=31, RightIDPath= "SysManage/RolesManage/RoleCreate",RightLevel=2, ID="RoleCreate",Name="角色添加",ParentID="RolesManage",Position="ListTop",ActionType="View",MenuUrl="/admin/sysrole/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=32, RightIDPath= "SysManage/RolesManage/RoleEdit",RightLevel=2, ID="RoleEdit",Name="角色编辑",ParentID="RolesManage",Position="ListRight",ActionType="View",MenuUrl="/admin/sysrole/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=33, RightIDPath= "SysManage/RolesManage/RoleDelete",RightLevel=2, ID="RoleDelete",Name="角色删除",ParentID="RolesManage",Position="ListTop",ActionType="Delete",MenuUrl="/admin/sysrole/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=34, RightIDPath= "SysManage/RolesManage/RoleAvailable",RightLevel=2, ID="RoleAvailable",Name="角色启用",ParentID="RolesManage",Position="ListTop",ActionType="Available",MenuUrl="/admin/sysrole/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=35, RightIDPath= "SysManage/RolesManage/RoleUnAvailable",RightLevel=2, ID="RoleUnAvailable",Name="角色禁用",ParentID="RolesManage",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/sysrole/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},

                    new SysRight {SortNo=41, RightIDPath= "SysManage/RightsManage/RightCreate",RightLevel=2, ID="RightCreate",Name="权限添加",ParentID="RightsManage",Position="ListTop",ActionType="View",MenuUrl="/admin/sysright/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=42, RightIDPath= "SysManage/RightsManage/RightEdit",RightLevel=2, ID="RightEdit",Name="权限编辑",ParentID="RightsManage",Position="ListRight",ActionType="View",MenuUrl="/admin/sysright/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=43, RightIDPath= "SysManage/RightsManage/RightDelete",RightLevel=2, ID="RightDelete",Name="权限删除",ParentID="RightsManage",Position="ListTop",ActionType="Delete",MenuUrl="/admin/sysright/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=44, RightIDPath= "SysManage/RightsManage/RightAvailable",RightLevel=2, ID="RightAvailable",Name="权限启用",ParentID="RightsManage",Position="ListTop",ActionType="Available",MenuUrl="/admin/sysright/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=45, RightIDPath= "SysManage/RightsManage/RightUnAvailable",RightLevel=2, ID="RightUnAvailable",Name="权限禁用",ParentID="RightsManage",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/sysright/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},

                    new SysRight {SortNo=51, RightIDPath= "InfoManage/InfoList/InfoCreate",RightLevel=2, ID="InfoCreate",Name="添加",ParentID="InfoList",Position="ListTop",ActionType="View",MenuUrl="/admin/info/create",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=52, RightIDPath= "InfoManage/InfoList/InfoEdit",RightLevel=2, ID="InfoEdit",Name="编辑",ParentID="InfoList",Position="ListRight",ActionType="View",MenuUrl="/admin/info/edit",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=53, RightIDPath= "InfoManage/InfoList/InfoDelete",RightLevel=2, ID="InfoDelete",Name="删除",ParentID="InfoList",Position="ListTop",ActionType="Delete",MenuUrl="/admin/info/delete",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},                    
                    new SysRight {SortNo=54, RightIDPath= "InfoManage/InfoList/InfoAvailable",RightLevel=2, ID="InfoAvailable",Name="启用",ParentID="InfoList",Position="ListTop",ActionType="Available",MenuUrl="/admin/info/available",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=55, RightIDPath= "InfoManage/InfoList/InfoUnAvailable",RightLevel=2, ID="InfoUnAvailable",Name="禁用",ParentID="InfoList",Position="ListTop",ActionType="UnAvailable",MenuUrl="/admin/info/unavailable",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
                    new SysRight {SortNo=56, RightIDPath= "InfoManage/InfoList/InfoView",RightLevel=2, ID="InfoView",Name="预览",ParentID="InfoList",Position="ListRight",ActionType="View",MenuUrl="/admin/info/infoview",AddBy="init",AddByName="init",AddDate=DateTime.Now,IsAvailable=true,IsDel=false},
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
                    new SysRoleRight {ID="20160314223000051",RoleID="superadmin",RightID="InfoView" },

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
