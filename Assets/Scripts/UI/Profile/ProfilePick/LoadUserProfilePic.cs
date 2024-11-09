using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
public class LoadUserProfilePic : MonoBehaviour
{
    [SerializeField] private AvatarsStorage avatarsStorage;
    [SerializeField] Image icon;
    [Inject] private UserInfoClient userInfoClient;
    [Inject] private ChooseAvatarPresenter _chooseAvatarPresenter;
	void Start()
    {
        loadIcon();
        _chooseAvatarPresenter.OnProfilePicUpdatedAsObservable().Subscribe(_ => loadIcon());
	}

    private async UniTask loadIcon()
    {
			UserProfile userProfile = await userInfoClient.GetUserProfileAsync();
            icon.sprite = avatarsStorage.iconsDictionary[userProfile.Icon];
        Debug.Log("Loaded icon succesfully");
	}
}
