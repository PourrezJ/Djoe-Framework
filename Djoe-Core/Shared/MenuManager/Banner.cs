namespace Shared.MenuManager
{
    public sealed class Banner
    {
        #region Public properties
        public string Dict;
        public string Name;
        #endregion

        #region Public static readonly properties
        public static readonly Banner Barber = new Banner("shopui_title_barber", "shopui_title_barber");
        #endregion

        #region Constructor
        private Banner(string dict, string name)
        {
            Dict = dict;
            Name = name;
        }
        #endregion
    }
}
