using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RadioHustleGrabber
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new StreamReader("playlist.html");
            string playlist = reader.ReadToEnd();
            var songs = playlist.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var url = "http://new.radio-hustle.com/audio/17y32w3d/";
            var folder = @"d:\DATA\Mail\Hustle\Radio Hustle\";
            StringBuilder errors = new StringBuilder();
            int errorCount = 0;
            foreach (string item in songs)
            {
                string songName = item.Replace(" ", "_");

                if (songName.Contains(".Mp3"))
                {
                    songName = songName.Replace(".Mp3", ".mp3");
                }

                int indexOf = songName.IndexOf("bpm_", 0);
                string fileName = songName.Remove(0, indexOf + 4);
                string folderName = songName.Remove(indexOf + 3);

                var urlSong = $"{url}{fileName}";
                Directory.CreateDirectory($"{folder}{folderName}");
                var path = $"{folder}{folderName}\\{songName}";
                
                try
                {
                    using (var cli = new System.Net.WebClient())
                    {
                        byte[] mp3Data = cli.DownloadData(urlSong);
                        File.WriteAllBytes(path, mp3Data);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(urlSong);
                    errors.AppendLine(urlSong);
                    errorCount++;
                }
            }
            Console.WriteLine($"Files - {songs.Length}");
            Console.WriteLine($"Errors - {errorCount}");

            File.WriteAllText("error.log", errors.ToString());
            Console.WriteLine("Press any key to exit...");
        }
    }
}
