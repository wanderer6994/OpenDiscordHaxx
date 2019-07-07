using System;
using System.IO;
using System.Windows.Forms;

namespace DiscordHaxx
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            UpdateTitle();

            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Text file |*.txt",
                Title = "Choose a file to load tokens from",
                InitialDirectory = new FileInfo(Application.ExecutablePath).DirectoryName
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Raidbot.TokenPath = dialog.FileName;
                Raidbot.LoadClients(File.ReadAllLines(dialog.FileName));
            }

            while (true)
            {
                CommandHandler.ShowActions();
                Console.WriteLine();
                Console.Write("Action: ");
                CommandHandler.HandleCommand(int.Parse(Console.ReadLine()));
            }
        }

        public static void UpdateTitle()
        {
            Console.Title = $"DiscordHaxx [BETA] | Bots: {Raidbot.Clients.Count}";
        }
    }
}