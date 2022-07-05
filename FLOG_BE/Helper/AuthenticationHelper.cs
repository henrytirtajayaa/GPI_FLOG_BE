using FLOG_BE.Helper.dto;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Central.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Helper
{
    public static class AuthenticationHelper
    {
        public static async Task<List<MenuItem>> GetMenus(FlogContext context, string securityRoleId)
        {
            var parent1 = await context.Forms
                .Where(x => x.FormLink == "#" && x.ParentId == null).ToListAsync();

            var parent2 = await context.Forms
                .Where(x => x.FormLink == "#" && x.ParentId != null).ToListAsync();

            parent1 = parent1.Concat(parent2).ToList();

            var parent3 = await context.SecurityRoleAccesses
               .Include(x => x.Form)
               .Where(x => x.SecurityRoleId == securityRoleId && x.Form != null)
               .Select(x => x.Form).ToListAsync();

            parent1 = parent1.Concat(parent3).ToList();
            parent1 = parent1.Distinct().ToList();
            var parentMenus = GetMenuItems(null, parent1);

            return parentMenus;
        }

        private static List<MenuItem> GetMenuItems(string parentId, List<Form> menus )
        {
            var temp = menus.Where(x => x.ParentId == parentId)
                .OrderBy(x=> x.SortNo)
                .Select(x => new MenuItem()
                {
                    Name = x.FormId,
                    IsVisible = x.IsVisible,
                    Title = x.FormName,
                    Group = x.FormId,
                    Href = x.FormLink,
                    Icon = x.MenuIcon,
                    Items = GetMenuItems(x.FormId, menus)
                }).ToList();

            return temp;
        }

    }
}
