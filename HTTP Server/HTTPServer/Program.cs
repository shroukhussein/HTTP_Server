using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 

            CreateRedirectionRulesFile();

            string filepath = @"E:\FCIS 4\1st semester\Network\Template[2021-2022]\HTTPServer\bin\Debug\redirectionRules.txt";

            //Start server

            // 1) Make server object on port 1000

            Server server = new Server(1000, filepath);

            // 2) Start Server

            server.StartServer();

        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt

            FileStream filestream = new FileStream(@"E:\FCIS 4\1st semester\Network\Template[2021-2022]\HTTPServer\bin\Debug\redirectionRules.txt", FileMode.OpenOrCreate);

            StreamWriter streamWriter = new StreamWriter(filestream);

            // each line in the file specify a redirection rule

            // example: "aboutus.html,aboutus2.html"

            streamWriter.WriteLine(@"aboutus.html,aboutus2.html");

            // means that when making request to aboutus.html, it redirects me to aboutus2

            streamWriter.Close();

        }

    }
}
