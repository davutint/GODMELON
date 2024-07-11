using UnityEngine;
using System;
using System.Collections;

public class MergeManager : MonoBehaviour
{
    [Header("Settings")]
    Planet lastSender;

    [Header("Actions")]
    public static Action<PlanetType, Vector2> onMergeProcessed;


    private void Awake()
    {
        Planet.onCollisionWithPlanet += CollisionBetweenPlanetCallback;
    }
    private void OnDestroy()
    {
        Planet.onCollisionWithPlanet -= CollisionBetweenPlanetCallback;
    }

    private void CollisionBetweenPlanetCallback(Planet sender, Planet otherPlanet)
    {
        if (lastSender != null)
        {
            return;
        }

        lastSender = sender;

        ProcessMerger(sender, otherPlanet);
        Debug.Log("Collision detected by" + sender.name);
    }

    private void ProcessMerger(Planet sender, Planet otherPlanet)//fruit isimlerini planet ile değiştir daha mantıklı olacaktır
    {

        PlanetType mergePlanetType = sender.GetPlanetType();
        mergePlanetType += 1;
        Vector2 planetSpawnPos = (sender.transform.position + otherPlanet.transform.position) / 2;

        sender.Merge();
        otherPlanet.Merge();

        StartCoroutine(ResetLastSenderCoroutine());

        onMergeProcessed?.Invoke(mergePlanetType, planetSpawnPos);
    }
    IEnumerator ResetLastSenderCoroutine()
    {
        yield return new WaitForEndOfFrame();

        lastSender = null;
    }
}
