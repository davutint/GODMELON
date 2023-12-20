using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    private const string ArkaPlanData = "ArkaPlanData";
    [Header("Elements")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject gameOverPanel;
    public Transform BestScoreObj, bestScoreTextObj, RestartButonobj, GameoverTextobj, SupportDeveloperObj, LiderTablosuobj;
    [SerializeField] private GameObject MaviArkaPlan, SiyahArkaPlan, PembeArkaPlan;
    public Authentication authentication;
    public GameObject KaraDelikButonobj, TextHolderobj, Butonobj;

    public static Action KaraDelikEvent;

    public static UIManager instance;

    public GameObject KırmızıOlcakPanel;

    private void Awake()
    {
        instance = this;
        GameManager.onGameStateChanged += GameStateChangedCallback;
        SelectStartBackGround();

    }




    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;

    }

    public void KaraDelikButonuAktifEt()
    {
        KaraDelikButonobj.SetActive(true);
        KaraDelikButonuAnim(0f);

    }

    public void KaraDelikButonEvent()//reklamı çağır, reklam içinde de karadelik eventini çağır
    {
        AudioManager.instance.ClickSoundCal();
        AdManager.instance.OdulluGoster(); // video çekerken böyle olsun yayınlarken yorum satırını kaldır
        //BlackHole.instance.BlackHoleProcessCallback();//bunu build alırken yorum satırı yap üsttekinin yorum satırını kaldır.
        KaraDelikButonuPasifEt();
    }


    public void KaraDelikButonuPasifEt()//bunu butona tıklayınca çalıştır
    {
        Butonobj.transform.DOLocalMoveX(2000, .55f).SetEase(Ease.InOutBack);
        TextHolderobj.transform.DOLocalMoveX(2000, .55f).SetEase(Ease.InOutBack).
        OnComplete(() =>
        {
            KaraDelikButonobj.SetActive(value: false);

        });
    }

    public void KaraDelikButonuAnim(float position)
    {
        Butonobj.transform.DOLocalMoveX(position, .55f).SetEase(Ease.InOutBack);
        TextHolderobj.transform.DOLocalMoveX(position, .55f).SetEase(Ease.InOutBack);
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:
                SetMenu();
                break;

            case GameState.Game:
                SetGame();
                break;

            case GameState.Gameover:
                SetGameOverMenu();
                break;
            case GameState.Settings:
                SetSettings();
                break;


        }
    }
    private void SetMenu()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        settingsPanel.SetActive(false);


    }

    private void SetGame()
    {
        gamePanel.SetActive(true);
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        settingsPanel.SetActive(false);

    }
    private void SetSettings()
    {
        DataManager.instance.GetSliderValueData();

        settingsPanel.SetActive(true);
        gamePanel.SetActive(false);
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);

    }

    private void SetGameOverMenu()//buraya bir dotween kullanarak animasyon oluştur
    {
        //bir başka paneli kırmızıya döndür ve kırmızıya döndürdükten sonra aşağıdakiler çalışsın.

        KırmızıOlcakPanel.GetComponent<Image>().DOFade(.8f, 1f).OnComplete(() =>
        {
            gameOverPanel.SetActive(true);
            menuPanel.SetActive(false);
            gamePanel.SetActive(false);
            settingsPanel.SetActive(false);

            //animasyonlar

            MenuAnimOynat();

        });


    }


    public void LiderTablosuAc()
    {
        authentication.OnShowLeaderboard();
        //yorum satırını kaldır yukardakinin
    }

    public void MenuAnimOynat()
    {

        GameoverTextobj.DOLocalMoveY(550f, 1f).SetEase(Ease.InOutBack);
        RestartButonobj.DOLocalMoveX(0, 1f).SetEase(Ease.InOutBack);
        LiderTablosuobj.DOLocalMoveX(0, 1f).SetEase(Ease.InOutBack);
        bestScoreTextObj.DOLocalMoveX(0, 1f).SetEase(Ease.InOutBack);
        BestScoreObj.DOLocalMoveX(0, 1f).SetEase(Ease.InOutBack);
        SupportDeveloperObj.DOLocalMoveX(230f, 1f).SetEase(Ease.InOutBack);
        SupportDeveloperObj.DOScale(new Vector2(.87f, 1.23f), .55f).SetLoops(-1, LoopType.Yoyo);



    }

    public void SelectStartBackGround()//burasıda awakede çalışmalı hangi arka plan olacak diye
    {
        int arkaplan = PlayerPrefs.GetInt(ArkaPlanData);
        switch (arkaplan)
        {
            case 0:
                MaviArkaPlan.SetActive(true);
                SiyahArkaPlan.SetActive(false);
                PembeArkaPlan.SetActive(false);
                break;
            case 1:
                PembeArkaPlan.SetActive(true);
                MaviArkaPlan.SetActive(false);
                SiyahArkaPlan.SetActive(false);
                break;
            case 2:
                SiyahArkaPlan.SetActive(true);
                PembeArkaPlan.SetActive(false);
                MaviArkaPlan.SetActive(false);
                break;
            default:
                PembeArkaPlan.SetActive(true);
                SiyahArkaPlan.SetActive(false);
                PembeArkaPlan.SetActive(false);
                break;
        }
    }

    public void SupportDeveloper()
    {
        AudioManager.instance.ClickSoundCal();
        AdManager.instance.OdulluGoster();
    }



    public void PlayButtonCallback()
    {
        AudioManager.instance.ClickSoundCal();
        GameManager.instance.SetGameState();
        SetGame();
    }

    public void RestartGame()
    {
        AudioManager.instance.ClickSoundCal();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
