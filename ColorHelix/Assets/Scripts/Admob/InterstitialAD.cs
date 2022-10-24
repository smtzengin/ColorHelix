
using UnityEngine;
using GoogleMobileAds.Api;


public class InterstitialAD : MonoBehaviour
{
    public static InterstitialAD instance;

    public InterstitialAd interstitial;


    private void Awake()
    {
        instance = this;
    }

    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        //this.interstitial.OnAdClosed += HandleOnAdClosed;

        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
        print("sorguyu çaðýrdým.");
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded() == true)
        {
            this.interstitial.Show();
            print("kodu gösterdim");
        }
    }

    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        
        print("reklamý yedim.");
    }

   
}
