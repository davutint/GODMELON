using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

public class GameCloudDatas : MonoBehaviour
{
	public static GameCloudDatas instance;
	public PlayerData PlayerData;
	public ShipData shipData;
	
	private void Awake()
	{
		instance=this;
		
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
}
