using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CreateNewPostInstaller : MonoInstaller
{
	[SerializeField] private CreateNewPostView _view;
	public override void InstallBindings()
	{
		Container.BindInstance(_view).AsSingle();
		Container.Bind<CreateNewPostModel>().AsSingle();
		Container.BindInterfacesAndSelfTo<CreateNewPostPresenter>().AsSingle().NonLazy();
	}
}
