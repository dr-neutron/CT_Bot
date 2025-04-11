using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Main;

namespace CT_Bot
{
    class Program
    {
        static async Task Main()
        {
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                Console.WriteLine("ВВЕДИТЕ ТОКЕН ВАШЕГО БОТА: ");
                var token = Console.ReadLine();
                using var cts = new CancellationTokenSource();
                var bot = new TelegramBotClient(token);
                var me = await bot.GetMeAsync();
                Console.WriteLine($"ЗАПУСТИТЬ БОТА ({me.FirstName})? \n Да - 1 \n Нет - 2 \n ОТВЕТ: ");

                if (int.TryParse(Console.ReadLine(), out int choiceStart))
                {
                    if (choiceStart == 1)
                    {
                        Console.WriteLine($"({time})     СТАРТ БОТА...");

                        var receiverOptions = new ReceiverOptions
                        {
                            AllowedUpdates = Array.Empty<UpdateType>()
                        };

                        var handler = new DefaultUpdateHandler(
                            updateHandler: async (botClient, update, cancellationToken) =>
                            {
                                await BotMethods.HandleUpdateAsync(botClient, update);
                            },
                            pollingErrorHandler: HandlePollingErrorAsync
                        );

                        bot.StartReceiving(
                            updateHandler: handler,
                            receiverOptions: receiverOptions,
                            cancellationToken: cts.Token
                        );

                        Console.WriteLine($"({time})     {me.FirstName} ЗАПУЩЕН");
                        Console.WriteLine($"({time})    @{me.FirstName} работает... Напишите что-то в консоль, чтобы выключить:");
                        Console.ReadLine();
                        cts.Cancel();
                    }
                    else if (choiceStart == 2)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("НЕВЕРНОЕ ЗНАЧЕНИЕ");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("НЕВЕРНЫЙ ВВОД. ВЫХОД...");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"({time})     ОШИБКА ПРИ ЗАПУСКЕ БОТА! \n ОШИБКА:    {ex.Message}");
            }
        }

        static Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken ct)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiException => $"Ошибка:\n[{apiException.ErrorCode}]\n{apiException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}