using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using SkiaSharp;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SocialMojifier.Helpers
{
    public class SkiaSharpDrawingService
    {
        public void ClearCanvas(SKImageInfo info, SKCanvas canvas)
        {
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White
            };

            canvas.DrawRect(info.Rect, paint);
        }

        public async void DrawPrediction(SKCanvas canvas, FaceRectangle box, float left, float top, float scale, string emotion, bool showEmoji)
        {
            var scaledBoxLeft = left + (scale * box.Left);
            var scaledBoxWidth = scale * box.Width;
            var scaledBoxTop = top + (scale * box.Top);
            var scaledBoxHeight = scale * box.Height;
            SKBitmap Image = GetEmojiBitmap(emotion);
            canvas.DrawBitmap(Image, new SKRect(scaledBoxLeft, scaledBoxTop, scaledBoxLeft + scaledBoxWidth, scaledBoxTop + scaledBoxHeight));
        }


        public void DrawEmotiocon(SKImageInfo info, SKCanvas canvas, string emotion)
        {

            SKBitmap Image = GetEmojiBitmap(emotion);
            var scale = Math.Min(info.Width / (float)Image.Width, info.Height / (float)Image.Height);

            var scaleHeight = scale * Image.Height;
            var scaleWidth = scale * Image.Width;

            var top = (info.Height - scaleHeight) / 2;
            var left = (info.Width - scaleWidth) / 2;

            canvas.DrawBitmap(Image, new SKRect(left, top, left + scaleWidth, top + scaleHeight));
        }

        public SKBitmap GetEmojiBitmap(string emotion)
        {
            string resourceID = GetEmotionImage.GetImageResourceId(emotion).ToString();
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            SKBitmap resourceBitmap = null;
            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                resourceBitmap = SKBitmap.Decode(stream);
            }
            return resourceBitmap;
        }

        private SKPath CreateBoxPath(float startLeft, float startTop, float scaledBoxWidth, float scaledBoxHeight)
        {
            var path = new SKPath();
            path.MoveTo(startLeft, startTop);

            path.LineTo(startLeft + scaledBoxWidth, startTop);
            path.LineTo(startLeft + scaledBoxWidth, startTop + scaledBoxHeight);
            path.LineTo(startLeft, startTop + scaledBoxHeight);
            path.LineTo(startLeft, startTop);

            return path;
        }
    }
}
