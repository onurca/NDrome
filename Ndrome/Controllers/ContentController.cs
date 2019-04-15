using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Ndrome.Model.Business;
using Ndrome.Service.Interfaces;

namespace Ndrome.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;

        public ContentController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var contents = _contentService.GetAll();
            return Ok(contents);
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            _contentService.Delete(id);
            return Ok();
        }
    }
}
