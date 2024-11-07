using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ConfirmEmailInstaller : MonoInstaller
{
	[SerializeField] private ConfirmEmailView _confirmEmailView;
	public override void InstallBindings()
	{
		Container.BindInstance(_confirmEmailView);
		Container.BindInterfacesAndSelfTo<ConfirmEmailPresenter>().AsSingle().NonLazy();
	}
}
