using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BookSearch
{
    /// <summary>
    /// This class is used for parsing the response from Google Speech API.
    /// </summary>
    public class JsonSpeech
    {
        [DataContract]
        public class RecognizedItem
        {
            [DataMember]
            public String utterance;

            [DataMember]
            public float confidence;
        }

        [DataContract]
        public  class RecognitionResult
        {
            [DataMember]
             public String status;

            [DataMember]
            public String id;

            [DataMember]
            public RecognizedItem[] hypotheses;
        }

        public static RecognitionResult Parse(String toParse)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RecognitionResult));

            MemoryStream stream1 = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(toParse));
            
            RecognitionResult result= (RecognitionResult)ser.ReadObject(stream1);
            return result;
        }
    }
    /// <summary>
    /// This class is used for parsing the response from Google Books Search.
    /// </summary>
    public class JsonBooks
    {
        [DataContract]
        public class SearchItem
        {
            [DataMember]
            public String page_id;

            [DataMember]
            public String page_number;

            [DataMember]
            public String snippet_text;

            [DataMember]
            public String page_url;
        }

        [DataContract]
        public class SearchSpell 
        {
            [DataMember]
            public String correct_spell;

            [DataMember]
            public String spell_corrected_url;
        }

        [DataContract]
        public class SearchResult
        {
            [DataMember]
            public int number_of_results;

            [DataMember]
            public SearchSpell[] spellresults;
            
            [DataMember]
            public SearchItem[] search_results;

            [DataMember]
            public String search_query_escaped;

            [DataMember]
            public String search_error;
        }

        public static SearchResult Parse(String toParse)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SearchResult));

            MemoryStream stream1 = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(toParse));

            SearchResult result = (SearchResult)ser.ReadObject(stream1);
            return result;
        }
    }
}
