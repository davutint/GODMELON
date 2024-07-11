using System;
using System.Linq;
using System.Threading.Tasks;
using Apple.GameKit;
using Unity.Services.Core;
using Apple.GameKit.Leaderboards;
using TMPro;
using UnityEngine;
using Unity.Services.Authentication;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using UnityEngine.SocialPlatforms.GameCenter;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Collections;



public class GameCenterManager : MonoBehaviour
{
	string Signature;
	string TeamPlayerID;
	string Salt;
	string PublicKeyUrl;
	ulong Timestamp;
	[SerializeField]private Image playerPhotoDisplay;
	[SerializeField]private TextMeshProUGUI playerName;
	private Sprite cachedPhotoSprite;
	
	public bool girisYaptı;
	[SerializeField]private CloudDatas cloudDatas;
	
	// Start is called before the first frame update
	private async void Awake()
	{
		
		try
		{
			await UnityServices.InitializeAsync();
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
		
	}
	  async void Start()
	{
		
		
		cloudDatas=GetComponent<CloudDatas>();
		await Login();
		//await SignInAnonymouslyAsync();
		
	}
	
async Task SignInAnonymouslyAsync()
{
	try
	{
		await AuthenticationService.Instance.SignInAnonymouslyAsync();
		Debug.Log("Sign in anonymously succeeded!");
		await CloudDatas.instance.LoadPlayerData();
		SetupEvents();
		//bütün dataları alıyoruz(eğer varsa,yoksa default değerleri atayım saveliyoruz.), bütün dataları cloudDatas scriptinden çekeceğiz.
		
		// Shows how to get the playerID
		Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); 

	}
	catch (AuthenticationException ex)
	{
		// Compare error code to AuthenticationErrorCodes
		// Notify the player with the proper error message
		Debug.LogException(ex);
	}
	catch (RequestFailedException ex)
	{
		// Compare error code to CommonErrorCodes
		// Notify the player with the proper error message
		Debug.LogException(ex);
	 }
}

	public async Task Login()
	{
		if (!GKLocalPlayer.Local.IsAuthenticated)
		{
			// Perform the authentication.
			var player = await GKLocalPlayer.Authenticate();
			Debug.Log($"GameKit Authentication: player {player}");

			// Grab the display name.
			var localPlayer = GKLocalPlayer.Local;
			Debug.Log($"Local Player: {localPlayer.DisplayName}");
			//playerName.text=localPlayer.DisplayName;
			// Fetch the items.
			var fetchItemsResponse =  await GKLocalPlayer.Local.FetchItems();

			Signature = Convert.ToBase64String(fetchItemsResponse.GetSignature());
			TeamPlayerID = localPlayer.TeamPlayerId;
			Debug.Log($"Team Player ID: {TeamPlayerID}");

			Salt = Convert.ToBase64String(fetchItemsResponse.GetSalt());
			PublicKeyUrl = fetchItemsResponse.PublicKeyUrl;
			Timestamp = fetchItemsResponse.Timestamp;
			await LoadPlayerPhoto(localPlayer);
			playerName.text=localPlayer.DisplayName;
			Debug.Log($"GameKit Authentication: signature => {Signature}");
			Debug.Log($"GameKit Authentication: publickeyurl => {PublicKeyUrl}");
			Debug.Log($"GameKit Authentication: salt => {Salt}");
			Debug.Log($"GameKit Authentication: Timestamp => {Timestamp}");
			await SignInWithAppleGameCenterAsync(Signature,TeamPlayerID,PublicKeyUrl,Salt,Timestamp);

		}
		else
		{
			Debug.Log("AppleGameCenter player already logged in.");
			await CloudDatas.instance.LoadPlayerData();
		}
	}
	async Task SignInWithAppleGameCenterAsync(string signature, string teamPlayerId, string   publicKeyURL, string salt, ulong timestamp)
	{
   		try
		{
		await AuthenticationService.Instance.SignInWithAppleGameCenterAsync(signature, teamPlayerId, publicKeyURL, salt, timestamp);
	   	 Debug.Log("SignIn is successful.");
		 await CloudDatas.instance.LoadPlayerData();
			
		}
   	 catch (AuthenticationException ex)
		{
		// Compare error code to AuthenticationErrorCodes
		// Notify the player with the proper error message
	 	   Debug.LogException(ex);
		}
		catch (RequestFailedException ex)
		{
		// Compare error code to CommonErrorCodes
		// Notify the player with the proper error message
	   	 Debug.LogException(ex);
		}
		SetupEvents();
	}
	
	void SetupEvents() {
  AuthenticationService.Instance.SignedIn += () => {
	// Shows how to get a playerID
	Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
	
	// Shows how to get an access token
	Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
	

  };

  AuthenticationService.Instance.SignInFailed += (err) => {
	Debug.LogError(err);
  };

  AuthenticationService.Instance.SignedOut += () => {
	Debug.Log("Player signed out.");
  };

  AuthenticationService.Instance.Expired += () =>
	{
		Debug.Log("Player session could not be refreshed and expired.");
	};
}

public async static Task SaveData<T>(T inData,string key)
{
	var data = new Dictionary<string, object>{{key,inData}};
	await CloudSaveService.Instance.Data.Player.SaveAsync(data);
}

public async static Task<T> LoadData<T>(string key)
{
	var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{key});
	if (playerData.TryGetValue(key, out var keyName)) {
		var item= keyName.Value.GetAs<T>();
		
		return item;
	}
	return default;
}


	private async Task LoadPlayerPhoto(GKLocalPlayer player)
	{
		// Oyuncu fotoğrafını yükle ve önbelleğe al
		if (cachedPhotoSprite == null)
		{
			try
			{
				var photo = await player.LoadPhoto(GKPlayer.PhotoSize.Small);
				if (photo != null)
				{
					cachedPhotoSprite = Texture2DToSprite(photo);
					DisplayPlayerPicture(cachedPhotoSprite);
					Debug.Log("FOTO BASARILI SEKILDE ALINDI");
				}
				else
				{
					Debug.LogError("Failed to load player photo");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"Error loading player photo: {ex.Message}");
			}
		}
		else
		{
			DisplayPlayerPicture(cachedPhotoSprite);
		}
	}

	private void DisplayPlayerPicture(Sprite photoSprite)
	{
		playerPhotoDisplay.sprite = photoSprite;
		
	}

	private Sprite Texture2DToSprite(Texture2D texture)
	{
		return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
	}

	public void ShowAccesPoint()
	{
		GKAccessPoint.Shared.IsActive = !GKAccessPoint.Shared.IsActive;


	}

	public async void OnShowLeaderboard()//bunu bir butona bağlamalıyız.
	{
		try
		{

			var leaderboards = await GKLeaderboard.LoadLeaderboards();
			var leaderboard = leaderboards.First(l => l.BaseLeaderboardId == "Godmelon");
			// Wait for player to close the dialog...
			var gameCenter = GKGameCenterViewController.Init(GKGameCenterViewController.GKGameCenterViewControllerState.Leaderboards);
			await gameCenter.Present();


			var scores = await leaderboard.LoadEntries(GKLeaderboard.PlayerScope.Global, GKLeaderboard.TimeScope.AllTime, 0, 100);

			Debug.LogError($"my score: {scores.LocalPlayerEntry.Score}");

			foreach (var score in scores.Entries)
			{
				Debug.LogError($"score: {score.Score} by {score.Player.DisplayName}");
			}

		}
		catch (Exception exception)
		{
			Debug.LogError(exception);
		}
	}


} 
