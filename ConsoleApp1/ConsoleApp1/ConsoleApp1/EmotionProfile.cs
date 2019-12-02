using System;
using System.Collections.Generic;
using System.Linq;

public class EmotionProfile
{
    // emotion attribute variables from Face/Emotion REST API reply
    SortedDictionary<string, double> emotionDictionary;

    /// <summary>
    /// Constructor creates an emotion profile for a detected face using raw JSON string
    /// </summary>
    /// <param name="json">Dynamic JSON string.</param>
    public EmotionProfile(dynamic json)
	{
        emotionDictionary = new SortedDictionary<string, double>();

        //emotionDictionary.Add("anger", (double)json[0].faceAttributes.emotion.anger);
        emotionDictionary.Add("contempt", (double)json[0].faceAttributes.emotion.contempt);
        emotionDictionary.Add("disgust", (double)json[0].faceAttributes.emotion.disgust);
        emotionDictionary.Add("fear", (double)json[0].faceAttributes.emotion.fear);
        emotionDictionary.Add("happiness", (double)json[0].faceAttributes.emotion.happiness);
        emotionDictionary.Add("neutral", (double)json[0].faceAttributes.emotion.neutral);
        emotionDictionary.Add("sadness", (double)json[0].faceAttributes.emotion.sadness);
        emotionDictionary.Add("surprise", (double)json[0].faceAttributes.emotion.surprise);
    }

    public double getEmotionValue(string key)
    {
        double value;
        emotionDictionary.TryGetValue(key, out value);
        return value;
    }

    public double getDominantEmotionValue()
    {
        return emotionDictionary.Values.Max();
    }

    public String getDominantEmotionKey()
    {
        return emotionDictionary.FirstOrDefault(x => x.Value == emotionDictionary.Values.Max()).Key;
    }

    /// <summary>
    /// toString method to print out emotion variables
    /// </summary>
    public string toString()
    {
        return ("anger: " + getEmotionValue("anger") + "\n" +
                "contempt: " + getEmotionValue("contempt") + "\n" +
                "disgust: " + getEmotionValue("disgust") + "\n" +
                "fear: " + getEmotionValue("fear") + "\n" +
                "happiness: " + getEmotionValue("happiness") + "\n" +
                "neutral: " + getEmotionValue("neutral") + "\n" +
                "sadness: " + getEmotionValue("sadness") + "\n" +
                "surprise: " + getEmotionValue("surprise") + "\n");
    }
}
