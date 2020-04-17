using System;
namespace LocalFaceApiDemo.Models
{
    public class FaceInfo
    {
        public string FaceId { get; set; }
        public FaceRectangle FaceRectangle { get; set; }
        public FaceAttributes FaceAttributes { get; set; }

        public string EmotionString
        {
            get
            {
                if (FaceAttributes != null)
                {
                    if (FaceAttributes.Emotion != null)
                    {
                        var emotion = GetMaxEmotion(FaceAttributes.Emotion);
                        return $"Emotion: {emotion}";
                    }
                }

                return "N/A";
            }
        }

        public string MaxEmotion { get; set; }

        public FaceInfo()
        {
            FaceAttributes = new FaceAttributes()
            {
                Gender = "N/A",
                Glasses = "N/A",
                Smile = -1,
                Age = "N/A",
                Emotion = null,
            };
        }

        private string GetMaxEmotion(Emotion emotion)
        {
            var maxScore = emotion.Anger;
            var maxEmotion = "Anger";

            if (emotion.Contempt > maxScore)
            {
                maxScore = emotion.Contempt;
                maxEmotion = "Contempt";
            }

            if (emotion.Disgust > maxScore)
            {
                maxScore = emotion.Disgust;
                maxEmotion = "Disgust";
            }

            if (emotion.Fear > maxScore)
            {
                maxScore = emotion.Fear;
                maxEmotion = "Fear";
            }

            if (emotion.Happiness > maxScore)
            {
                maxScore = emotion.Happiness;
                maxEmotion = "Happiness";
            }

            if (emotion.Neutral > maxScore)
            {
                maxScore = emotion.Neutral;
                maxEmotion = "Neutral";
            }

            if (emotion.Sadness > maxScore)
            {
                maxScore = emotion.Sadness;
                maxEmotion = "Sadness";
            }

            if (emotion.Surprise > maxScore)
            {
                maxScore = emotion.Surprise;
                maxEmotion = "Surprise";
            }

            MaxEmotion = maxEmotion;

            return $"{maxEmotion} ({maxScore * 100.0} %)";
        }
    }

    public class FaceRectangle
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class FaceAttributes
    {
        public double Smile { get; set; }
        public string Gender { get; set; }
        public string Glasses { get; set; }
        public string Age { get; set; }
        public Emotion Emotion { get; set; }
    }

    public class Emotion
    {
        public double Anger { get; set; }
        public double Contempt { get; set; }
        public double Disgust { get; set; }
        public double Fear { get; set; }
        public double Happiness { get; set; }
        public double Neutral { get; set; }
        public double Sadness { get; set; }
        public double Surprise { get; set; }
    }

    public class BoundingBox
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
