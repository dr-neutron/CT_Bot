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

        private static readonly Dictionary<long, string> activeGames = new Dictionary<long, string>();

        public static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update)
        {
            // Получаем текущее время для логирования
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (update.Type != UpdateType.Message || update.Message == null)
                return;

            var msg = update.Message; // Текущее сообщение
            var me = await bot.GetMeAsync();

            if (msg.Text == "/author")
            {
                await bot.SendTextMessageAsync(msg.Chat.Id, "Соцсети автора: \n Telegram: @johnqwertyp42 \n VK: https://vk.com/id661426682");
                return;
            }

            if (msg.Text == "/start")
            {
                await bot.SendTextMessageAsync(msg.Chat.Id, $"Привет, {msg.From?.FirstName}! Этот бот позволяет переводить сообщения с русского на английский и турецкий.");
                await bot.SendTextMessageAsync(msg.Chat.Id, $"Чтобы начать пользоватся {me.FirstName}, нужно отправить любое русское слово и дождатся ответа.");
                return;
            }
            
            //  мини-игра
            if (msg.Text == "/minigame")
            {
                
                
                
                    
                    // случайное русское слово
                    var word = GetRandomRussianWord();

                    // Переводим это слово на англ
                    var correctTranslation = await TranslateTextAsync(word, "ru", "en");

                    // Сохраняем правильный ответ в словаре activeGames.
                    activeGames[msg.Chat.Id] = correctTranslation;

                    await bot.SendTextMessageAsync(msg.Chat.Id, $"Мини-игра началась! Переведите слово: '{word}'");
                    Console.WriteLine($"({time})     Мини-игра запущена для чата {msg.Chat.Id}. Слово: '{word}', Ответ: '{correctTranslation}'");
                    return;

                
            }

            // Если игра уже активна для этого чата, проверяем ответ пользователя.
            if (activeGames.ContainsKey(msg.Chat.Id))
            {
                
                
                
                    int check = 0;
                    var userAnswer = msg.Text.Trim().ToLower();
                    var correctAnswer = activeGames[msg.Chat.Id].ToLower(); // Правильный ответ 

                    if (userAnswer == correctAnswer)
                    {
                        check++;
                        await bot.SendTextMessageAsync(msg.Chat.Id, $"Правильно! Вы молодец! Ваш счёт - {check}");
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(msg.Chat.Id, $"Неправильно. Правильный ответ: '{correctAnswer}'. Ваш счет был - {check}.");
                        check = 0;                     
                    }
                
              

                // Удаляем игру из словаря activeGames, так как она завершена
                activeGames.Remove(msg.Chat.Id);
                return;
            }

            // Если сообщение не является командой или частью игры, выполняем перевод текста
            if (!string.IsNullOrEmpty(msg.Text))
            {
                // Переводим текст на английский и турецкий языки.
                var englishTranslation = await TranslateTextAsync(msg.Text, "ru", "en");
                var turkishTranslation = await TranslateTextAsync(msg.Text, "ru", "tr");

                // Отправляем результат перевода пользователю.
                await bot.SendTextMessageAsync(msg.Chat.Id, $"Перевод с Google Translate:\n Английский: {englishTranslation}\n Турецкий: {turkishTranslation}");
                Console.WriteLine($"({time})     Получено сообщение - '{msg.Text}' в чате: {msg.Chat.Id}");
            }
        }
        private static async Task<string> TranslateTextAsync(string text, string sourceLang, string targetLang)
        {
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLang}&tl={targetLang}&dt=t&q={Uri.EscapeDataString(text)}";

            var response = await httpClient.GetStringAsync(url);

            var result = JsonSerializer.Deserialize<List<object>>(response);

            // Извлекаем переведенный текст из ответа.
            return result?[0]?.ToString()?.Split('"')[1] ?? "Ошибка: Нет перевода";
        }

        // Метод возвращает случайное слово из списка
        private static string GetRandomRussianWord()
        {
            var words = new List<string>
            {
                "дом", "Полина", "собака", "Санкт-Петербург", "книга", "телефон", "окно", "человек", "город", "очки", "кошка", "слон", "привет"
            };

            var random = new Random();
            return words[random.Next(words.Count)];
        }
    }
}