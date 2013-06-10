using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;

namespace BookSearch
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            /*
            //Test of the audio recording and speech recognition
            String wavFileName = @"tmp.wav";
            int recordingLength = 5000; 
            
            AudioRecording.RecordAudio(wavFileName, recordingLength);

            String speechRecognitionResult = "";
            speechRecognitionResult = SpeechRecognition.GetRecognizedText(wavFileName);
            MessageBox.Show(speechRecognitionResult, "Recognition result", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            */

            //Test of the text search for books
            String bookIdentifier = "6CgNP0U7AeAC"; //Google Books Search format
            String requestedText = "the";
            //requestedText = speechRecognitionResult;
            JsonBooks.SearchResult searchResult = GoogleBooksSearch.GetResult(bookIdentifier, requestedText);
            requestedText += ""; //Breakpoint
        }
    }
}
