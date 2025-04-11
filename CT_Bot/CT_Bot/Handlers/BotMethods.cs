using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Main
{
    public static class BotMethods
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update)
        {
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (update.Type != UpdateType.Message || update.Message == null)
                return;

            var msg = update.Message;

            if (msg.Text == "/author")
            {
                await bot.SendTextMessageAsync(msg.Chat.Id, "Соцсети автора: \n Telegram: @johnqwertyp42 \n VK: https://vk.com/id661426682");
                return;
            }

            if (msg.Text == "/start")
            {
                await bot.SendTextMessageAsync(msg.Chat.Id, $"Привет, {msg.From?.FirstName}! Этот бот позволяет переводить сообщения с русского на английский и турецкий.");
                await bot.SendTextMessageAsync(msg.Chat.Id, $"Чтобы начать пользоватся {msg.From?.FirstName}, нужно отправить любое русское слово и дождатся ответа.");
                return;
            }

            if (!string.IsNullOrEmpty(msg.Text))
            {
                var englishTranslation = await TranslateTextAsync(msg.Text, "ru", "en");
                var turkishTranslation = await TranslateTextAsync(msg.Text, "ru", "tr");

                await bot.SendTextMessageAsync(msg.Chat.Id, $"Перевод с Google Translate:\n Английский: {englishTranslation}\n Турецкий: {turkishTranslation}");
                Console.WriteLine($"({time})     Получено сообщение - '{msg.Text}' в чате: {msg.Chat.Id}");
            }
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