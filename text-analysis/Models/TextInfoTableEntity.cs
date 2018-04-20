using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace text_analysis.Models
{
    public class TextInfoTableEntity : TableEntity
    {
        public TextInfoTableEntity()
        {
        }

        public TextInfoTableEntity(string id)
        {
            this.RowKey = id;
            this.PartitionKey = "text";
        }

        public int WordCount
        {
            get;
            set;
        }

        public int UniqueWords
        {
            get;
            set;
        }

        public int Complexity
        {
            get;
            set;
        }
    }
}