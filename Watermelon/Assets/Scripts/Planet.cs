using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;

public class Planet : MonoBehaviour,ISelectable
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
	public static Action<Planet, Planet> onCollisionWithPlanet;
	[Header("Effects")]
	[SerializeField] private ParticleSystem mergeParticles;

	[SerializeField] private ParticleSystem destroyEffect;
	[SerializeField] private GameObject selectedImage;
	
	private void Awake()
	{
		GameManager.onGameStateChanged += changeMergeboolean;
		mergeParticles=GetComponentInChildren<ParticleSystem>();
		selectedImage=transform.Find("SelectedImage").gameObject;
		destroyEffect = transform.Find("DestroyEffect").GetComponent<ParticleSystem>();
		

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
		if (state==GameState.Game)
		{
			EnablePhysics();
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

		if (collision.collider.TryGetComponent(out Planet otherPlanet))
		{
			if (otherPlanet.GetPlanetType() != planetType) return;

			if (!otherPlanet.CanBeMerged()) return;

			onCollisionWithPlanet?.Invoke(this, otherPlanet);

		}
	}
	public void BlackHoleEffect(GameObject BlackHoleobj)
	{

		this.transform.DOScale(new Vector3(0, 0, 0), scaleSpeed);
		this.transform.DOMove(BlackHoleobj.transform.position, speed).SetEase(Ease.InOutBack).OnComplete(() =>
		{
			AudioManager.instance.PopupCal();
			ScoreManager.instance.KaraDelikGezegenleriYuttuPuanı(GetPlanetType());
		});

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



	public Transform GetTransform()
	{
		return this.transform;
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

	public Vector3 GetPosition()
	{
		return transform.position; // Gezegenin pozisyonunu döndür

	}

	public void DestroyObj()
	{
	   // Parçacık efektleri ve diğer gerekli temizlemeleri yaparak gezegeni yok et	
		if (destroyEffect != null)
		{
			GetComponentInChildren<SpriteRenderer>().DOColor(Color.red,1f).OnComplete(()=>
			{
				destroyEffect.transform.SetParent(null);
				destroyEffect.Play();
				ScoreManager.instance.score+=(int)planetType*75;
				ScoreManager.instance.UpdateScoreText();
				Destroy(gameObject);
			});
			
		}
		
	}

	public void SelectedImageOpen()
	{
		selectedImage.SetActive(true);
	}
}
