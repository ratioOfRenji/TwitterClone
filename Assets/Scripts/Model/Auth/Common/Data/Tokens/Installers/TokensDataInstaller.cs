using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TokensDataInstaller : MonoInstaller
{

	public override void InstallBindings()
	{
		Container.BindInterfacesAndSelfTo<TokensStorage>().AsSingle().NonLazy();
		Container.BindInterfacesAndSelfTo<UserDataStorage>().AsSingle().NonLazy();
	}
}
