using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ChooseAvatarModel
{
	UserInfoClient _userInfoClient;
	
	public ChooseAvatarModel(UserInfoClient userInfoClient)
	{
		_userInfoClient = userInfoClient;
	}
	public EIconType _cachedImage { get; private set; }

	public void UpdateCachedImage(EIconType image)
	{
		Debug.Log("Updated cached image");
		_cachedImage = image;
	}

	public async UniTask<bool> UpdateProfilePick()
	{
		bool success= await _userInfoClient.ChangeProfileIconAsync(_cachedImage);
		return success;
	}
}
