using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
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
            var emotions = await FaceAPIDetection(client);
            if(emotions != null) await DisplayAlert("Response", emotions.ToString(), "Ok");
            else await DisplayAlert("Response", "", "Ok");
        }

        public async Task<DetectedFace> FaceAPIDetection(IFaceClient client)
        {
            var faceAPIResponseList = await client.Face.DetectWithStreamAsync(capture.GetStream(), 
                returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Emotion });

            return faceAPIResponseList.FirstOrDefault();
        }
    }
}