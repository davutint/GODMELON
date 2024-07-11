using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaraDelikDeneme : MonoBehaviour
{
	public static KaraDelikDeneme instance;
	public Vector3 blackHoleSize;
	
	public List<Planet> toplananPlanetler;
	private BlackHoleData blackHoleData;
	public static Action<PlanetType, Vector2> onMergeProcessed;

	[SerializeField]private float blackHoleForce;
	private void Awake()
	{
		instance=this;
		Planet.onCollisionWithPlanet += CollisionBetweenPlanetCallback;
	}
	private void Start()//bu deneme scriptini silmek gerekecek.
	{
		BoyutAyarla();
	}

	private void CollisionBetweenPlanetCallback(Planet planet1, Planet planet2)
	{
		toplananPlanetler.Remove(planet1);
		toplananPlanetler.Remove(planet2);
	}

	private void OnDestroy()
	{
		Planet.onCollisionWithPlanet -= CollisionBetweenPlanetCallback;
	}

	private void Update()
	{
		EatPlanets();
	}
	public void BoyutAyarla()
	{
		blackHoleData=CloudDatas.instance.blackHoleData;
		blackHoleSize=new Vector3(blackHoleData.size,blackHoleData.size,blackHoleData.size);
		transform.localScale=blackHoleSize;
		blackHoleForce=blackHoleSize.x*10;
		
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
				
				if ((int)planet.GetPlanetType()<=blackHoleForce)
				{
					planet.GetComponent<Collider2D>().enabled = false;
					planet.BlackHoleEffect(this.gameObject);
					planetsToRemove.Add(planet);
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
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 10);
	}
}
