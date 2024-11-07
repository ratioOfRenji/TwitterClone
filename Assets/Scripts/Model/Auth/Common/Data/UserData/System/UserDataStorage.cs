using System.Collections;
using System.IO;
using UnityEngine;
using Zenject;
using Newtonsoft.Json;

public class UserDataStorage : IInitializable
{
	public UserData _userData { get; private set; }
	private string _saveFileName = "userEmail";

	public void Initialize()
	{
		LoadData();
	}

	public void LoadData()
	{
		string filePath = Path.Combine(Application.persistentDataPath, _saveFileName);

		if (File.Exists(filePath))
		{
			string jsonData = File.ReadAllText(filePath);

			// Deserialize JSON and assign to UserEmail
			var data = JsonConvert.DeserializeObject<UserData>(jsonData);
			_userData = data;
		}
		else
		{
			_userData = new UserData();
			Debug.LogWarning("No data file found. UserEmail not loaded.");
		}
	}

	public void SaveData(string email)
	{
		_userData.Email = email;

		
		string jsonData = JsonConvert.SerializeObject(_userData);

		string filePath = Path.Combine(Application.persistentDataPath, _saveFileName);
		File.WriteAllText(filePath, jsonData);
	}

	
}