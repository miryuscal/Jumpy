using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Pause Men�s� paneli
    public GameObject optionsMenuPanel; // Options Men�s� paneli
    public GameObject firstPauseButton; // Pause men�s�ndeki ilk buton
    public GameObject firstOptionsButton; // Options men�s�ndeki ilk buton
    private bool isPaused = false; // Oyunun durup durmad���n� kontrol etmek i�in

    private void Start()
    {
        // Oyun ba�larken pause men� ve options men� kapal�
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
        pauseMenuPanel.SetActive(true); // Pause men�s�n� a�

        // Pause men�s�ndeki ilk butonu se�
        SelectButton(firstPauseButton);
    }

    public void ResumeGame()
    {
        // Oyunu devam ettir
        isPaused = false;
        Time.timeScale = 1f; // Oyunu devam ettirir
        pauseMenuPanel.SetActive(false); // Pause men�s�n� kapat
    }

    public void RestartLevel()
    {
        // Aktif sahneyi yeniden y�kle
        Time.timeScale = 1f; // Zaman� s�f�rlar
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Ge�erli sahneyi yeniden y�kler
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); 
    }

    public void OpenOptionsMenu()
    {
        // Options men�s�n� a�, pause men�s�n� kapat
        pauseMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);

        // Options men�s�ndeki ilk butonu se�
        SelectButton(firstOptionsButton);
    }

    private void SelectButton(GameObject button)
    {
        // EventSystem'de se�imi g�ncelle
        EventSystem.current.SetSelectedGameObject(null); // �nce se�imi temizle
        EventSystem.current.SetSelectedGameObject(button);
    }
}
