using System.Collections;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public ButtonPress button1;      // Ýlk buton
    public ButtonPress button2;      // Ýkinci buton
    public GameObject[] wallPieces;  // Duvar parçalarý

    public float blinkDuration = 1f; // Yanýp sönme süresi
    public float blinkSpeed = 0.1f;  // Yanýp sönme hýzý

    // Ýki butonun durumunu kontrol eder
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

        // Duvar parçalarýnýn yanýp sönme efektini gerçekleþtirir
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

        // Yanýp sönme efekti sona erdiðinde duvar parçalarýný devre dýþý býrakýr
        foreach (GameObject wallPiece in wallPieces)
        {
            wallPiece.SetActive(false);
        }
    }
}
