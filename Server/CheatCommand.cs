using System;
using System.IO;

namespace DarkMultiPlayerServer
{
    public class CheatCommand
    {
        public static void HandleCommand(string commandArgs)
        {
            string func = "";
            string playerName = "";

            func = commandArgs;
            if (commandArgs.Contains(" "))
            {
                func = commandArgs.Substring(0, commandArgs.IndexOf(" ", StringComparison.Ordinal));
                if (commandArgs.Substring(func.Length).Contains(" "))
                {
                    playerName = commandArgs.Substring(func.Length + 1);
                }
            }

            switch (func)
            {
                default:
                    DarkLog.Normal("Undefined function. Usage: /cheat [add|del] playername or /cheat show");
                    break;
                case "add":
                    if (File.Exists(Path.Combine(Server.universeDirectory, "Players", playerName + ".txt")))
                    {
                        if (!CheatSystem.fetch.IsCheatUser(playerName))
                        {
                            DarkLog.Normal("Added '" + playerName + "' to cheat list.");
                            CheatSystem.fetch.AddCheatUser(playerName);
                            RefreshPlayerCheatSettings(playerName);
                        }
                        else
                        {
                            DarkLog.Normal("'" + playerName + "' is already on the cheat list.");
                        }
                    }
                    else
                    {
                        DarkLog.Normal("'" + playerName + "' does not exist.");
                    }
                    break;
                case "del":
                    if (CheatSystem.fetch.IsCheatUser(playerName))
                    {
                        DarkLog.Normal("Removed '" + playerName + "' from the cheat list.");
                        CheatSystem.fetch.RemoveCheatUser(playerName);
                        RefreshPlayerCheatSettings(playerName);
                    }
                    else
                    {
                        DarkLog.Normal("'" + playerName + "' is not on the cheat list.");
                    }
                    break;
                case "show":
                    foreach (string player in CheatSystem.fetch.GetCheatUsers())
                    {
                        DarkLog.Normal(player);
                    }
                    break;
            }
        }

        private static void RefreshPlayerCheatSettings(string playerName)
        {
            ClientObject client = ClientHandler.GetClientByName(playerName);
            if (client != null)
            {
                Messages.ServerSettings.SendServerSettings(client);
            }
        }
    }
}
