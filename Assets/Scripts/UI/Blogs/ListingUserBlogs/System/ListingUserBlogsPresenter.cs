using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ListingUserBlogsPresenter : IInitializable, IDisposable
{
	private readonly ListingUserBlogsView _view;
	private readonly ListingUserBlogsModel _model;
	public ListingUserBlogsPresenter(ListingUserBlogsView view, ListingUserBlogsModel model)
	{
		_view = view;
		_model = model;
	}
	public void Initialize()
	{
		_model.LoadAndSpawnUserBlogs(_view.BlogsParent);
	}

	public void Dispose()
	{
	}
}
