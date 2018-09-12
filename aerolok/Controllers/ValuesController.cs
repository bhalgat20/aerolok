using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace aerolok.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{path}")]
        public string Get(string path)
        {
            return RestAPICall(path).Result;
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]string baseData)
        {
            var data = baseData;
            return data != null ? data : "Empty Data";
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private async Task<string> RestAPICall(string path)
        {
            var filepath = "input/mycontainer/" + path;
            var scoreRequest = new
            {
                GlobalParameters = new Dictionary<string, string>() {
        { "Path to container, directory or blob",  filepath},
}
            };

            // Replace these values with your API key and URI found on https://services.azureml.net/
            const string apiKey = "/pdhxZ1fqVcHtdARF+2c8hfvkHhKbamPR+GLHFk+zcwUd/h7ehmu0QC6UGQh0iWdarGfV5RJxJLCJx8om67Vog==";
            const string apiUri = "https://ussouthcentral.services.azureml.net/workspaces/f357977d76974a8f87368f838ae1e4c0/services/4c1ab5560cf2432397e4fa68e78776f7/execute?api-version=2.0&format=swagger";


            //string baseAddress = "https://ussouthcentral.services.azureml.net/workspaces/9a384544677346ebbe420da42384cb80/services/aa9382242ca64033b406f3f09288b1af/execute?api-version=2.0&format=swagger";

            // Start OWIN host 

            // Create HttpCient and make a request to api/values 
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            client.BaseAddress = new Uri(apiUri);
            HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Result: {0}", result);
                return result;
            }
            else
            {
                Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                // Print the headers - they include the requert ID and the timestamp,
                // which are useful for debugging the failure
                Console.WriteLine(response.Headers.ToString());

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                return string.Format("The request failed with status code: {0}", response.StatusCode);
            }
        }
    }
}
