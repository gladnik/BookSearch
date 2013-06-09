using System;
using System.IO;
using System.Linq;
using System.Net;

namespace BookSearch
{
    public class GoogleBooksSearch
    {
        /*
        public GoogleBooksSearch(String bookIdentifier, String requestedText)
        {
            Uri requestUri = FormRequestUri(bookIdentifier, requestedText);
            String searchResponse = RequestGoogleBooks(requestUri);
            SaveResponse(searchResponse);
        }
        ~GoogleBooksSearch() { }
        */

        public int numberOfResults = 0;
        
        private static Uri FormRequestUri(String bookIdentifier, String requestedText)
        {
            //TODO: Change PA1 
            String requestString = "http://books.google.com/books?id=" + bookIdentifier + "&pg=PA1&q=" + requestedText + "&redir_esc=n";
            Uri requestUri = new Uri(requestString);

            return requestUri;
        }

        private static String RequestGoogleBooks(Uri requestUri)
        {
            //Create request
            WebRequest request = WebRequest.Create(requestUri);
            request.Method = "GET";
            //Get response
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            //Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        //TODO: Parse without saving
        private static void SaveResponse(String response)
        {
            //Path to file with response.
            string responseFileName = @"googleBooksResponse.txt";
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(responseFileName))
            {
                sw.Write(response);
            }
        }

        //TODO: Get results
    }
}
