using UnityEngine;

public class DustEffectManager : MonoBehaviour
{
    public GameObject dustLandingPrefab; // Landing dust efekti prefab
    public GameObject dustJumpingPrefab; // Jumping dust efekti prefab

    public void SpawnDustEffect(GameObject dustPrefab, Vector3 position, Vector3 offset)
    {
        // Efekti belirlenen pozisyonda ve offset ile olu�tur
        GameObject dust = Instantiate(dustPrefab, position + offset, Quaternion.identity);

        // Efekti belirli bir s�re sonra yok et
        Destroy(dust, 0.8f); // Efektin animasyon s�resine g�re ayarla
    }
}
