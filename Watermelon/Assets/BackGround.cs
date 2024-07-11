using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
	
	[SerializeField]float _leftx;
	[SerializeField]float _rightx;
	
	float newXPosition;
	private void Start()
	{
		newXPosition=Random.Range(_leftx,_rightx);
		transform.position=new Vector3(newXPosition,0,0);
		
	}
	
}
