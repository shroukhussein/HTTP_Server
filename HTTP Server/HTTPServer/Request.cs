using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;

        string method;

        public string relativeURI;

        Dictionary<string, string> headerLines = new Dictionary<string, string> { };

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        string httpVersion;

        string requestString;

        string[] contentLines;

        int index;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            //throw new NotImplementedException();

            //TODO: parse the receivedRequest using the \r\n delimeter   

            string[] separators = new string[]{ "\r\n" };


            requestLines = requestString.Split(separators, StringSplitOptions.None);

            // check that there is at least 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)

            // Parse Request line

            // Load header lines into headerLines dictionary

            bool check1 = ParseRequestLine();

            bool check2 = LoadHeaderLines(requestLines, 1);

            bool check3 = ValidateBlankLine();

            if (check1 && check2 && check3)
            {
                return true;

            }
            else
            {

                return false;

            }

        }

        private bool ParseRequestLine()
        {
            //throw new NotImplementedException();

            string[] tokens = requestLines[0].Split(' ');

            method = tokens[0];

            relativeURI = tokens[1];

            httpVersion = tokens[2];

            return true;

        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines(string[] requestlines, int headerlines_start_index)
        {
            //throw new NotImplementedException();

            string[] separators = new string[] { ": " };

            while (!string.IsNullOrEmpty(requestLines[headerlines_start_index]))
            {

                string header_content = requestLines[headerlines_start_index];
                string[] data = header_content.Split(separators, StringSplitOptions.None);
                headerLines.Add(data[0], data[1]);
                headerlines_start_index++;
                index = headerlines_start_index;

            }

            return true;

        }

        private bool ValidateBlankLine()
        {
            //throw new NotImplementedException();

            if (string.IsNullOrEmpty(requestLines[index]))
            {

                return true;

            }
            else
            {

                return false;

            }
        }

    }
}
