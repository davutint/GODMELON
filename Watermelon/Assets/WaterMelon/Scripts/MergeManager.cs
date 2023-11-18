using UnityEngine;
using System;
using System.Collections;

public class MergeManager : MonoBehaviour
{
    [Header("Settings")]
    Fruit lastSender;

    [Header("Actions")]
    public static Action<PlanetType, Vector2> onMergeProcessed;


    private void Awake()
    {
        Fruit.onCollisionWithFruit += CollisionBetweenFruitCallback;
    }
    private void OnDestroy()
    {
        Fruit.onCollisionWithFruit -= CollisionBetweenFruitCallback;
    }

    private void CollisionBetweenFruitCallback(Fruit sender, Fruit otherFruit)
    {
        if (lastSender != null)
        {
            return;
        }

        lastSender = sender;

        ProcessMerger(sender, otherFruit);
        Debug.Log("Collision detected by" + sender.name);
    }

    private void ProcessMerger(Fruit sender, Fruit otherFruit)//fruit isimlerini planet ile değiştir daha mantıklı olacaktır
    {

        PlanetType mergePlanetType = sender.GetPlanetType();
        mergePlanetType += 1;
        Vector2 planetSpawnPos = (sender.transform.position + otherFruit.transform.position) / 2;

        sender.Merge();
        otherFruit.Merge();

        StartCoroutine(ResetLastSenderCoroutine());

        onMergeProcessed?.Invoke(mergePlanetType, planetSpawnPos);
    }
    IEnumerator ResetLastSenderCoroutine()
    {
        yield return new WaitForEndOfFrame();

        lastSender = null;
    }
}
