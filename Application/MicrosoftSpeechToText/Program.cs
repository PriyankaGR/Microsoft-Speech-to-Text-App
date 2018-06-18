using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MicrosoftSpeechToText
{
    class Program
    {
        static SpeechFactory factory = null;
        static void Main(string[] args)
        {
            factory= SpeechFactory.FromSubscription(ConfigurationManager.AppSettings["SubscriptionKey"], ConfigurationManager.AppSettings["ServiceRegion"]);
            Console.WriteLine("1: Mic Input \n 2: WAV input");
            Console.Write("Chosse an option:");
            int selectedOption = Convert.ToInt16(Console.ReadLine());
            switch (selectedOption)
            {
                case 1:
                    ContinuousRecognitionAsyncMic().Wait();
                    break;
                case 2:
                    ContinuousRecognitionAsyncWAV().Wait();
                    break;
            }
         }


        // Speech recognition with events
        public static async Task ContinuousRecognitionAsyncWAV()
        {
            // Creates a speech recognizer using WAV file as audio input.
            Console.Write("Enter a fileName:");
            string fileName = Console.ReadLine();
            if (System.IO.File.Exists(fileName))
            {
                //C:/Users/LENOVO/Documents/male.wav
                using (var recognizer = factory.CreateIntentRecognizerWithFileInput(fileName))
                {
                    //Subscribes to events.
                    recognizer.IntermediateResultReceived += (s, e) =>
                    {
                        Console.WriteLine($"\n    Partial result: {e.Result.Text}.");
                    };

                    recognizer.FinalResultReceived += (s, e) =>
                    {
                        if (e.Result.RecognitionStatus == RecognitionStatus.Recognized)
                        {
                            Console.WriteLine($"\n    Final result: Status: {e.Result.RecognitionStatus.ToString()}, Text: {e.Result.Text}.");
                        }
                        else
                        {
                            Console.WriteLine($"\n    Final result: Status: {e.Result.RecognitionStatus.ToString()}, FailureReason: {e.Result.RecognitionFailureReason}.");
                        }
                    };

                    recognizer.RecognitionErrorRaised += (s, e) =>
                    {
                        Console.WriteLine($"\n    An error occurred. Status: {e.Status.ToString()}, FailureReason: {e.FailureReason}");
                    };

                    recognizer.OnSessionEvent += (s, e) =>
                    {
                        Console.WriteLine($"\n    Session event. Event: {e.EventType.ToString()}.");
                    };

                    // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                    Console.WriteLine("Say something...");
                    await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                    Console.WriteLine("Press any key to stop");
                    Console.ReadKey();

                    await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                }
            }

            else
            {
                Console.WriteLine("Selected file doesnt exist");
            }
        }

        public static async Task ContinuousRecognitionAsyncMic()
        {
           

            // Creates a speech recognizer using microphone as audio input.

            using (var recognizer = factory.CreateSpeechRecognizer())
            {
                //Subscribes to events.
                recognizer.IntermediateResultReceived += (s, e) =>
                {
                    Console.WriteLine($"\n    Partial result: {e.Result.Text}.");
                };

                recognizer.FinalResultReceived += (s, e) =>
                {
                    if (e.Result.RecognitionStatus == RecognitionStatus.Recognized)
                    {
                        Console.WriteLine($"\n    Final result: Status: {e.Result.RecognitionStatus.ToString()}, Text: {e.Result.Text}.");
                    }
                    else
                    {
                        Console.WriteLine($"\n    Final result: Status: {e.Result.RecognitionStatus.ToString()}, FailureReason: {e.Result.RecognitionFailureReason}.");
                    }
                };

                recognizer.RecognitionErrorRaised += (s, e) =>
                {
                    Console.WriteLine($"\n    An error occurred. Status: {e.Status.ToString()}, FailureReason: {e.FailureReason}");
                };

                recognizer.OnSessionEvent += (s, e) =>
                {
                    Console.WriteLine($"\n    Session event. Event: {e.EventType.ToString()}.");
                };

                // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                Console.WriteLine("Say something...");
                await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                Console.WriteLine("Press any key to stop");
                Console.ReadKey();

                await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

            }
        }
    }
}
