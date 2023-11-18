using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Fruit : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;
    [SerializeField] private float scaleSpeed;
    [Header("Data")]
    [SerializeField] private PlanetType planetType;
    private bool canBeMerged;
    private bool hasCollided;
    [Header("Actions")]
    public static Action<Fruit, Fruit> onCollisionWithFruit;
    [Header("Effects")]
    [SerializeField] private ParticleSystem mergeParticles;


    private void Awake()
    {
        GameManager.onGameStateChanged += changeMergeboolean;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= changeMergeboolean;
    }
    private void changeMergeboolean(GameState state)
    {
        if (state == GameState.Gameover)
        {
            canBeMerged = false;
        }
    }

    private void Start()
    {
        Invoke("AllowMerge", .25f);

    }

    private void AllowMerge()
    {
        canBeMerged = true;
    }
    public void EnablePhysics()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
    }

    public void MoveTo(Vector2 targetPosition)
    {
        transform.position = targetPosition;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        ManageCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ManageCollision(collision);
    }

    private void ManageCollision(Collision2D collision)
    {

        hasCollided = true;

        if (!canBeMerged) return;

        if (collision.collider.TryGetComponent(out Fruit otherPlanet))
        {
            if (otherPlanet.GetPlanetType() != planetType) return;

            if (!otherPlanet.CanBeMerged()) return;

            onCollisionWithFruit?.Invoke(this, otherPlanet);

        }
    }
    public void BlackHoleEffect(Fruit fruit, GameObject BlackHoleobj)
    {

        this.transform.DOScale(new Vector3(0, 0, 0), scaleSpeed);
        this.transform.DOMove(BlackHoleobj.transform.position, speed).SetEase(Ease.InOutBack);

    }


    public void Merge()
    {
        if (mergeParticles != null)
        {
            mergeParticles.transform.SetParent(null);
            mergeParticles.Play();
        }

        Destroy(gameObject);
    }




    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }
    public PlanetType GetPlanetType()
    {
        return planetType;
    }

    public bool HasCollided()
    {
        return hasCollided;
    }

    public bool CanBeMerged()
    {
        return canBeMerged;
    }
}
