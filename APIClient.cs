using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MembershipCharge.SeedWork.API
{
    public class APIClient
    {
        public ComandHttp Comand { get; set; }
        public string Url { get; set; }

        public string Data { get; set; }

        public int TimeOut { get; set; }

        public Token Token { get; set; }

        public APIClient(string url, int timeOut, ComandHttp comand, Token token)
        {
            this.Url = url;
            this.TimeOut = timeOut;
            this.Token = token;
            this.Comand = comand;
        }

        public async Task<HttpResponseMessage> ExecuteCall()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();

            using (HttpClient client = new HttpClient())
            {

                client.Timeout = new TimeSpan(0, 0, (TimeOut > 0 ? TimeOut : 60));

                if (Token != null && !string.IsNullOrEmpty(Token.Value))
                {
                    if ("BrasilIoAPI".Equals(Token.Name))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", Token.Value);
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Token.Value);
                    }
                }

                HttpContent httpContent;

                switch (Comand)
                {
                    case ComandHttp.Get:
                        responseMessage = await client.GetAsync(Url);
                        break;

                    case ComandHttp.Post:
                        httpContent = new StringContent(Data, Encoding.UTF8, "application/json");
                        responseMessage = await client.PostAsync(Url, httpContent);
                        break;

                    case ComandHttp.Put:
                        httpContent = new StringContent(Data, Encoding.UTF8, "application/json");
                        responseMessage = await client.PutAsync(Url, httpContent);
                        break;

                    case ComandHttp.Patch:
                        httpContent = new StringContent(Data, Encoding.UTF8, "application/json");
                        responseMessage = await client.PatchAsync(Url, httpContent);
                        break;

                    case ComandHttp.Delete:
                        responseMessage = await client.DeleteAsync(Url);
                        break;

                }


            }

            return responseMessage;
        }
    }

    public enum ComandHttp
    {
        Post,
        Get,
        Put,
        Delete,
        Patch
    }

    public class Token
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string ExpirationDate { get; set; }
    }

}
