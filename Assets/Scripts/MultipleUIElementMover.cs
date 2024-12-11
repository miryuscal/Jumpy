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
        public float moveDuration = 1f; // Hareket süresi
    }

    public List<UIElementData> uiElements = new List<UIElementData>(); // UI elementlerinin listesi

    private void Start()
    {
        // Her element için hareket animasyonunu baþlat
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
        Vector2 initialPosition = data.element.anchoredPosition; // Baþlangýç pozisyonu
        float elapsed = 0f;

        while (elapsed < data.moveDuration)
        {
            // Geçen süre oranýna göre pozisyonu hesapla
            data.element.anchoredPosition = Vector2.Lerp(initialPosition, data.targetPosition, elapsed / data.moveDuration);

            // Zamaný güncelle
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Son pozisyona yerleþtir
        data.element.anchoredPosition = data.targetPosition;
    }
}
