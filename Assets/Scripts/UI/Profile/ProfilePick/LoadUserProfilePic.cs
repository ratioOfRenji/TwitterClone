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
    [Inject] private StartUp _startUp;
    private CompositeDisposable _disposables = new CompositeDisposable();  
	void Awake()
    {
        _chooseAvatarPresenter.OnProfilePicUpdatedAsObservable().Subscribe(_ => loadIcon()).AddTo(_disposables);
        _startUp.OnTokensLoadedAsObservable().Subscribe(success => { if (success) loadIcon(); }).AddTo(_disposables);
	}

    private async UniTask loadIcon()
    {
			UserProfile userProfile = await userInfoClient.GetUserProfileAsync();
            icon.sprite = avatarsStorage.iconsDictionary[userProfile.Icon];
        Debug.Log("Loaded icon succesfully");
	}
	private void OnDestroy()
	{
		_disposables.Dispose();
	}
}
