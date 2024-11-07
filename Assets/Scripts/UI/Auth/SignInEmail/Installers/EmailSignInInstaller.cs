using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EmailSignInInstaller : MonoInstaller
{
	[SerializeField] private SignInEmailView _signInEmailView;
	public override void InstallBindings()
	{
		Container.BindInstance(_signInEmailView);
		Container.BindInterfacesAndSelfTo<SignInEmailPresenter>().AsSingle().NonLazy();
	}
}
