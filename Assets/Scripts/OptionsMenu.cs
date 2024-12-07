using UnityEngine;
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

    public string language_options;

    private void Start()
    {
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
}
