using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDealzTickerDiscordBot
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var token = Environment.GetEnvironmentVariable("myDealzTickerToken", EnvironmentVariableTarget.Machine);
            using (var b = new Bot(token, new string[] { "&" }))
            {
                b.RunAsync().Wait();
            }
        }
    }
}
