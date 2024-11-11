using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SettingsInstaller : MonoInstaller
{
	[SerializeField] private SettingsView _settingsView;
	[SerializeField] private GoogleAuthentication _googleAuthentication;
	public override void InstallBindings()
	{
		Container.BindInstance(_settingsView);
		Container.BindInstance(_googleAuthentication).AsSingle();
		Container.Bind<SettingsModel>().AsSingle();
		Container.BindInterfacesAndSelfTo<SettingsPresenter>().AsSingle().NonLazy();
	}
}
