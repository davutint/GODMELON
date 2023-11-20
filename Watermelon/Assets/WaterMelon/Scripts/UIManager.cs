using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;
    public Transform BestScoreObj, bestScoreTextObj, RestartButonobj, GameoverTextobj;


    private void Awake()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;

    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;

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

            case GameState.GameoverMenu:
                SetGameOverMenu();
                break;

        }
    }
    private void SetMenu()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);

    }

    private void SetGame()
    {
        gamePanel.SetActive(true);
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);

    }

    private void SetGameOverMenu()//buraya bir dotween kullanarak animasyon oluştur
    {
        gameOverPanel.SetActive(true);
        menuPanel.SetActive(false);
        gamePanel.SetActive(false);
        //animasyonlar

        animasyonBekletveOynat();

    }




    public void animasyonBekletveOynat()
    {

        GameoverTextobj.DOLocalMoveY(550f, .5f).SetEase(Ease.InOutBack);
        RestartButonobj.DOLocalMoveX(0, .5f).SetEase(Ease.InOutBack);
        bestScoreTextObj.DOLocalMoveX(0, .5f).SetEase(Ease.InOutBack);
        BestScoreObj.DOLocalMoveX(0, .5f).SetEase(Ease.InOutBack);



    }





    public void PlayButtonCallback()
    {
        GameManager.instance.SetGameState();
        SetGame();
    }

    public void NextButtonCallback()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
