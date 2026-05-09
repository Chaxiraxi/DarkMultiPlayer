using System;
using System.Collections.Generic;
using System.IO;

namespace DarkMultiPlayerServer
{
    public class CheatSystem
    {
        private static CheatSystem instance;
        private static string cheatListFile;
        private List<string> cheatUsers = new List<string>();

        public static CheatSystem fetch
        {
            get
            {
                //Lazy loading
                if (instance == null)
                {
                    cheatListFile = Path.Combine(Server.configDirectory, "cheats.txt");
                    instance = new CheatSystem();
                    instance.LoadCheatUsers();
                }
                return instance;
            }
        }

        private void LoadCheatUsers()
        {
            DarkLog.Debug("Loading cheat users");
            lock (cheatUsers)
            {
                cheatUsers.Clear();

                if (File.Exists(cheatListFile))
                {
                    foreach (string user in File.ReadAllLines(cheatListFile))
                    {
                        if (!string.IsNullOrWhiteSpace(user) && !cheatUsers.Contains(user))
                        {
                            cheatUsers.Add(user);
                        }
                    }
                }
                else
                {
                    SaveCheatUsers();
                }
            }
        }

        private void SaveCheatUsers()
        {
            DarkLog.Debug("Saving cheat users");
            try
            {
                if (File.Exists(cheatListFile))
                {
                    File.SetAttributes(cheatListFile, FileAttributes.Normal);
                }

                using (StreamWriter sw = new StreamWriter(cheatListFile))
                {
                    foreach (string user in cheatUsers)
                    {
                        sw.WriteLine(user);
                    }
                }
            }
            catch (Exception e)
            {
                DarkLog.Error("Error saving cheat user list!, Exception: " + e);
            }
        }

        public void AddCheatUser(string playerName)
        {
            lock (cheatUsers)
            {
                if (!cheatUsers.Contains(playerName))
                {
                    cheatUsers.Add(playerName);
                    SaveCheatUsers();
                }
            }
        }

        public void RemoveCheatUser(string playerName)
        {
            lock (cheatUsers)
            {
                if (cheatUsers.Contains(playerName))
                {
                    cheatUsers.Remove(playerName);
                    SaveCheatUsers();
                }
            }
        }

        public bool IsCheatUser(string playerName)
        {
            lock (cheatUsers)
            {
                return cheatUsers.Contains(playerName);
            }
        }

        public string[] GetCheatUsers()
        {
            lock (cheatUsers)
            {
                return cheatUsers.ToArray();
            }
        }
    }
}
