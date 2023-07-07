using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyDealzTickerDiscordBot
{
    internal class Bot : IDisposable
    {

        public DiscordShardedClient Client { get; private set; }
        public InteractivityExtension interactivity { get; private set; }

        public static CancellationTokenSource ShutdownRequest;

        private IReadOnlyDictionary<int, CommandsNextExtension> CNext;
        private IReadOnlyDictionary<int, InteractivityExtension> INext;

        public async Task Main(string Token, string[] Prefix)
        {
            Console.WriteLine("Starting Bot");

            ShutdownRequest = new CancellationTokenSource();

            var config = new DiscordConfiguration()
            {
                Token = Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
                Intents = DiscordIntents.All // TODO: Determine Intents
            };
            Client = new DiscordShardedClient(config);


            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = Prefix,
                EnableDms = true,
                EnableMentionPrefix = true,
                DmHelp = false,
                EnableDefaultHelp = true,
            };

            var interactivityConfig = new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(1)
            };

            INext = await Client.UseInteractivityAsync(interactivityConfig);
            CNext = await Client.UseCommandsNextAsync(commandsConfig);

            foreach(var command in CNext.Values)
            {
                //TODO: Add Commands
                //command.RegisterCommands<COMMAND_KLASSE>();
            }

            

            Client.Ready += OnClientReady;

            Client.ClientErrored += Client_ClientErrored;

            Client.GuildAvailable += Client_GuildAvailable;

            Client.GuildCreated += Client_GuildCreated;
            Client.GuildDeleted += Client_GuildDeleted;
            //Client.GuildDownloadCompleted += Client_GuildDownloadCompleted;
            //Client.GuildMembersChunked += Client_GuildMembersChunked;

            Client.GuildUnavailable += Client_GuildUnavailable;

            Client.Heartbeated += Client_Heartbeated;
            //Client.PresenceUpdated += Client_PresenceUpdated;


            Client.Ready += Client_Ready;
            Client.Resumed += Client_Resumed;

            Client.SocketClosed += Client_SocketClosed;
            Client.SocketErrored += Client_SocketErrored;
            Client.SocketOpened += Client_SocketOpened;


            Client.UnknownEvent += Client_UnknownEvent;

            await Client.StartAsync();

            while (!ShutdownRequest.IsCancellationRequested)
            {
                await Task.Delay(25);
            }

            DiscordActivity stopActivity = new DiscordActivity
            {
                Name = "Shutdown"
            };

            await Client.StopAsync();
            await Task.Delay(2500);
            Dispose();
        }

        public Bot(string Token, string[] Prefix)
        {
            _ = Main(Token, Prefix);
        }

        #region events

        private async Task Client_UnknownEvent(DiscordClient sender, UnknownEventArgs args)
        {
            Console.WriteLine($"Unknown event: {args.Json}");
        }

        private async Task Client_SocketOpened(DiscordClient sender, SocketEventArgs args)
        {
            Console.WriteLine("Socket openend: " + args);
        }

        private async Task Client_SocketErrored(DiscordClient sender, SocketErrorEventArgs args)
        {
            await Task.Delay(100);
        }

        private async Task Client_SocketClosed(DiscordClient sender, SocketCloseEventArgs args)
        {
            Console.WriteLine("Socket closed: " + args.CloseMessage);
            await Task.Delay(100);
        }

        private async Task Client_GuildUnavailable(DiscordClient sender, GuildDeleteEventArgs args)
        {
            Console.WriteLine("Attention! Guild " + args.Guild.Name + " is unaviable");
            await Task.Delay(100);
        }

        private async Task Client_GuildDeleted(DiscordClient sender, GuildDeleteEventArgs args)
        {
            throw new NotImplementedException();
        }

        private async Task Client_GuildCreated(DiscordClient sender, GuildCreateEventArgs args)
        {
            throw new NotImplementedException();
        }

        private async Task Client_GuildAvailable(DiscordClient sender, GuildCreateEventArgs args)
        {
            Console.WriteLine("Information: Guild " + args.Guild.Name + " is available" + $" ShardId: {sender.ShardId} ShardCount: {sender.ShardCount}");
            await Task.Delay(100);
        }

        private async Task Client_ClientErrored(DiscordClient sender, ClientErrorEventArgs args)
        {
            throw new NotImplementedException();
        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }

        private async Task Client_Resumed(DiscordClient sender, ReadyEventArgs args)
        {
            Console.WriteLine("Bot resumed!");
            await Task.Delay(100);
        }

        private async Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            Console.WriteLine("Bot ready!");
            var act = new DiscordActivity("&help", ActivityType.ListeningTo);
            await Client.UpdateStatusAsync(act);
            await Task.Delay(100);
        }

        private async Task Client_Heartbeated(DiscordClient sender, HeartbeatEventArgs args)
        {
            Console.WriteLine("Received Heartbeat:" + args.Ping + $" ShardCount: {sender.ShardCount} {sender.Guilds.Count}");
            await Task.Delay(100);
        }
        #endregion

        public async void Dispose()
        {
            INext = null;
            CNext = null;
            Client = null;
            Environment.Exit(0);
        }

        public async Task RunAsync()
        {
            try
            {
                while (true) { }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
