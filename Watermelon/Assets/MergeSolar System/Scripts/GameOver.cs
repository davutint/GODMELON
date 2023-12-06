using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [Header("Elements")]
    //[SerializeField] private GameObject deadLine;
    [SerializeField] private Transform planetParent;

    [Header("Timer")]
    [SerializeField] private float durationThreshold;
    private float timer;
    private float Yaklastıtimer;

    private bool timerOn;
    private bool YaklastıtimerOn;

    private bool isGameover;
    private bool isClose;



    private void Update()
    {
        if (!isGameover)
        {
            ManageGameOver();

        }
        if (!isGameover && !isClose)
        {
            ManageYaklastı();
        }



    }

    private void ManageGameOver()
    {
        if (timerOn)
        {
            ManageTimerOn();

        }
        else
        {
            if (IsPlanetAboveLine()) StartTimer();
        }



    }

    private void ManageYaklastı()
    {
        if (YaklastıtimerOn)
        {
            ManageYaklastıTimerOn();

        }
        else
        {
            if (BıtıseYaklastı()) YaklastıStartTimer();
        }



    }



    private void ManageTimerOn()
    {
        timer += Time.deltaTime;

        if (!IsPlanetAboveLine())
        {
            StopTimer();
        }

        if (timer >= durationThreshold) Gameover();

    }

    private void ManageYaklastıTimerOn()
    {
        Yaklastıtimer += Time.deltaTime;

        if (!BıtıseYaklastı())
        {
            YaklastıStopTimer();
        }

        if (Yaklastıtimer >= durationThreshold)
        {
            UIManager.instance.KaraDelikButonuAktifEt();
            isClose = true;
        }
    }



    private void Gameover()
    {
        Debug.LogError("Game Over");
        isGameover = true;
        GameManager.instance.SetGameoverState();
    }

    private bool IsPlanetAboveLine()
    {
        for (int i = 0; i < planetParent.childCount; i++)
        {
            Planet planet = planetParent.GetChild(i).GetComponent<Planet>();

            if (!planet.HasCollided()) continue;

            if (IsPlanetAboveLine(planetParent.GetChild(i))) return true;

        }
        return false;

    }

    private bool BıtıseYaklastı()
    {
        for (int i = 0; i < planetParent.childCount; i++)
        {
            Planet planet = planetParent.GetChild(i).GetComponent<Planet>();

            if (!planet.HasCollided()) continue;

            if (BıtıseYaklastı(planetParent.GetChild(i)))
            {
                //UIManager.instance.KaraDelikButonuAktifEt();
                //isClose = true;
                return true;
            }


        }
        return false;

    }

    private bool BıtıseYaklastı(Transform planet)//bu olduğunda karadelik butonu çıkmalı
    {
        if (planet.position.y > 2.7f)//test için -3.5 normalı 2.7
        {
            return true;
        }
        else
            return false;
    }


    private bool IsPlanetAboveLine(Transform planet)
    {
        if (planet.position.y > 3.5f)//deadLine.transform.position.y) bunu iptal etmemin sebebi restart yaptığımda 4 tane kırmızı error veriyor,missing hatası
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
    private void YaklastıStartTimer()
    {
        Yaklastıtimer = 0;
        YaklastıtimerOn = true;
    }
    private void YaklastıStopTimer()
    {
        YaklastıtimerOn = false;

    }

}
