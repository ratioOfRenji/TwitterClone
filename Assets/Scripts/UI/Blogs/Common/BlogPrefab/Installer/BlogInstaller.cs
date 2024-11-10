using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BlogInstaller : MonoInstaller
{
	[SerializeField]
	private BlogView _view;

	public override void InstallBindings()
	{
		Container.BindInstance(_view);
		Container.Bind<BlogModel>().AsSingle();
		Container.BindInterfacesAndSelfTo<BlogPresenter>().AsSingle().NonLazy();
	}
}
