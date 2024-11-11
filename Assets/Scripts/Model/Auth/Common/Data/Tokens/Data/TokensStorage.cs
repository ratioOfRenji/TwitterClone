using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
public class TokensStorage
{
	public TokensDataInstance UserTokens {  get; private set; }

	private string _saveFileName = "userTokens";

	private readonly TokenRefresh _tokenRefresh;

	private Subject<Unit> onTokenSetSubject = new Subject<Unit>();
	public IObservable<Unit> OnTokensSetAsObservable()
	{
		return onTokenSetSubject.AsObservable();
	}
	public TokensStorage(TokenRefresh tokenRefresh)
	{
		_tokenRefresh = tokenRefresh;
	}


	public async UniTask<bool> LoadData()
	{
		string filePath = Path.Combine(Application.persistentDataPath, _saveFileName);

		if (File.Exists(filePath))
		{
			string jsonData = File.ReadAllText(filePath);
			UserTokens = JsonConvert.DeserializeObject<TokensDataInstance>(jsonData);
			if(!string.IsNullOrEmpty(UserTokens.RefreshToken))
			{
				try
				{
					TokensDataInstance refreshed = await _tokenRefresh.RefreshTokenAsync(UserTokens);
					UpdateData(refreshed);
					Debug.Log("Tokens refreshed successfuly!");
					return true;
				}
				catch
				{
					Debug.LogError("Unable to refresh tokens");
					return false;
				}
			}
			
		}
		else
		{
			UserTokens = new TokensDataInstance("", "");
			SaveData();
		}
		return false;
	}

	public void SaveData()
	{
		string jsonData = JsonConvert.SerializeObject(UserTokens);
		string filePath = Path.Combine(Application.persistentDataPath, _saveFileName);
		File.WriteAllText(filePath, jsonData);
	}

	public void SetData(TokensDataInstance userTokens)
	{
		UserTokens = userTokens;
		SaveData();
		onTokenSetSubject.OnNext(Unit.Default);
	}
	public void UpdateData(TokensDataInstance userTokens)
	{
		UserTokens = userTokens;
		SaveData();
	}
}
