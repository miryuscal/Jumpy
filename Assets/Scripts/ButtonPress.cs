using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private Animator animator;
    private bool isPressed = false;  // Bu butonun basýlý olup olmadýðýný kontrol eder
    public string requiredTag = "Player"; // Hangi tag ile tetikleneceði belirlenir
    public WallController wallController; // Duvarýn kaybolmasýný kontrol eden script

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Eðer belirli bir tag ile tetiklenmiþse ve buton daha önce basýlmamýþsa animasyonu tetikler
        if ((collision.CompareTag("Player") || collision.CompareTag("Box")) && !isPressed)
        {
            isPressed = true;
            animator.SetTrigger("Press");

            // Duvarýn kaybolma durumunu kontrol et
            wallController.CheckButtons();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Butondan ayrýldýðýnda basýlý durumu sýfýrlanýr ve animasyonu eski haline getirir
        if ((collision.CompareTag("Player") || collision.CompareTag("Box")) && isPressed)
        {
            isPressed = false;
            animator.ResetTrigger("Press");  // Press trigger'ýný sýfýrlayýn
            animator.SetTrigger("Release");  // Butonu eski haline getirmek için yeni bir trigger tetikleyin
        }
    }

    // Butonun basýlý olup olmadýðýný dýþarýya döndüren bir fonksiyon
    public bool IsPressed()
    {
        return isPressed;
    }
}
