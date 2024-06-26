using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{

    public static AdManager instance;
#if UNITY_IPHONE
    string appKey = "1c8f541d5";

#else
 string appKey="unexpecred_platform"; 
#endif
    private void Awake()
    {
        instance = this;
        InitializeAds();

    }

    private void Start()
    {
        //0.2.2de bu awakede çalışıyor,çıktı aldıktan sonra starta çektim güncellemede reklam düzgün çalısırsa tekrara awake al
        LoadBanner();
        //lider tablosu ekledikten sonraki güncellemede banner reklamı etkinleştirirsin.
    }

    public void InitializeAds()
    {

        //IronSource.Agent.setMetaData("is_test_suite", "enable");
        //test için eklenen satırları silmeyi unutma
        IronSource.Agent.setConsent(true);
        IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.BANNER);

    }


    public void OdulluGoster()// UI managerdan buton ile çekeceğiz
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            print("Rewarded not ready !!");
        }

    }

    public void LoadBanner()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
    }
    public void DestroyBanner()
    {
        IronSource.Agent.destroyBanner();//bunu sanırım oyun sonu yok etmek gerekiyor ancak teste muhtaç
    }


    public void SdkInitializationCompletedEvent()
    {
        Debug.Log("IronSource has been initialized with success");
        IronSource.Agent.validateIntegration(); // reklam entegrasyonunda hata varsa xcode testinde bunu ayrıntılı görmemizi sağlayacak
        //IronSource.Agent.launchTestSuite(); //bunu yayımlarken sil test suiti bu


    }



    private void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }
    private void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;


        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;

        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;




        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;

    }
    //Add AdInfo Rewarded Video Events



    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {

    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
        Debug.Log("unavailable reklam");

    }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {


    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {

        BlackHole.instance.BlackHoleProcessCallback();//bu görsel birşey olduğu için oyuncu reklamı komple kapattığında bu gerçekleşmeli
        //editore video çekerken bu olmuyor yayınlarken yorum satırından kaldır.
    }

    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        Debug.Log("ÖDÜLLÜ GÖRÜLDÜ VE ÖDÜL VERİLDİ");

        //burada ödül verdim ancak ödül verildi demesine rağmen reklam kapatılmadığı için kara delik eventini göremedim
        //bu yüzden üst callbackte karadeliği çağırıyorum.

    }
    // The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {

    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {


    }

    /************* Banner AdInfo Delegates *************/
    //Invoked once the banner has loaded
    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
    {
    }
    //Invoked when the banner loading process has failed.
    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
    {
    }
    // Invoked when end user clicks on the banner ad
    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
    }
    //Notifies the presentation of a full screen content following user click
    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
    {
    }
    //Notifies the presented screen has been dismissed
    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
    {
    }
    //Invoked when the user leaves the app
    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
    {
    }
}
