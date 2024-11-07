using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StartUpInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Container.BindInterfacesAndSelfTo<StartUp>().AsSingle().NonLazy();
	}
}
