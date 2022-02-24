using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;

        public string ResponseString
        {
            get
            {

                return responseString;

            }
        }

        StatusCode code;

        List<string> headerLines = new List<string>();

        public Response(StatusCode code, string contentType, string content, string redirectionPath)
        {
            //throw new NotImplementedException();

            // TODO: Add headlines (Content-Type, Content-Length, Date, [location if there is redirection])

            headerLines.Add(contentType);

            headerLines.Add(content.Length.ToString());

            headerLines.Add(DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'EST' "));

            string statusCode = GetStatusLine(code);

            if(code == StatusCode.Redirect)
            {

                headerLines.Add(redirectionPath);

                responseString = statusCode + "\r\n" +
                    "Content-Type: " + headerLines[0] + "\r\n" +
                    "Content-Length: " + headerLines[1] + "\r\n" +
                    "Date: " + headerLines[2] + "\r\n" +
                    "Location: " + headerLines[3] + "\r\n" +
                    "\r\n" +
                    "Content: " + content;

            }

            // TODO: Create the request string

            responseString = statusCode + "\r\n" +
                    "Content-Type: " + headerLines[0] + "\r\n" +
                    "Content-Length: " + headerLines[1] + "\r\n" +
                    "Date: " + headerLines[2] + "\r\n" +
                    "\r\n" +
                    "Content: " + content;

        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it

            string statusLine = string.Empty;

            if (code == StatusCode.OK)
            {
                statusLine = "HTTP/1.1" + " " + code + " " + "OK";

            }
            else if (code == StatusCode.Redirect)
            {

                statusLine = "HTTP/1.1" + " " + code + " " + "Redirect";

            }

            else if (code == StatusCode.BadRequest)
            {

                statusLine = "HTTP/1.1" + " " + code + " " + "Bad Request";


            }

            else if (code == StatusCode.InternalServerError)
            {

                statusLine = "HTTP/1.1" + " " + code + " " + "Internal Server Error";

            }

            else
            {

                statusLine = "HTTP/1.1" + " " + code + " " + "Not found";

            }

            return statusLine;

        }
    }
}

