using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ServiceA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AworldController : ControllerBase
    {
        //https://localhost:44340/api/aworld
        // GET api/aworld
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Hello", "ServiceA - Aworld" };
        }

        // GET api/aworld/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<string>> Get(int id)
        {

            return CallServiceB(id);

        }

        private static string[] CallServiceB(int id)
        {
            string url = string.Format("{0}{1}", "https://localhost:44310/api/bworld/", id);
            string[] result = null;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                string[] b = response.Content.ReadAsAsync<string[]>().Result;
                //Console.WriteLine(b);
                result = b ?? null;
            }
            else
            {
                result = new string[] { response.StatusCode.ToString(), response.ReasonPhrase };
            }
            client.Dispose();
            return result;
        }

    }
}
