using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Configuration;
using TableStorageHandsOn;

namespace TableStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            // 01 - Connect to your azure storage account
            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=aubgtestatorage;AccountKey=8MktAwg721zQRBK4ZMUxGJvvvMjq0WslLkilbnjndbf96yS5or3PhvoZTpruX7mTIeQNBaE5YgSkhJBBCiG7jg==;");
            var client = storageAccount.CreateCloudTableClient();

            // 02 - Create a table called "customers"
            var table = client.GetTableReference("customers");
            table.CreateIfNotExists();

            // 03 - Insert single entity (instance of the CustomerEntity class) into the table
            var customer1 = new CustomerEntity("Bulgaria", "Maria");

            var insertOperation = TableOperation.Insert(customer1);
            table.Execute(insertOperation);

            // 04 - Insert two additional CustomerEntity objects using batching (use PartitionKey "Netherlands")
            var customer2 = new CustomerEntity("Netherlands", "Thijs");
            var customer3 = new CustomerEntity("Netherlands", "OtherGuy");

            var batchOperation = new TableBatchOperation();
            batchOperation.Insert(customer2);
            batchOperation.Insert(customer3);
            table.ExecuteBatch(batchOperation);

            // 05 - Retrieve one of the entities using TableOperation.Retrieve and print its PartitionKey using Console.WriteLine()
            var retrieve = TableOperation.Retrieve<CustomerEntity>("Bulgaria", "Maria");
            var singleResult = table.Execute(retrieve).Result as CustomerEntity;

            Console.WriteLine(singleResult.PartitionKey);

            // 06 - Retrieve all entities with PartitionKey "Netherlands" using TableQuery and print their RowKey using Console.WriteLine()
            var query = new TableQuery<CustomerEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Netherlands"));
            var result = table.ExecuteQuery(query);

            foreach (var item in result)
            {
                Console.WriteLine(item.RowKey);
            }

            // 07 - Delete one of the entities from the table 
            var deleteOperation = TableOperation.Delete(singleResult);
            table.Execute(deleteOperation);

            // 08 - Insert a new entity into "customers" using DynamicTableEntity instead of CustomerEntity
            var dynamicCustomer = new DynamicTableEntity("Bulgaria", "Georgi");
            dynamicCustomer.Properties["NewProperty"] = new EntityProperty("AUBG");

            var insertDynamic = TableOperation.Insert(dynamicCustomer);
            table.Execute(insertDynamic);
        }
    }
}
