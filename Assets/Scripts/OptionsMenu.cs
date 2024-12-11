using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    // Checkbox ve butonlar
    public Toggle buttonToggle; // Checkbox referansý
    public GameObject button1;  // Ýlk buton (invisible yapacaðýz)
    public GameObject button2;  // Ýkinci buton (invisible yapacaðýz)

    // Dil kontrolü
    public Slider languageSlider;   // Dil için slider (0: Türkçe, 1: Ýngilizce)
    public Image flagImage;         // Bayrak görseli
    public Sprite turkishFlag;      // Türk bayraðý sprite
    public Sprite englishFlag;      // Ýngiliz bayraðý sprite

    // Ses kontrolü
    public AudioSource effectsSource; // Ses efektleri AudioSource
    public AudioSource musicSource;   // Müzik AudioSource
    public Slider effectsSlider;      // Ses efektleri için ses seviyesi slider'ý
    public Slider musicSlider;        // Müzik için ses seviyesi slider'ý

    // Menü geçiþi
    public GameObject pauseMenu;   // Pause menüsü paneli
    public GameObject optionsMenu; // Options menüsü paneli
    public GameObject firstOptionsButton; // Options menüsündeki ilk seçilecek buton
    public GameObject firstPauseButton;

    public string language_options; // Dil seçimi deðiþkeni

    // Ana menüden açýlýþ için
    public bool openDirectly = false;

    public RectTransform uiElement; // Hareket ettirilecek UI elementi
    public Vector2 targetPosition;  // UI elementinin ulaþmasýný istediðiniz pozisyon
    public Vector2 targetPositionTwo;
    public float moveDuration = 1f; // Hareket süresi

    private Vector2 initialPosition; // Baþlangýç pozisyonu

    private bool closing = false;

    public GameObject buttonplaceholder;


 

    private void Start()
    {
        initialPosition = uiElement.anchoredPosition;
        LoadSettings(); // Kaydedilmiþ ayarlarý yükle

        // Baþlangýç ayarlarý
        buttonToggle.onValueChanged.AddListener(OnButtonToggleChanged); // Checkbox dinleyicisi
        languageSlider.onValueChanged.AddListener(OnLanguageChanged);   // Dil slider'ý dinleyicisi
        effectsSlider.onValueChanged.AddListener(OnEffectsVolumeChanged); // Ses efekti slider'ý dinleyicisi
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);     // Müzik slider'ý dinleyicisi

        // Baþlangýç durumu
        OnButtonToggleChanged(buttonToggle.isOn); // Butonlarýn görünürlüðünü güncelle
        OnLanguageChanged(languageSlider.value);  // Dil ayarýný güncelle
        OnEffectsVolumeChanged(effectsSlider.value); // Ses efektleri seviyesini ayarla
        OnMusicVolumeChanged(musicSlider.value);   // Müzik seviyesini ayarla

        // Eðer doðrudan açýlma isteði varsa
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

    // Options panelini doðrudan aç
    public void OpenOptionsDirectly()
    {
        optionsMenu.SetActive(true);
        SelectButton(firstOptionsButton); // Ýlk butonu seç
        StartCoroutine(MoveToPosition(targetPosition, moveDuration));

    }

    // Checkbox deðiþtiðinde butonlarý görünür/görünmez yap
    private void OnButtonToggleChanged(bool isOn)
    {
        button1.SetActive(isOn);
        button2.SetActive(isOn);
        PlayerPrefs.SetInt("ButtonToggle", isOn ? 1 : 0); // Kaydet
    }

    // Dil seçimi deðiþtiðinde bayraðý ve deðeri deðiþtir
    private void OnLanguageChanged(float value)
    {
        if (value == 0) // Türkçe
        {
            flagImage.sprite = turkishFlag;
            language_options = "tr";
        }
        else if (value == 1) // Ýngilizce
        {
            flagImage.sprite = englishFlag;
            language_options = "en";
        }

        PlayerPrefs.SetFloat("LanguageSlider", value); // Kaydet
    }

    // Ses efektleri ses seviyesi deðiþtiðinde çaðrýlýr
    private void OnEffectsVolumeChanged(float value)
    {
        effectsSource.volume = value; // AudioSource ses seviyesini ayarla
        PlayerPrefs.SetFloat("EffectsVolume", value); // Kaydet
    }

    // Müzik ses seviyesi deðiþtiðinde çaðrýlýr
    private void OnMusicVolumeChanged(float value)
    {
        musicSource.volume = value; // AudioSource ses seviyesini ayarla
        PlayerPrefs.SetFloat("MusicVolume", value); // Kaydet
    }

    // Pause menüsüne geri dön
    public void BackToPauseMenu()
    {
        LanguageInitiate();
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);

        // Pause menüsündeki ilk butonu seç
        SelectButton(firstPauseButton);
    }

    private void SelectButton(GameObject button)
    {
        // EventSystem'de seçimi güncelle
        EventSystem.current.SetSelectedGameObject(null); // Önce seçimi temizle
        EventSystem.current.SetSelectedGameObject(button);
    }

    public string languageSender()
    {
        return language_options;
    }

    private void LoadSettings()
    {
        // Kaydedilmiþ ayarlarý yükle
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
        float elapsed = 0f; // Geçen süreyi takip et

        while (elapsed < duration)
        {
            // Geçen süre oranýna göre pozisyonu hesapla
            uiElement.anchoredPosition = Vector2.Lerp(initialPosition, target, elapsed / duration);

            // Zamaný güncelle
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Son pozisyona yerleþtir
        uiElement.anchoredPosition = target;

        

    }

    private IEnumerator CloseAndDisable(Vector2 target, float duration)
    {
        // Baþlangýç pozisyonunu güncelle
        initialPosition = uiElement.anchoredPosition;

        float elapsed = 0f; // Geçen süreyi takip et

        // Pozisyonu hedefe doðru yavaþça taþý
        while (elapsed < duration)
        {
            uiElement.anchoredPosition = Vector2.Lerp(initialPosition, target, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Son pozisyona yerleþtir
        uiElement.anchoredPosition = target;

        // Elementi devre dýþý býrak
        uiElement.gameObject.SetActive(false);
        SelectButton(buttonplaceholder); 
    }

}
