

using UnityEngine;
using System;


public class FruitManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] Fruit[] fruitPrefabs;
    [SerializeField] Fruit[] spawnableFruits;
    [SerializeField] Transform fruitParent;
    [SerializeField] LineRenderer fruitSpawnLine;
    private Fruit currentFruit;

    [Header("Settings")]
    [SerializeField] private float fruitYSpawnPos;
    [SerializeField] private float spawnDelay;

    [Header("Next Fruit Settings")]
    private int nextFruitIndex;

    [Header("Actions")]
    public static Action onNextFruitIndexSet;
    private bool canControl;
    private bool isControlling;

    [Header("Debug")]
    [SerializeField] private bool enableGizmos;

    private void Awake()
    {
        MergeManager.onMergeProcessed += MergeProcessedCallback;
    }

    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
    }

    private void Start()
    {
        SetNextFruitındex();
        canControl = true;
        HideLine();

    }
    private void Update()
    {
        if (!GameManager.instance.IsGameState())
        {
            return;
        }
        if (canControl)
        {
            ManagePlayerInput();
        }



    }


    private void ManagePlayerInput()
    {


        if (Input.GetMouseButtonDown(0))
            MouseDownCallback();

        else if (Input.GetMouseButton(0))
        {
            if (isControlling)
                MouseDragCallback();
            else
                MouseDownCallback();
        }

        else if (Input.GetMouseButtonUp(0) && isControlling)
            MouseUpCallback();



    }
    private void MouseDownCallback()
    {
        DisplayLine();
        PlaceLineAtClickedPosition();

        SpawnFruit();

        isControlling = true;
    }

    private void MouseDragCallback()
    {
        PlaceLineAtClickedPosition();

        currentFruit.MoveTo(new Vector2(GetSpawnPosition().x, fruitYSpawnPos));
    }

    private void MouseUpCallback()
    {
        HideLine();

        if (currentFruit != null)
            currentFruit.EnablePhysics();

        canControl = false;
        StartControlTimer();

        isControlling = false;
    }



    private void StartControlTimer()
    {
        Invoke("StopControlTimer", spawnDelay);
    }

    private void StopControlTimer()
    {
        canControl = true;
    }
    private void SpawnFruit()
    {

        Vector2 spawnPosition = GetSpawnPosition();

        Fruit fruitToInstantiate = spawnableFruits[nextFruitIndex];
        currentFruit = Instantiate(fruitToInstantiate,
         spawnPosition, Quaternion.identity, fruitParent);

        SetNextFruitındex();
    }

    private void SetNextFruitındex()
    {
        nextFruitIndex = UnityEngine.Random.Range(0, spawnableFruits.Length);
        onNextFruitIndexSet?.Invoke();
    }
    public Sprite GetNextFruitSprite()
    {
        return spawnableFruits[nextFruitIndex].GetSprite();
    }
    public string GetNextFruitName()
    {
        return spawnableFruits[nextFruitIndex].name;
    }
    private Vector2 GetClickedWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private Vector2 GetSpawnPosition()
    {
        Vector2 worldClickedPosition = GetClickedWorldPosition();
        worldClickedPosition.y = fruitYSpawnPos;
        return worldClickedPosition;
    }

    private void PlaceLineAtClickedPosition()
    {
        fruitSpawnLine.SetPosition(0, GetSpawnPosition());
        fruitSpawnLine.SetPosition(1, GetSpawnPosition() + Vector2.down * 15);
    }

    private void HideLine()
    {
        fruitSpawnLine.enabled = false;
    }
    private void DisplayLine()
    {
        fruitSpawnLine.enabled = true;
    }
    private void MergeProcessedCallback(PlanetType planetType, Vector2 spawnPosition)
    {
        for (int i = 0; i < fruitPrefabs.Length; i++)
        {
            if (fruitPrefabs[i].GetPlanetType() == planetType)
            {
                SpawnMergedFruit(fruitPrefabs[i], spawnPosition);
                break;
            }
        }
    }
    private void SpawnMergedFruit(Fruit fruit, Vector2 spawnPosition)
    {
        Fruit fruitInstance = Instantiate(fruit, spawnPosition,
         Quaternion.identity, fruitParent);
        fruitInstance.EnablePhysics();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!enableGizmos)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-50, fruitYSpawnPos, 0), new Vector3(50, fruitYSpawnPos, 0));

    }
#endif
}
