using System;
using System.Threading;
using NAudio.Wave;

namespace BookSearch
{
    /// <summary>
    /// This class is for audio recording from microphone to .wav file.
    /// </summary>
    public class AudioRecording
    {
        //WaveIn - stream for writing.
        private static WaveInEvent waveIn;
        //Class for recording to the file.
        private static WaveFileWriter writer;

        /// <summary>
        /// Initializes objects for the recording the audiostream and starts recording.
        /// </summary>
        /// <param name="wavFilePath">String with path to .wav output file.</param>
        private static void InitializeRecording(object wavFilePath)
        {
            String outputFilePath = (String)wavFilePath;
            //Initialize WaveIn object
            waveIn = new WaveInEvent();
            //Default input device (if any)
            waveIn.DeviceNumber = 0;
            //Add an event handler for record available data
            waveIn.DataAvailable += waveIn_DataAvailable;
            //Format of the output .wav file - sample rate and number of channels
            waveIn.WaveFormat = new WaveFormat(8000, 1);
            //Initialize WaveFileWriter object
            writer = new WaveFileWriter(outputFilePath, waveIn.WaveFormat);
            //Starts recording
            waveIn.StartRecording();
        }

        /// <summary>
        /// Stops recording and releases all occupaied resources.
        /// </summary>
        private static void EndRecording()
        {
            if (waveIn != null)
            {
                //Clean up
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
                writer.Close();
                writer = null;
            }
        }
        /// <summary>
        /// Event handler for writing new data from input buffer.
        /// </summary>
        private static void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            //Write data to file from buffer
            writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        /// <summary>
        /// Records audio in .wav format. Suspends the main thread until the end of recording.
        /// </summary>
        /// <param name="wavFilePath">Full path to the output .wav file.</param>
        /// <param name="recordingLength">Length of the recording in milliseconds.</param>
        public static void RecordAudio(String wavFilePath, int recordingLength)
        {
            //Initialize a timer for recording length tracking
            System.Timers.Timer recordingTimer = new System.Timers.Timer();
            //Set timer interval to recordingLenght
            recordingTimer.Interval = recordingLength;
            //Create a new thread for recording
            Thread recordingThread = new Thread(new ParameterizedThreadStart(InitializeRecording));
            //Start thread for recording
            recordingThread.Start(wavFilePath);
            //Start timer
            recordingTimer.Start();

            //Using delegate as timer elapsed event handler
            //TODO: Look for a better way to realize delay here
            recordingTimer.Elapsed += delegate
            {
                //Stop timer
                recordingTimer.Stop();
            };
            //Wait till timer is elapsed
            while (recordingTimer.Enabled)
            {
                //Suspend main thread
                Thread.Sleep(0);
            }
            //End the thread for recording
            recordingThread.Join();
            //End recording
            EndRecording();
        }

        //TODO: Implement this feature
        /// <summary>
        /// Records audio in .wav format. Suspends the main thread until the end of recording. NOT IMPLEMENTED YET.
        /// </summary>
        /// <param name="wavFilePath">Full path to the .wav file.</param>
        public static void RecordAudio(String infilePath)
        {
            //Initializes recording.
            //Automatically detects the end of the recording and ends it.
            throw new NotImplementedException();
        }
    }
}
