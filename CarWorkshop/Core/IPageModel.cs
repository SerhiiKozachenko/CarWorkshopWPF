namespace CarWorkshop.WPF.Core
{
    public interface IPageModel
    {
        string Name { get; }

        void OnPageInit();
    }
}
