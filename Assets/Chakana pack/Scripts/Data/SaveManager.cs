using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager{

    //DIFERENCIAR LA DATA AL CAMBIAR DE ESCENA, TOCAR EL CHECKPOINT U OTRA SITUACION
    public static void SavePlayerData(Hoyustus hoyustus){
        PlayerData data = new PlayerData(hoyustus);
        string dataPath = Application.persistentDataPath + "/player.save";
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static void SavePlayerData(float vida, int gold, float condor, float serpiente, float lanza, float curacion, float ataque)
    {
        //Simplificar la firma
        PlayerData data = new PlayerData(vida, gold, condor, serpiente, lanza, curacion, ataque);
        string dataPath = Application.persistentDataPath + "/player.save";
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static PlayerData LoadPlayerData() {
        string dataPath = Application.persistentDataPath + "/player.save";
        if (File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            PlayerData data = (PlayerData)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return data;
        }
        else {
            return null;
        }
    }
}
