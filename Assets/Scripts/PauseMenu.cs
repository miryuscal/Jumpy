using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Pause Menüsü paneli
    public GameObject optionsMenuPanel; // Options Menüsü paneli
    public GameObject firstPauseButton; // Pause menüsündeki ilk buton
    public GameObject firstOptionsButton; // Options menüsündeki ilk buton
    private bool isPaused = false; // Oyunun durup durmadýðýný kontrol etmek için

    private void Start()
    {
        // Oyun baþlarken pause menü ve options menü kapalý
        pauseMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if(pauseMenuPanel.activeSelf == false && optionsMenuPanel.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        // Oyunu durdur
        isPaused = true;
        Time.timeScale = 0f; // Oyunu durdurur
        pauseMenuPanel.SetActive(true); // Pause menüsünü aç

        // Pause menüsündeki ilk butonu seç
        SelectButton(firstPauseButton);
    }

    public void ResumeGame()
    {
        // Oyunu devam ettir
        isPaused = false;
        Time.timeScale = 1f; // Oyunu devam ettirir
        pauseMenuPanel.SetActive(false); // Pause menüsünü kapat
    }

    public void RestartLevel()
    {
        // Aktif sahneyi yeniden yükle
        Time.timeScale = 1f; // Zamaný sýfýrlar
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Geçerli sahneyi yeniden yükler
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); 
    }

    public void OpenOptionsMenu()
    {
        // Options menüsünü aç, pause menüsünü kapat
        pauseMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);

        // Options menüsündeki ilk butonu seç
        SelectButton(firstOptionsButton);
    }

    private void SelectButton(GameObject button)
    {
        // EventSystem'de seçimi güncelle
        EventSystem.current.SetSelectedGameObject(null); // Önce seçimi temizle
        EventSystem.current.SetSelectedGameObject(button);
    }
}
