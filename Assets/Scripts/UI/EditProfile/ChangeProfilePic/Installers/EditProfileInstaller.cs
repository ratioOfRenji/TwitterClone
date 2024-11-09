using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EditProfileInstaller : MonoInstaller
{
	[SerializeField] private ChooseAvatarView _view;
	public override void InstallBindings()
	{
		Container.Bind<ChooseAvatarModel>().AsSingle();
		Container.BindInstance(_view).AsSingle();
		Container.BindInterfacesAndSelfTo<ChooseAvatarPresenter>().AsSingle().NonLazy();
	}
}
