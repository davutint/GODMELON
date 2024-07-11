using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[Header("Elements")]
	[SerializeField] private AudioSource mergeSource;
	[SerializeField] private AudioSource anaSesSource;
	[SerializeField] private AudioSource clickSource;
	[SerializeField] private AudioSource BlackHoleSource;
	[SerializeField] private AudioSource PopupSource;
	[SerializeField] private AudioSource lazerSource;
	[SerializeField] private AudioSource portalSource;

	public static AudioManager instance;
	private void Awake()
	{
		MergeManager.onMergeProcessed += MergeProcessedCallback;
		GameManager.onGameStateChanged += StopAnaSes;
		instance = this;

	}

	public void LazerSound()
	{
		lazerSource.Play();
	}
	public void PortalSound()
	{
		portalSource.Play();
	}
	private void StopAnaSes(GameState state)
	{
		if (state == GameState.Gameover)
		{
			anaSesSource.Stop();
		}
		if (state==GameState.ProtonBeam)
		{
			StartCoroutine(bekle());
		}
	}
	IEnumerator bekle()
	{
		yield return new WaitForSeconds(1f);
		PortalSound();
	}

	private void OnDestroy()
	{
		MergeManager.onMergeProcessed -= MergeProcessedCallback;
		GameManager.onGameStateChanged -= StopAnaSes;
	}

	private void MergeProcessedCallback(PlanetType type, Vector2 mergePos)
	{
		PlayMergeSound();
	}

	public void PlayMergeSound()
	{
		mergeSource.pitch = Random.Range(.9f, 1.1f);
		mergeSource.Play();
	}

	public void ClickSoundCal()
	{
		clickSource.Play();
	}
	public void BlackHoleSoundCal()
	{
		BlackHoleSource.Play();
	}

	public void AnaSesCal()
	{
		anaSesSource.Play();
	}
	public void AnaSesDurdur()
	{
		anaSesSource.Stop();
	}


	public void PopupCal()
	{
		PopupSource.pitch = Random.Range(.9f, 1.1f);
		PopupSource.Play();
	}



}
