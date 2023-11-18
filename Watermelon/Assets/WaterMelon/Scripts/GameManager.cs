using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Setting")]
    private GameState gameState;

    [Header("Actions")]
    public static Action<GameState> onGameStateChanged;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

    }
    private void Start()
    {
        SetMenu();

    }

    private void SetMenu()
    {
        SetGameState(GameState.Menu);
    }

    private void SetGame()
    {
        SetGameState(GameState.Game);

    }
    private void SetGameOverState()
    {
        SetGameState(GameState.Gameover);

    }

    private void SetGameOverMenu()
    {
        //blackhole
        //BlackHoleProcessCallback(BlackHoleobj.transform.position);
        SetGameState(GameState.GameoverMenu);

    }

    private void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        onGameStateChanged?.Invoke(obj: gameState);
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState()
    {
        SetGame();
    }

    public bool IsGameState()
    {
        return gameState == GameState.Game;
    }

    public void SetGameoverMenuState()
    {
        SetGameOverMenu();
    }
    public void SetGameoverState()
    {
        SetGameOverState();
    }


}
