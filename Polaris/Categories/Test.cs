using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Polaris.Categories
{
    [Hidden]
    public class Test : BaseCommandModule
    {
        [Command("regex"), Cooldown(2, 50, CooldownBucketType.User)]
        public async Task EmailRegex(CommandContext ctx, string email)
        {
            var pattern = @"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+";

            Console.WriteLine(Regex.IsMatch(email, pattern));
        }
    }
}