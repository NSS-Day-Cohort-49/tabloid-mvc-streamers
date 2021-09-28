using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class TagController : Controller
    {
        // GET: TagController1
        public ActionResult Index()
        {
            List<Tag> tags = _tagRepository.GetAllTags();

            return View(tags);
        }

        // GET: TagController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TagController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepository.Add(tag);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }

        // GET: TagController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TagController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TagController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TagController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private readonly ITagRepository _tagRepository;
        
        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        
    }
}
