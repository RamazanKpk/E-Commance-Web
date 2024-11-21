using Buisness.Interface.Abstract.WebAbstract;
using DataModel.BrandViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Interface.Service.BrandServices
{
    public class BrandService : IAllWebService<BrandListModel>
    {
        public List<BrandListModel> GetApiToWeb()
        {
            List<BrandListModel> brands = new List<BrandListModel>();
            string api = ConfigurationManager.AppSettings["GetBrandEndPoint"].ToString();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    brands = JsonConvert.DeserializeObject<List<BrandListModel>>(json);
                }
            }
            return brands;
        }
    }
}
