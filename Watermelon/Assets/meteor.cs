using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteor : MonoBehaviour
{
	Animator anim;
	private void Start()
	{
		anim = GetComponent<Animator>();
	}
	

	private void OnCollisionEnter2D(Collision2D collision)
	{
		anim.SetTrigger("carptı");
		Destroy(this.gameObject, .5f);
		if (collision.collider.TryGetComponent(out Planet planet))
		{
			planet.Merge();
		   
		}
	}
	
}
