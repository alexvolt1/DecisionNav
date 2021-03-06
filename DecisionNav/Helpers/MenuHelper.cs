﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using DecisionNav.Models.ViewModels;
using DecisionNav.Data;
using DecisionNav.Models;
using Microsoft.Extensions.Caching.Memory;

namespace DecisionNav.Helpers
{
    public class MenuHelper
    {
        private static string connStr =$"DataSource ={ Path.Combine(Directory.GetCurrentDirectory(),"demo.db")}";


        public static IList<Menu> GetAllMenuItemsMS()
        {
            List<Menu> menuList = new List<Menu>();

                using (SqlConnection cn = new SqlConnection("Server=AVOLT10L\\AVOLT10L;Database=TestMenu;Trusted_Connection=True;MultipleActiveResultSets=true;"))
                {
                    cn.Open();
                    SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [TestMenu].[dbo].[Menu]", cn);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Menu menu = new Menu();
                        menu.ID = (string)reader["ID"];
                        menu.ParentID = (reader["ParentID"] == DBNull.Value) ? null : reader["ParentID"].ToString();
                        menu.Content = (string)reader["Content"];
                        menu.IconClass = (string)reader["IconClass"];
                        menu.Href = (string)reader["Href"];

                        menuList.Add(menu);
                    }
                    cn.Close();
                }
            return menuList;
        }

        public static IList<Menu> GetChildrenMenu(IList<Menu> menuList, string parentId = null)
        {
            return menuList.Where(x => x.ParentID == parentId).OrderBy(x => x.Order).ToList();
        }

        public static IList<NavigationList> GetChildrenMenuNavList(IList<NavigationList> menuList, string parentId = null)
        {
            return menuList.Where(x => x.ParentId == parentId).ToList();
        }

        public static Menu GetMenuItem(IList<Menu> menuList, string id)
        {
            return menuList.FirstOrDefault(x => x.ID == id);
        }

        public static NavigationList GetMenuItemNavList(IList<NavigationList> menuList, string id)
        {
            return menuList.FirstOrDefault(x => x.Id == id);
        }
        public static NavigationItem_View GetViewId(IList<NavigationItem_View> navItemView, string id, string RType)
        {
            NavigationItem_View navView = new NavigationItem_View();

            navView= navItemView.FirstOrDefault(x => x.TopicId == id && x.RType.ToLower()== RType);
            return navView;
        }

        public static IList<MenuViewModel> GetMenu(IList<Menu> menuList, string parentId)
        {
            var children = MenuHelper.GetChildrenMenu(menuList, parentId);

            if (!children.Any())
            {
                return new List<MenuViewModel>();
            }

            var vmList = new List<MenuViewModel>();
            foreach (var item in children)
            {
                var menu = MenuHelper.GetMenuItem(menuList, item.ID);

                var vm = new MenuViewModel();

                vm.ID = menu.ID;
                vm.Content = menu.Content;
                vm.IconClass = menu.IconClass;
                vm.Href = menu.Href;
                vm.Children = GetMenu(menuList, menu.ID);

                vmList.Add(vm);
            }

            return vmList;
        }

        public static IList<NavigationListViewModel> GetMenuNavList(IList<NavigationList> menuList, IList<NavigationItem_View> navItemView, string parentId)
        {
            var children = MenuHelper.GetChildrenMenuNavList(menuList, parentId);

            if (!children.Any())
            {
                return new List<NavigationListViewModel>();
            }

            var vmList = new List<NavigationListViewModel>();
            foreach (var item in children)
            {
                var menu = MenuHelper.GetMenuItemNavList(menuList, item.Id);
                string RType = "rel";
                var navItem = MenuHelper.GetViewId(navItemView, item.Id, RType);

                var vm = new NavigationListViewModel();

                vm.Id = menu.Id;

                vm.ViewId = navItem?.ViewId;
                vm.DefaultName = menu.DefaultName;
                vm.ImageUrl = menu.ImageUrl;
                vm.Children = GetMenuNavList(menuList, navItemView, menu.Id);

                vmList.Add(vm);
            }

            return vmList;
        }

    }
}
