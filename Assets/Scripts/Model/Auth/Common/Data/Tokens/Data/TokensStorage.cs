using Codice.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;
using static PlasticPipe.Server.MonitorStats;

public class TokensStorage : IInitializable
{
	public TokensDataInstance UserTokens {  get; private set; }

	private string _saveFileName = "userTokens";
	public void Initialize()
	{
		LoadData();
	}


	private void LoadData()
	{
		string filePath = Path.Combine(Application.persistentDataPath, _saveFileName);

		if (File.Exists(filePath))
		{
			string jsonData = File.ReadAllText(filePath);
			UserTokens = JsonUtility.FromJson<TokensDataInstance>(jsonData);
		}
		else
		{
			UserTokens = new TokensDataInstance("", "");
			SaveData();
		}
	}

	public void SaveData()
	{
		string jsonData = JsonUtility.ToJson(UserTokens, true);
		string filePath = Path.Combine(Application.persistentDataPath, _saveFileName);
		File.WriteAllText(filePath, jsonData);
	}

	public void UpdateData(TokensDataInstance userTokens)
	{
		UserTokens = userTokens;
		SaveData();
	}
}
