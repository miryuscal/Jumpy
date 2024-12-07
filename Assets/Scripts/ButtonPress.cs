using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private Animator animator;
    private bool isPressed = false;  // Bu butonun bas�l� olup olmad���n� kontrol eder
    public string requiredTag = "Player"; // Hangi tag ile tetiklenece�i belirlenir
    public WallController wallController; // Duvar�n kaybolmas�n� kontrol eden script

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // E�er belirli bir tag ile tetiklenmi�se ve buton daha �nce bas�lmam��sa animasyonu tetikler
        if ((collision.CompareTag("Player") || collision.CompareTag("Box")) && !isPressed)
        {
            isPressed = true;
            animator.SetTrigger("Press");

            // Duvar�n kaybolma durumunu kontrol et
            wallController.CheckButtons();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Butondan ayr�ld���nda bas�l� durumu s�f�rlan�r ve animasyonu eski haline getirir
        if ((collision.CompareTag("Player") || collision.CompareTag("Box")) && isPressed)
        {
            isPressed = false;
            animator.ResetTrigger("Press");  // Press trigger'�n� s�f�rlay�n
            animator.SetTrigger("Release");  // Butonu eski haline getirmek i�in yeni bir trigger tetikleyin
        }
    }

    // Butonun bas�l� olup olmad���n� d��ar�ya d�nd�ren bir fonksiyon
    public bool IsPressed()
    {
        return isPressed;
    }
}
