using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public Slider slider;


    [Header("ELEMENTS")]

    private const string ArkaPlanSes = "ArkaPlanSes";
    private const string ArkaPlanData = "ArkaPlanData";


    private void Awake()
    {
        instance = this;
    }
    public void SaveData()//burada slider datasını kaydetmeliyiz,backto game butonunda yapmalıyız
    {
        PlayerPrefs.SetFloat(ArkaPlanSes, slider.value);//bunu çekeceğiz;
        AudioManager.instance.SesAyarla();
        GameManager.instance.SetGameState();
    }
    //burası startta çalışmalı ses manager bunu çekmeli
    public float LoadData()//bunuda oyun basında sesmanagerdan çekip buradaki değerleri ses değeri olarak oyun basında ayarlamalıyız
    {
        float ArkaPlanSesData = PlayerPrefs.GetFloat(ArkaPlanSes);
        return ArkaPlanSesData;
    }

    public void SaveBackGroundData(int arkaPlan)//oyun basında burada sormalıyız hangi arka plan olsun diye
    {
        PlayerPrefs.SetInt(ArkaPlanData, arkaPlan);
        UIManager.instance.SelectStartBackGround();
    }
    public void GetSliderValueData()//bunu setting açıldığında çekmelisin
    {
        slider.value = PlayerPrefs.GetFloat(ArkaPlanSes);
    }



}
