using Codice.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;
using Newtonsoft.Json;
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
			UserTokens = JsonConvert.DeserializeObject<TokensDataInstance>(jsonData);
		}
		else
		{
			UserTokens = new TokensDataInstance("", "");
			SaveData();
		}
	}

	public void SaveData()
	{
		string jsonData = JsonConvert.SerializeObject(UserTokens);
		string filePath = Path.Combine(Application.persistentDataPath, _saveFileName);
		File.WriteAllText(filePath, jsonData);
	}

	public void UpdateData(TokensDataInstance userTokens)
	{
		UserTokens = userTokens;
		SaveData();
	}
}
