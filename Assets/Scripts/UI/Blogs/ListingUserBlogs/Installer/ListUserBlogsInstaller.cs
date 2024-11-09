using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class ListUserBlogsInstaller : MonoInstaller
{
	[SerializeField] private GameObject BlogPrefab;
	[SerializeField] private ListingUserBlogsView _view;
	public override void InstallBindings()
	{
		Container.Bind<BlogView>().FromComponentInNewPrefab(BlogPrefab).AsTransient();
		Container.Bind<ListingUserBlogsModel>().AsSingle();
		Container.BindInstance(_view).AsSingle();
		Container.BindInterfacesAndSelfTo<ListingUserBlogsPresenter>().AsSingle().NonLazy();
	}
}
