namespace ChatIdSender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string token = "";
            BotHandler handle = new(token);

            try
            {
                handle.BotHandle().Wait();
            }
            catch
            {
                handle.BotHandle().Wait();
            }
        }
    }
}
