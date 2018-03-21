using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Sample.LuisBot
{
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        const string MESSAGE_ENTITY = "Message";

        static int count = 0;

        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"None Count : {++count}");
            context.Wait(MessageReceived);
        }

        [LuisIntent("SendMessage")]
        public async Task SendMessageIntent(IDialogContext context, LuisResult result)
        {
            EntityRecommendation entity;
            if (result.TryFindEntity(MESSAGE_ENTITY, out entity))
            {
                await context.PostAsync($"Message : {entity.Entity}");
            }
            else
            {
                await context.PostAsync($"Je n'ai pas compris le message");
            }
            context.Wait(MessageReceived);
        }

        //[LuisIntent("Greeting")]
        //public async Task GreetingIntent(IDialogContext context, LuisResult result)
        //{
        //    await ShowLuisResult(context, result);
        //}

        //[LuisIntent("Cancel")]
        //public async Task CancelIntent(IDialogContext context, LuisResult result)
        //{
        //    await ShowLuisResult(context, result);
        //}

        //[LuisIntent("Help")]
        //public async Task HelpIntent(IDialogContext context, LuisResult result)
        //{
        //    await ShowLuisResult(context, result);
        //}

        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
    }
}