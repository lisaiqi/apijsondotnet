using APIJSON.NET.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace APIJSON.NET.Services
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context;
        private List<Role> roles;
        private DbContext db;
        
        public IdentityService(IHttpContextAccessor context, IOptions<List<Role>> _roles)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            roles = _roles.Value;
        }
        public string GetUserIdentity()
        {
            return _context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        //获取当前登陆用户的角色名
        public string GetUserRoleName()
        {
            return _context.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }
        public Role GetRole()
        {
            var role = new Role();
            if (string.IsNullOrEmpty(GetUserRoleName()))//没登录默认取第一个
            {
                role = roles.FirstOrDefault();

            }
            else
            {
                role = roles.FirstOrDefault(it => it.Name.Equals(GetUserRoleName(), StringComparison.CurrentCultureIgnoreCase));
            }
            
            return role;
        }
        public (bool, string) GetSelectRole(string table)
        {
            List<Role> Role_data = list_roles("");
            var role = GetRole();
           
            if (role == null || role.Select == null || role.Select.Table == null)
            {
                return (false, $"appsettings.json权限配置不正确！");
            }
            string tablerole = role.Select.Table.FirstOrDefault(it => it == "*" || it.Equals(table, StringComparison.CurrentCultureIgnoreCase));
            //db = new DbContext();
            //var tablerole = db.RoleDb.GetList(it => it.rolecode == role.Name || it.Equals(role));
            if (string.IsNullOrEmpty(tablerole))
            {
                return (false, $"表名{table}没权限查询！");
            }
            int index = Array.IndexOf(role.Select.Table, tablerole);
            string selectrole = role.Select.Column[index];
            return (true, selectrole);
        }
        public bool ColIsRole(string col, string[] selectrole)
        {
            if (selectrole.Contains("*"))
            {
                return true;
            }
            else
            {
                if (col.Contains("(") && col.Contains(")"))
                {
                    Regex reg = new Regex(@"\(([^)]*)\)");
                    Match m = reg.Match(col);
                    if (selectrole.Contains(m.Result("$1"), StringComparer.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (selectrole.Contains(col, StringComparer.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        Dictionary<int, string[]> Dic_Data = new Dictionary<int, string[]>();

        public List<Role> list_roles(string aa) {
            aa = "role1";
            db = new DbContext();
            var tablerole = db.RoleDb.GetList(it => it.rolecode == aa);
            List<Role> ListRole = new List<Role>();
            Role RoleData = new Role();
            RoleItem RoleItemData = new RoleItem();
            for (int i = 0; i < tablerole.Count; i++)
            {
                if (tablerole[i].operation == "select")
                {
                    if (Dic_Data.ContainsKey(1))
                    {
                        string[] b = Dic_Data[1];
                        b = b.Concat(new string[] { tablerole[i].tables }).ToArray();
                        Dic_Data.Add(1, b);
                    }
                    else
                    {
                        string[] b = { tablerole[i].tables };
                        Dic_Data.Add(1, b);
                    }
                    if (Dic_Data.ContainsKey(2))
                    {
                        string[] c = Dic_Data[2];
                        c = c.Concat(new string[] { tablerole[i].column }).ToArray();
                        Dic_Data.Add(2, c);
                    }
                    else
                    {
                        string[] c = { tablerole[i].column };
                        Dic_Data.Add(2, c);
                    }
                    if (Dic_Data.ContainsKey(3))
                    {
                        string[] d = Dic_Data[3];
                        d = d.Concat(new string[] { tablerole[i].where }).ToArray();
                        Dic_Data.Add(3, d);
                    }
                    else
                    {
                        string[] d = { tablerole[i].where };
                        Dic_Data.Add(3, d);
                    }
                }
                else if (tablerole[i].operation == "insert")
                {
                    // RoleItemData.Table = tablerole[i].tables;
                    if (Dic_Data.ContainsKey(4))
                    {
                        string[] b = Dic_Data[4];
                        b = b.Concat(new string[] { tablerole[i].tables }).ToArray();
                        Dic_Data.Add(4, b);
                    }
                    else
                    {
                        string[] b = { tablerole[i].tables };
                        Dic_Data.Add(4, b);
                    }
                    if (Dic_Data.ContainsKey(5))
                    {
                        string[] c = Dic_Data[5];
                        c = c.Concat(new string[] { tablerole[i].column }).ToArray();
                        Dic_Data.Add(5, c);
                    }
                    else
                    {
                        string[] c = { tablerole[i].column };
                        Dic_Data.Add(5, c);
                    }
                    if (Dic_Data.ContainsKey(6))
                    {
                        if (tablerole[i].where != null)
                        {
                            string[] d = Dic_Data[6];
                            d = d.Concat(new string[] { tablerole[i].where }).ToArray();
                            Dic_Data.Add(6, d);
                        }
                    }
                    else
                    {
                        if (tablerole[i].where != null)
                        {
                            string[] d = { tablerole[i].where };
                            Dic_Data.Add(6, d);
                        }
                    }

                }
                else if (tablerole[i].operation == "updare")
                {
                    // RoleItemData.Table = tablerole[i].tables;
                    if (Dic_Data.ContainsKey(7))
                    {
                        string[] b = Dic_Data[7];
                        b = b.Concat(new string[] { tablerole[i].tables }).ToArray();
                        Dic_Data.Add(7, b);
                    }
                    else
                    {
                        string[] b = { tablerole[i].tables };
                        Dic_Data.Add(7, b);
                    }
                    if (Dic_Data.ContainsKey(8))
                    {
                        string[] c = Dic_Data[8];
                        c = c.Concat(new string[] { tablerole[i].column }).ToArray();
                        Dic_Data.Add(8, c);
                    }
                    else
                    {
                        string[] c = { tablerole[i].column };
                        Dic_Data.Add(8, c);
                    }
                    if (Dic_Data.ContainsKey(9))
                    {
                        if (tablerole[i].where != null)
                        {
                            string[] d = Dic_Data[9];
                            d = d.Concat(new string[] { tablerole[i].where }).ToArray();
                            Dic_Data.Add(9, d);
                        }
                    }
                    else
                    {
                        if (tablerole[i].where != null)
                        {
                            string[] d = { tablerole[i].where };
                            Dic_Data.Add(9, d);
                        }
                    }

                }
                else if (tablerole[i].operation == "delete")
                {
                    // RoleItemData.Table = tablerole[i].tables;
                    if (Dic_Data.ContainsKey(10))
                    {
                        string[] b = Dic_Data[10];
                        b = b.Concat(new string[] { tablerole[i].tables }).ToArray();
                        Dic_Data.Add(10, b);
                    }
                    else
                    {
                        string[] b = { tablerole[i].tables };
                        Dic_Data.Add(10, b);
                    }
                    if (Dic_Data.ContainsKey(11))
                    {
                        string[] c = Dic_Data[11];
                        c = c.Concat(new string[] { tablerole[i].column }).ToArray();
                        Dic_Data.Add(11, c);
                    }
                    else
                    {
                        string[] c = { tablerole[i].column };
                        Dic_Data.Add(11, c);
                    }
                    if (Dic_Data.ContainsKey(12))
                    {
                        if (tablerole[i].where != null)
                        {
                            string[] d = Dic_Data[12];
                            d = d.Concat(new string[] { tablerole[i].where }).ToArray();
                            Dic_Data.Add(12, d);
                        }
                    }
                    else
                    {
                        if (tablerole[i].where != null)
                        {
                            string[] d = { tablerole[i].where };
                            Dic_Data.Add(12, d);
                        }
                    }

                }
            }
            if (Dic_Data.ContainsKey(1))
            {
                if (RoleData.Select == null)
                {
                    RoleData.Select = new RoleItem();
                    RoleData.Select.Table = Dic_Data[1];
                }
                else
                {
                    RoleData.Select.Table = Dic_Data[1];
                }
               
            }
            if (Dic_Data.ContainsKey(2))
            {
                if (RoleData.Select == null)
                {
                    RoleData.Select = new RoleItem();
                    RoleData.Select.Column = Dic_Data[2];
                }
                else
                {
                    RoleData.Select.Column = Dic_Data[2];
                }
            }
            if (Dic_Data.ContainsKey(3))
            {
                if (RoleData.Select == null)
                {
                    RoleData.Select = new RoleItem();
                    RoleData.Select.Filter = Dic_Data[3];
                }
                else
                {
                    RoleData.Select.Filter = Dic_Data[3];
                }
            }
            if (Dic_Data.ContainsKey(4))
            {
                if (RoleData.Insert == null)
                {
                    RoleData.Insert = new RoleItem();
                    RoleData.Insert.Table = Dic_Data[4];
                }
                else
                {
                    RoleData.Insert.Table = Dic_Data[4];
                }
            }
            if (Dic_Data.ContainsKey(5))
            {
                if (RoleData.Insert == null)
                {
                    RoleData.Insert = new RoleItem();
                    RoleData.Insert.Column = Dic_Data[5];
                }
                else
                {
                    RoleData.Insert.Column = Dic_Data[5];
                }
            }
            if (Dic_Data.ContainsKey(6))
            {
                if (RoleData.Insert == null)
                {
                    RoleData.Insert = new RoleItem();
                    RoleData.Insert.Filter = Dic_Data[6];
                }
                else
                {
                    RoleData.Insert.Filter = Dic_Data[6];
                }
            }
            if (Dic_Data.ContainsKey(7))
            {
                if (RoleData.Update == null)
                {
                    RoleData.Update = new RoleItem();
                    RoleData.Update.Table = Dic_Data[7];
                }
                else
                {
                    RoleData.Update.Table = Dic_Data[7];
                }
            }
            if (Dic_Data.ContainsKey(8))
            {
                if (RoleData.Update == null)
                {
                    RoleData.Update = new RoleItem();
                    RoleData.Update.Column = Dic_Data[8];
                }
                else
                {
                    RoleData.Update.Column = Dic_Data[8];
                }
            }
            if (Dic_Data.ContainsKey(9))
            {
                if (RoleData.Update == null)
                {
                    RoleData.Update = new RoleItem();
                    RoleData.Update.Filter = Dic_Data[9];
                }
                else
                {
                    RoleData.Update.Filter = Dic_Data[9];
                }
            }
            if (Dic_Data.ContainsKey(10))
            {
                if (RoleData.Delete == null)
                {
                    RoleData.Delete = new RoleItem();
                    RoleData.Delete.Table = Dic_Data[10];
                }
                else
                {
                    RoleData.Delete.Table = Dic_Data[10];
                }
            }
            if (Dic_Data.ContainsKey(11))
            {
                if (RoleData.Delete == null)
                {
                    RoleData.Delete = new RoleItem();
                    RoleData.Delete.Column = Dic_Data[11];
                }
                else
                {
                    RoleData.Delete.Column = Dic_Data[11];
                }
            }
            if (Dic_Data.ContainsKey(12))
            {
                if (RoleData.Delete == null)
                {
                    RoleData.Delete = new RoleItem();
                    RoleData.Delete.Filter = Dic_Data[12];
                }
                else
                {
                    RoleData.Delete.Filter = Dic_Data[12];
                }
            }

            RoleData.Name = tablerole[0].rolecode;
            ListRole.Add(RoleData);
            return ListRole;
        }
    }
}
