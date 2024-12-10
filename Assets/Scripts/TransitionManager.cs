using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public GameObject transitionPrefab; // Animasyon i�in prefab
    public float delayBeforeStart = 2f; // Animasyon ba�lamadan �nceki bekleme s�resi

    private GameObject transitionInstance; // Olu�turulan prefab referans�
    private Animator animator;

    public AudioClip transitionSound;
    public AudioSource audioSource;

    void Start()
    {
        // Prefab'� sahneye hemen instantiate et
        transitionInstance = Instantiate(transitionPrefab, Vector3.zero, Quaternion.identity);

        // Ekran�n ortas�na ta�� (UI Canvas'ta de�ilse)
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

        // Animasyonun bitmesini bekle
        yield return new WaitForSeconds(animationLength);

        // Prefab'� yok et
        Destroy(transitionInstance);
    }
}
