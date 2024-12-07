using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scissors : MonoBehaviour
{
    public float distance = 1.7f;
    public float speed = 2f;

    private Vector3 startPosition;
    private bool movingRight = false;

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if(movingRight == true)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if(transform.position.x >= startPosition.x + distance)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if(transform.position.x <= startPosition.x - distance)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }


}
