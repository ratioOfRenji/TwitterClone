using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SettingsModel
{
	private readonly DeleteUser _deleteUser;
	private readonly GoogleAuthentication _googleAuthentication;

	public SettingsModel(DeleteUser deleteUser, GoogleAuthentication googleAuthentication)
	{
		_deleteUser = deleteUser;
		_googleAuthentication = googleAuthentication;
	}

	public async void DeleteUser()
	{
		Debug.Log("Deleting user");
		bool deleted =await _deleteUser.DeleteUserAsync();
		if (!deleted) 
		{
			Debug.LogError("Failed To Delete User");
			return;
		}
#if !UNITY_EDITOR
		_googleAuthentication.OnSignOut();
#endif
		PlayerPrefs.DeleteAll();

		ClearCache();

		DeleteLocalSaveFiles();
		SceneManager.LoadScene("BlogScene");
	}

	public void SignOut()
	{
		Debug.Log("Signing out");
#if !UNITY_EDITOR
		_googleAuthentication.OnSignOut();
#endif

		PlayerPrefs.DeleteAll();

		ClearCache();

		DeleteLocalSaveFiles();
		SceneManager.LoadScene("BlogScene");
	}

	private void ClearCache()
	{
		if (Caching.ClearCache())
		{
			Debug.Log("Cache cleared successfully.");
		}
		else
		{
			Debug.LogWarning("Failed to clear cache.");
		}
	}

	private void DeleteLocalSaveFiles()
	{
		string savePath = Application.persistentDataPath;

		if (Directory.Exists(savePath))
		{
			Directory.Delete(savePath, true);
			Debug.Log("Local save files deleted successfully.");
		}
		else
		{
			Debug.LogWarning("No local save files found to delete.");
		}
	}
}
