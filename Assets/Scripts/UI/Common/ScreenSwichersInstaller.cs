using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class ScreenSwichersInstaller : MonoInstaller
{
	[SerializeField] private AuthScreenSwicher _authScreenSwicher;
	[SerializeField] private BlogsScreensSwicher _blogsScreenSwicher;
	public override void InstallBindings()
	{
		Container.BindInstance(_authScreenSwicher).AsSingle();
		Container.BindInstance(_blogsScreenSwicher).AsSingle();
	}
}
