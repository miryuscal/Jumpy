using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public GameObject transitionPrefab; // Animasyon için prefab
    public float delayBeforeStart = 2f; // Animasyon baþlamadan önceki bekleme süresi

    private GameObject transitionInstance; // Oluþturulan prefab referansý
    private Animator animator;

    public AudioClip transitionSound;
    public AudioSource audioSource;

    void Start()
    {
        // Prefab'ý sahneye hemen instantiate et
        transitionInstance = Instantiate(transitionPrefab, Vector3.zero, Quaternion.identity);

        // Ekranýn ortasýna taþý (UI Canvas'ta deðilse)
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

        // Animasyonun bitmesini bekle
        yield return new WaitForSeconds(animationLength);

        // Prefab'ý yok et
        Destroy(transitionInstance);
    }
}
