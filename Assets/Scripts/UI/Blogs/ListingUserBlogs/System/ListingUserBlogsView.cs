using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class ListingUserBlogsView : MonoBehaviour
{
    [SerializeField] private GameObject blogsParent;
    public GameObject BlogsParent => blogsParent;

	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private float bottomThreshold = -0.05f;

	private bool isScrollingToBottom = false;

	private Subject<Unit> onBottomReachedSubject = new Subject<Unit>();
	public IObservable<Unit> OnBottomReachedAsObservable()
	{
		return onBottomReachedSubject.AsObservable();
	}
	private void Update()
	{
		if (scrollRect.verticalNormalizedPosition <= bottomThreshold && !isScrollingToBottom)
		{
			isScrollingToBottom = true;
			onBottomReachedSubject.OnNext(Unit.Default);
			Debug.Log("ReavhedBottom");
		}
		else if (scrollRect.verticalNormalizedPosition > bottomThreshold)
		{
			isScrollingToBottom = false;
		}
	}

}
