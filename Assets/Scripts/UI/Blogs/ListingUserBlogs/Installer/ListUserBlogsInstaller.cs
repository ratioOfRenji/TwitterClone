using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class ListUserBlogsInstaller : MonoInstaller
{
	[SerializeField] private BlogView _blogPrefab;
	[SerializeField] private ListingUserBlogsView _view;
	public override void InstallBindings()
	{
		Container.BindFactory<BlogView, BlogView.Factory>()
			.FromComponentInNewPrefab(_blogPrefab)
			.AsSingle();
		Container.Bind<ListingUserBlogsModel>().AsSingle();
		Container.BindInstance(_view).AsSingle();
		Container.BindInterfacesAndSelfTo<ListingUserBlogsPresenter>().AsSingle().NonLazy();
	}
}
