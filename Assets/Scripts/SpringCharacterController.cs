using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpringCharacterController : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D characterCollider;
    public float jumpForce = 10f;
    public float speed = 5f;
    public float burnRiseForce = 5f;
    private bool isBurning = false;
    private bool isGrounded = false;
    private bool canJump = false;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    private float targetInput = 0f;
    private float currentInput = 0f;
    public float smoothingSpeed = 5f;

    public DustEffectManager dustEffectManager;
    public DialogueLoader dialogueLoader;
    public string currentLevelKey = "level1";

    private int currentLevel = 1;
    private bool dialogueTriggered = false;

    public AudioClip jumpClip;
    public AudioClip groundClip;
    public AudioClip burnClip;
    public AudioClip transitionSound;

    public OptionsMenu optionsMenu;
    public string languageFromOptions;

    // Transition animasyonu için deðiþkenler
    public GameObject transitionPrefab;
    public float delayBeforeSceneReload = 2f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Application.targetFrameRate = 60;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterCollider = GetComponent<Collider2D>();
        languageFromOptions = optionsMenu.languageSender();
        Debug.Log(languageFromOptions);
    }

    void Update()
    {
        if (isBurning) return;

        if (canJump)
        {
            Jump();
        }

        if (isMovingLeft)
            targetInput = -1f;
        else if (isMovingRight)
            targetInput = 1f;
        else
            targetInput = Input.GetAxis("Horizontal");

        currentInput = Mathf.Lerp(currentInput, targetInput, Time.deltaTime * smoothingSpeed);

        if (Mathf.Abs(currentInput) > 0.01f)
        {
            transform.Translate(currentInput * Time.deltaTime * speed, 0, 0);
            FlipCharacter(currentInput);
        }

        languageFromOptions = optionsMenu.languageSender();
    }

    private void Jump()
    {
        animator.SetBool("IsJumping", true);
        animator.SetBool("IsGrounded", false);
        rb.velocity = Vector2.up * jumpForce;
        canJump = false;

        if (dustEffectManager != null)
        {
            audioSource.PlayOneShot(jumpClip);
            Vector3 dustPosition = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
            dustEffectManager.SpawnDustEffect(dustEffectManager.dustJumpingPrefab, dustPosition, Vector3.zero);
        }
    }

    private void PlayLandingAnimation()
    {
        animator.SetBool("IsGrounded", true);
        animator.SetBool("IsJumping", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && rb.velocity.y <= 0 && !isBurning)
        {
            isGrounded = true;
            PlayLandingAnimation();
            Invoke("EnableJump", 0.583f);

            if (dustEffectManager != null)
            {
                audioSource.PlayOneShot(groundClip);
                Vector3 dustPosition = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
                dustEffectManager.SpawnDustEffect(dustEffectManager.dustLandingPrefab, dustPosition, new Vector3(0, -0.1f, 0));
            }
        }
    }

    private void EnableJump()
    {
        if (isGrounded)
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsGrounded", false);
        }
    }

    private void FlipCharacter(float horizontalInput)
    {
        if (horizontalInput > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Saw") && !isBurning)
        {
            StartCoroutine(BurnEffect());
            audioSource.PlayOneShot(burnClip);
        }
        else if (collision.gameObject.CompareTag("LevelEndTrigger") && !dialogueTriggered)
        {
            dialogueTriggered = true;
            string fileName = $"dialogues_level{currentLevel}_{languageFromOptions}.json";
            Debug.Log($"Tetiklendi! Yükleniyor: {fileName}");
            dialogueLoader.LoadDialogueForLevel(fileName);
        }
    }

    private IEnumerator BurnEffect()
    {
        isBurning = true;

        Cinemachine.CinemachineVirtualCamera virtualCamera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = null;
        }

        spriteRenderer.color = Color.red;
        characterCollider.enabled = false;

        rb.velocity = Vector2.up * burnRiseForce;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;

        spriteRenderer.sortingOrder = 10;

        yield return new WaitForSeconds(1f);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity = Vector2.zero;

        yield return new WaitUntil(() => transform.position.y < -10);

        StartCoroutine(PlayTransitionAndReloadScene());
    }

    private IEnumerator PlayTransitionAndReloadScene()
    {
        // Prefabý kameranýn ortasýnda oluþtur
        Camera mainCamera = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 worldCenter = mainCamera.ScreenToWorldPoint(screenCenter);
        worldCenter.z = 0; // Z eksenini sýfýrla (UI elemaný deðilse)

        GameObject transitionInstance = Instantiate(transitionPrefab, worldCenter, Quaternion.identity);

        // Transition animasyonu için Animator referansý
        Animator transitionAnimator = transitionInstance.GetComponent<Animator>();

        if (transitionAnimator == null)
        {
            Debug.LogError("Transition prefab does not have an Animator component!");
            yield break;
        }

        // Animasyon uzunluðunu al
        float animationLength = transitionAnimator.GetCurrentAnimatorStateInfo(0).length;

        audioSource.PlayOneShot(transitionSound);

        // Animasyon süresini ve gecikmeyi bekle
        yield return new WaitForSeconds(animationLength + delayBeforeSceneReload);

       

        // Sahneyi yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void MoveLeft(bool isPressed)
    {
        isMovingLeft = isPressed;
    }

    public void MoveRight(bool isPressed)
    {
        isMovingRight = isPressed;
    }
}
