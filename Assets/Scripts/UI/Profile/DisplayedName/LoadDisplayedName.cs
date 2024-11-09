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
	[Inject] private UserInfoClient userInfoClient;
    [Inject] private ChangeDisplayedNamePresenter changeDisplayedNamePresenter;

    private CompositeDisposable _disposables = new CompositeDisposable();
	void Start()
    {
        changeDisplayedNamePresenter.OnNameChangedAsObservable().Subscribe(name => LoadName()).AddTo(_disposables);
        LoadName();


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
