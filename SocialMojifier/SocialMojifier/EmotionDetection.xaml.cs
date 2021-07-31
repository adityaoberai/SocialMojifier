using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SocialMojifier.Helpers;
using System.IO;
using Plugin.Media.Abstractions;
using System.Net.Http;
using System.Net.Http.Headers;
using SocialMojifier.Models;
using System.Reflection;
using System.Drawing.Imaging;

namespace SocialMojifier
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmotionDetection : ContentPage
    {
        public MediaFile capture;
        public EmotionDetection(MediaFile image, string filePath)
        {
            InitializeComponent();
            capture = image;
            DetectEmotion();
        }

        public async void DetectEmotion()
        {
            FaceAPICredentials credentials = new FaceAPICredentials();
            IFaceClient client = new FaceClient(new ApiKeyServiceClientCredentials(credentials.APIKey)) { Endpoint = credentials.Endpoint };
            var face = await FaceAPIDetection(client);
            if (face == null)
            {
                await DisplayAlert("Response", "Face Not Detected", "Ok");
                await Navigation.PopModalAsync();
            }
            var detectedFace = new SMDetectedFace
            {
                FaceRectangle = face.FaceRectangle,
            };
            var getEmotionImage = new GetEmotionImage();
            
            detectedFace.PredominantEmotion = FindPredominantEmotion(face.FaceAttributes.Emotion);
            /*
            System.Drawing.Image backImg = System.Drawing.Image.FromStream(capture.GetStream());

            System.Drawing.Image mrkImg = System.Drawing.Image.FromFile(getEmotionImage.GetImageResourceId(detectedFace.PredominantEmotion).ToString());
            Graphics g = Graphics.FromImage(backImg);
            g.DrawImage(mrkImg, face.FaceRectangle.Left, face.FaceRectangle.Top, face.FaceRectangle.Width, face.FaceRectangle.Height);
            */
            EmojifiedImage.Source = getEmotionImage.GetImageResourceId(detectedFace.PredominantEmotion).ToString();

        }

        public Stream ToStream(System.Drawing.Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        public async Task<DetectedFace> FaceAPIDetection(IFaceClient client)
        {
            var faceAPIResponseList = await client.Face.DetectWithStreamAsync(capture.GetStream(), 
                returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Emotion });

            return faceAPIResponseList.FirstOrDefault();
        }

        public string FindPredominantEmotion(Emotion emotion)
        {
            double max = 0;
            PropertyInfo prop = null;

            var emotionsValues = typeof(Emotion).GetProperties();
            foreach (PropertyInfo property in emotionsValues)
            {
                var value = (double)property.GetValue(emotion);

                if (value > max)
                {
                    max = value;
                    prop = property;
                }
            }
            return prop.Name.ToString();
        }
    }
}