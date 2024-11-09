using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="ScriptableObjects/AvatarStorage")]
public class AvatarsStorage : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites;

    public Dictionary<EIconType, Sprite> iconsDictionary => FillDictionary();

	private Dictionary<EIconType, Sprite> FillDictionary()
	{
		var dictionary = new Dictionary<EIconType, Sprite>();
		var enumValues = (EIconType[])System.Enum.GetValues(typeof(EIconType));

		if (sprites.Count != enumValues.Length)
		{
			Debug.LogError("Sprites list count does not match the number of EIconType values!");
			return dictionary;
		}

		for (int i = 0; i < enumValues.Length; i++)
		{
			dictionary[enumValues[i]] = sprites[i];
		}

		return dictionary;
	}

}
