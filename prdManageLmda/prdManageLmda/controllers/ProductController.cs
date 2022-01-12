using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using Amazon;
using Amazon.DynamoDBv2.DocumentModel;

namespace prdManageLmda.controllers
{
    //class ProductController
    //{
    //}

    [Route("products")]

    public sealed class ProductController : Controller
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _serviceUrl;
        private const string TableName = "Products";

        public ProductController()
        {
            _accessKey = "AKIAX2EXRNUVBY3KN6E7"; //Environment.GetEnvironmentVariable("AccessKey");
            _secretKey = "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"; //Environment.GetEnvironmentVariable("SecretKey");
            _serviceUrl = "https://dynamodb.us-east-2.amazonaws.com";//Environment.GetEnvironmentVariable("ServiceURL");
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(GetProducts());
        }

        [HttpPost]
        public async  Task<string> CreateProduct(Product product)
            {
            //Write Log to Cloud Watch using Console.WriteLline.    
            Console.WriteLine("Execution started for function -  {0} at {1}",
                                "CreateProduct", DateTime.Now);

            // Create  dynamodb client  
            var dynamoDbClient = new AmazonDynamoDBClient(
                new BasicAWSCredentials(_accessKey, _secretKey),
                new AmazonDynamoDBConfig
                {
                    ServiceURL = _serviceUrl,
                    RegionEndpoint = RegionEndpoint.USEast2
                });

            //Create Table if it Does Not Exists  
            await CreateTable(dynamoDbClient, TableName);

            // Insert record in dynamodbtable  
            LambdaLogger.Log("Insert record in the table");
            
             await dynamoDbClient.PutItemAsync(TableName, new Dictionary<string, AttributeValue>
             {
                    { "Id",new AttributeValue{ N = product.Id.ToString()} },
                    { "ProductName", new AttributeValue(product.ProductName) },
                    { "Shortdesc",new AttributeValue(product.Shortdesc) },
                    { "Detailedesc", new AttributeValue(product.Detailedesc) },
                    { "Category",new AttributeValue(product.Category) },
                    { "StartPrice", new AttributeValue(product.StartPrice) },
                    { "Bidenddt",new AttributeValue(product.Bidenddt) }

             });
            return "Product Added.";
        }

        private async Task CreateTable(IAmazonDynamoDB amazonDynamoDBclient, string tableName)
        {
            //Write Log to Cloud Watch using LambdaLogger.Log Method  
            LambdaLogger.Log(string.Format("Creating {0} Table", tableName));

            var tableCollection = await amazonDynamoDBclient.ListTablesAsync();

            if (!tableCollection.TableNames.Contains(tableName))
                await amazonDynamoDBclient.CreateTableAsync(new CreateTableRequest
                {
                    TableName = tableName,
                    AttributeDefinitions = new List<AttributeDefinition>() {
                      new AttributeDefinition { AttributeName="Id", AttributeType="N" }//,
                      //new AttributeDefinition { AttributeName ="ProductName",AttributeType="S"},
                      //new AttributeDefinition { AttributeName="Shortdesc", AttributeType="S" },
                      //new AttributeDefinition { AttributeName ="Detailedesc",AttributeType="S"},
                      //new AttributeDefinition { AttributeName="Category", AttributeType="S" },
                      //new AttributeDefinition { AttributeName ="StartPrice",AttributeType="S"},
                      //new AttributeDefinition { AttributeName="Bidenddt", AttributeType="S" }
               },
                    KeySchema = new List<KeySchemaElement> {
                       new KeySchemaElement { AttributeName="Id",  KeyType= KeyType.HASH }//,
                      //new KeySchemaElement { AttributeName="ProductName",  KeyType= KeyType.RANGE},
                      //new KeySchemaElement { AttributeName="Shortdesc",  KeyType= KeyType.RANGE},
                      //new KeySchemaElement { AttributeName="Detailedesc",  KeyType= KeyType.RANGE},
                      //new KeySchemaElement { AttributeName="Category",  KeyType= KeyType.RANGE},
                      //new KeySchemaElement { AttributeName="StartPrice",  KeyType= KeyType.RANGE},
                      //new KeySchemaElement { AttributeName="Bidenddt",  KeyType= KeyType.RANGE},
                  },
                    
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    },
                });




        }

        [HttpDelete]
        public async Task<string> DeleteProduct(int id)
        {
            Console.WriteLine("Execution started for function -  {0} at {1}", "GetProducts", DateTime.Now);
            // Create  dynamodb client  
            var dynamoDbClient = new AmazonDynamoDBClient(
                new BasicAWSCredentials(_accessKey, _secretKey),
                new AmazonDynamoDBConfig
                {
                    ServiceURL = _serviceUrl,
                    RegionEndpoint = RegionEndpoint.USEast2
                });
            Table products = Table.LoadTable(dynamoDbClient, TableName);
            await products.DeleteItemAsync(id);
            return "Product Deleted.";
        }

        private  int GetProducts()
        {
            IList<Product> products = new List<Product>();
           
            //Write Log to Cloud Watch using Console.WriteLline.    
            Console.WriteLine("Execution started for function -  {0} at {1}","GetProducts", DateTime.Now);
            // Create  dynamodb client  
            var dynamoDbClient = new AmazonDynamoDBClient(
                new BasicAWSCredentials(_accessKey, _secretKey),
                new AmazonDynamoDBConfig
                {
                    ServiceURL = _serviceUrl,
                    RegionEndpoint = RegionEndpoint.USEast2
                });
            
            Table results =  Table.LoadTable(dynamoDbClient, TableName);
            //int rowcount = results.HashKeys.Count;

            ScanFilter scanFilter = new ScanFilter();
            scanFilter.AddCondition("Id", ScanOperator.GreaterThanOrEqual, 0);

            var search = results.Scan(scanFilter);
            int rowcount = search.Count;
            //List<Document> documentList = new List<Document>();
            //do
            //{
            //    AttributeValue aval = new AttributeValue();

            //    var indirows = search.NextKey;
            //    indirows.TryGetValue("Id",out aval);


            //} while (!search.IsDone);

            //foreach (Dictionary<string, AttributeValue> item in search.NextKey)
            //{
            //    var doc = Document.FromAttributeMap(item);
            //    var typedDoc = context.FromDocument<MyClass>(doc);
            //    result.Add(typedDoc);
            //}

            //Write Log to cloud watch using context.Logger.Log Method  

            return rowcount;

        }

    }
}
