using AS.CMS.Business.Interfaces;
using AS.CMS.Data.Interfaces;
using AS.CMS.Domain.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using AS.CMS.Business.Helpers;

namespace AS.CMS.Business.Services
{
    public class MenuService : IMenuService
    {
        #region Repository Injection

        private IBaseRepository<Menu> _menuRepository;

        public MenuService(IBaseRepository<Menu> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        #endregion

        #region Controller Based Methods

        public bool SaveMenuItem(Menu menuEntity)
        {
            if (menuEntity.ID == 0)
            {
                _menuRepository.Create(menuEntity);
            }
            else
            {
                _menuRepository.Update(menuEntity);
            }

            return true;
        }

        public bool SaveMenuPosition(string menuPositions)
        {
            bool result = true;

            int menuItemOrder = 0;
            int menuItemChildOrder = 0;
            int lastParentItemID = 0;
            var menuPositionArray = JToken.Parse(menuPositions);

            ExpandNode(menuPositionArray, null, prop =>
            {
                if (prop.Name == "id")
                {
                    int menuItemID = prop.First.ToObject<int>();
                    JToken parent = GetParentOfCurrentNode(prop.First, 2);
                    int itemParentID = parent != null ? parent.First.ToObject<int>() : 0;

                    if (itemParentID > 0)
                    {
                        if (itemParentID != lastParentItemID)
                        {
                            menuItemChildOrder = 0;
                        }

                        UpdateMenuPosition(menuItemID, ++menuItemChildOrder, itemParentID);
                        lastParentItemID = itemParentID;
                    }
                    else
                    {
                        UpdateMenuPosition(menuItemID, ++menuItemOrder, itemParentID);
                        menuItemChildOrder = 0;
                    }
                }
            });

            return result;
        }

        public string GetPublishedMenuItems()
        {
            //TODO : Recursive yaz.

            IList<Menu> menuList = _menuRepository.GetWithCriteria(CriteriaHelper.GetDefaultCriteria<Menu>(), CriteriaHelper.GetDefaultOrder("ItemOrder", true));
            StringBuilder menuSb = new StringBuilder();

            foreach (var menuItem in menuList)
            {
                if (menuItem.ParentID > 0)
                {
                    continue;
                }

                menuSb.Append("<li class=\"dd-item\" data-id=\"" + menuItem.ID + "\"><span data-id=\"" + menuItem.ID + "\" id=\"menu-remove-button\" class=\"fa fa-times menu-order-remove-button\"></span><span data-id=\"" + menuItem.ID + "\" id=\"menu-edit-button\" class=\"fa fa-pencil menu-order-edit-button\"></span><div class=\"dd-handle\">" + menuItem.Name + "</div>");


                IList<Menu> firstChildMenu = _menuRepository.GetParent(menuItem.ID);

                if (firstChildMenu.Count > 0 && menuItem.ParentID == 0)
                {
                    menuSb.Append("<ol class=\"dd-list\">");

                    foreach (var childMenuItem in firstChildMenu)
                    {
                        menuSb.Append("<li class=\"dd-item\" data-id=\"" + childMenuItem.ID + "\"><span data-id=\"" + menuItem.ID + "\" id=\"menu-remove-button\" class=\"fa fa-times menu-order-remove-button\"></span><span data-id=\"" + menuItem.ID + "\" id=\"menu-edit-button\" class=\"fa fa-pencil menu-order-edit-button\"></span><div class=\"dd-handle\">" + childMenuItem.Name + "</div>");

                        IList<Menu> secondChildMenu = _menuRepository.GetParent(childMenuItem.ID);

                        if (secondChildMenu.Count > 0)
                        {
                            menuSb.Append("<ol class=\"dd-list\">");

                            foreach (var secondChildMenuItem in secondChildMenu)
                            {
                                menuSb.Append("<li class=\"dd-item\" data-id=\"" + secondChildMenuItem.ID + "\"><span data-id=\"" + menuItem.ID + "\" id=\"menu-remove-button\" class=\"fa fa-times menu-order-remove-button\"></span><span data-id=\"" + menuItem.ID + "\" id=\"menu-edit-button\" class=\"fa fa-pencil menu-order-edit-button\"></span><div class=\"dd-handle\">" + secondChildMenuItem.Name + "</div>");

                                IList<Menu> thirdChildMenu = _menuRepository.GetParent(secondChildMenuItem.ID);

                                if (thirdChildMenu.Count > 0)
                                {
                                    menuSb.Append("<ol class=\"dd-list\">");

                                    foreach (var thirdChildMenuItem in thirdChildMenu)
                                    {
                                        menuSb.Append("<li class=\"dd-item\" data-id=\"" + thirdChildMenuItem.ID + "\"><span data-id=\"" + menuItem.ID + "\" id=\"menu-remove-button\" class=\"fa fa-times menu-order-remove-button\"></span><span data-id=\"" + menuItem.ID + "\" id=\"menu-edit-button\" class=\"fa fa-pencil menu-order-edit-button\"></span><div class=\"dd-handle\">" + thirdChildMenuItem.Name + "</div>");

                                        IList<Menu> fourthChildMenu = _menuRepository.GetParent(thirdChildMenuItem.ID);

                                        if (fourthChildMenu.Count > 0)
                                        {
                                            menuSb.Append("<ol class=\"dd-list\">");

                                            foreach (var fourthChildMenuItem in fourthChildMenu)
                                            {
                                                menuSb.Append("<li class=\"dd-item\" data-id=\"" + fourthChildMenuItem.ID + "\"><span data-id=\"" + menuItem.ID + "\" id=\"menu-remove-button\" class=\"fa fa-times menu-order-remove-button\"></span><span data-id=\"" + menuItem.ID + "\" id=\"menu-edit-button\" class=\"fa fa-pencil menu-order-edit-button\"></span><div class=\"dd-handle\">" + fourthChildMenuItem.Name + "</div>");
                                            }

                                            menuSb.Append("</ol>");
                                        }

                                        menuSb.Append("</li>");
                                    }

                                    menuSb.Append("</ol>");
                                }

                                menuSb.Append("</li>");
                            }

                            menuSb.Append("</ol>");
                        }

                        menuSb.Append("</li>");
                    }

                    menuSb.Append("</ol>");
                }

                menuSb.Append("</li>");
            }

            return menuSb.ToString();
        }

        public bool RemoveMenuItem(int MenuID)
        {
            bool result = true;
            Menu menuEntity = _menuRepository.GetById(MenuID);
            menuEntity.IsActive = false;
            _menuRepository.Update(menuEntity);
            return result;
        }

        public Menu GetMenuWithID(int menuID)
        {
            return _menuRepository.GetById(menuID);
        }

        #endregion

        #region Helper Methods

        public void UpdateMenuPosition(int ID, int order, int parentID)
        {
            Menu menuItem = _menuRepository.GetById(ID);
            menuItem.ItemOrder = order;
            menuItem.ParentID = parentID > 0 ? parentID : 0;

            _menuRepository.Update(menuItem);
        }

        private static void ExpandNode(JToken node, Action<JObject> objectAction = null, Action<JProperty> propertyAction = null)
        {
            if (node.Type == JTokenType.Object)
            {
                if (objectAction != null) objectAction((JObject)node);

                foreach (JProperty child in node.Children<JProperty>())
                {
                    if (propertyAction != null) propertyAction(child);
                    ExpandNode(child.Value, objectAction, propertyAction);
                }
            }
            else if (node.Type == JTokenType.Array)
            {
                foreach (JToken child in node.Children())
                {
                    ExpandNode(child, objectAction, propertyAction);
                }
            }
        }

        public static JToken GetParentOfCurrentNode(JToken token, int parent = 0)
        {
            if (token == null)
                return null;
            if (parent < 0)
                throw new ArgumentOutOfRangeException("Must be positive");

            var skipTokens = new[]
            {
                typeof(JProperty),
            };

            return token.Ancestors()
                .Where(a => skipTokens.All(t => !t.IsInstanceOfType(a)))
                .Skip(parent)
                .FirstOrDefault();
        }

        #endregion
    }
}