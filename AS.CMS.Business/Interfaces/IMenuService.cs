using AS.CMS.Domain.Base;

namespace AS.CMS.Business.Interfaces
{
    public interface IMenuService
    {
        bool SaveMenuItem(Menu menuEntity);
        string GetPublishedMenuItems();
        bool SaveMenuPosition(string menuPositions);
        bool RemoveMenuItem(int MenuID);
        Menu GetMenuWithID(int menuID);
    }
}
