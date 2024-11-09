using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BlogView : MonoBehaviour
{
    [SerializeField] private Image _profilePic;
    [SerializeField] private TMP_Text _blogAuthor;
    [SerializeField] private TMP_Text _BlogText;
    [SerializeField] private TMP_Text _blogDate;

    public void SetupBlogText(Blog blog)
    {
        _blogAuthor.text = blog.BlogAuthor;
        _BlogText.text = blog.BlogDescription;
        _blogDate.text = blog.CreatedAt.ToLongDateString();
    }
}
