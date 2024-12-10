using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        public string speaker;   // Konu�mac�n�n ad�
        public string text;      // Diyalog metni
        public Sprite portrait;  // Konu�mac�n�n portresi
    }

    public GameObject optionsMenu;
    public GameObject pauseMenu;

    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Image characterPortrait; // Tek bir karakter portresi
    public GameObject dialogueBox;

    public AudioClip[] letterSounds; // 33 Ses Dosyas� (A-Z + Bubble sound)
    public AudioSource audioSource; // Ses oynat�c�

    public GameObject transitionPrefab; // Ge�i� animasyonu prefab
    public float delayBeforeTransition = 2f; // Ge�i� �ncesi bekleme s�resi

    private List<Dialogue> dialogues;
    private int currentIndex;
    private bool isTyping;

    public void LoadDialogues(List<Dialogue> dialogueList)
    {
        if (dialogueList == null || dialogueList.Count == 0)
        {
            Debug.LogError("Y�klenecek diyalog bulunamad�.");
            return;
        }

        dialogues = dialogueList;
        currentIndex = 0;
        ShowDialogueBox(true);
        StartCoroutine(TypeDialogue());
    }

    IEnumerator TypeDialogue()
    {
        isTyping = true;
        dialogueText.text = "";
        speakerText.text = dialogues[currentIndex].speaker;

        // Portreyi de�i�tir
        characterPortrait.sprite = dialogues[currentIndex].portrait;

        foreach (char letter in dialogues[currentIndex].text)
        {
            dialogueText.text += letter;
            PlayLetterSound(letter); // Harfe �zel ses �al
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }

    public void OnNextDialogue()
    {
        if (isTyping)
        {
            // Yazarken t�m metni hemen g�ster
            StopAllCoroutines();
            dialogueText.text = dialogues[currentIndex].text;
            isTyping = false;
        }
        else
        {
            currentIndex++;
            if (currentIndex < dialogues.Count)
            {
                StartCoroutine(TypeDialogue());
            }
            else
            {
                ShowDialogueBox(false); // Diyalog bitince diyalog kutusunu kapat
                StartCoroutine(HandleEndOfDialogue());
            }
        }
    }

    void ShowDialogueBox(bool isVisible)
    {
        dialogueBox.SetActive(isVisible);
    }

    void Update()
    {
        if ((pauseMenu.activeSelf == false && optionsMenu.activeSelf == false))
        {
            // Ekrana t�klama veya bir tu�a basma ile sonraki diyaloga ge�
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                OnNextDialogue();
            }
        }
    }

    private void PlayLetterSound(char letter)
    {
        letter = char.ToUpper(letter); // Harfi b�y�k harfe �evir
        int index = GetLetterIndex(letter);

        if (index >= 0 && index < letterSounds.Length)
        {
            audioSource.PlayOneShot(letterSounds[index]);
        }
        else
        {
            // E�er harf e�le�mezse "Bubble sound" �al
            audioSource.PlayOneShot(letterSounds[32]);
        }
    }

    private int GetLetterIndex(char letter)
    {
        switch (letter)
        {
            case 'A': return 0;
            case 'B': return 1;
            case 'C': return 2;
            case '�': return 3;
            case 'D': return 4;
            case 'E': return 5;
            case 'F': return 6;
            case 'G': return 7;
            case '�': return 8;
            case 'H': return 9;
            case 'I': return 10;
            case '�': return 11;
            case 'J': return 12;
            case 'K': return 13;
            case 'L': return 14;
            case 'M': return 15;
            case 'N': return 16;
            case 'O': return 17;
            case '�': return 18;
            case 'P': return 19;
            case 'Q': return 20;
            case 'R': return 21;
            case 'S': return 22;
            case '�': return 23;
            case 'T': return 24;
            case 'U': return 25;
            case '�': return 26;
            case 'V': return 27;
            case 'W': return 28;
            case 'X': return 29;
            case 'Y': return 30;
            case 'Z': return 31;
            default: return -1; // E�le�me yoksa -1 d�nd�r
        }
    }

    private IEnumerator HandleEndOfDialogue()
    {
        // Diyalog paneli kapand�ktan sonra bir s�re bekle
        yield return new WaitForSeconds(delayBeforeTransition);

        // Ge�i� animasyonu oynat
        Camera mainCamera = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 worldCenter = mainCamera.ScreenToWorldPoint(screenCenter);
        worldCenter.z = 0;

        GameObject transitionInstance = Instantiate(transitionPrefab, worldCenter, Quaternion.identity);
        Animator animator = transitionInstance.GetComponent<Animator>();

        if (animator != null)
        {
            float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationLength);
        }

        // Bir sonraki sahneye ge�
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
