using System;
using System.IO;
using Newtonsoft.Json;
using RTTicTacToe.Forms.Models;

namespace RTTicTacToe.Forms.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private static string PlayerFileName = "StoredPLayer.json";

        public Player GetStoredPlayer()
        {
            var localFilePath = GetLocalPath(PlayerFileName);
            if (!File.Exists(localFilePath))
            {
                return null;
            }

            try
            {
                var fileText = File.ReadAllText(localFilePath);
                return JsonConvert.DeserializeObject<Player>(fileText);
            }
            catch
            {
                //TODO: log this error
                return null;
            }
        }

        public bool SavePlayer(Player player)
        {
            if(player == null)
            {
                return false;
            }
            try
            {
                var localFilePath = GetLocalPath(PlayerFileName);
                File.WriteAllText(localFilePath, JsonConvert.SerializeObject(player));
                return true;
            }
            catch
            {
                //TODO: Log this error
                return false;
            }

        }

        private string GetLocalPath(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
        }
    }
}
