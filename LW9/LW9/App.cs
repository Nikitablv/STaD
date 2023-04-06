using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LW9
{
    public class App
    {
        public App()
        {
            client = new(new HttpClientHandler())
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public async Task<JArray> GetProducts()
        {
            HttpResponseMessage response = await client.GetAsync(baseUrl + getProducts);

            return JArray.Parse(JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync())!.ToString());
        }

        public async Task<JObject> CreateProduct(Product product)
        {
            string jsonObject = JsonConvert.SerializeObject(product);
            StringContent requestContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(baseUrl + addProduct, requestContent);

            return JObject.Parse(JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()).ToString());
        }

        public async Task<JObject> DeleteProduct(int idProduct)
        {
            HttpResponseMessage response = await client.GetAsync(baseUrl + deleteProduct + $"?id={idProduct}");

            return JObject.Parse(JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()).ToString());
        }

        public async Task<JObject> EditProduct(Product product)
        {
            string jsonObject = JsonConvert.SerializeObject(product);
            StringContent requestContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(baseUrl + updateProduct, requestContent);

            return JObject.Parse(JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()).ToString());
        }

        private readonly string baseUrl = "http://shop.qatl.ru/";
        private readonly string getProducts = "api/products";
        private readonly string addProduct = "api/addproduct";
        private readonly string deleteProduct = "api/deleteproduct";
        private readonly string updateProduct = "api/editproduct";
        private readonly HttpClient client;
    }
}
