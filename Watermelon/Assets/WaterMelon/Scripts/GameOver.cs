using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [Header("Elements")]
    //[SerializeField] private GameObject deadLine;
    [SerializeField] private Transform fruitsParent;

    [Header("Timer")]
    [SerializeField] private float durationThreshold;
    private float timer;
    private bool timerOn;
    private bool isGameover;

    private void Update()
    {
        if (!isGameover) ManageGameOver();

    }

    private void ManageGameOver()
    {
        if (timerOn)
        {
            ManageTimerOn();

        }
        else
        {
            if (IsFruitAboveLine()) StartTimer();
        }

    }



    private void ManageTimerOn()
    {
        timer += Time.deltaTime;

        if (!IsFruitAboveLine())
        {
            StopTimer();
        }

        if (timer >= durationThreshold) Gameover();

    }

    private void Gameover()
    {
        Debug.LogError("Game Over");
        isGameover = true;
        GameManager.instance.SetGameoverState();
    }

    private bool IsFruitAboveLine()
    {
        for (int i = 0; i < fruitsParent.childCount; i++)
        {
            Fruit fruit = fruitsParent.GetChild(i).GetComponent<Fruit>();

            if (!fruit.HasCollided()) continue;

            if (IsFruitAboveLine(fruitsParent.GetChild(i))) return true;

        }
        return false;

    }

    private bool IsFruitAboveLine(Transform fruit)
    {
        if (fruit.position.y > 3.5f)//deadLine.transform.position.y) bunu iptal etmemin sebebi restart yaptığımda 4 tane kırmızı error veriyor,missing hatası
        {//if şartını 3.5f yapman gerekiyor test iöin .5 ideal
            return true;
        }
        else
        {
            return false;
        }


    }

    private void StartTimer()
    {
        timer = 0;
        timerOn = true;
    }
    private void StopTimer()
    {
        timerOn = false;

    }

}
