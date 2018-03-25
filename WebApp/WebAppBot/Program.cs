using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using WebAppBot.Data;
using WebAppBot.Model;

namespace WebAppBot
{
    public static class Program
    {
        const string CONN = "DefaultEndpointsProtocol=https;AccountName=hackrc;AccountKey=u9HV3MqCpl+W9EuSgE9n7qVa/CN3DcMP0L1+P3nkJomfiElOEe7N0Fd9HeZpn5F6gPYsSzuXvp1uW+sYx1jHVA==;EndpointSuffix=core.windows.net";

        public static void Main(string[] args)
        {
            if (CloudStorageAccount.TryParse(CONN, out var storageAccount))
            {
                var cloudContainerRef = storageAccount.CreateCloudBlobClient().
                        GetContainerReference("hackrccontainer");

                var cloudBlockBlob = cloudContainerRef.GetBlockBlobReference("wiki.fr.vec");
                cloudBlockBlob.DownloadToFileAsync("wiki.fr.vec", FileMode.Create).Wait();
            }
            else
            {
                throw new System.Exception();
            }

            MongoController.Initialize();
            // MongoController.PostDocument(new TestMongoModel(1111, "titlellelle"), "testColl").Wait();
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}