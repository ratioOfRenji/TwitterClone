using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ResetPasswordInstaller : MonoInstaller
{
	[SerializeField] private ResetPasswordView _view;
	public override void InstallBindings()
	{
		Container.BindInstance(_view).AsSingle();
		Container.BindInterfacesAndSelfTo<ResetPasswordPresenter>().AsSingle().NonLazy();
	}
}
