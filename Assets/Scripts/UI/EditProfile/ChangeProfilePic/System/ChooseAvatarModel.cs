using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
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
		return await _userInfoClient.ChangeProfileIconAsync(_cachedImage);
	}
}
