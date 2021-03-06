using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        List<Post> GetAllPublishedPosts();
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        List<Post> GetUserPostsById(int userProfileId);
        void UpdatePost(Post post);
        void DeletePost(int id);
        void InsertTag(int post, int tag);

        List<Tag> GetTagsByPostId(int postId);
    }
}