using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMojifier.Helpers
{
    public static class GetEmotionImage
    {
        public static StringBuilder GetImageResourceId(string emotion)
        {
            StringBuilder resId = new StringBuilder("SocialMojifier.Emojis.");
            switch (emotion)
            {
                case "Anger":
                    resId.Append("angry.png");
                    break;
                case "Contempt":
                    resId.Append("dislike.png");
                    break;
                case "Disgust":
                    resId.Append("disgust.png");
                    break;
                case "Fear":
                    resId.Append("fear.png");
                    break;
                case "Happiness":
                    resId.Append("happy.png");
                    break;
                case "Neutral":
                    resId.Append("neutral.png");
                    break;
                case "Sadness":
                    resId.Append("sad.png");
                    break;
                case "Surprise":
                    resId.Append("surprised.png");
                    break;

            }
            return resId;
        }
    }
}
