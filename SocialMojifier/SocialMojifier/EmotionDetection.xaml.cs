using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SocialMojifier.Helpers;
using System.IO;
using Plugin.Media.Abstractions;
using SocialMojifier.Models;
using SkiaSharp;
using Xamarin.Essentials;

namespace SocialMojifier
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmotionDetection : ContentPage
    {
        public MediaFile capture;
        private SKBitmap Image;
        private SkiaSharpDrawingService skiaDrawingService;
        private bool drawemoji = true;
        private Lazy<List<SMDetectedFace>> detectedFaces = new Lazy<List<SMDetectedFace>>();
        public SKCanvas canvas;
        public SKSurface surface;
        public MemoryStream imageStream;
        public EmotionDetection(MediaFile image, string filePath)
        {
            InitializeComponent();
            skiaDrawingService = new SkiaSharpDrawingService();
            capture = image;
            DetectEmotion();
            SetImageInImageView(capture);
            imageStream = new MemoryStream();
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
            detectedFaces.Value.Add(new SMDetectedFace
            {
                FaceRectangle = face.FaceRectangle,
                PredominantEmotion = FindPredominantEmotion(face.FaceAttributes.Emotion)
            });
            capturedImage.InvalidateSurface();
        }

        private void CapturedImage_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            surface = args.Surface;
            canvas = args.Surface.Canvas;
            skiaDrawingService.ClearCanvas(info, canvas);
            if (Image != null)
            {
                try
                {
                    var scale = Math.Min(info.Width / (float)Image.Width, info.Height / (float)Image.Height);

                    var scaleHeight = scale * Image.Height;
                    var scaleWidth = scale * Image.Width;

                    var top = (info.Height - scaleHeight) / 2;
                    var left = (info.Width - scaleWidth) / 2;

                    canvas.DrawBitmap(Image, new SKRect(left, top, left + scaleWidth, top + scaleHeight));

                    canvas = skiaDrawingService.DrawPrediction(canvas, detectedFaces.Value.FirstOrDefault().FaceRectangle, left, top, scale, detectedFaces.Value.FirstOrDefault().PredominantEmotion, drawemoji);

                    var m_skImage = surface.Snapshot();
                    SKRectI rec = new SKRectI((int)left, (int)top, (int)(left + scaleWidth), (int)(top + scaleHeight));
                    m_skImage = m_skImage.Subset(rec);
                    var data = m_skImage.Encode(SKEncodedImageFormat.Png, 80);
                    var ResultImageStream = data.AsStream();
                    ResultImageStream.CopyTo(imageStream);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void SetImageInImageView(MediaFile image)
        {
            Image = SKBitmap.Decode(image.GetStreamWithImageRotatedForExternalStorage());
            capturedImage.InvalidateSurface();
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

        private async void ShareButton_Clicked(object sender, EventArgs e)
        {
            var data = imageStream;
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            documentsPath = Path.Combine(documentsPath, "SocialMojifier");
            Directory.CreateDirectory(documentsPath);
            string filePath = Path.Combine(documentsPath, $"result-{DateTime.Now.ToString()}.jpg");
            byte[] bArray = new byte[data.Length];
            FileStream fs = new FileStream(filePath, FileMode.CreateNew);
            bArray = imageStream.ToArray();
            fs.Write(bArray, 0, bArray.Length);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Emojified Image",
                File = new ShareFile(filePath)
            }) ;
        }
    }
}