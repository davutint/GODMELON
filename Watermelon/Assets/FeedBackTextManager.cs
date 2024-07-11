using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FeedBackTextManager : MonoBehaviour
{
	
	public Vector3 targetPoz;
	
	private void Start()
	{
		GetBigger();
	}
	
	private void GetBigger()
	{
		transform.DOScale(targetPoz,.3f).SetEase(Ease.InOutBack);
		StartCoroutine(BekleVeYokOl());
	}
	IEnumerator BekleVeYokOl()
	{
		yield return new WaitForSeconds(.8f);
		transform.DOScale(Vector3.zero,.15f).SetEase(Ease.InOutBack).OnComplete(()=>
		{
			Destroy(gameObject);
		});
	}
}
