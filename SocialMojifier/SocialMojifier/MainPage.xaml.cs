﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SocialMojifier
{
    public partial class MainPage : ContentPage
    {
        public MediaFile capturedImage;
        public string filePath;
        public MainPage()
        {
            InitializeComponent();

            Capture.Source = null;
            GetEmotion.IsVisible = false;
            GetEmotion.IsEnabled = false;
        }

        private async void CaptureImage_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "SocialMojifier",
                Name = "capture.jpg",
                SaveToAlbum = true,
                PhotoSize = PhotoSize.Medium,
                DefaultCamera = CameraDevice.Front
            });

            if (file == null)
            {
                return;
            }

            capturedImage = file;
            filePath = file.Path;

            Capture.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
            GetEmotion.IsVisible = true;
            GetEmotion.IsEnabled = true;
        }

        private void GetEmotion_Clicked(object sender, EventArgs e)
        {
            if (Capture.Source != null)
            {
                Navigation.PushModalAsync(new EmotionDetection(capturedImage, filePath));
            }
        }
    }
}
