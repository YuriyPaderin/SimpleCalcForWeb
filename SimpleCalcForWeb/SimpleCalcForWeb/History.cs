using System;
using System.IO;
using System.Collections.Generic;

namespace SimpleCalcForWeb
{ 
    public class History
    {
        public static List<string> GetHistory()
        {
            using (var textFileReader = new StreamReader("history.txt"))
            {
                var noteCollection = new List<string>();

                while (true)
                {
                    var buffer = textFileReader.ReadLine();
                    if (buffer == null || buffer.Length == 0)
                        break;

                    noteCollection.Add(buffer);
                }

                noteCollection.Reverse();

                return noteCollection;
            }
        }

        public static void PutRecord(string record)
        {
            using (var textFileWriter = new StreamWriter("history.txt", true))
            {
                textFileWriter.WriteLine(record);
            }
        }
    }
}
