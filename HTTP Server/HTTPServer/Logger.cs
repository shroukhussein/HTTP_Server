using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it

            FileStream filestream = new FileStream(@"E:\FCIS 4\1st semester\Network\Template[2021-2022]\HTTPServer\bin\Debug\log.txt", FileMode.Open);

            StreamWriter sr = new StreamWriter(filestream);

            // for each exception write its details associated with datetime 

            //Datetime:

            string datetime = DateTime.Now.ToString();
            
            sr.WriteLine("Datetime: " + datetime);

            //message:

            sr.WriteLine("Message: " + ex.Message);

            sr.WriteLine("\n");

            sr.Close();

            filestream.Close();

        }

    }
}
