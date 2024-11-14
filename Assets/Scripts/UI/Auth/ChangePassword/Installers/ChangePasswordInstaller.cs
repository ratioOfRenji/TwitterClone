using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChangePasswordInstaller : MonoInstaller
{
	[SerializeField] private ChangePasswordView _view;
	public override void InstallBindings()
	{
		Container.BindInstance(_view).AsSingle();
		Container.BindInterfacesAndSelfTo<ChangePasswordPresenter>().AsSingle().NonLazy();
	}
}
