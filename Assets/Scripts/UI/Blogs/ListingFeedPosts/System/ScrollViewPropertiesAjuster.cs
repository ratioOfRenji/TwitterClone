using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewPropertiesAjuster : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField] private SafeArea safeArea;
    void Start()
    {
        layoutGroup.padding.bottom = Mathf.CeilToInt(safeArea.CalculateSafeAreaHeight());
	}

}
