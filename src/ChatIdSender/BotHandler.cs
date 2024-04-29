using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatIdSender
{
    public class BotHandler
    {
        private readonly string botToken;

        public BotHandler(string token)
        {
            botToken = token;
        }

        public async Task BotHandle()
        {
            var botClient = new TelegramBotClient(botToken);

            using CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();
        }

        private async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Message is not { } message)
                return;
            try
            {
                await client.SendTextMessageAsync(
                        chatId: "",
                        text: message.Chat.Id.ToString(),
                        cancellationToken: token);
            }
            catch
            {
                Console.WriteLine("Error");
            }
        }
    }
}
