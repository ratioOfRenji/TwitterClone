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
		Container.Bind<ConfirmEmail>().AsSingle();
		Container.Bind<TokenRefresh>().AsSingle();
		Container.Bind<UserInfoClient>().AsSingle();
		Container.Bind<BlogClient>().AsSingle();	
		Container.Bind<DeleteUser>().AsSingle();
		Container.Bind<ResetPassword>().AsSingle();
		Container.Bind<ChangePassword>().AsSingle();
	}
}
