using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace BookSearch
{
    /// <summary>
    /// This class incudes methods for searching text in books via Google Books Search.
    /// </summary>
    public class GoogleBooksSearch
    {
        //Pattern for retrieving search results: {"number_of_results"+...+},{}); from Google Books response.
        //Maybe it should be @"\{"+"\"number_of_results\""+@".*?(\},\{.*?\}\);)" for {"number_of_results"+...+},{+...+});
        private static String rgxPattern = @"\{" + "\"number_of_results\"" + @".*?(\},\{\}\);)";
        //Expected response tail - ,{});
        private static String responseTail = @",{});";
        //Create Regex object once.
        private static Regex rgx = new Regex(rgxPattern);
        
        /// <summary>
        /// Forms request Uri to Google Books.
        /// </summary>
        /// <param name="bookIdentifier">String with book ID in a Google Books format.</param>
        /// <param name="requestedText">String with the text to search fo.r</param>
        /// <returns>Returns a Uri object for the request to Google Books.</returns>
        private static Uri FormRequestUri(String bookIdentifier, String requestedText)
        {
            //TODO: Change PA1 
            String requestString = "http://books.google.com/books?id=" + bookIdentifier + "&pg=PA1&q=" + requestedText + "&redir_esc=n";
            Uri requestUri = new Uri(requestString);

            return requestUri;
        }

        /// <summary>
        /// Performs GET request and transfers response to string.
        /// </summary>
        /// <param name="requestUri">Uri object with request.</param>
        /// <returns>Returns a string with the response from server.</returns>
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
            String responseFromServer = reader.ReadToEnd();
            //Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        /// <summary>
        /// Truncates the string with response to JSON with results and parses it to save as JsonBooks.SearchResult.
        /// </summary>
        /// <param name="searchResponse">String with the response from Google Books.</param>
        /// <returns>Returns a JsonBooks.SearchResult object with the search results.</returns>
        private static JsonBooks.SearchResult ParseBooksSearchResults(String searchResponse)
        {
            //Create a JsonBooks.SearchResult object for results
            JsonBooks.SearchResult parsingResult = new JsonBooks.SearchResult();
            //Find a match to result pattern in the response from Google Books
            Match rgxResult = rgx.Match(searchResponse);
            //Get a string that matches result pattern
            String processedResponse = rgxResult.ToString();
            //Try to parse it
            try
            {
                //Check if the end of this string looks as expected or not
                if (processedResponse.Substring(processedResponse.Length - responseTail.Length, responseTail.Length) == responseTail)
                {
                    //Truncate that string to JSON format string
                    processedResponse = processedResponse.Substring(0, processedResponse.Length - responseTail.Length);
                    //Parse it
                    parsingResult = JsonBooks.Parse(processedResponse);
                }
                else
                {
                    parsingResult.search_error = "Error: Unexpected response format. Server response: " + searchResponse;
                }
            }
            catch(Exception e)
            {
                parsingResult.search_error = "Error: "+ e.Message +". Server response: " + searchResponse;
            }

            return parsingResult;
        }

        /// <summary>
        /// Gives a result of the search of requested text in requested book.
        /// </summary>
        /// <param name="bookIdentifier">String with book ID in a Google Books format.</param>
        /// <param name="requestedText">String with the text to search fo.r</param>
        /// <returns>Returns a JsonBooks.SearchResult object with the search results.</returns>
        public static JsonBooks.SearchResult GetResult(String bookIdentifier, String requestedText)
        {
            //Create a JsonBooks.SearchResult object for result
            JsonBooks.SearchResult result = new JsonBooks.SearchResult();
            try
            {
                //Form a Uri object with the to Google Books
                Uri requestUri = FormRequestUri(bookIdentifier, requestedText);
                //Get a string with the response from server
                String searchResponse = RequestGoogleBooks(requestUri);
                //Parse this string and get a JsonBooks.SearchResult object with the search results
                result = ParseBooksSearchResults(searchResponse);
            }
            catch (Exception e)
            {
                result.search_error = "Error: "+ e.Message;
            }

            return result;
        }
    }
}
