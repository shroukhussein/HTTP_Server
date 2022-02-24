using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {

        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {

            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it

            this.LoadRedirectionRules(redirectionMatrixPath);

            //TODO: initialize this.serverSocket

            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint hostEndPoint = new IPEndPoint(IPAddress.Any, portNumber);

            this.serverSocket.Bind(hostEndPoint);

        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.

            this.serverSocket.Listen(100);

            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"

            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.

                Socket clientSocket = this.serverSocket.Accept();

                Console.WriteLine("Connection accepted !");

                Thread newthread = new Thread(new ParameterizedThreadStart(HandleConnection));

                newthread.Start(clientSocket);

            }
        }

        public void HandleConnection(object obj)
        {

            // TODO: Create client socket 

            Socket clientSock = (Socket)obj;

            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period

            clientSock.ReceiveTimeout = 0;

            // TODO: receive requests in while true until remote client closes the socket.

            while (true)
            {
                try
                {
                    // TODO: Receive request

                    byte[] requestByteArr = new byte[1048576];

                   int receivedLen = clientSock.Receive(requestByteArr);

                    string requestString = Encoding.ASCII.GetString(requestByteArr);

                    // TODO: break the while loop if receivedLen == 0

                    if(receivedLen == 0)
                    {

                        break;

                    }

                    // TODO: Create a Request object using received request string

                    Request clientrequest = new Request(requestString);

                    // TODO: Call HandleRequest Method that returns the response

                    Response serverresponse = HandleRequest(clientrequest);

                    // TODO: Send Response back to client

                    String responseString = serverresponse.ResponseString;

                    byte[] ResponseByteArr = Encoding.ASCII.GetBytes(responseString);

                    clientSock.Send(ResponseByteArr);

                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class

                    Logger.LogException(ex);

                }
            }

            // TODO: close client socket

            clientSock.Close();

        }

        Response HandleRequest(Request request)
        {
           // throw new NotImplementedException();

            StatusCode sc;
            string content;
            string physical_path;
            String location;
            Response response;

            try
            {
                //TODO: check for bad request 

                if (!request.ParseRequest())
                {

                    //Do something...

                    sc = (StatusCode)400;

                    physical_path = Configuration.RootPath + "\\" + "BadRequest.html";

                    content = File.ReadAllText(physical_path);

                }

                //TODO: map the relativeURI in request to get the physical path of the resource.

                string[] path = request.relativeURI.Split('/');

                physical_path = Configuration.RootPath + "\\" + path[1];

                //TODO: check for redirect

                if (GetRedirectionPagePathIFExist(path[1]) != string.Empty)
                {
                    sc = (StatusCode)301;

                    string new_path = GetRedirectionPagePathIFExist(path[1]);

                    physical_path = Configuration.RootPath + "\\" + new_path;

                    content = File.ReadAllText(physical_path);

                    location = "http://localhost:1000/" + new_path;

                    response = new Response(sc, "text/html", content, location);

                    return response;

                }

                

                //TODO: check file exists

                if (File.Exists(physical_path) == false)
                {

                    sc = (StatusCode)404;

                    physical_path = Configuration.RootPath + '\\' + "NotFound.html";

                    content = File.ReadAllText(physical_path);

                }

                //TODO: read the physical file
                else
                {

                    sc = (StatusCode)200;

                    content = LoadDefaultPage(path[1]);

                }

                // Create OK response

                response = new Response(sc, "text/html", content, physical_path);

                return response;

            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class

                Logger.LogException(ex);

                // TODO: in case of exception, return Internal Server Error. 

                sc = (StatusCode)500;

                physical_path = Configuration.RootPath + '\\' + "InternalError.html";

                content = File.ReadAllText(physical_path);

                response = new Response(sc, "text/html", content, physical_path);

                return response;

            }

        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            
            for(int i=0;i< Configuration.RedirectionRules.Count; i++)
            {

                if(relativePath == Configuration.RedirectionRules.Keys.ElementAt(i).ToString())
                {

                    String redirected_path = Configuration.RedirectionRules.Values.ElementAt(i).ToString();
                    //String physical_path = Configuration.RootPath + '\\' + redirected_path;
                    return redirected_path;

                }

            }

            return string.Empty;

        }

        private string LoadDefaultPage(string defaultPageName)
        {

            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);

            // TODO: check if filepath not exist log exception using Logger class and return empty string

            String content = "";

            try
            {
                if (File.Exists(filePath))
                {

                    content = File.ReadAllText(filePath);

                }

            }catch(Exception ex)
            {

                Logger.LogException(ex);

            }

            // else read file and return its content

            return content;

        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 

                FileStream fs = new FileStream(filePath, FileMode.Open);

                StreamReader sr = new StreamReader(fs);

                // then fill Configuration.RedirectionRules dictionary 

                while (sr.Peek() != -1) {

                    string l = sr.ReadLine();

                    string[] data = l.Split(',');

                    if(data[0] != "")
                    {

                        Configuration.RedirectionRules.Add(data[0], data[1]);

                    }
                    else
                    {

                        break;

                    }

                }

                fs.Close();

            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class

                Logger.LogException(ex);

                Environment.Exit(1);

            }
        }
    }
}
