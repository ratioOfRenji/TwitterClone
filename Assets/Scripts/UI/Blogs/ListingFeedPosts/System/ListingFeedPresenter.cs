using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class ListingFeedPresenter : IInitializable, IDisposable
{
	private readonly ListingFeedPostsView _view;
	private readonly ListingFeedModel _model;
	private readonly CreateNewPostPresenter _createNewPostPresenter;
	private readonly StartUp _startUp;
	private CompositeDisposable _disposables = new CompositeDisposable();

	private bool loadingNewBatch = false;
	public ListingFeedPresenter(ListingFeedPostsView view, ListingFeedModel model, CreateNewPostPresenter createNewPostPresenter, StartUp startUp)
	{
		_view = view;
		_model = model;
		_createNewPostPresenter = createNewPostPresenter;
		_startUp = startUp;
	}
	public void Initialize()
	{
		_startUp.OnTokensLoadedAsObservable().Subscribe(success => { if (success) _model.LoadAndSpawnFeedBlogs(_view.BlogsParent, 1, 5); }).AddTo(_disposables);
		_createNewPostPresenter.OnBlogPostedAsObservable().Subscribe(_ => { ReloadPosts(); }).AddTo(_disposables);
		_view.OnBottomReachedAsObservable().Subscribe(_ => { if (!loadingNewBatch && _model._cachedUserPagination.HasMore) LoadNewBatch(); }).AddTo(_disposables);
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
