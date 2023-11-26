using System;
using System.Linq;
using System.Threading.Tasks;
using Apple.GameKit;
using Apple.GameKit.Leaderboards;
using UnityEngine;

public class Authentication : MonoBehaviour
{
    private async void Awake()
    {
        await Login();
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

            // Fetch the items.
            //var fetchItemsResponse = await GKLocalPlayer.Local.FetchItems();

        }
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