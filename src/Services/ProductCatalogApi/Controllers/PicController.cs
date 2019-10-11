using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;


namespace ProductCatalogApi.Controllers
{
    [Route("api/[controller]")]
    public class PicController : Controller
    {
        private readonly HostingEnvironment _env;

        public PicController(HostingEnvironment env)
        {
            _env = env;
        }

        // GET api/pic
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "pic1", "pic2" };
        }
        
        // GET /api/pic/GetImage/5
        [HttpGet("{id}")]
        public IActionResult GetImage(int id)
        {
            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot + "/Pics", "shoes-" + id + ".png");
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, "image/png");
        }
    }
}