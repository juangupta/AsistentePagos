using AsistentePagos.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AsistentePagos.Core.Service
{
    public class ApiService
    {
        HttpClient client;

        public ApiService()
        {
            client = new HttpClient();
        }

        public async Task<Response> Get<T>(string urlBase, string servicePrefix, string controller,
            string user, string password)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Base64Encode(user + ":" + password));
                client.DefaultRequestHeaders.Add("x-ibm-client-id", "9de78f88-553c-461b-bfd3-d69d498d3890");
                client.DefaultRequestHeaders.Add("x-ibm-client-secret", "I7kP2yP6pA6oC6hL8oR3oR3fT5aR8rT2mN0rA7vJ7jJ5uY7pS2");

                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Post<T>(string urlBase, string servicePrefix, string controller, T model,
           string user, string password)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Base64Encode(user + ":" + password));
                client.DefaultRequestHeaders.Add("x-ibm-client-id", "9de78f88-553c-461b-bfd3-d69d498d3890");
                client.DefaultRequestHeaders.Add("x-ibm-client-secret", "I7kP2yP6pA6oC6hL8oR3oR3fT5aR8rT2mN0rA7vJ7jJ5uY7pS2");
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var postResponse = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = postResponse,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
