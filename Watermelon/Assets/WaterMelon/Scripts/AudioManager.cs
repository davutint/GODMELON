using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private AudioSource mergeSource;

    private void Awake()
    {
        MergeManager.onMergeProcessed += MergeProcessedCallback;
    }

    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
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
}
