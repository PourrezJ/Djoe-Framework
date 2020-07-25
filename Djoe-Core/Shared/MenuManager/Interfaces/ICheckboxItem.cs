namespace Shared.MenuManager
{
    interface ICheckboxItem
    {
        bool Checked { get; set; }

        bool IsInput();
    }
}