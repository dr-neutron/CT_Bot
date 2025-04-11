using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Main 
{ 
    public static class BotMethods 
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public static async void HandleUpdateAsync(ITelegramBotClient bot, Update update)
        {
            var time = DateTime.Now.ToString();

            if (update.Type != UpdateType.Message)
                return;

            var msg = update.Message;
            if (msg == null)
                return;

            if (msg.Text == "/author")
            {
                await bot.SendMessage(msg.Chat, "Соцсети автора: \n Telegram: @johnqwertyp42 \n VK: https://vk.com/id661426682");
                return;
            }

            if (msg.Text == "/start")
            {
                await bot.SendMessage(msg.Chat, $"Привет, {msg.From}! Этот бот позволяет переводить сообщения, c английского на французкий.");
                return;
            }
            
            var englishTranslation = await TranslateTextAsync(msg.Text, "ru", "en");
            var turkishTranslation = await TranslateTextAsync(msg.Text, "ru", "tr");

            await bot.SendMessage(msg.Chat, $"Перевод с Google Translate:\n Английский: {englishTranslation}\n Турецкий: {turkishTranslation}");
            Console.WriteLine($"({time})     Получено сообщение - '{msg.Text}' в чате: {msg.Chat.Id}");
        }
        private static async Task<string> TranslateTextAsync(string text, string sourceLang, string targetLang)
        {

            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLang}&tl={targetLang}&dt=t&q={Uri.EscapeDataString(text)}";
            var response = await httpClient.GetStringAsync(url);

            var result = JsonSerializer.Deserialize<List<object>>(response);
            return result?[0]?.ToString()?.Split('"')[1] ?? "Ошибка: Нет перевода";
        }
        
    }
}

