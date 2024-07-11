using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="LevelConfig",menuName ="ScriptableObjects/LevelConfig",order =1)]
public class LevelConfigSO : ScriptableObject
{
	[Header("Animation Curve")]
	public AnimationCurve AnimationCurve;
	public int MaxLevel;
	public int MaxRequiredExp;
	
	public int GetRequiredExp(int level)
	{	
		Debug.Log("Animaton Curve Değeri: "+AnimationCurve);
		int requiredExperience=Mathf.RoundToInt(AnimationCurve.Evaluate(Mathf.InverseLerp(0,MaxLevel,level))*MaxRequiredExp);
		return requiredExperience;
	}
}