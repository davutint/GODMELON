using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class PlanetContentManager : MonoBehaviour
{
	public int planetSize;
	public PlanetType planetType;
	private GameObject lockImage;
	private Image planetImage;
	private void Awake()
	{
		lockImage=transform.Find("Icon_Lock").gameObject;
		planetImage=transform.Find("ItemIcon").GetComponent<Image>();
		planetSize=(int)planetType;
	}

	public void BlackHoleCanEat()
	{
		lockImage.SetActive(false);
		planetImage.DOFade(1,.1f);
	}
}
