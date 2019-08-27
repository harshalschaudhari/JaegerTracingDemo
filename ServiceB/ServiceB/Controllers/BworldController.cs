using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ServiceB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BworldController : ControllerBase
    {
        //https://localhost:44310/api/bworld
        // GET api/bworld
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Hello", "ServiceB - Bworld" };
        }

        // GET api/bworld/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<string>> Get(int id)
        {
            id *= 10;
            return new string[] { "B_value", id.ToString() };
        }

    }
}
