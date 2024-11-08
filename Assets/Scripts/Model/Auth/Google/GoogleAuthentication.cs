using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Google;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Zenject;
using UniRx;

public class GoogleAuthentication : MonoBehaviour
{
	private GoogleSignInConfiguration configuration;
	public string webClientId = "499839630768-g1jj981p26cpi1fcaugigrmriv264t0o.apps.googleusercontent.com";
	private string apiBaseUrl = "https://webapiwithconfirm-production.up.railway.app/api/Account"; // Replace with your API base URL
	[Inject] private TokensStorage _tokenStorage;
	
	private Subject<bool> onAuthObservable = new Subject<bool>();
	public IObservable<bool> OnAuthObservable()
	{
		return onAuthObservable.AsObservable();
	}
	void Awake()
	{
		configuration = new GoogleSignInConfiguration
		{
			WebClientId = webClientId,
			RequestIdToken = true,
			UseGameSignIn = false,
			RequestEmail = true
		};
	}
	public void OnSignInButton()
	{
		OnSignIn().Forget();
	}
	private async UniTask OnSignIn()
	{
		GoogleSignIn.Configuration = configuration;
		try
		{
			GoogleSignInUser user = await GoogleSignIn.DefaultInstance.SignIn().AsUniTask();
			if (user != null)
			{
				var tokensData = await SendLoginRequest(user.IdToken);
				_tokenStorage.UpdateData(tokensData);
				onAuthObservable.OnNext(true);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Error during sign-in: " + ex.Message);
		}
	}
	public void OnRegisterButton()
	{
		OnRegister().Forget();
	}
	private async UniTask OnRegister()
	{
		GoogleSignIn.Configuration = configuration;
		try
		{
			GoogleSignInUser user = await GoogleSignIn.DefaultInstance.SignIn().AsUniTask();
			if (user != null)
			{
				var tokensData = await SendRegisterRequest(user.IdToken);
				_tokenStorage.UpdateData(tokensData);
				onAuthObservable.OnNext(true);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Error during registration: " + ex.Message);
		}
	}

	private async UniTask<TokensDataInstance> SendLoginRequest(string idToken)
	{
		var url = $"{apiBaseUrl}/signin/google";
		return await SendAuthRequest(url, idToken);
	}

	private async UniTask<TokensDataInstance> SendRegisterRequest(string idToken)
	{
		var url = $"{apiBaseUrl}/register/google";
		return await SendAuthRequest(url, idToken);
	}

	private async UniTask<TokensDataInstance> SendAuthRequest(string url, string idToken)
	{
		using var request = new UnityWebRequest(url, "POST");
		byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(idToken);
		request.uploadHandler = new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "text/plain");

		await request.SendWebRequest().ToUniTask();

		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("Request successful. Response: " + request.downloadHandler.text);
			return JsonConvert.DeserializeObject<TokensDataInstance>(request.downloadHandler.text);
		}
		else
		{
			Debug.LogError("Error with request: " + request.error);
			Debug.LogError("Response: " + request.downloadHandler.text);
			return null;
		}
	}

	public void OnSignOut()
	{
		Debug.Log("Signing out...");
		GoogleSignIn.DefaultInstance.SignOut();
	}
}