namespace ChatIdSender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string token = "6756315206:AAHBksgO4m46pU0gxH43A35lqyRu6diBhdY";
            BotHandler handle = new BotHandler(token);

            try
            {
                handle.BotHandle().Wait();
            } catch
            {
                handle.BotHandle().Wait();
            }
        }
    }
}
