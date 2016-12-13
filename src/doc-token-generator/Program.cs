using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace doc_token_generator
{
    public class Program
    {
        //public static void Main(string[] args) => GetClientToken().GetAwaiter().GetResult();
        public static void Main(string[] args) => GetResultWithAccessToken().GetAwaiter().GetResult();

        private async static Task GetResultWithAccessToken()
        {
            var accessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImQyMzc1NTRmOWI5MDE3ZTZhODU1YzIzNjkxYzJhMjdhNzhjZDQ5ZWU0OTkyZGNiYmVjZGIxODc2ZDI4ZjBiMGYiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE0ODE1MjE1NTcsImV4cCI6MTQ4MTUyNTE1NywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC9yZXNvdXJjZXMiLCJkb2Mtc3RhY2stYXBwLWFwaSJdLCJjbGllbnRfaWQiOiJtdmMiLCJzdWIiOiIyIiwiYXV0aF90aW1lIjoxNDgxNTIxNTEyLCJpZHAiOiJsb2NhbCIsInNjb3BlIjpbIm9wZW5pZCIsInByb2ZpbGUiLCJkb2Mtc3RhY2stYXBwLWFwaSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJwd2QiXX0.AhmIyAroGlinYscCuyUuKOOtnO5TQwyQQbv1vKYJrW3iYg-jNn6oiF6qBYgTodMyEFUrTNyNsNfPqDpPxRE-oTb41ABKHhOWkszlBjL42vu9vzTOT9pf45ylee5PDL7fqnqa8bIjqwIlXJn8FwNODaCUJe10P6BKk_COLwC5QMK2hG5ddBuqPTkySdiuT2jbdR4b_ff2Ba3jHjDKEs1FwybL2Mu8_B370ei_qBl_PSc0bM_pgwlElJqIumN1IRk-HuhW-GkRDU7mMxulH1mf-UpJSUwEqzCWf3Gh7GH_TN2l6T1_lYQdB29bFJO1JaO1HLBooni3Pqdq8q7aX_Um6A";

            //var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var content = await client.GetStringAsync("http://localhost:5001/identity");

            var result = JArray.Parse(content).ToString();

            Console.WriteLine(result);
        }

        private async static Task GetResourceOwnerToken()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("bob", "password", "doc-stack-app-api");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");


            await CallApiWithToken(tokenResponse);
        }

        private async static Task GetClientToken()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("doc-stack-app-api");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            await CallApiWithToken(tokenResponse);
        }

        private static async Task CallApiWithToken(TokenResponse tokenResponse)
        {
            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
