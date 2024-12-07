using System.Collections;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public ButtonPress button1;      // �lk buton
    public ButtonPress button2;      // �kinci buton
    public GameObject[] wallPieces;  // Duvar par�alar�

    public float blinkDuration = 1f; // Yan�p s�nme s�resi
    public float blinkSpeed = 0.1f;  // Yan�p s�nme h�z�

    // �ki butonun durumunu kontrol eder
    public void CheckButtons()
    {
        if (button1.IsPressed() && button2.IsPressed())
        {
            StartCoroutine(BlinkAndDisappear());
        }
    }

    private IEnumerator BlinkAndDisappear()
    {
        float elapsed = 0f;

        // Duvar par�alar�n�n yan�p s�nme efektini ger�ekle�tirir
        while (elapsed < blinkDuration)
        {
            foreach (GameObject wallPiece in wallPieces)
            {
                SpriteRenderer spriteRenderer = wallPiece.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    Color color = spriteRenderer.color;
                    color.a = Mathf.PingPong(Time.time * (1 / blinkSpeed), 1f);
                    spriteRenderer.color = color;
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Yan�p s�nme efekti sona erdi�inde duvar par�alar�n� devre d��� b�rak�r
        foreach (GameObject wallPiece in wallPieces)
        {
            wallPiece.SetActive(false);
        }
    }
}
