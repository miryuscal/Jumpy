using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // UnityWebRequest i�in
using System.IO; // Path ve File i�in
using UnityEngine.UI; // UI kontrol� i�in

public class DialogueLoader : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [System.Serializable]
    public class DialogueData
    {
        public string speaker;
        public string text;
        public string portrait;
    }


    [System.Serializable]
    public class LevelDialogue
    {
        public List<DialogueData> dialogues;
    }

    public Sprite wiseSpringPortrait;
    public Sprite jumpyPortrait;
    public Sprite hippieSpringPortrait;

    private Dictionary<string, Sprite> portraitMap;

    void Awake()
    {
        // Portre e�lemesini ba�lat
        portraitMap = new Dictionary<string, Sprite>
        {
            { "WiseSpring", wiseSpringPortrait },
            { "Jumpy", jumpyPortrait },
            { "HippieSpring", hippieSpringPortrait }
        };
    }

    public void LoadDialogueForLevel(string levelFileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, levelFileName);

        // UnityWebRequest ile JSON'u y�kle
        StartCoroutine(LoadJson(filePath));
    }

    private IEnumerator LoadJson(string filePath)
    {
        if (filePath.Contains("://") || filePath.Contains(":///")) // Android ve iOS
        {
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonData = request.downloadHandler.text;
                Debug.Log("JSON ba�ar�yla y�klendi: " + jsonData);
                ParseJsonData(jsonData);
            }
            else
            {
                Debug.LogError("JSON dosyas� y�klenemedi: " + filePath);

            }
        }
        else // Masa�st�
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                Debug.Log("JSON ba�ar�yla y�klendi: " + jsonData);
                ParseJsonData(jsonData);
            }
            else
            {
                Debug.LogError("JSON dosyas� bulunamad�: " + filePath);
            }
        }
    }

    private void ParseJsonData(string jsonData)
    {
        LevelDialogue levelDialogue = JsonUtility.FromJson<LevelDialogue>(jsonData);

        if (levelDialogue == null || levelDialogue.dialogues == null)
        {
            Debug.LogError("JSON deserialize edilemedi veya diyaloglar bo�!");
            return;
        }

        List<DialogueManager.Dialogue> dialogues = new List<DialogueManager.Dialogue>();

        foreach (var data in levelDialogue.dialogues)
        {
            if (!portraitMap.ContainsKey(data.portrait))
            {
                Debug.LogWarning("Portre anahtar� bulunamad�: " + data.portrait);
                continue;
            }

            dialogues.Add(new DialogueManager.Dialogue
            {
                speaker = data.speaker,
                text = data.text,
                portrait = portraitMap[data.portrait]
            });
        }

        Debug.Log("Diyaloglar y�klendi. Diyalog say�s�: " + dialogues.Count);

        dialogueManager.LoadDialogues(dialogues);
    }
}