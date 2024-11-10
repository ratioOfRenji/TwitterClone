using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DeleteBlog : MonoBehaviour
{
    [Inject] private BlogClient _blogClient;
    [Inject] private BlogModel _blogModel;
    public async void DeleteBlogm()
    {
       bool success = await _blogClient.DeleteBlogAsync(_blogModel.BlogInfo.BlogId);
        if (success)
        {
            Destroy(this.gameObject);
        }
    }
}
