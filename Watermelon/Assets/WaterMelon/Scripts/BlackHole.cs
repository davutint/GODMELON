using System;
using System.Collections;

using UnityEngine;
using DG.Tweening;


public class BlackHole : MonoBehaviour
{
    [Header("blackhole")]
    [SerializeField] private GameObject blackHole;
    private Vector2 holePos;
    private void Awake()
    {
        GameManager.onGameStateChanged += BlackHoleEffectDeneme;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= BlackHoleEffectDeneme;
    }

    private void BlackHoleEffectDeneme(GameState state)
    {
        if (state == GameState.Gameover)
        {
            //blackhole;
            //eğer sahnede black hole varsa game over menüsü gelmesin
            BlackHoleProcessCallback();

        }
    }

    private void BlackHoleProcessCallback()
    {
        blackHole.SetActive(true);
        holePos = blackHole.transform.position;

        blackHole.transform.DOScale(new Vector3(2, 2, 2), 1f).OnComplete(() =>
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(holePos, 10f);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Fruit fruit))
                {
                    fruit.GetComponent<Collider2D>().enabled = false;
                    fruit.BlackHoleEffect(fruit, blackHole);
                    StartCoroutine(BlackHoleClose());

                }
            }
        });

    }

    IEnumerator BlackHoleClose()
    {
        yield return new WaitForSeconds(2.5f);
        blackHole.transform.DOScale(new Vector3(0, 0, 0), 1).SetEase(Ease.InOutBack).OnComplete(() =>
          {
              GameManager.instance.SetGameoverMenuState();
          });
    }
}
