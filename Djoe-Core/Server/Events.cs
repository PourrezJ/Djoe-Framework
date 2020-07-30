using Server.Menus;
using Server.Utils;
using System;
using System.IO;

namespace Server
{
    public static class Events
    {
        public static void Init()
        {
            GameMode.RegisterEventHandler("RegisterCoords", new Action<string, string, string, string, string>(SaveCoords));
        }

        private static void SaveCoords(string coordName, string posx, string posy, string posz, string heading)
        {
            StreamWriter coordsFile;
            if (!File.Exists("SavedCoords.txt"))
            {
                coordsFile = new StreamWriter("SavedCoords.txt");
            }
            else
            {
                coordsFile = File.AppendText("SavedCoords.txt");
            }
            var data = $"| {coordName} | pos: new Vector3({posx}f,{posy}f,{posz}f); heading: {heading});";
            Logger.Info(data);
            coordsFile.WriteLine(data);
            coordsFile.Close();
        }
    }
}
