using UnityEngine;

public class DustEffectManager : MonoBehaviour
{
    public GameObject dustLandingPrefab; // Landing dust efekti prefab
    public GameObject dustJumpingPrefab; // Jumping dust efekti prefab

    public void SpawnDustEffect(GameObject dustPrefab, Vector3 position, Vector3 offset)
    {
        // Efekti belirlenen pozisyonda ve offset ile oluþtur
        GameObject dust = Instantiate(dustPrefab, position + offset, Quaternion.identity);

        // Efekti belirli bir süre sonra yok et
        Destroy(dust, 0.8f); // Efektin animasyon süresine göre ayarla
    }
}
