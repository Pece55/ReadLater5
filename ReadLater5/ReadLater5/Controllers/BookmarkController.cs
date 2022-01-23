using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadLater5.Models;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReadLater5.Controllers
{
    [Authorize]
    public class BookmarkController : Controller
    {
        private readonly IBookmarkService _bookmarkService;
        private readonly ICategoryService _categoryService;

        public BookmarkController(IBookmarkService bookmarkService, ICategoryService categoryService)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int id)
        {
            var entities = await _bookmarkService.GetBookmarksForCategory(id);
            ViewBag.Page = "Index";
            if (entities.Any())
            {
                return View(entities.Select(x => DTO.AutoMapper.MapProfile.DTOBookmarkForModel(x)).ToList());
            }
            return View(new List<BookmarkModel>());
        }

        public async Task<IActionResult> MyBookmarks()
        {
            ViewBag.Page = "MyBookmarks";
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Bookmark> entities = await _bookmarkService.GetBookmarkByUserId(userId);
            return View(entities.Select(x => DTO.AutoMapper.MapProfile.DTOBookmarkForModel(x)).ToList());
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _bookmarkService.GetBookmarkById(id);
            return View(DTO.AutoMapper.MapProfile.DTOBookmarkForModel(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookmarkModel bookmark)
        {
            if (ModelState.IsValid)
            {
                var entity = await _bookmarkService.GetBookmarkById(bookmark.ID);
                if (entity != null)
                {
                    await _bookmarkService.UpdateBookmark(bookmark.URL, bookmark.ShortDescription, entity);
                }
                return RedirectToAction("Index", new { id = entity.CategoryId });
            }
            return View(bookmark);
        }


        public  IActionResult Create()
        {
            var categories = _categoryService.GetCategories();
            var model = new BookmarkModel
            {
                Categories = categories.Select(x => DTO.AutoMapper.MapProfile.DTOCategoryToModel(x)).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public  IActionResult Create(BookmarkModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                if (model.CategoryId == 0 && !string.IsNullOrEmpty(model.NewCategoryName))
                {
                    _categoryService.CreateCategory(new Category { Name = model.NewCategoryName, UserId = userId, Bookmarks = new List<Bookmark> { new Bookmark { URL = model.URL, ShortDescription = model.ShortDescription, UserId = userId, CreateDate = DateTime.Now } } });
                    return RedirectToAction("Index", "Categories");
                }
                else
                {
                    model.UserId = userId;
                     _bookmarkService.CreateBookmark(DTO.AutoMapper.MapProfile.ModelToBookmark(model));
                    return RedirectToAction("Index", "Categories");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { ErrorMessage = ex.Message });
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _bookmarkService.GetBookmarkById(id);
            if (entity != null)
            {
                await _bookmarkService.DeleteBookmark(entity);
            }
            return RedirectToAction("Index", new { id = entity.CategoryId });
        }
    }
}
