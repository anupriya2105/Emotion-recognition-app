using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using MetriCam;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConsoleApp1
{
    class Program
    {
        static EmotionProfile ep;
        static MusicController mc;

        static string dominantEmotion;
        static double dominantEmotionValue;

        static string songURI;

        static string subscriptionKey;
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect";

        static void Main()
        {
            // Get subscription key from user
            Console.Write("Enter Azure API subcription key: ");
            subscriptionKey = Console.ReadLine();

            //Capture the Image with webcam
            Console.WriteLine("\nDetecting face:" + "(WebCam will capture the picture)");
           
            Image image = CaptureImage();

            // Execute the REST API call.
            MakeAnalysisRequest(image);

            Console.WriteLine("\nPlease wait a moment for the results to appear. Then, press Enter to exit...\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Gets the analysis of the specified image file by using the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file.</param>
        static async void MakeAnalysisRequest(Image image)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(image);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the complete JSON response.
                Console.WriteLine("\nComplete API Response:\n");
                Console.WriteLine(JsonPrettyPrint(contentString));

                // Creates EmotionProfile using JSON raw data
                dynamic reply = JArray.Parse(contentString);
                ep = new EmotionProfile(reply);

                // Display the emotion portion of the JSON response
                Console.WriteLine("\nEmotion values:\n");
                Console.WriteLine(ep.toString());

                // Get dominant emotion
                dominantEmotion = ep.getDominantEmotionKey();
                dominantEmotionValue = ep.getDominantEmotionValue();

                // Display calculated dominant emotion
                Console.WriteLine("\nCalculated dominant emotion: " + dominantEmotion);
                Console.WriteLine("Emotion value: " + dominantEmotionValue);

                // Playlist configuration
                mc = new MusicController();
                songURI = mc.getMusicURI(dominantEmotion);

                // Plays music
                //Console.WriteLine("\n...Now playing: \"" + playlist + "\" on Spotify Music.");
                startMusic(songURI);
                if(dominantEmotion == "happiness") {
                    Console.Write("Seems your day went great!");
                    MessageBox.Show("Time to party.Now you will be redirected to spotify list of happy songs");
                    Process.Start("https://open.spotify.com/user/redmusiccompany/playlist/0deORnapZgrxFY4nsKr9JA");
                }
                else if(dominantEmotion== "neutral")
                {
                    Console.Write("Shift your gear up and smile");
                    MessageBox.Show("“A positive attitude causes a chain reaction of positive thoughts, events and outcomes. It is a catalyst and it sparks extraordinary results.” —Wade Boggs.Now you will be redirected to spotify list of neutral mood songs");
                    Process.Start("https://open.spotify.com/user/digster.fr/playlist/1QxdL1fPW4X4eex4CrFryW");
                }
                else if (dominantEmotion == "anger")
                {
                    Console.Write("Anger reduces your capability to think");
                    MessageBox.Show("For every minute you remain angry, you give up sixty seconds of peace of mind. Ralph Waldo Emerson.Now you will be redirected to spotify list of angry mood songs");
                    Process.Start("https://open.spotify.com/user/popsugarsmart/playlist/0KPEhXA3O9jHFtpd1Ix5OB");
                }
                else if (dominantEmotion == "sadness")
                {
                    Console.Write("Cheer up! Smile");
                    MessageBox.Show("Reject your sense of injury and the injury itself disappears.Now you will be redirected to spotify list of sad songs");
                    Process.Start("https://open.spotify.com/user/sanik007/playlist/7ABD15iASBIpPP5uJ5awvq");
                  
                }
                else if (dominantEmotion == "contempt")
                {
                    Console.Write("");
                    MessageBox.Show("");
                    Process.Start("https://open.spotify.com/user/spotify/playlist/4bsCGnGIhrR1xIcXslAMcH");
                }
                else if (dominantEmotion == "surprise")
                {
                    Console.Write("");
                    MessageBox.Show("The best things in life happens unexpectedly, so LET LIFE SURPRISE YOU.Now you will be redirected to spotify list of surprise songs");
                    Process.Start("https://open.spotify.com/user/spotify/playlist/4bsCGnGIhrR1xIcXslAMcH");
                }
                else if (dominantEmotion == "fear")
                {
                    Console.Write("");
                    MessageBox.Show("Don't let fear or insecurity stop you from trying new things. Believe in yourself. Do what you love. And most importantly, be kind to others, even if you don't like them.Now you will be redirected to spotify list of songs");
                    Process.Start("https://open.spotify.com/user/ardengard/playlist/5Ryp1G66sjS1S1ZYLTmrQw");
                }
                else if (dominantEmotion == "disgust")
                {
                    Console.Write("Feeling disgust, why?????");
                    MessageBox.Show("Don't feel disgusted");
                    Process.Start("https://open.spotify.com/user/rancidswan99/playlist/3k2E7g0Cs7PFaSvv5SXOqM");
                }
                
            }
        }

        static void startMusic(string URI)
        {
            System.Diagnostics.Process.Start(URI);
        }

        /// <summary>
        /// Captures an image using webcam. (Based on metricam library)
        /// </summary>
        /// <returns>Image object as captured</returns>
        static Image CaptureImage()
        {
            WebCam camera = new WebCam();
            camera.Connect();
            Image image = camera.GetBitmap();
            camera.Disconnect();
            return image;
        }

        /// <summary>
        /// Returns the contents of the specified image object as a byte array.
        /// </summary>
        /// <param name="img">The image object captured with webcam.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Formats the given JSON string using indented formatting
        /// </summary>
        /// <param name="json">The raw JSON string to format.</param>
        /// <returns>The formatted JSON string.</returns>
        static string JsonPrettyPrint(string json)
        {
            JToken parsedJson = JToken.Parse(json);
            var beautifulText = parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);

            return (string)beautifulText;
        }
    }
}