using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public OptionsMenu optionsMenu;

    public GameObject transitionPrefab; // Animasyon i�in prefab
    public float delayBeforeStart = 2f; // Animasyon ba�lamadan �nceki bekleme s�resi

    private GameObject transitionInstance; // Olu�turulan prefab referans�
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

        // Animator referans� al
        animator = transitionInstance.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Transition prefab does not have an Animator component!");
            return;
        }

        // Animasyonu gecikmeli olarak ba�lat
        StartCoroutine(StartAnimationAfterDelay());

        
    }

    public void OpenOptionsFromMainMenu()
    {

        optionsMenu.openDirectly = true;
        optionsMenu.OpenOptionsDirectly();
    }

    private IEnumerator StartAnimationAfterDelay()
    {
        // Belirtilen s�reyi bekle
        yield return new WaitForSeconds(delayBeforeStart);

        // Ses efektini �al
        audioSource.PlayOneShot(transitionSound);

        // Animator'daki Trigger'� tetikle
        animator.SetTrigger("StartAnimation");

        // Animasyonun uzunlu�unu al
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animationState.length;

        x.SetActive(false);
        y.SetActive(false);

        // Animasyonun bitmesini bekle
        yield return new WaitForSeconds(animationLength);

        // Prefab'� yok et
        yield return new WaitForSeconds(1);

      

        SceneManager.LoadScene(1);
    }

}
