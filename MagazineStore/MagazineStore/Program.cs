using MagazineStore.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Linq;
namespace ConsoleApp1
{
     class Program
    {
        static string url = "http://magazinestore.azurewebsites.net/";
        static HttpClient client;

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()

        {

            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            Console.WriteLine("Welcome to Babu Magazine store (built using .NET Core 2.0)");

            Console.WriteLine("Started at:" + DateTime.Now);

            string token =await getToken();
            Console.WriteLine("Token:" + token);

            IEnumerable<string> categories = await getCategories(token);
            Console.WriteLine("Categories:" + categories.Count());

            List<Magazine> allMagazines = new List<Magazine>();
            foreach (string category in categories)
            {
                allMagazines.AddRange(await getMagazines(token, category));
            }
            Console.WriteLine("Total Magazines:" + allMagazines.Count());

            List<string> subscriberIds = new List<string>();

            IEnumerable<Subsciber> subscribers= await getSubscribers(token);
            Console.WriteLine("Subscribers:" + subscribers.Count());

            foreach (Subsciber subscriber in subscribers)
            {
                int subscriptionCount = 0;

                foreach (string category in categories)
                {
                    var exists = allMagazines.Where(a => a.category == category).Select(b => b.id).Any(x => subscriber.magazineIds.Contains(x));
                    if (!exists)
                    {
                        break;
                    }
                    else {
                        subscriptionCount++;
                    }
                }
                if (subscriptionCount == categories.Count())
                {
                    subscriberIds.Add(subscriber.id);
                }
            }
            Console.WriteLine("Subscriptions found:" + subscriberIds.Count());

            AnswerResponse result= new AnswerResponse();
            if (subscriberIds.Count > 0)
            {
               result= await postAnswer(token, subscriberIds);
            }

      
            Console.WriteLine("Answer Correct:" + result.answerCorrect);
            Console.WriteLine("Answer Shoud be:" + result.shouldBe);
            Console.WriteLine("Answer Total Time:" + result.totalTime);

            Console.WriteLine("Ended at:"+ DateTime.Now);
            Console.WriteLine("Have a great day!");

            Console.ReadKey();
        }

         static async Task<string> getToken(){
            HttpResponseMessage response = await client.GetAsync("api/token");
            string token = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                token = await response.Content.ReadAsStringAsync();
            }
            JObject apiResponse = JObject.Parse(token);
            return (string)apiResponse["token"];

        }

        static async Task<IEnumerable<string>> getCategories(string token)
        {
            HttpResponseMessage response = await client.GetAsync("api/categories/" + token) ;
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            JObject apiResponse = JObject.Parse(result);
            IEnumerable<string> output = apiResponse.GetValue("data").AsJEnumerable().Values<string>();
 
            return output;
        }

        static async Task<IEnumerable<Magazine>> getMagazines(string token, string category)
        {
            HttpResponseMessage response = await client.GetAsync("api/magazines/" + token + "/" + category);
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            JObject apiResponse = JObject.Parse(result);
            IEnumerable<Magazine> output = apiResponse.GetValue("data").ToObject<IEnumerable<Magazine>>();

            return output;
        }
        static async Task<IEnumerable<Subsciber>> getSubscribers(string token)
        {
            HttpResponseMessage response = await client.GetAsync("api/subscribers/" + token );
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            JObject apiResponse = JObject.Parse(result);
            IEnumerable<Subsciber> output = apiResponse.GetValue("data").ToObject<IEnumerable<Subsciber>>();

            return output;
        }

        static async Task<AnswerResponse> postAnswer(string token,List<string> subscribers)
        {
            SubscibersList listToSubmit = new SubscibersList() { subscribers = subscribers };
            HttpResponseMessage response = await client.PostAsJsonAsync("api/answer/" + token, JToken.FromObject(listToSubmit));
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            JObject apiResponse = JObject.Parse(result);
            AnswerResponse output = apiResponse.GetValue("data").ToObject<AnswerResponse>();

            return output;
        }
    }
}

