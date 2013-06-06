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
        String wavFileName = @"tmp.wav";
        int recordingLength = 5000;

        public MainForm()
        {
            InitializeComponent();

            AudioRecording.RecordAudio(wavFileName, recordingLength);

            String testSpeechRecognition = "";
            testSpeechRecognition = SpeechRecognition.GetRecognizedText(wavFileName);
            testSpeechRecognition += "";


        }
    }
}
