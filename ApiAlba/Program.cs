using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Extensions.Polling;
using Newtonsoft.Json.Linq;

public class ApiAlba
{
    public static void GetNews(string job, ITelegramBotClient botClient, Update update) {

        List<News> News = new List<News>();
        ApplicationContext db = new ApplicationContext();

        News = db.NEWS.ToList();

        int a = 0;

        foreach (var news in News)
        {
            if (a >= 3) return;

            Print(news.name.ToUpper(), botClient, update);

            Print(news.text, botClient, update);
            a++;
        }
        
    }
    public static void Print(string massage, ITelegramBotClient botClient, Update update)
    {
        Thread.Sleep(50);
        var message = update.Message;
        botClient.SendTextMessageAsync(message.Chat, $"{massage}");    

    }
    public static void Main()
    {

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

    static ITelegramBotClient bot = new TelegramBotClient("5780617332:AAG540pUZn9qeyCGmOGwP6aZtRewSqkzrek");
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                case ("/главный директор"):
                    GetNews("главный директор", botClient, update);
                    albertsend = null;
                    break;
                case ("/бухгалтер"):
                    GetNews("бухгалтер", botClient, update);
                    albertsend = null;;             
                    break;
                default:
                    albertsend = "/start /Главный Директор /Бухгалтер";
                    break;
            }
            if (albertsend != null)
            {
                await botClient.SendTextMessageAsync(message.Chat, albertsend);
            }
            return;
        }
    }
    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }
}








