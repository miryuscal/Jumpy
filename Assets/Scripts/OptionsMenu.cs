using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    // Checkbox ve butonlar
    public Toggle buttonToggle; // Checkbox referans�
    public GameObject button1;  // �lk buton (invisible yapaca��z)
    public GameObject button2;  // �kinci buton (invisible yapaca��z)

    // Dil kontrol�
    public Slider languageSlider;   // Dil i�in slider (0: T�rk�e, 1: �ngilizce)
    public Image flagImage;         // Bayrak g�rseli
    public Sprite turkishFlag;      // T�rk bayra�� sprite
    public Sprite englishFlag;      // �ngiliz bayra�� sprite

    // Ses kontrol�
    public AudioSource effectsSource; // Ses efektleri AudioSource
    public AudioSource musicSource;   // M�zik AudioSource
    public Slider effectsSlider;      // Ses efektleri i�in ses seviyesi slider'�
    public Slider musicSlider;        // M�zik i�in ses seviyesi slider'�

    // Men� ge�i�i
    public GameObject pauseMenu;   // Pause men�s� paneli
    public GameObject optionsMenu; // Options men�s� paneli
    public GameObject firstOptionsButton; // Options men�s�ndeki ilk se�ilecek buton
    public GameObject firstPauseButton;

    public string language_options; // Dil se�imi de�i�keni

    // Ana men�den a��l�� i�in
    public bool openDirectly = false;

    public RectTransform uiElement; // Hareket ettirilecek UI elementi
    public Vector2 targetPosition;  // UI elementinin ula�mas�n� istedi�iniz pozisyon
    public Vector2 targetPositionTwo;
    public float moveDuration = 1f; // Hareket s�resi

    private Vector2 initialPosition; // Ba�lang�� pozisyonu

    private bool closing = false;

    public GameObject buttonplaceholder;


 

    private void Start()
    {
        initialPosition = uiElement.anchoredPosition;
        LoadSettings(); // Kaydedilmi� ayarlar� y�kle

        // Ba�lang�� ayarlar�
        buttonToggle.onValueChanged.AddListener(OnButtonToggleChanged); // Checkbox dinleyicisi
        languageSlider.onValueChanged.AddListener(OnLanguageChanged);   // Dil slider'� dinleyicisi
        effectsSlider.onValueChanged.AddListener(OnEffectsVolumeChanged); // Ses efekti slider'� dinleyicisi
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);     // M�zik slider'� dinleyicisi

        // Ba�lang�� durumu
        OnButtonToggleChanged(buttonToggle.isOn); // Butonlar�n g�r�n�rl���n� g�ncelle
        OnLanguageChanged(languageSlider.value);  // Dil ayar�n� g�ncelle
        OnEffectsVolumeChanged(effectsSlider.value); // Ses efektleri seviyesini ayarla
        OnMusicVolumeChanged(musicSlider.value);   // M�zik seviyesini ayarla

        // E�er do�rudan a��lma iste�i varsa
        if (openDirectly)
        {
            OpenOptionsDirectly();
        }
    }

    private void Update()
    {
        LanguageInitiate();
    }

    private void LanguageInitiate()
    {
        if (languageSlider.value == 0)
        {
            language_options = "tr";
        }
        else if (languageSlider.value == 1)
        {
            language_options = "en";
        }
    }

    // Options panelini do�rudan a�
    public void OpenOptionsDirectly()
    {
        optionsMenu.SetActive(true);
        SelectButton(firstOptionsButton); // �lk butonu se�
        StartCoroutine(MoveToPosition(targetPosition, moveDuration));

    }

    // Checkbox de�i�ti�inde butonlar� g�r�n�r/g�r�nmez yap
    private void OnButtonToggleChanged(bool isOn)
    {
        button1.SetActive(isOn);
        button2.SetActive(isOn);
        PlayerPrefs.SetInt("ButtonToggle", isOn ? 1 : 0); // Kaydet
    }

    // Dil se�imi de�i�ti�inde bayra�� ve de�eri de�i�tir
    private void OnLanguageChanged(float value)
    {
        if (value == 0) // T�rk�e
        {
            flagImage.sprite = turkishFlag;
            language_options = "tr";
        }
        else if (value == 1) // �ngilizce
        {
            flagImage.sprite = englishFlag;
            language_options = "en";
        }

        PlayerPrefs.SetFloat("LanguageSlider", value); // Kaydet
    }

    // Ses efektleri ses seviyesi de�i�ti�inde �a�r�l�r
    private void OnEffectsVolumeChanged(float value)
    {
        effectsSource.volume = value; // AudioSource ses seviyesini ayarla
        PlayerPrefs.SetFloat("EffectsVolume", value); // Kaydet
    }

    // M�zik ses seviyesi de�i�ti�inde �a�r�l�r
    private void OnMusicVolumeChanged(float value)
    {
        musicSource.volume = value; // AudioSource ses seviyesini ayarla
        PlayerPrefs.SetFloat("MusicVolume", value); // Kaydet
    }

    // Pause men�s�ne geri d�n
    public void BackToPauseMenu()
    {
        LanguageInitiate();
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);

        // Pause men�s�ndeki ilk butonu se�
        SelectButton(firstPauseButton);
    }

    private void SelectButton(GameObject button)
    {
        // EventSystem'de se�imi g�ncelle
        EventSystem.current.SetSelectedGameObject(null); // �nce se�imi temizle
        EventSystem.current.SetSelectedGameObject(button);
    }

    public string languageSender()
    {
        return language_options;
    }

    private void LoadSettings()
    {
        // Kaydedilmi� ayarlar� y�kle
        if (PlayerPrefs.HasKey("ButtonToggle"))
        {
            buttonToggle.isOn = PlayerPrefs.GetInt("ButtonToggle") == 1;
        }

        if (PlayerPrefs.HasKey("LanguageSlider"))
        {
            languageSlider.value = PlayerPrefs.GetFloat("LanguageSlider");
        }

        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
            effectsSource.volume = effectsSlider.value;
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            musicSource.volume = musicSlider.value;
        }
    }

    public void CloseOptionsMenu()
    {
        StartCoroutine(CloseAndDisable(targetPositionTwo, moveDuration));
        closing = true;
        
    }


    private IEnumerator MoveToPosition(Vector2 target, float duration)
    {
        float elapsed = 0f; // Ge�en s�reyi takip et

        while (elapsed < duration)
        {
            // Ge�en s�re oran�na g�re pozisyonu hesapla
            uiElement.anchoredPosition = Vector2.Lerp(initialPosition, target, elapsed / duration);

            // Zaman� g�ncelle
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Son pozisyona yerle�tir
        uiElement.anchoredPosition = target;

        

    }

    private IEnumerator CloseAndDisable(Vector2 target, float duration)
    {
        // Ba�lang�� pozisyonunu g�ncelle
        initialPosition = uiElement.anchoredPosition;

        float elapsed = 0f; // Ge�en s�reyi takip et

        // Pozisyonu hedefe do�ru yava��a ta��
        while (elapsed < duration)
        {
            uiElement.anchoredPosition = Vector2.Lerp(initialPosition, target, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Son pozisyona yerle�tir
        uiElement.anchoredPosition = target;

        // Elementi devre d��� b�rak
        uiElement.gameObject.SetActive(false);
        SelectButton(buttonplaceholder); 
    }

}
