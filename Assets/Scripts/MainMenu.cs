using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public OptionsMenu optionsMenu;

    public GameObject transitionPrefab; // Animasyon için prefab
    public float delayBeforeStart = 2f; // Animasyon baþlamadan önceki bekleme süresi

    private GameObject transitionInstance; // Oluþturulan prefab referansý
    private Animator animator;

    public AudioClip transitionSound;
    public AudioSource audioSource;

    public GameObject x, y;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        transitionInstance = Instantiate(transitionPrefab, Vector3.zero, Quaternion.identity);

        if (transitionInstance.GetComponent<RectTransform>() == null)
        {
            Camera mainCamera = Camera.main;
            transitionInstance.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, mainCamera.nearClipPlane + 1));
        }

        // Animator referansý al
        animator = transitionInstance.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Transition prefab does not have an Animator component!");
            return;
        }

        // Animasyonu gecikmeli olarak baþlat
        StartCoroutine(StartAnimationAfterDelay());

        
    }

    public void OpenOptionsFromMainMenu()
    {

        optionsMenu.openDirectly = true;
        optionsMenu.OpenOptionsDirectly();
    }

    private IEnumerator StartAnimationAfterDelay()
    {
        // Belirtilen süreyi bekle
        yield return new WaitForSeconds(delayBeforeStart);

        // Ses efektini çal
        audioSource.PlayOneShot(transitionSound);

        // Animator'daki Trigger'ý tetikle
        animator.SetTrigger("StartAnimation");

        // Animasyonun uzunluðunu al
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animationState.length;

        x.SetActive(false);
        y.SetActive(false);

        // Animasyonun bitmesini bekle
        yield return new WaitForSeconds(animationLength);

        // Prefab'ý yok et
        yield return new WaitForSeconds(1);

      

        SceneManager.LoadScene(1);
    }

}
