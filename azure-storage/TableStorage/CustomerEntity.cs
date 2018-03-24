using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableStorageHandsOn
{
    public class CustomerEntity : TableEntity
    {
        // Needed by SDK
        public CustomerEntity()
        {
            this.BirthDate = DateTime.Now;
        }

        public CustomerEntity(string country, string name)
            : this()
        {
            this.PartitionKey = country;
            this.RowKey = name;
        }

        public DateTime BirthDate
        {
            get;
            set;
        } 
    }
}
