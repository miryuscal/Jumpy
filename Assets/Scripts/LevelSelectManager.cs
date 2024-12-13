using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public Button button;          // Seviyeyi temsil eden buton
        public TextMeshProUGUI levelText;  // Seviye numarasý
        public Image buttonImage;      // Kilit/açýk sprite'ýný kontrol eden görsel
        public string sceneName;       // Seviyeye karþýlýk gelen sahne adý
    }

    public List<Level> levels;          // Tüm seviyeler
    public Sprite lockedSprite;        // Kilitli durumdaki buton için sprite
    public Sprite unlockedSprite;      // Açýk durumdaki buton için sprite

    private void Start()
    {
        InitializeLevels();
    }

    private void InitializeLevels()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); // Açýlmýþ en yüksek level ID (1. level varsayýlan)

        foreach (var level in levels)
        {
            int levelID = GetLevelIDFromSceneName(level.sceneName);

            if (levelID <= unlockedLevel)
            {
                // Level açýk
                level.button.interactable = true;
                level.buttonImage.sprite = unlockedSprite;
                level.levelText.text = levelID.ToString();
                level.button.onClick.AddListener(() => LoadLevel(level.sceneName));
            }
            else
            {
                // Level kilitli
                level.button.interactable = false;
                level.buttonImage.sprite = lockedSprite;
                level.levelText.text = "";
            }
        }
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void UnlockNextLevel(string currentSceneName)
    {
        int currentLevelID = GetLevelIDFromSceneName(currentSceneName);
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentLevelID + 1 > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevelID + 1);
        }

        // Bir sonraki levelin sahne adýna geçiþ yap
        string nextSceneName = GetSceneNameFromLevelID(currentLevelID + 1);
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // Eðer son seviyedeyseniz, bitiþ ekranýna gidin (isteðe baðlý)
            SceneManager.LoadScene("EndScene");
        }
    }

    private static int GetLevelIDFromSceneName(string sceneName)
    {
        if (sceneName.StartsWith("Level"))
        {
            string levelNumber = sceneName.Substring(5);
            if (int.TryParse(levelNumber, out int levelID))
            {
                return levelID;
            }
        }

        return 0;
    }

    private static string GetSceneNameFromLevelID(int levelID)
    {
        // Level ID'ye karþýlýk gelen sahne adýný döndür
        return "Level" + levelID;
    }
}
