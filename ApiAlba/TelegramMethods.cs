using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Extensions.Polling;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Tracing;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class TelegramMethods
 {

    static ITelegramBotClient bot = new TelegramBotClient("5780617332:AAG540pUZn9qeyCGmOGwP6aZtRewSqkzrek");

    
    public static void Startup() {

        Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { },
        };
        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();

    }
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
    {

        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject($"{update.Message?.Chat.FirstName} {update.Message?.Chat.LastName} написал: {update.Message?.Text}"));

        if (update.Type == UpdateType.Message)
        {
            var message = update.Message;
            string albertsend;
            switch (message?.Text?.ToLower())
            {
                case (null):
                    albertsend = "Извините, я не понял что вы написали :(";
                    break;
                case ("/start"):
                    albertsend = "Добро пожаловать, тут находятсья самые актуальные новости!";
                    break;
                case ("главный директор"):
                    GetNewsForTelegramBot("главный директор",botClient,update);
                    albertsend = null;
                    break;
                case ("/digest1"):
                    GetNewsForTelegramBot("главный директор", botClient, update);
                    albertsend = null;
                    break;
                case ("/digest2"):
                    GetNewsForTelegramBot("бухгалтер", botClient, update);
                    albertsend = null; ;
                    break;
                case ("бухгалтер"):
                    GetNewsForTelegramBot("бухгалтер", botClient, update);
                    albertsend = null; ;
                    break;
                default:
                    albertsend = "/digest1 or /digest2";
                    break;
            }
            if (albertsend != null)
            {
                await botClient.SendTextMessageAsync(message.Chat, albertsend);
            }
            return;
        }
    }
    public static void GetNewsForTelegramBot(string job,ITelegramBotClient botClient, Telegram.Bot.Types.Update update)
    {

        var relevantnews = ApiAlba.FilteringAlgorithm(job);

        var message = update.Message;

        int count1 = 0;

        foreach (var news in relevantnews)
        {
            count1++;
        }

        if (count1 <= 0) { botClient.SendTextMessageAsync(message.Chat, "Дайджесты на сегодня кончились, приходите завтра"); ; return; }

        int count2 = 0;

        foreach (var news in relevantnews)
        {
            if (count2 >= 3) return;
            Thread.Sleep(50);
            botClient.SendTextMessageAsync(message.Chat, $"{news.Key.Item1.ToUpper()}\n{news.Key.Item2}");
            ApiAlba.spentnews.Add(news.Key, news.Value);
            count2++;
        }
    }

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }

    public static void Print(string name,string text, Telegram.Bot.Types.Update update, ITelegramBotClient botClient)
    {
 
        var message = update.Message;
        botClient.SendTextMessageAsync(message.Chat, $"{name.ToUpper()}\n{text}");
    }


}

