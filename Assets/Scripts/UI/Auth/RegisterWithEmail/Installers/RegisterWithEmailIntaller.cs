using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RegisterWithEmailIntaller : MonoInstaller
{
	[SerializeField] private RegisterWithEmailView _registerWithEmailView;
	public override void InstallBindings()
	{
		Container.BindInstance(_registerWithEmailView);
		Container.BindInterfacesAndSelfTo<RegisterWithEmailPresenter>().AsSingle().NonLazy();
	}
}
