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
            int separatorIndex = commandArgs.IndexOf(" ", StringComparison.Ordinal);
            if (separatorIndex != -1)
            {
                func = commandArgs.Substring(0, separatorIndex);
                if (commandArgs.Length > separatorIndex + 1)
                {
                    playerName = commandArgs.Substring(separatorIndex + 1);
                }
            }

            switch (func)
            {
                default:
                    DarkLog.Normal("Undefined function. Usage: /cheat [add|del|show] [playername]");
                    break;
                case "add":
                    if (string.IsNullOrWhiteSpace(playerName) || !SafeFile.IsNameSafe(playerName))
                    {
                        DarkLog.Normal("Invalid player name.");
                        break;
                    }
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
                    if (string.IsNullOrWhiteSpace(playerName) || !SafeFile.IsNameSafe(playerName))
                    {
                        DarkLog.Normal("Invalid player name.");
                        break;
                    }
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
                    string[] cheatUsers = CheatSystem.fetch.GetCheatUsers();
                    if (cheatUsers.Length == 0)
                    {
                        DarkLog.Normal("No players on cheat list.");
                    }
                    else
                    {
                        foreach (string player in cheatUsers)
                        {
                            DarkLog.Normal(player);
                        }
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
