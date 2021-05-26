using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void Save(Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data,true);
        PlayerPrefs.SetString(key,jsonData);
        PlayerPrefs.Save();
    }

    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            //Debug.Log("Has");
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key),data);
        }
    }

    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData,GameManager.Instance.playerStats.characterData.name);
    }
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData,GameManager.Instance.playerStats.characterData.name);
    }
}
