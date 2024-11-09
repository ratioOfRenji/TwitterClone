using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChangeDisplayedNameInstaller : MonoInstaller
{
	[SerializeField] private ChangeDisplayedNameView _view;
	public override void InstallBindings()
	{
		Container.Bind<ChangeDisplayedNameModel>().AsSingle();
		Container.BindInstance(_view).AsSingle();
		Container.BindInterfacesAndSelfTo<ChangeDisplayedNamePresenter>().AsSingle().NonLazy();
	}
}
