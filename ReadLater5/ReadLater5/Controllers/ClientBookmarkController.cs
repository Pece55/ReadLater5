using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadLater5.Auth;
using ReadLater5.CustomExceptions;
using ReadLater5.Models;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReadLater5.Controllers
{
    [Route("api/v1/")]
    public class ClientBookmarkController : BaseController
    {

        private readonly IBookmarkService _bookmarkService;

        public ClientBookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpPost]
        [Authorize]
        [Route("create-bookmark")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status201Created)]
        public  IActionResult CreateBookmark([FromBody] BookmarkModel bookmarkData)
        {
            bookmarkData.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState.IsValid)
            {
               _bookmarkService.CreateBookmark(DTO.AutoMapper.MapProfile.ModelToBookmark(bookmarkData));
            }
            return Ok(bookmarkData);
        }

        [HttpGet]
        [Route("all-bookmark")]
        [BasicAuthorizeAttribute(Entity.AuthEntity.Permission.Read)]
        public async Task<List<ClientBookmarkModel>> GetAllBookmarks()
        {
            var entities = await _bookmarkService.GetAllBookmarks();
            if (entities.Any())
            {
                var model = entities.Select(x => DTO.AutoMapper.MapProfile.BookmarkToCLModel(x)).ToList();
                return model;
            }
            return new List<ClientBookmarkModel>();
        }


        [HttpGet]
        [Route("bookmark-by-id")]
        [BasicAuthorizeAttribute(Entity.AuthEntity.Permission.Read)]
        public async Task<ClientBookmarkModel> GetBookmarkByID(int id)
        {
            var entity = await _bookmarkService.GetBookmarkById(id);
            if (entity != null)
            {
                var model = DTO.AutoMapper.MapProfile.BookmarkToCLModel(entity);
                return model;
            }
            return new ClientBookmarkModel();
        }

        [HttpPost]
        [BasicAuthorizeAttribute(Entity.AuthEntity.Permission.ReadWrite)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] BookmarkModel model)
        {
            if (model != null)
            {
                try
                {
                     _bookmarkService.CreateBookmark(DTO.AutoMapper.MapProfile.ModelToBookmark(model));
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
                    return Ok(new { Message = "Bookmark was successfully created" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ErrorMessage = ex.Message });
                }
            }
            return BadRequest(model);
        }

        [HttpPut]
        [Route("update-bookmark-by-id")]
        [BasicAuthorizeAttribute(Entity.AuthEntity.Permission.ReadWrite)]
        public async Task<IActionResult> Put(int id, [FromBody] BookmarkModel bookmarkData)
        {
            var entity = await _bookmarkService.GetBookmarkById(id);
            if (entity != null)
            {
                try
                {
                    await _bookmarkService.UpdateBookmark(bookmarkData.URL, bookmarkData.ShortDescription, entity);
                    return Ok(new { Message = "Bookmark was successfully updated" });
                }
                catch (Exception)
                {
                    return BadRequest(new { ErrorMessage = "Something went wrong" });
                }
            }
            return BadRequest(new { ErrorMessage = "Bookmark not found" });
        }

        [HttpDelete]
        [Route("delete-bookmark-by-id")]
        [BasicAuthorizeAttribute(Entity.AuthEntity.Permission.ReadWrite)]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _bookmarkService.GetBookmarkById(id);
            if (entity != null)
            {
                try
                {
                    await _bookmarkService.DeleteBookmark(entity);
                    return Ok(new { Message = "Bookmark was successfully deleted" });
                }
                catch (Exception)
                {
                    return BadRequest(new { ErrorMessage = "Something went wrong" });
                }
            }
            return BadRequest(new { ErrorMessage = "Bookmark not found" });
        }
    }
}
