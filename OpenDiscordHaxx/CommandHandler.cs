using System;

namespace DiscordHaxx
{
    public static class CommandHandler
    {
        public static void ShowActions()
        {
            Console.WriteLine(
@"Mass join - 1
Mass leave - 2
Flood - 3
Friend spam - 4
Checker - 5");
        }

        public static void HandleCommand(int type)
        {
            switch (type)
            {
                case 1:
                    Console.Write("Invite: ");
                    string invite = Console.ReadLine().Replace("https://discord.gg/", "");
                    Console.Clear();
                    Raidbot.MassJoin(invite);
                    Console.Clear();
                    break;
                case 2:
                    Console.Write("Server ID: ");
                    long guildId = long.Parse(Console.ReadLine());
                    Console.Clear();
                    Raidbot.MassLeave(guildId);
                    Console.Clear();
                    break;
                case 3:
                    Console.Write("Channel ID: ");
                    long channelId = long.Parse(Console.ReadLine());
                    Console.Write("Message: ");
                    string msg = Console.ReadLine();
                    Console.Write("Add ascii? (Y/N): ");
                    bool addAscii = Console.ReadLine().ToLower() == "y";
                    Console.Clear();
                    Raidbot.Flood(channelId, msg, addAscii);
                    Console.Clear();
                    break;
                case 4:
                    Console.Write("User: ");
                    string[] user = Console.ReadLine().Split('#');
                    Console.Clear();
                    Raidbot.FriendSpam(user[0], int.Parse(user[1]));
                    Console.Clear();
                    break;
                case 5:
                    Console.Clear();
                    Checker.Check();
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Unknown action");
                    break;
            }
        }
    }
}
