namespace App.Ads
{
    public interface IBannerAd
    {
        bool VisibleByContext { get; set; }
        bool CanChangeState { get; set; }
        bool Allowed { get; set; }
        bool IsShowing { get; }

        void Init();
        void Refresh();
        void Show();
        void Hide();
    }
}