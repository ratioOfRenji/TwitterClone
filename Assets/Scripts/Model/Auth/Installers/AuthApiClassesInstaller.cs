using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AuthApiClassesInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Container.Bind<SignInWithEmail>().AsSingle();
		Container.Bind<RegisterWithEmail>().AsSingle();
	}
}
