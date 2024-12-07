using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // UnityWebRequest için
using System.IO; // Path ve File için
using UnityEngine.UI; // UI kontrolü için

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
        // Portre eþlemesini baþlat
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

        // UnityWebRequest ile JSON'u yükle
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
                Debug.Log("JSON baþarýyla yüklendi: " + jsonData);
                ParseJsonData(jsonData);
            }
            else
            {
                Debug.LogError("JSON dosyasý yüklenemedi: " + filePath);

            }
        }
        else // Masaüstü
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                Debug.Log("JSON baþarýyla yüklendi: " + jsonData);
                ParseJsonData(jsonData);
            }
            else
            {
                Debug.LogError("JSON dosyasý bulunamadý: " + filePath);
            }
        }
    }

    private void ParseJsonData(string jsonData)
    {
        LevelDialogue levelDialogue = JsonUtility.FromJson<LevelDialogue>(jsonData);

        if (levelDialogue == null || levelDialogue.dialogues == null)
        {
            Debug.LogError("JSON deserialize edilemedi veya diyaloglar boþ!");
            return;
        }

        List<DialogueManager.Dialogue> dialogues = new List<DialogueManager.Dialogue>();

        foreach (var data in levelDialogue.dialogues)
        {
            if (!portraitMap.ContainsKey(data.portrait))
            {
                Debug.LogWarning("Portre anahtarý bulunamadý: " + data.portrait);
                continue;
            }

            dialogues.Add(new DialogueManager.Dialogue
            {
                speaker = data.speaker,
                text = data.text,
                portrait = portraitMap[data.portrait]
            });
        }

        Debug.Log("Diyaloglar yüklendi. Diyalog sayýsý: " + dialogues.Count);

        dialogueManager.LoadDialogues(dialogues);
    }
}