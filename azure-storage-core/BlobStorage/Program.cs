using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorage
{
    class Program
    {
        public static async Task MainAsync(string[] args)
        {
             // 01 - Connect to your azure storage account
            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=fdfgdfgdfgd;AccountKey=jEyGRzzyuf0hdXb9EgXyftmqWqBHl8xpxdXqmxbonjco+R2VuldtaC5zP9VLXoF2sEq9nnhJ7rPLTS/NEoFSJA==;EndpointSuffix=core.windows.net");
            var client = storageAccount.CreateCloudBlobClient();

            // 02 - Create a container called "text-files"
            var container = client.GetContainerReference("text-files");
            await container.CreateIfNotExistsAsync();

            // 03 - Set the container permissions to BlobContainerPublicAccessType.Blob
            await container.SetPermissionsAsync(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });

            // 04 - Upload SampleText.txt to a block block called "UploadedSampleText.txt"
            var bytes = File.ReadAllBytes("SampleText.txt");
            var blob = container.GetBlockBlobReference("UploadedSampleText.txt");
            await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);

            // 05 - Download "UploadedSampleText.txt" from the storage account and print its contents using Console.WriteLine()
            var downloadStream = new MemoryStream();
            blob = container.GetBlockBlobReference("UploadedSampleText.txt");
            await blob.DownloadToStreamAsync(downloadStream);
            downloadStream.Position = 0;
            var reader = new StreamReader(downloadStream);

            Console.WriteLine(reader.ReadToEnd());

            // 05 - Delete UploadedSampleText.txt from the storage
            await blob.DeleteAsync();
        }
        static void  Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();

            Console.ReadKey();
        }
    }
}
