using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadData {
    public static int saveLimit = 10;
    public static List<PlayerData> savedPlayerData = new List<PlayerData>();

    public static void Save() {
        if(PlayerData.current != null) {

            if(PlayerData.current.bag != null) {
                PlayerData.current.bag.Format();
            }

            if( PlayerData.current.slot < 0 ||
                PlayerData.current.slot >= savedPlayerData.Count) {
                savedPlayerData.Add(PlayerData.current);
                Debug.Log("Saving as new slot");
            } else {
                savedPlayerData[PlayerData.current.slot] = PlayerData.current;
                Debug.Log("Saving to slot " + PlayerData.current.slot);
            }
        }

        for(int i = 0; i < savedPlayerData.Count; i++) {
            savedPlayerData[i].slot = i;
        }
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saves.ftd");
        bf.Serialize(file, SaveLoadData.savedPlayerData);
        file.Close();
    }

    public static void Load() {
        if(!File.Exists(Application.persistentDataPath + "/saves.ftd")) {
            File.Create(Application.persistentDataPath + "/saves.ftd");
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/saves.ftd", FileMode.Open);

        if(file.Length > 0) {
            SaveLoadData.savedPlayerData = (List<PlayerData>)bf.Deserialize(file);
        }
        
        Debug.Log(savedPlayerData.Count + " saves found.");
        file.Close();
    }
}
