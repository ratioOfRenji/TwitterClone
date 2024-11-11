using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using UniRx;

public class LoadDisplayedName : MonoBehaviour
{
    [SerializeField] private TMP_Text displayedNameText;
	[Inject] private readonly UserInfoClient userInfoClient;
    [Inject] private readonly ChangeDisplayedNamePresenter changeDisplayedNamePresenter;
    [Inject] private readonly StartUp _startUp;
    [Inject] private readonly TokensStorage _tokensStorage;
    private CompositeDisposable _disposables = new CompositeDisposable();
	void Awake()
    {
        changeDisplayedNamePresenter.OnNameChangedAsObservable().Subscribe(name => LoadName()).AddTo(_disposables);
        _startUp.OnTokensLoadedAsObservable().Subscribe(success => { if (success) { LoadName(); } }).AddTo(_disposables);   
        _tokensStorage.OnTokensSetAsObservable().Subscribe(_ => LoadName()).AddTo(_disposables);

	}

    private async UniTask LoadName()
    {
        UserProfile profile = await userInfoClient.GetUserProfileAsync();
        displayedNameText.text = profile.DisplayedName;
        Debug.Log("loaded Name Succesfuly");

	}
	private void OnDestroy()
	{
		_disposables.Dispose();
	}
}
