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
            String searchResult = "";
            String bookIdentifier = ""; //Google Books Search format
            String requestedText = "";
            //searchResult = GoogleBooksSearch.RequestGoogleBooks();
            MessageBox.Show(searchResult, "Search results", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //GoogleBooksSearch.SaveResponse();

        }
    }
}
