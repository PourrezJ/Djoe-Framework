using System.Collections.Generic;

namespace Shared.MenuManager
{
    public interface IListItem : IMenuItem
    {
        List<string> Items { get; set; }
        int SelectedItem { get; set; }
        bool ExecuteCallbackListChange { get; set; }
    }
}