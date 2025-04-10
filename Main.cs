
using GTranslatorAPI;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using static System.Runtime.InteropServices.JavaScript.JSType;




using var cts = new CancellationTokenSource();

var bot = new TelegramBotClient("8018355549:AAEdmO5Sjw0lM8b40-eUtwpnKv3Vc-kSe2s", cancellationToken: cts.Token);
var me = await bot.GetMe();


Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel();
bot.OnError += OnError;
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;





void HandlePollingErrorAsync()
{
    async Task OnError(Exception exception, HandleErrorSource source)
    {
        Console.WriteLine(exception);
    }
}


void HandleUpdateAsync()
{
    async Task OnMessage(Message msg, UpdateType type)
    {
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


        //var translator = new Translator();
        //var result = await translator.TranslateAsync(Languages.ru, Languages.en, msg);

        //Console.WriteLine(result.TranslatedText);



    }
}
class BotMethods 
{
    static void Main()
    {
        HandlePollingErrorAsync();
        HandleUpdateAsync();
    }
}


//class BotMethods 
//{
//    static void Main()
//    {
//        HandlePollingErrorAsync();
//    }
//}