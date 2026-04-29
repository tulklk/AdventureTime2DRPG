using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class Interstitial : MonoBehaviour
{
    private string _adUnitId = "ca-app-pub-1161594319723764/2311496235";
    private InterstitialAd _interstitialAd;
    private System.Action actionOnAdComplete; // Lưu hành động sau khi quảng cáo kết thúc

    private void Start()
    {
        LoadInterstitialAd();
    }

    public void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        var adRequest = new AdRequest();
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial ad failed to load: " + error);
                    return;
                }

                _interstitialAd = ad;
                RegisterReloadHandler(_interstitialAd);
            });
    }

    public void ShowInterstitialAd(System.Action onComplete)
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            actionOnAdComplete = onComplete; // Lưu hành động cần thực hiện sau khi xem xong
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready, proceeding directly.");
            onComplete?.Invoke(); // Nếu quảng cáo không sẵn sàng, thực hiện ngay hành động
        }
    }

    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad closed.");
            actionOnAdComplete?.Invoke(); // Gọi hành động khi quảng cáo kết thúc
            LoadInterstitialAd();
        };

        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed: " + error);
            actionOnAdComplete?.Invoke(); // Nếu quảng cáo lỗi, vẫn tiếp tục hành động
            LoadInterstitialAd();
        };
    }
}
