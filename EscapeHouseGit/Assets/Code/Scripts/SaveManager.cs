using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    public Transform playerTransform;
    public DoorInteractController firstDoorInteractController;

    private string saveFileName = "saveData.json";

    void Start()
    {
        LoadGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
            Debug.Log("Game Saved");
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadGame();
            Debug.Log("Game Loaded");
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            DeleteSave();
  
        }
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.playerPosition = playerTransform.position;
        saveData.playerRotation = playerTransform.rotation;

        saveData.isDoorLocked = firstDoorInteractController._isLocked;
        saveData.usedKeys = firstDoorInteractController._usedKeys;

        saveData.gameVolume = AudioListener.volume;

        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(saveFileName, json);
    }


    public void LoadGame()
    {
        if (System.IO.File.Exists(saveFileName))
        {
            string json = System.IO.File.ReadAllText(saveFileName);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            playerTransform.position = saveData.playerPosition;
            playerTransform.rotation = saveData.playerRotation;

            firstDoorInteractController._isLocked = saveData.isDoorLocked;
            firstDoorInteractController._usedKeys = saveData.usedKeys;

            AudioListener.volume = saveData.gameVolume;
        }
        else
        {
            Debug.Log("Save file not found");
        }
    }

    public void DeleteSave()
    {
        if (System.IO.File.Exists(saveFileName))
        {
            System.IO.File.Delete(saveFileName);
            Debug.Log("Save Deleted");
        }
        else
        {
            Debug.Log("Save file not found");
        }
    }
}
