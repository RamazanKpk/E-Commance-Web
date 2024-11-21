using Buisness.Interface.Abstract.WebAbstract;
using DataModel.CategoryViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Interface.Service.CategoryServices
{
    public class CategoryService : IAllWebService<CategoryListModel>
    {
        public List<CategoryListModel> GetApiToWeb()
        {
            List<CategoryListModel> categories = new List<CategoryListModel>();
            string api = ConfigurationManager.AppSettings["GetCategoryEndPoint"].ToString();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    categories = JsonConvert.DeserializeObject<List<CategoryListModel>>(json);
                }
            }
            return categories;
        }
    }
}
