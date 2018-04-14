using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace TableStorage
{
    class Program
    {
        public static async Task MainAsync(string[] args)
        {
             // 01 - Connect to your azure storage account
            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=aubgtestatorage;AccountKey=8MktAwg721zQRBK4ZMUxGJvvvMjq0WslLkilbnjndbf96yS5or3PhvoZTpruX7mTIeQNBaE5YgSkhJBBCiG7jg==;");
            var client = storageAccount.CreateCloudTableClient();

            // 02 - Create a table called "customers"
            var table = client.GetTableReference("customers");
            await table.CreateIfNotExistsAsync();

            // 03 - Insert single entity (instance of the CustomerEntity class) into the table
            var customer1 = new CustomerEntity("Bulgaria", "Maria");

            var insertOperation = TableOperation.Insert(customer1);
            await table.ExecuteAsync(insertOperation);

            // 04 - Insert two additional CustomerEntity objects using batching (use PartitionKey "Netherlands")
            var customer2 = new CustomerEntity("Netherlands", "Thijs");
            var customer3 = new CustomerEntity("Netherlands", "OtherGuy");

            var batchOperation = new TableBatchOperation();
            batchOperation.Insert(customer2);
            batchOperation.Insert(customer3);
            await table.ExecuteBatchAsync(batchOperation);

            // 05 - Retrieve one of the entities using TableOperation.Retrieve and print its PartitionKey using Console.WriteLine()
            var retrieve = TableOperation.Retrieve<CustomerEntity>("Bulgaria", "Maria");
            var singleResult = (await table.ExecuteAsync(retrieve)).Result as CustomerEntity;
            
            Console.WriteLine(singleResult.PartitionKey);

            // 07 - Delete one of the entities from the table 
            var deleteOperation = TableOperation.Delete(singleResult);
            await table.ExecuteAsync(deleteOperation);

            // 08 - Insert a new entity into "customers" using DynamicTableEntity instead of CustomerEntity
            var dynamicCustomer = new DynamicTableEntity("Bulgaria", "Georgi");
            dynamicCustomer.Properties["NewProperty"] = new EntityProperty("AUBG");

            var insertDynamic = TableOperation.Insert(dynamicCustomer);
            await table.ExecuteAsync(insertDynamic);
        }
        static void  Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();

            Console.ReadKey();
        }
    }
}
