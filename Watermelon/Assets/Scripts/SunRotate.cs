using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SunRotate : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] Vector3 rotate;
	[SerializeField] float speed;
	[SerializeField]private GameObject blackhole;
	bool ok;
	private void Start()
	{
	   
		transform.position = new Vector3(Random.Range(-1.5f, 1.5f), transform.position.y, transform.position.z);
	}
	void Update()
	{
		transform.Rotate(rotate, Time.deltaTime * speed);
		GetHolled();
		
	}
	
	private void GetHolled()
	{
		if (BlackHole.instance.blackHoleData.size>=1&&!ok)
		{
			
			this.transform.DOScale(new Vector3(0, 0, 0), 1);	
			this.transform.DOMove(transform.position, 1).SetEase(Ease.InOutBack).OnComplete(() =>
		{
			AudioManager.instance.PopupCal();
			ok=true;
		});
		}
	}
}
