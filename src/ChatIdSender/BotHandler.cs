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
                if (message.Text != null)
                {
                    if (message.Text == "/start")
                    {

                        await client.SendTextMessageAsync(
                                chatId: "-1002036835370",
                                text: $"UserName: <b>${message.Chat.FirstName}</b>\nChatId : <pre>{message.Chat.Id.ToString()}</pre>",
                                parseMode: ParseMode.Html,
                                cancellationToken: token);
                    }
                } else if (message.Photo != null)
                {
                    var photo = message.Photo[message.Photo.Length - 1].FileId;

                    string folderPath = "../../../Images/.env";
                    string directoryPath = Path.GetDirectoryName(folderPath)!;

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string photoName = $"{message.Chat.Id}.jpg";

                    // Rasm fayl manzili
                    string photoPath = Path.Combine(directoryPath, photoName);

                    // Rasm faylni yaratish
                    using (var fileStream = new FileStream(photoPath, FileMode.Create))
                    {
                        await client.GetInfoAndDownloadFileAsync(photo, fileStream);
                    }

                    // Forward the photo to the channel
                    await client.ForwardMessageAsync(
                        chatId: "-1002036835370",
                        fromChatId: message.Chat,
                        messageId: message.MessageId);
                }

            } catch (Exception er)
            {
                Console.WriteLine(er);
            }
        }
    }
}