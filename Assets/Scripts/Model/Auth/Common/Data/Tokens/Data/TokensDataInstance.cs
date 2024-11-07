using Newtonsoft.Json;
using System;

[Serializable]
public class TokensDataInstance
{
	[JsonProperty("accessToken")]
	public string AccessToken {  get; private set; }
	[JsonProperty("refreshToken")]
	public string RefreshToken { get; private set; }

	public TokensDataInstance(string accessToken, string refreshToken)
	{
		AccessToken = accessToken;
		RefreshToken = refreshToken;
	}
}
