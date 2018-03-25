﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using WebAppBot.Data;
using WebAppBot.Model;

namespace WebAppBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            MongoController.Initialize();
            // MongoController.PostDocument(new TestMongoModel(1111, "titlellelle"), "testColl").Wait();
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}