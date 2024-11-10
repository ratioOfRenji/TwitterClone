using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ListingFeedPostsView : MonoBehaviour
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
			Debug.Log("ReachedBottom");
		}
		else if (scrollRect.verticalNormalizedPosition > bottomThreshold)
		{
			isScrollingToBottom = false;
		}
	}
}
