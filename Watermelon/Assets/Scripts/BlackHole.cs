using System;
using System.Collections;

using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;



public class BlackHole : MonoBehaviour
{
	[Header("blackhole")]
	[SerializeField] private GameObject blackHole;
	
	public Vector3 blackHoleSize;//bunu data kısmında çektiğimiz değere eşitlemek gerekir.
	
	public List<Planet> toplananPlanetler;
	public static BlackHole instance;
	public BlackHoleData blackHoleData;
	private async void Awake()
	{
		instance = this;
	 	blackHoleData=await GameCloudDatas.LoadData<BlackHoleData>("blackhole");
		blackHoleSize=new Vector3(blackHoleData.size,blackHoleData.size,blackHoleData.size);
	}
	

	//radius her zaman blackhole scale'ın /0.3'ü kadar olmalı.
	//karşılaştırılacak olan x değeri ise her zaman blackhole scale'ın 6 katı olmalı. 
	//Başlangıç Scale 0.1 ise karşılaştırılan scale 0.6 olmalı.
	//blackhole scale 3 katı ise radius olmalı, karşılaştırılan scaleın ise /3'ü kadar olmalı. 
	//Transform.Scale 1 ise yani en büyük boyutta ise;
	//radius=3 olmalı karşılaştırılan scale ise 6 olmalıdır. Bu sayede HERŞEYİ yutabilir.
	//Sanırım bunu switch case ile sabir bir kural haline getirmeliyiz.
	//belki planettype'a göre yapabiliriz. Scale ile 10 çarparsak 1 elde ederiz, 1 ve küçük olanları yutabilsin şeklinde
	public void BlackHoleProcessCallback()//bunu butona bağladığında iş tamam.
	{
		blackHole.SetActive(true);
		
		blackHole.transform.DOScale(blackHoleSize, .7f).OnComplete(() =>
		{
			EatPlanets();
			
		});
		

	}
	private void EatPlanets()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

		foreach (Collider2D collider in colliders)
		{
			if (collider.TryGetComponent(out Planet planet))
			{
				if (!planet.CanBeMerged()) return;
				if (toplananPlanetler.Contains(planet)) return;
				toplananPlanetler.Add(planet);
			}
		}

		if (toplananPlanetler.Count > 0)
		{
			List<Planet> planetsToRemove = new List<Planet>();

			foreach (Planet planet in toplananPlanetler)
			{
				
				if ((int)planet.GetPlanetType()<=blackHoleData.size*10)
				{
					planet.GetComponent<Collider2D>().enabled = false;
					planet.BlackHoleEffect(this.gameObject);
					planetsToRemove.Add(planet);
					Debug.Log("YUTULAN GEZEGEN= "+planet.name);
				}
				else
				{
					//planet.DropText();
					Debug.Log(planet.name + " " + planet.GetTransform().localScale + " büyüktür blackhole boyutundan ");
					planetsToRemove.Add(planet);
				}
			
			}

			foreach (Planet planet in planetsToRemove)
			{
				toplananPlanetler.Remove(planet);
			}
		}
		
		StartCoroutine(BlackHoleClose());
	}


	public IEnumerator BlackHoleClose()
	{
		
		yield return new WaitForSeconds(2.5f);
		
		blackHole.transform.DOScale(new Vector3(0, 0, 0), 1).SetEase(Ease.InOutBack).OnComplete(() =>
		  {
			 //GameManager.instance.SetGameState();  burası artık bir event değil bir oyun sonu oldu bu nedenle setgameover yapıyoruz;
			  	blackHole.SetActive(false);
				GameManager.instance.SetGameState(GameManager.instance.selectingGameState);
				if (GameManager.instance.IsGameState())
				{
					UIManager.instance.BlackHoleButtonObj.SetActive(false);
				}
				GameManager.instance.blackHoleActive=false;
				
		  });
	}
	
}


