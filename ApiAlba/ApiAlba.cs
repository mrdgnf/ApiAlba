
using Telegram.Bot;
using Telegram.Bot.Types;

public class ApiAlba
{
    public static Dictionary<(string, string), int> spentnews = new Dictionary<(string, string), int>();
    public static IOrderedEnumerable<KeyValuePair<(string,string),int>> FilteringAlgorithm(string job) {

        var News = new List<News>();

        var relevantnews = new Dictionary<(string, string), int>();

        var keyWords = KeyWords(job);

        ApplicationContext db = new ApplicationContext();

        News = db.NEWS.ToList();

        foreach (var news in News)
        {
            foreach (var word in news.text.Split().ToList())
            {
                foreach (var keywords in keyWords)
                {
                    if (word.ToLower().Contains(keywords.Key.Item1.ToLower()))
                    {
                        keyWords[keywords.Key]++;
                    }
                }
            }
            int IndexByRelevant = 0;
            foreach (var keywords in keyWords)
            {
                IndexByRelevant += keywords.Key.Item2 * keywords.Value;
            }
            if (!spentnews.ContainsKey((news.name, news.text)))
                relevantnews.Add((news.name, news.text), IndexByRelevant);
        }
        return relevantnews.OrderByDescending(news => news.Value);
    }
    

    public static Dictionary<(string, int),int> KeyWords(string job)
    {
        Dictionary<(string, int), int> KeyWords = new Dictionary<(string, int), int>();
        switch (job)
        {
            case ("главный директор"):
                KeyWords.Add(("финан", 3),0);
                KeyWords.Add(("эконом", 3),0);
                KeyWords.Add(("санкц", 1),0);
                KeyWords.Add(("нефть", 1),0);
                KeyWords.Add(("курс", 3),0);
                KeyWords.Add(("доллар", 1),0);
                KeyWords.Add(("евро", 1),0);
                KeyWords.Add(("банк", 1),0);
                KeyWords.Add(("ввп", 1),0);
                KeyWords.Add(("   ", 0),0);
                break;
            case ("бухгалтер"):
                KeyWords.Add(("налог", 5),0);
                KeyWords.Add(("закон", 8),0);
                KeyWords.Add(("указ", 5),0);
                KeyWords.Add(("мрот", 1),0);
                KeyWords.Add(("путин", 1),0);
                KeyWords.Add(("      ", 0),0);
                KeyWords.Add(("       ", 0),0);
                KeyWords.Add(("        ", 0),0);

                break;
            default:
                break;
        }
        return KeyWords;
    }
    public static void Main()
     {

        TelegramMethods.Startup();
        
    }

}








