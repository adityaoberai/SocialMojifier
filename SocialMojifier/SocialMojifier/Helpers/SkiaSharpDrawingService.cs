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

        public SKCanvas DrawPrediction(SKCanvas canvas, FaceRectangle box, float left, float top, float scale, string emotion, bool showEmoji)
        {
            var scaledBoxLeft = left + (scale * box.Left);
            var scaledBoxWidth = scale * box.Width;
            var scaledBoxTop = top + (scale * box.Top);
            var scaledBoxHeight = scale * box.Height;
            SKBitmap Image = GetEmojiBitmap(emotion);
            canvas.DrawBitmap(Image, new SKRect(scaledBoxLeft, scaledBoxTop, scaledBoxLeft + scaledBoxWidth, scaledBoxTop + scaledBoxHeight));
            return canvas;
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
    }
}
