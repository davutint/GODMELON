using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Gley.EasyIAP;

public class IAPManager2 : MonoBehaviour
{
	public static IAPManager2 instance;
	public bool removeAds;
	public bool AndromedaUnlocked;
	public bool TriangledUnlocked;
	string andromeda="Andromeda";
	string triangled="Triangulum";
	[SerializeField]TextMeshProUGUI andromedaBuyText;
	[SerializeField]TextMeshProUGUI triangledBuyText;
	[SerializeField]GameObject andromedaBuyButton;
	[SerializeField]GameObject triangledBuyButton;
	[SerializeField]GameObject andromedaStartButton;
	[SerializeField]GameObject triangledStartButton;
	
	private void Awake()
	{
		instance=this;
		
	}
	void Start()
	{
		Gley.EasyIAP.API.Initialize(InitializationComplete);
		
		
	}
	
	public void BuyAndromeda()
	{
		Gley.EasyIAP.API.BuyProduct(ShopProductNames.Andromeda,ProductBought);
	}
	public void BuyTriangled()
	{
		Gley.EasyIAP.API.BuyProduct(ShopProductNames.Triangulum,ProductBought);
	}
	private void ProductBought(IAPOperationStatus status, string message, StoreProduct product)
	{
		if (status == IAPOperationStatus.Success)
		{
			//since all consumable products reward the same coin, a simple type check is enough 
			
			if (product.productName == andromeda)
			{
				AndromedaUnlocked = true;
				andromedaBuyButton.SetActive(false);
				andromedaStartButton.SetActive(true);
				//andromeda unlock et
			}
			if (product.productName == triangled)
			{
				TriangledUnlocked = true;
				triangledBuyButton.SetActive(false);
				triangledStartButton.SetActive(true);
				// üçgeni unluck et
			}
		}
	}

	private void InitializationComplete(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
	{
		if (status == IAPOperationStatus.Success)
		{
			//IAP was successfully initialized
			//loop through all products
			for (int i = 0; i < shopProducts.Count; i++)
			{
				
				if (shopProducts[i].productName == andromeda)
				{
					//if the active property is true, the product is bought
					if (shopProducts[i].active)
					{
						AndromedaUnlocked = true;
						andromedaBuyButton.SetActive(false);
						andromedaStartButton.SetActive(true);
					}
				}
				if (shopProducts[i].productName == triangled	)
				{
					//if the active property is true, the product is bought
					if (shopProducts[i].active)
					{
						TriangledUnlocked = true;
						triangledBuyButton.SetActive(false);
						triangledStartButton.SetActive(true);
					}
				}
			}
		}
		else
		{
			Debug.Log("Error occurred: " + message);
		}
		triangledBuyText.text = $"UNLOCK TRIANGLED FOR {Gley.EasyIAP.API.GetLocalizedPriceString(ShopProductNames.Triangulum)}";
		andromedaBuyText.text = $"UNLOCK ANDROMEDA FOR {Gley.EasyIAP.API.GetLocalizedPriceString(ShopProductNames.Andromeda)}";
	}
	
	public void RestorePurchases()//restore butonuna bağla
	{
		Gley.EasyIAP.API.RestorePurchases(ProductRestored);
	}

	private void ProductRestored(IAPOperationStatus status, string message, StoreProduct product)
	{
		if (status == IAPOperationStatus.Success)
		{
			
			if (product.productName == andromeda)
			{
				AndromedaUnlocked = true;
				andromedaBuyButton.SetActive(false);
				andromedaStartButton.SetActive(true);
				//disable ads here
			}
		
			if (product.productName == triangled)
			{
				TriangledUnlocked = true;
				triangledBuyButton.SetActive(false);
				triangledStartButton.SetActive(true);
				//disable ads here
			}
		}
		else
		{
			Debug.Log("Error occurred: " + message);
		}
	}
}
