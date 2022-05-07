using MembershipCharge.SeedWork.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MembershipCharge.ServiceProxy.GoogleAPI
{
    public class GoogleAPI
    {
        private readonly string _url = "https://maps.googleapis.com/maps/api/geocode/json?address={0}&key=YOUR_GOOGLE_KEY";
        private readonly int _timeout = 30;

        public GeoCodeResult GetGeoCodeForZipCode(string zipCode)
        {
            GeoCodeResult geoCode = new GeoCodeResult();
            string urlViaCep = string.Format(_url, zipCode);

            try
            {


                APIClient api = new APIClient(urlViaCep, _timeout, ComandHttp.Get, null);
                HttpResponseMessage response = api.ExecuteCall().Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = response.Content.ReadAsStringAsync().Result;
                    geoCode = JsonConvert.DeserializeObject<GeoCodeResult>(jsonResult);
                }
                else
                {
                    geoCode = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro get geoCode of addre: " + zipCode);
                Console.WriteLine("Exception: " + ex.Message);
                geoCode = null;
            }

            return geoCode;
        }
    }
}
