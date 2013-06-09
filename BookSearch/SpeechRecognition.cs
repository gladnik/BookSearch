using System;
using System.IO;
using System.Linq;
using System.Net;
using CUETools.Codecs;
using CUETools.Codecs.FLAKE;

namespace BookSearch
{
    /// <summary>
    /// This class incudes methods for converting .wav files to text via Google Speech API.
    /// </summary>
    public class SpeechRecognition
    {
        //TODO: Change it
        public static String flacFileName = @"tmp.flac";
        public static double confidenceLowerBound = 0.5;

        /// <summary>
        /// Converts .wav to .flac and returns sample rate via CUETools.
        /// </summary>
        /// <param name="wavFilePath">Full path to the .wav file.</param>
        /// <param name="flacFilePath">Full path to the .flac file.</param>
        /// <returns>Returns sample rate of the .flac file.</returns>
        public static int ConvertWavToFlac(String wavFilePath, String flacFilePath)
        {
            int sampleRate = 0;

            IAudioSource audioSource = new WAVReader(wavFilePath, null);
            AudioBuffer buff = new AudioBuffer(audioSource, 0x10000);

            FlakeWriter flakewriter = new FlakeWriter(flacFilePath, audioSource.PCM);
            sampleRate = audioSource.PCM.SampleRate;

            FlakeWriter audioDest = flakewriter;
            while (audioSource.Read(buff, -1) != 0)
            {
                audioDest.Write(buff);
            }
            audioDest.Close(); //TODO: Check - maybe it should be flakewriter.Close();

            audioDest.Close();

            return sampleRate;
        }

        /// <summary>
        /// Sends .flac file to Google Speech and returns result in JSON format.
        /// </summary>
        /// <param name="flacFilePath">Full path to the .flac file.</param>
        /// <param name="sampleRate">Sample rate of the .flac file.</param>
        /// <returns>Returns the result of request to the Google Speech API in JSON format.</returns>
        public static String RequestGoogleSpeech(String flacFilePath, int sampleRate)
        {
            //TODO: Add language selector
            //TODO: Fix exception that appears here
            WebRequest request = WebRequest.Create("https://www.google.com/speech-api/v1/recognize?xjerr=1&client=chromium&lang=en-EN");
            request.Method = "POST";

            byte[] byteArray = File.ReadAllBytes(flacFilePath);

            //Set the ContentType property of the WebRequest.
            request.ContentType = "audio/x-flac; rate=" + sampleRate; //"8000 or 16000"        
            request.ContentLength = byteArray.Length;

            //Get the request stream.
            Stream dataStream = request.GetRequestStream();
            //Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);

            dataStream.Close();

            //Get the response.
            WebResponse response = request.GetResponse();

            dataStream = response.GetResponseStream();
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

        /// <summary>
        /// Parses JSON string with response from Google Speech API.
        /// </summary>
        /// <param name="requestResult">String with the result of request to the Google Speech API in JSON format.</param>
        /// <returns>Returns the recognized text if any and if the confidence is higher than confidenceLowerBound. Otherwise returns error.</returns>
        public static String ParseJson(String requestResult)
        {
            /*
            Что означают поля ответа:
            поле status = 0 — запись успешно распознана
            поле status = 5 — запись не распознана
            поле id — это уникальный идентификатор запроса
            поле hypotheses — результат распознования, в нем 2 подполя:
            utterance — распознанная фраза
            confidence — достоверность распознавания
            */
            String parsingResult = "";
            Json.RecognitionResult result = Json.Parse(requestResult);
            if (result.hypotheses.Length > 0)
            {
                Json.RecognizedItem item = result.hypotheses.First();
                if (item.confidence > confidenceLowerBound)
                {
                    parsingResult = item.utterance;
                }
                else
                {
                    parsingResult = "Error: Low confidence";
                }
            }
            else
            {
                parsingResult = "Error: No recognized text";
            }
            return parsingResult;
        }

        /// <summary>
        /// Recognizes text from .wav file using Google Speech API.
        /// </summary>
        /// <param name="wavFilePath">Full path to the .wav file.</param>
        /// <returns>Returns recognized text.</returns>
        public static String GetRecognizedText(String wavFilePath)
        {
            String recognitionResult = "";

            try
            {
                String flacFilePath = flacFileName;

                int sampleRate = ConvertWavToFlac(wavFilePath, flacFilePath);

                String requestResult = RequestGoogleSpeech(flacFilePath, sampleRate);

                recognitionResult = ParseJson(requestResult);
            }
            catch (Exception e)
            {
                recognitionResult = e.Message;
            }

            return recognitionResult;
        }
    }
}
