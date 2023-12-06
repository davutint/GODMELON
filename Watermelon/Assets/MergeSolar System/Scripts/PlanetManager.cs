

using UnityEngine;
using System;


public class PlanetManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] Planet[] planetPrefabs;
    [SerializeField] Planet[] spawnablePlanets;
    [SerializeField] Transform planetParent;
    [SerializeField] LineRenderer fruitSpawnLine;
    private Planet currentPlanet;

    [Header("Settings")]
    [SerializeField] private float fruitYSpawnPos;
    [SerializeField] private float spawnDelay;

    [Header("Next Fruit Settings")]
    private int nextFruitIndex;

    [Header("Actions")]
    public static Action onNextPlanetIndexSet;
    private bool canControl;
    private bool isControlling;

    [Header("Debug")]
    [SerializeField] private bool enableGizmos;

    private void Awake()
    {
        MergeManager.onMergeProcessed += MergeProcessedCallback;
        GameManager.onGameStateChanged += OyunBıtınceSpawnEtme;
    }

    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
        GameManager.onGameStateChanged -= OyunBıtınceSpawnEtme;
    }

    private void OyunBıtınceSpawnEtme(GameState state)
    {
        if (state == GameState.Game)
        {
            canControl = true;
        }
        else canControl = false;

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

        currentPlanet.MoveTo(new Vector2(GetSpawnPosition().x, fruitYSpawnPos));
    }

    private void MouseUpCallback()
    {
        HideLine();

        if (currentPlanet != null)
            currentPlanet.EnablePhysics();

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

        Planet planetToInstantiate = spawnablePlanets[nextFruitIndex];
        currentPlanet = Instantiate(planetToInstantiate,
         spawnPosition, Quaternion.identity, planetParent);

        SetNextFruitındex();
    }

    private void SetNextFruitındex()
    {
        nextFruitIndex = UnityEngine.Random.Range(0, spawnablePlanets.Length);
        onNextPlanetIndexSet?.Invoke();
    }
    public Sprite GetNextFruitSprite()
    {
        return spawnablePlanets[nextFruitIndex].GetSprite();
    }
    public string GetNextFruitName()
    {
        return spawnablePlanets[nextFruitIndex].name;
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
        for (int i = 0; i < planetPrefabs.Length; i++)
        {
            if (planetPrefabs[i].GetPlanetType() == planetType)
            {
                SpawnMergedFruit(planetPrefabs[i], spawnPosition);
                break;
            }
        }
    }
    private void SpawnMergedFruit(Planet planet, Vector2 spawnPosition)
    {
        Planet fruitInstance = Instantiate(planet, spawnPosition,
         Quaternion.identity, planetParent);
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
