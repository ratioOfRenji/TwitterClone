using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ListingFeedInstaller : MonoInstaller
{
	[SerializeField] private ListingFeedPostsView _view;
	public override void InstallBindings()
	{
		Container.Bind<ListingFeedModel>().AsSingle();
		Container.BindInstance(_view);
		Container.BindInterfacesAndSelfTo<ListingFeedPresenter>().AsSingle().NonLazy();
	}
}
