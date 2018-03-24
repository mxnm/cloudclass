using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace BlobStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            // 01 - Connect to your azure storage account
            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=aubgtestatorage;AccountKey=8MktAwg721zQRBK4ZMUxGJvvvMjq0WslLkilbnjndbf96yS5or3PhvoZTpruX7mTIeQNBaE5YgSkhJBBCiG7jg==;");
            var client = storageAccount.CreateCloudBlobClient();

            // 02 - Create a container called "text-files"
            var container = client.GetContainerReference("text-files");
            container.CreateIfNotExists();

            // 03 - Set the container permissions to BlobContainerPublicAccessType.Blob
            container.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });

            // 04 - Upload SampleText.txt to a block block called "UploadedSampleText.txt"
            var bytes = File.ReadAllBytes("SampleText.txt");
            var blob = container.GetBlockBlobReference("UploadedSampleText.txt");
            blob.UploadFromByteArray(bytes, 0, bytes.Length);

            // 05 - Download "UploadedSampleText.txt" from the storage account and print its contents using Console.WriteLine()
            var downloadStream = new MemoryStream();
            blob = container.GetBlockBlobReference("UploadedSampleText.txt");
            blob.DownloadToStream(downloadStream);
            downloadStream.Position = 0;
            var reader = new StreamReader(downloadStream);

            Console.WriteLine(reader.ReadToEnd());

            // 05 - Delete UploadedSampleText.txt from the storage
            blob.Delete();

            Console.ReadLine();
        }
    }
}
