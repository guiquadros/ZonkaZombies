namespace ZonkaZombies.Controllers
{
    public interface IView
    {
        void OnShow();
        void OnHide();
        void CallView();
    }
}