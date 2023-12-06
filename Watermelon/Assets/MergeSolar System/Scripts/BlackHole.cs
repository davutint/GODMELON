using System;
using System.Collections;

using UnityEngine;
using DG.Tweening;


public class BlackHole : MonoBehaviour
{
    [Header("blackhole")]
    [SerializeField] private GameObject blackHole;
    private Vector2 holePos;

    public static BlackHole instance;
    private void Awake()
    {
        instance = this;
        //GameManager.onGameStateChanged += BlackHoleEffectDeneme;
    }

    /* private void OnDestroy()
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
     }*/

    public void BlackHoleProcessCallback()//bunu butona bağladığında iş tamam.
    {
        blackHole.SetActive(true);
        AudioManager.instance.BlackHoleSoundCal();
        holePos = blackHole.transform.position;

        blackHole.transform.DOScale(new Vector3(2, 2, 2), 1f).OnComplete(() =>
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(holePos, 10f);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Planet planet))
                {
                    planet.GetComponent<Collider2D>().enabled = false;
                    planet.BlackHoleEffect(planet, blackHole);
                    StartCoroutine(BlackHoleClose());

                }
            }
        });

    }

    public IEnumerator BlackHoleClose()
    {
        yield return new WaitForSeconds(2.5f);
        blackHole.transform.DOScale(new Vector3(0, 0, 0), 1).SetEase(Ease.InOutBack);/*.OnComplete(() =>
          {
              GameManager.instance.SetGameoverMenuState();
          });*/
    }
}
