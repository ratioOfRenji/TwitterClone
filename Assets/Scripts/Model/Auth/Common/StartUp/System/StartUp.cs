using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System;

public class StartUp : IInitializable
{
	private TokensStorage _tokensStorage;
	private Subject<bool> onTokensLoadedSubject = new Subject<bool>();
	public IObservable<bool> OnTokensLoadedAsObservable()
	{
	return onTokensLoadedSubject.AsObservable();
	}
	public StartUp(TokensStorage tokensStorage)
	{
		_tokensStorage = tokensStorage;
	}
	public void Initialize()
	{
		TokensRefresh();
	}

	public async UniTask<bool> TokensRefresh()
	{
		await UniTask.Delay(1000);
		bool signedIn= await _tokensStorage.LoadData();
		onTokensLoadedSubject.OnNext(signedIn);
		return signedIn;
	}
}
