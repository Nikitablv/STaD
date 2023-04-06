using Microsoft.VisualStudio.TestTools.UnitTesting;
using LW9;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Tests
{
    [TestClass]
    public class ApiTests
    {
        private readonly App client = new App();
        private readonly IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("E:/Nikita's documents/3 YEAR/STaD/LW9/API_Tests/Testing.json").Build();
        private readonly List<int> ids = new();

        private bool AreProductEqual(Product p1, Product p2)
        {
            Assert.AreEqual(p1.title, p2.title);
            Assert.AreEqual(p1.content, p2.content);
            Assert.AreEqual(p1.price, p2.price);
            Assert.AreEqual(p1.status, p2.status);
            Assert.AreEqual(p1.keywords, p2.keywords);
            Assert.AreEqual(p1.description, p2.description);
            Assert.AreEqual(p1.hit, p2.hit);

            return true;
        }

        private static JToken GetCertainProduct(int id, JArray products)
        {
            foreach (JToken product in products)
            {
                if (product["id"].ToObject<int>() == id)
                    return product;
            }

            return null;
        }

        [TestCleanup]
        public async Task Delete()
        {
            foreach (int id in ids)
                await client.DeleteProduct(id);
            ids.Clear();
        }

        [TestMethod]
        public async Task Get_non_empty_product_list()
        {
            //Arrange

            //Act
            JArray response = await client.GetProducts();

            //Assert
            Assert.IsTrue(response.Count > 0);
        }

        [TestMethod]
        public async Task Add_valid_product()
        {
            //Arrange
            Product product = config.GetSection("valid_item").Get<Product>();

            //Act
            JObject response = await client.CreateProduct(product);
            int productId = response["id"].ToObject<int>();
            ids.Add(response["id"].ToObject<int>());
            JArray products = await client.GetProducts();
            Product createdProduct = GetCertainProduct(productId, products).ToObject<Product>();

            //Assert
            Assert.IsNotNull(createdProduct);
            Assert.IsTrue(AreProductEqual(product, createdProduct));
        }

        [TestMethod]
        public async Task Add_invalid_product_with_category_id()
        {
            //Arrange
            Product product = config.GetSection("invalid_item_1").Get<Product>();

            //Act
            JObject response = await client.CreateProduct(product);
            int productId = response["id"].ToObject<int>();
            ids.Add(productId);
            JArray products = await client.GetProducts();

            //Assert
            Assert.AreEqual(0, response["status"].ToObject<int>());
            Assert.IsNull(GetCertainProduct(productId, products));
        }

        [TestMethod]
        public async Task Add_invalid_product_with_hit()
        {
            //Arrange
            Product product = config.GetSection("invalid_item_2").Get<Product>();

            //Act
            JObject response = await client.CreateProduct(product);
            int productId = response["id"].ToObject<int>();
            ids.Add(productId);
            JArray products = await client.GetProducts();

            //Assert
            Assert.AreEqual(0, response["status"].ToObject<int>());
            Assert.IsNull(GetCertainProduct(productId, products));
        }

        [TestMethod]
        public async Task Add_invalid_product_with_status()
        {
            //Arrange
            Product product = config.GetSection("invalid_item_3").Get<Product>();

            //Act
            JObject response = await client.CreateProduct(product);
            int productId = response["id"].ToObject<int>();
            ids.Add(productId);
            JArray products = await client.GetProducts();

            //Assert
            Assert.AreEqual(0, response["status"].ToObject<int>());
            Assert.IsNull(GetCertainProduct(productId, products));
        }

        [TestMethod]
        public async Task Add_same_valid_product_few_times()
        {
            //Arrange
            Product product = config.GetSection("valid_item").Get<Product>();

            //Act
            JObject response1 = await client.CreateProduct(product);
            JObject response2 = await client.CreateProduct(product);
            int firstId = response1["id"].ToObject<int>();
            int secondId = response2["id"].ToObject<int>();
            ids.Add(firstId);
            ids.Add(secondId);
            JArray products = await client.GetProducts();
            Product firstProduct = GetCertainProduct(firstId, products).ToObject<Product>();
            Product secondProduct = GetCertainProduct(secondId, products).ToObject<Product>();

            //Assert
            Assert.IsNotNull(firstProduct);
            Assert.IsNotNull(secondProduct);
            Assert.IsTrue(AreProductEqual(product, firstProduct));
            Assert.IsTrue(AreProductEqual(product, secondProduct));
            Assert.AreEqual(secondProduct.alias, secondProduct.title.ToLower() + "-0");
        }

        [TestMethod]
        public async Task Edit_existing_product()
        {
            //Arrange
            Product product = config.GetSection("valid_item").Get<Product>();
            Product editProduct = config.GetSection("edit_valid_item").Get<Product>();

            //Act
            JObject createResponse = await client.CreateProduct(product);
            int productId = createResponse["id"].ToObject<int>();
            ids.Add(productId);
            editProduct.id = productId;

            JObject editResponse = await client.EditProduct(editProduct);
            JArray products = await client.GetProducts();
            Product resultProduct = GetCertainProduct(productId, products).ToObject<Product>()!;

            //Assert
            Assert.AreEqual(1, editResponse["status"].ToObject<int>());
            Assert.IsTrue(AreProductEqual(resultProduct, editProduct));
            Assert.AreEqual(resultProduct.alias, editProduct.title.ToLower());
        }

        [TestMethod]
        public async Task Edit_not_existing_product()
        {
            //Arrange
            Product editProduct = config.GetSection("edit_not_existing_item").Get<Product>();
            JArray products = await client.GetProducts();
            int count1 = products.Count;

            //Act
            JObject response = await client.EditProduct(editProduct);
            int resultStatus = response["status"].ToObject<int>();
            products = await client.GetProducts();
            int lastProductId = products.Last["id"].ToObject<int>();
            ids.Add(lastProductId);
            int count2 = products.Count;

            //Assert
            Assert.AreEqual(count1, count2);
            Assert.AreEqual(0, resultStatus);
        }

        [TestMethod]
        public async Task Edit_product_with_no_id()
        {
            //Arrange
            Product editProduct = config.GetSection("edit_no_id_item").Get<Product>();

            //Act
            JObject response = await client.EditProduct(editProduct);

            //Assert
            Assert.AreEqual(0, response["status"].ToObject<int>());
        }

        [TestMethod]
        public async Task Delete_existing_product()
        {
            //Arrange
            Product product = config.GetSection("valid_item").Get<Product>();
            JObject createResponse = await client.CreateProduct(product);
            int productId = createResponse["id"].ToObject<int>();
            ids.Add(productId);
            JArray products = await client.GetProducts();
            int count1 = products.Count;

            //Act
            JObject deleteResponse = await client.DeleteProduct(productId);
            products = await client.GetProducts();
            int count2 = products.Count;

            //Assert
            Assert.AreEqual(1, deleteResponse["status"].ToObject<int>());
            Assert.AreEqual(count1, count2 + 1);
            Assert.IsNull(GetCertainProduct(productId, products));
        }

        [TestMethod]
        public async Task Delete_not_existing_product()
        {
            //Arrange
            int productId = 200000;
            JArray products = await client.GetProducts();
            int count1 = products.Count;

            //Act
            JObject deleteResponse = await client.DeleteProduct(productId);
            products = await client.GetProducts();
            int count2 = products.Count;

            //Assert
            Assert.AreEqual(0, deleteResponse["status"].ToObject<int>());
            Assert.AreEqual(count1, count2);
        }

    }
}
