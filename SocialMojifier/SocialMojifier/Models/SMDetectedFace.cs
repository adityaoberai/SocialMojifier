using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMojifier.Models
{
    public class SMDetectedFace : DetectedFace
    {
        public string PredominantEmotion { get; set; }
    }
}
