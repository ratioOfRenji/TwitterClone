using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class ConstantsInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Container.Bind<Constants>().AsSingle();
	}
}
