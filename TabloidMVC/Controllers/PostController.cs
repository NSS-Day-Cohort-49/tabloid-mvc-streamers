using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, IUserProfileRepository userProfileRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _userProfileRepository = userProfileRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult MyPost()
        {
            int userProfileId = GetCurrentUserProfileId();
            var posts = _postRepository.GetUserPostsById(userProfileId);
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            List<Tag> tags = _tagRepository.GetAllTags();
            var postId = _postRepository.GetTagsByPostId(post.Id);
            var vm = new PostTagViewModel()
            {
                Post = post,
                Tags = tags,
                PostId = postId
                
            };

            return View(vm);
        }

        public IActionResult TagManagement()
        {

            List<Tag> tags = _tagRepository.GetAllTags();

            return View(tags);

        }


        //Would be used to save the tag to a post.
        //Did not finish all the methods to save a post to a tag
/*        public IActionResult AddPostTag()
        {
            List<Tag> tags = _tagRepository.GetAllTags();

        }*/

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        // GET: Post/Edit
        public IActionResult Edit(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            
            if (post == null)
            {
                return NotFound();
            }

            return View(post);

        }

        // POST: Post/Edit
        [HttpPost]
        public IActionResult Edit(int id, Post post)
        {
            try
            {
                _postRepository.UpdatePost(post);
                return RedirectToAction("Details", new { id = post.Id });
            }
            catch
            {
                return View(post);
            }
        }

        public IActionResult InsertTag(int post, int tag)
        {
            _postRepository.InsertTag(post, tag);

            return RedirectToAction("Details", new { id = post });
        }
        // GET: Post/Delete
        [Authorize]
        public ActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);
            return View(post);
        }

        // POST: Post/Delete/
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                post = _postRepository.GetPublishedPostById(id);
                _postRepository.DeletePost(id);
                return RedirectToAction("MyPost");
            }
            catch 
            {
                return View("Details", new {id = post.Id});
            }
        }
    }
}
