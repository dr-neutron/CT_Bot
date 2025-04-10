
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System;



namespace Handler
{
    
    public static class BotMethods
    {

        private static readonly HttpClient httpClient = new HttpClient();

        public async static Task HandleUpdateAsync(Message msg, UpdateType type)
        {
            using var cts = new CancellationTokenSource();
            var bot = new TelegramBotClient("8018355549:AAEdmO5Sjw0lM8b40-eUtwpnKv3Vc-kSe2s", cancellationToken: cts.Token);
            var me = await bot.GetMe();

            cts.Cancel();
            Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
            Console.ReadLine();

        
          //public async static void HandlePollingErrorAsync(Exception exception, Update update)
               //{
             //    Console.WriteLine(exception);
             //}


            if (msg.Text == "/author")
            {
                await bot.SendMessage(msg.Chat, "Соцсети автора: \n Telegram: @johnqwertyp42 \n VK: https://vk.com/id661426682");
            }

            if (msg.Text == "/start")
            {
                await bot.SendMessage(msg.Chat, $"Привет, {msg.From}! Этот бот позволяет переводить сообщения");
            }
            if (msg.Text is null) return;
            

            if (msg.Text is null) return;
            Console.WriteLine($"Received {type} '{msg.Text}' in {msg.Chat}");
            var englishTranslation = await TranslateTextAsync(msg.Text, "ru", "en");
            var turkishTranslation = await TranslateTextAsync(msg.Text, "ru", "tr");

            await bot.SendMessage(msg.Chat, $"Перевод с Google Translate:\n Английский: {englishTranslation}\n Турецкий: {turkishTranslation}");
        }
        private static async Task<string> TranslateTextAsync(string text, string sourceLang, string targetLang)
        {

            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLang}&tl={targetLang}&dt=t&q={Uri.EscapeDataString(text)}";
            var response = await httpClient.GetStringAsync(url);

            var result = JsonSerializer.Deserialize<List<object>>(response);
            return result?[0]?.ToString()?.Split('"')[1] ?? "Ошибка: Нет перевода";
        }
       
        
    }
    class Program
    {
        static void Main()
        {
            HandleUpdateAsync();


        }
    }
}


