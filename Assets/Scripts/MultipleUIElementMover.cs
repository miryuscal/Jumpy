using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleUIElementMover : MonoBehaviour
{
    [System.Serializable]
    public class UIElementData
    {
        public RectTransform element; // Hareket ettirilecek UI elementi
        public Vector2 targetPosition; // Hedef pozisyon
        public float moveDuration = 1f; // Hareket s�resi
    }

    public List<UIElementData> uiElements = new List<UIElementData>(); // UI elementlerinin listesi

    private void Start()
    {
        // Her element i�in hareket animasyonunu ba�lat
        foreach (var uiElementData in uiElements)
        {
            if (uiElementData.element != null)
            {
                StartCoroutine(MoveToPosition(uiElementData));
            }
        }
    }

    private IEnumerator MoveToPosition(UIElementData data)
    {
        Vector2 initialPosition = data.element.anchoredPosition; // Ba�lang�� pozisyonu
        float elapsed = 0f;

        while (elapsed < data.moveDuration)
        {
            // Ge�en s�re oran�na g�re pozisyonu hesapla
            data.element.anchoredPosition = Vector2.Lerp(initialPosition, data.targetPosition, elapsed / data.moveDuration);

            // Zaman� g�ncelle
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Son pozisyona yerle�tir
        data.element.anchoredPosition = data.targetPosition;
    }
}
