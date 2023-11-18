using UnityEngine;
using TMPro;
using UnityEngine.UI;
[RequireComponent(typeof(FruitManager))]
public class FruitManagerUI : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Image nextFruitImage;
    private FruitManager fruitManager;
    private void Awake()
    {
        FruitManager.onNextFruitIndexSet += UpdateNextFruitİmage;

    }

    private void OnDestroy()
    {
        FruitManager.onNextFruitIndexSet -= UpdateNextFruitİmage;

    }


    private void UpdateNextFruitİmage()
    {
        if (fruitManager == null)
        {
            fruitManager = GetComponent<FruitManager>();
        }
        nextFruitImage.sprite = fruitManager.GetNextFruitSprite();
    }
}
