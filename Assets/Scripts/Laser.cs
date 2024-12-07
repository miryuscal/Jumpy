using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Laser : MonoBehaviour
{
    public LayerMask obstacleMask; // Lazerin �arpaca�� engellerin katman�
    public float maxLength = 10f; // Lazerin maksimum uzunlu�u

    private LineRenderer lineRenderer;
    private BoxCollider2D laserCollider;

    void Start()
    {
        // Lazerin g�rselini �izecek LineRenderer bile�enini al�yoruz
        lineRenderer = GetComponent<LineRenderer>();
        laserCollider = GetComponent<BoxCollider2D>();
        laserCollider.isTrigger = true; // Collider'� Trigger yap
    }

    void Update()
    {
        DrawLaser();
    }

    void DrawLaser()
    {
        // Lazerin ��k�� noktas�ndan ileriye do�ru bir ray g�nderiyoruz
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, maxLength, obstacleMask);

        // LineRenderer'�n ba�lang�� noktas�n� belirle
        lineRenderer.SetPosition(0, transform.position);

        Vector3 laserEndPoint;

        if (hit.collider != null)
        {
            // E�er bir engele �arparsa lazerin sonu engele kadar olur
            laserEndPoint = hit.point;

            // E�er lazer bir Player'a �arparsa yanma efektini tetikle
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<SpringCharacterController>()?.StartCoroutine("BurnEffect");
            }
        }
        else
        {
            // E�er engel yoksa lazer maksimum uzunlu�a kadar uzar
            laserEndPoint = transform.position + transform.up * maxLength;
        }

        // LineRenderer'�n biti� noktas�n� ayarla
        lineRenderer.SetPosition(1, laserEndPoint);

        // Collider'� lazerin uzunlu�una ve pozisyonuna g�re ayarla
        UpdateCollider(transform.position, laserEndPoint);
    }

    void UpdateCollider(Vector3 startPoint, Vector3 endPoint)
    {
        // Lazerin ortas�n� hesapla
        Vector3 midPoint = (startPoint + endPoint) / 2;

        // Lazerin uzunlu�unu hesapla
        float laserLength = Vector3.Distance(startPoint, endPoint);

        // Collider'�n pozisyonunu ve boyutunu g�ncelle
        laserCollider.size = new Vector2(0.1f, laserLength); // X ekseni geni�li�i, Y ekseni uzunlu�u
        laserCollider.offset = transform.InverseTransformPoint(midPoint); // Ortas�n� ayarla
        laserCollider.transform.rotation = transform.rotation; // Lazerin rotasyonuna uygun hale getir
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // E�er Player lazerle temas ederse yanma efektini tetikle
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<SpringCharacterController>()?.StartCoroutine("BurnEffect");
        }
    }
}
