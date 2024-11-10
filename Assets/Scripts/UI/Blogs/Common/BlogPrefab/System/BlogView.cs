using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BlogView : MonoBehaviour
{
    [SerializeField] private Image _profilePic;
    [SerializeField] private TMP_Text _blogAuthor;
    [SerializeField] private TMP_Text _BlogText;
    [SerializeField] private TMP_Text _blogDate;
	[SerializeField] private AvatarsStorage _avatarsStorage;
    [SerializeField] private GameObject _deleteBlogButton;
    private Subject<Blog> _blogSubject = new Subject<Blog>();
    public IObservable<Blog> OnBlogCreatedAsObservable()
    {
        return _blogSubject.AsObservable(); 
    }

    public void AssignBlogInfo(Blog blog)
    {
        _blogSubject.OnNext(blog);
    }
    public void SetupBlogText(Blog blog)
    {
        _BlogText.text = blog.BlogDescription;
        _blogDate.text = blog.CreatedAt.ToLongDateString();
    }

    public void AssignAuthorInfo(EIconType avatarType, string authorName)
    {
        _profilePic.sprite = _avatarsStorage.iconsDictionary[avatarType];
        _blogAuthor.text = authorName;
    }

    public void ShowDeleteBlogButton(bool active)
    {
        _deleteBlogButton.SetActive(active);
    }
	public class Factory : PlaceholderFactory<BlogView>
	{
	}
}
