using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetUnlocker : MonoBehaviour
{
	//datayı buradan çekicez;
	public GameObject[] planetContents;
	public static PlanetUnlocker instance;
	private void Awake()
	{
		instance=this;
	}
	
	
	public  void CheckPlanets()//karadelik update edince de bu çağrılmalı
	{
		List<PlanetContentManager> planetContentManagers=new List<PlanetContentManager>();
		foreach (var planets in planetContents)
		{
			if (planets.TryGetComponent(out PlanetContentManager planet))
			{
				planetContentManagers.Add(planet);
			}
		}
		foreach (var planet in planetContentManagers)
		{
			int i=planet.planetSize;
			if (i<=CloudDatas.instance.blackHoleData.size*10)//10 ile çarpıyoruz ve enum 1 den başladığı için 0.1=1 oluyor
			{
				planet.BlackHoleCanEat();
			}
		}
	}
}
