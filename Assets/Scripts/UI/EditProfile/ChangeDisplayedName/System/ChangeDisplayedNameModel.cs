using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDisplayedNameModel  
{
	private readonly UserInfoClient _userInfoClient;
	public ChangeDisplayedNameModel(UserInfoClient userInfo)
	{
		_userInfoClient = userInfo;
	}

	public async UniTask<bool> TryUpdateDisplayedName(string newName)
	{
		if (newName.Length < 2) return false;

		bool success = await _userInfoClient.ChangeDisplayedNameAsync(newName);
		if (success) { Debug.Log("Succesfully changed name"); }
		return success;
	}
}
