using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;

[CreateAssetMenu(fileName = "RandomTwitchUsernameGenerator", menuName = "Emotes 2/Twitch/Random Username Generator")]
public class RandomTwitchUsernameGenerator : ScriptableObject, ITwitchUsernameProvider
{
    [Range(0, 1)] public float chanceOfNumber = 0.05f;
    public string[] adjectives = [];
    public string[] nouns = [];
    public string[] verbs = [];
    
    public TwitchUsername GetUsername()
    {
        var username = GenerateRandomUsername();
        var usernameColor = Random.ColorHSV(0, 1, 1f, 1f, 1f, 1f, 1f, 1f);

        return new TwitchUsername(username, usernameColor);
    }

    private string GenerateRandomUsername()
    {
        var username = "";

        if (Random.value >= 0.5f)
            username += adjectives[Random.Range(0, adjectives.Length)];

        username += nouns[Random.Range(0, nouns.Length)];

        username += verbs[Random.Range(0, verbs.Length)];

        var randCasing = Random.value;
        if (randCasing >= 0.5f)
            username = username.ToUpper();
        if (randCasing >= 0.75f)
            username = username.ToLower();

        if (Random.value <= chanceOfNumber)
            username += $"{Random.Range(1, 1000)}";

        return username;
    }
}