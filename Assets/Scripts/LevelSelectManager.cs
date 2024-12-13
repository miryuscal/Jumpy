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
        public TextMeshProUGUI levelText;  // Seviye numaras�
        public Image buttonImage;      // Kilit/a��k sprite'�n� kontrol eden g�rsel
        public string sceneName;       // Seviyeye kar��l�k gelen sahne ad�
    }

    public List<Level> levels;          // T�m seviyeler
    public Sprite lockedSprite;        // Kilitli durumdaki buton i�in sprite
    public Sprite unlockedSprite;      // A��k durumdaki buton i�in sprite

    private void Start()
    {
        InitializeLevels();
    }

    private void InitializeLevels()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); // A��lm�� en y�ksek level ID (1. level varsay�lan)

        foreach (var level in levels)
        {
            int levelID = GetLevelIDFromSceneName(level.sceneName);

            if (levelID <= unlockedLevel)
            {
                // Level a��k
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

        // Bir sonraki levelin sahne ad�na ge�i� yap
        string nextSceneName = GetSceneNameFromLevelID(currentLevelID + 1);
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // E�er son seviyedeyseniz, biti� ekran�na gidin (iste�e ba�l�)
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
        // Level ID'ye kar��l�k gelen sahne ad�n� d�nd�r
        return "Level" + levelID;
    }
}
