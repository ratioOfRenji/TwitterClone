using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
public class BlogPresenter : IInitializable, IDisposable
{
	private readonly BlogModel _model;
	private readonly BlogView _view;

	public BlogPresenter(BlogModel model, BlogView view)
	{
		_model = model;
		_view = view;
	}

	public void Initialize()
	{
		_view.OnBlogCreatedAsObservable().Subscribe(blog => OnBlogCreated(blog));
	}

	private async void OnBlogCreated(Blog blog)
	{
		_model.AssignBlog(blog);
		bool success = await _model.GetAuthorInfo();
		if (success)
		{
			_view.AssignAuthorInfo(_model.AuthorsAvatar, _model.AuthorName);
		}
		else
		{
			_view.AssignAuthorInfo(EIconType.Icon3, "John Doe");
		}


	}
	public void Dispose()
	{

	}
}
