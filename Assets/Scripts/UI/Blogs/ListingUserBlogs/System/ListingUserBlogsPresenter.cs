using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
public class ListingUserBlogsPresenter : IInitializable, IDisposable
{
	private readonly ListingUserBlogsView _view;
	private readonly ListingUserBlogsModel _model;
	private readonly CreateNewPostPresenter _createNewPostPresenter;
	private readonly StartUp _startUp;
	private readonly TokensStorage _tokensStorage;
	private readonly UserInfoClient _userInfoClient;
	private CompositeDisposable _disposables = new CompositeDisposable();

	private bool loadingNewBatch = false;

	

	public ListingUserBlogsPresenter(ListingUserBlogsView view, 
		ListingUserBlogsModel model, 
		CreateNewPostPresenter createNewPostPresenter, 
		StartUp startUp, 
		TokensStorage tokensStorage, 
		UserInfoClient userInfoClient)
	{
		_view = view;
		_model = model;
		_createNewPostPresenter = createNewPostPresenter;
		_startUp = startUp;
		_tokensStorage = tokensStorage;
		_userInfoClient = userInfoClient;
	}
	public void Initialize()
	{
		_userInfoClient.OnUserDataChangedAsObservable().Subscribe(success => { ReloadPosts(); });
		_tokensStorage.OnTokensSetAsObservable().Subscribe(_ => _model.LoadAndSpawnUserBlogs(_view.BlogsParent, 1, 5));
		_startUp.OnTokensLoadedAsObservable().Subscribe(success => { if (success) _model.LoadAndSpawnUserBlogs(_view.BlogsParent,1,5); }).AddTo(_disposables);
		_createNewPostPresenter.OnBlogPostedAsObservable().Subscribe(_ => { ReloadPosts(); }).AddTo(_disposables);
		_view.OnBottomReachedAsObservable().Subscribe(_ => { if(!loadingNewBatch&& _model._cachedUserPagination.HasMore) LoadNewBatch(); }).AddTo(_disposables);
	}

	private async void ReloadPosts()
	{
		_model.ReloadPosts(_view.BlogsParent);
	}
	private async void LoadNewBatch()
	{
		loadingNewBatch = true;
		await _model.LoadAndSpawnNextBatch(_view.BlogsParent);
		loadingNewBatch = false;
	}
	public void Dispose()
	{
		_disposables.Dispose();
	}
}
