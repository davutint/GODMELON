using UnityEngine;
using TMPro;
using UnityEngine.UI;
[RequireComponent(typeof(PlanetManager))]
public class PlanetManagerUI : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Image nextPlanetImage;
    private PlanetManager planetManager;
    private void Awake()
    {
        PlanetManager.onNextPlanetIndexSet += UpdateNextFruitİmage;

    }

    private void OnDestroy()
    {
        PlanetManager.onNextPlanetIndexSet -= UpdateNextFruitİmage;

    }


    private void UpdateNextFruitİmage()
    {
        if (planetManager == null)
        {
            planetManager = GetComponent<PlanetManager>();
        }
        nextPlanetImage.sprite = planetManager.GetNextFruitSprite();
    }
}
