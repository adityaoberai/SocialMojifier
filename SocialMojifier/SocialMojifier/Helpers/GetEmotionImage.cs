using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMojifier.Helpers
{
    public class GetEmotionImage
    {
        public StringBuilder GetImageResourceId(string emotion)
        {
            StringBuilder resId = new StringBuilder("");
            switch (emotion)
            {
                case "Anger":
                    resId.Append("https://raw.githubusercontent.com/adityaoberai/SocialMojifier/main/SocialMojifier/SocialMojifier/Emojis/angry.png?token=AHPSLXIRIZUJQSQKUKJ5XDLBBZPMI");
                    break;
                case "Contempt":
                    resId.Append("https://raw.githubusercontent.com/adityaoberai/SocialMojifier/main/SocialMojifier/SocialMojifier/Emojis/dislike.png?token=AHPSLXJH7QHDBZMPOYFV3UTBBZPNI");
                    break;
                case "Disgust":
                    resId.Append("https://raw.githubusercontent.com/adityaoberai/SocialMojifier/main/SocialMojifier/SocialMojifier/Emojis/disgust.png?token=AHPSLXKKFVQSONXMCP3QYY3BBZPOI");
                    break;
                case "Fear":
                    resId.Append("https://raw.githubusercontent.com/adityaoberai/SocialMojifier/main/SocialMojifier/SocialMojifier/Emojis/fear.png?token=AHPSLXOSGIDZ5X7UEMRKTCTBBZPPK");
                    break;
                case "Happiness":
                    resId.Append("https://raw.githubusercontent.com/adityaoberai/SocialMojifier/main/SocialMojifier/SocialMojifier/Emojis/happy.png?token=AHPSLXOC5TVMYNHPJO522UDBBZPH2");
                    break;
                case "Neutral":
                    resId.Append("https://raw.githubusercontent.com/adityaoberai/SocialMojifier/main/SocialMojifier/SocialMojifier/Emojis/neutral.png?token=AHPSLXOZTIHLZG66X4KQPWTBBZPQ2");
                    break;
                case "Sadness":
                    resId.Append("https://raw.githubusercontent.com/adityaoberai/SocialMojifier/main/SocialMojifier/SocialMojifier/Emojis/sad.png?token=AHPSLXN4ILZVLTWUFA2BH7TBBZPRW");
                    break;
                case "Surprise":
                    resId.Append("https://raw.githubusercontent.com/adityaoberai/SocialMojifier/main/SocialMojifier/SocialMojifier/Emojis/surprised.png?token=AHPSLXPGKJSM7XUNIMFIHB3BBZPTY");
                    break;

            }
            return resId;
        }
    }
}
