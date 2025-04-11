using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Main;
using Telegram.Bot.Types;

class Program
{
    static async void Main()
    {
        var time = DateTime.Now.ToString();
        try
        {
            using var cts = new CancellationTokenSource();
            var bot = new TelegramBotClient("8018355549:AAH8TwRQEU0nvIprunmVVUkndZssjCCZmEE", cancellationToken: cts.Token);
            var me = await bot.GetMe();
            Console.WriteLine($"ЗАПУСТИТЬ БОТА({me.FirstName})? \n Да - 1 \n Нет - 2 \n ОТВЕТ: ");
            int ChoiseStart = int.Parse(Console.ReadLine());
            if (ChoiseStart == 1)
            {
                Console.WriteLine($"({time})     СТАРТ БОТА...");

                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                };
                bot.StartReceiving(
                    updateHandler: BotMethods.HandleUpdateAsync,
                    pollingErrorHandler: HandlePollingErrorAsync,
                    receiverOptions: receiverOptions,
                    cancellationToken: CancellationToken.None
                );
                Console.WriteLine($"({time})     {me} ЗАПУЩЕН");
                Console.WriteLine($"({time})    @{me.FirstName} работает... Напишите, что-то в консоль, чтобы выключить:");
                Console.ReadLine();
                cts.Cancel();
            }
            else if (ChoiseStart == 2)
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("НЕВЕРНОЕ ЗНАЧЕНИЕ");
                Environment.Exit(0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"({time})     ОШИБКА ПРИ ЗАПУСКЕ БОТА! \n ОШИБКА:    {ex.ToString()}");
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