using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Central save/load for offline  data.
/// Path and Save/Load situations defined here so timer/wallet don't care about file locations.
/// I put path inside project because this way is easy to inspect. Normally, I use Application.persistentDataPath for build.
/// </summary>
public static class SaveLoadManager
{
    public static OfflineSaveData Data { get; private set; }

    private const string FileName = "offline_data.json";
    private static readonly string BasePath = Path.Combine(Application.dataPath, "SaveLoadData");

    private static bool _loaded;

    public static void LoadOrInitialize()
    {
        if (_loaded) return;

        string path = Path.Combine(BasePath, FileName);

        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                Data = JsonUtility.FromJson<OfflineSaveData>(json);
            }
            catch
            {
                Data = null;
            }
        }

        if (Data == null)
            Data = new OfflineSaveData();

        if (Data.lastCollectUtcSeconds <= 0)
            Data.lastCollectUtcSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        if (Data.coins < 0) Data.coins = 0;
        if (Data.hammers < 0) Data.hammers = 0;

        Save();
        _loaded = true;
    }

    public static void Save()
    {
        if (Data == null) return;

        string path = Path.Combine(BasePath, FileName);

        if (!Directory.Exists(BasePath))
            Directory.CreateDirectory(BasePath);

        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(path, json);
    }
}