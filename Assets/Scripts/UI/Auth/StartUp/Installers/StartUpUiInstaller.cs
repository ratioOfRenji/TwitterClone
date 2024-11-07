using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class StartUpUiInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Container.BindInterfacesAndSelfTo<StartupUi>().AsSingle().NonLazy();
	}
}
