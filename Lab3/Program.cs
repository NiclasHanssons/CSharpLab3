using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Lab3
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length > 0 && File.Exists(args[0]))
            {
                FileStream inputFromFile = new FileStream(args[0], FileMode.Open);

                int fileSize = (int)inputFromFile.Length;
                byte[] data = new byte[fileSize];
                inputFromFile.Read(data, 0, fileSize);

                string bmpHexCheck = "424D";
                string pngHexCheck = "89504E470D0A1A0A";

                int hexLength = 8;
                string hex = "";

                for (int i = 0; i < hexLength; i++)
                {
                    hex += data[i].ToString("X2");
                }

                if (hex.Substring(0, 4) == bmpHexCheck)
                {

                    string hexStringWidth = "";
                    
                    for (int i = 21; i > 17; i--)
                    {
                        
                        hexStringWidth += data[i].ToString("X2");
                    }
                    string hexStringHeight = "";

                    for (int i = 25; i > 21; i--)
                    {
                        hexStringHeight += data[i].ToString("X2");
                    }

                    int width = Convert.ToInt32(hexStringWidth, 16);
                    int height = Convert.ToInt32(hexStringHeight, 16);

                    Console.WriteLine($"This is a .bmp image. Resolution: {width}x{height} pixels");
                    Console.WriteLine($"File size: {fileSize} byte");
                }

                else if (hex.Substring(0, 16) == pngHexCheck)
                {

                    string hexStringWidth = "";

                    for (int i = 16; i < 20; i++)
                    {
                        hexStringWidth += data[i].ToString("X2");
                    }

                    string hexStringHeight = "";

                    for (int i = 20; i < 24; i++)
                    {
                        hexStringHeight += data[i].ToString("X2");
                    }

                    int width = Convert.ToInt32(hexStringWidth, 16);
                    int height = Convert.ToInt32(hexStringHeight, 16);

                    Console.WriteLine($"This is a .png image. Resolution: {width}x{height} pixels");
                    Console.WriteLine($"File size: {fileSize} byte");

                    int nextChunk = 0;

                    for (int i = 8; i < data.Length; i += (nextChunk + 12))
                    {
                        string position = data[i].ToString("X2") + data[i + 1].ToString("X2") + data[i + 2].ToString("X2") + data[i + 3].ToString("X2");
                        nextChunk = Convert.ToInt32(position, 16);
                        Console.WriteLine(($"Chunk type: { Encoding.ASCII.GetString(data, i + 4, 4) }, chunk size: {nextChunk + 12} byte"));
                    }
                }
                else
                {
                    Console.WriteLine("This is not a valid .bmp or .png file!");
                }
                
                inputFromFile.Close();

            }
            else
            {
                Console.WriteLine("No file found!");
            }
        }
    }
}
