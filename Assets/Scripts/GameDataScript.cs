using System;
using UnityEngine;

public enum BonusKind
{
    Slow,
    Fast,
    Plus2,
    Plus10
}

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class GameDataScript : ScriptableObject
{
    public bool resetOnStart;
    public int level = 1;
    public int balls = 6;
    public int points = 0;
    public int pointsToBall = 0;
    public bool music = true;
    public bool sound = true;

    [Range(0f, 1f)] public float musicVolume = 0.2f;
    [Range(0f, 1f)] public float soundVolume = 1f;

    public void Reset()
    {
        level = 1;
        balls = 6;
        points = 0;
        pointsToBall = 0;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("balls", balls);
        PlayerPrefs.SetInt("points", points);
        PlayerPrefs.SetInt("pointsToBall", pointsToBall);
        PlayerPrefs.SetInt("music", music ? 1 : 0);
        PlayerPrefs.SetInt("sound", sound ? 1 : 0);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("soundVolume", soundVolume);
    }

    public void Load()
    {
        level = PlayerPrefs.GetInt("level", 1);
        balls = PlayerPrefs.GetInt("balls", 6);
        points = PlayerPrefs.GetInt("points", 0);
        pointsToBall = PlayerPrefs.GetInt("pointsToBall", 0);
        music = PlayerPrefs.GetInt("music", 1) == 1;
        sound = PlayerPrefs.GetInt("sound", 1) == 1;
        musicVolume = PlayerPrefs.GetFloat("musicVolume", musicVolume);
        soundVolume = PlayerPrefs.GetFloat("soundVolume", soundVolume);
    }

    [Serializable]
    public struct BonusWeight
    {
        public BonusKind kind;
        [Min(0f)] public float weight;
    }

    [Header("Bonus spawn weights")]
    public BonusWeight[] bonusWeights = new[]
    {
        new BonusWeight { kind = BonusKind.Slow, weight = 5f },
        new BonusWeight { kind = BonusKind.Fast, weight = 2f },
        new BonusWeight { kind = BonusKind.Plus2, weight = 2f },
        new BonusWeight { kind = BonusKind.Plus10, weight = 1f },
    };

    public BonusKind GetRandomBonusKind()
    {
        float total = 0f;
        foreach (var b in bonusWeights) total += Mathf.Max(0f, b.weight);
        if (total <= 0f) return BonusKind.Slow;

        float r = UnityEngine.Random.Range(0f, total);
        float acc = 0f;
        foreach (var b in bonusWeights)
        {
            acc += Mathf.Max(0f, b.weight);
            if (r <= acc) return b.kind;
        }
        return BonusKind.Slow;
    }
}