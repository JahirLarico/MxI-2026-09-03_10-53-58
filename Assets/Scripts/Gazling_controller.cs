using UnityEngine;
using UnityEngine.UIElements;

public class Gazling_controller : MonoBehaviour
{

    public float speed = 0.6f;
    public bool moveRight = true;

    private bool isBated;
    public Animator animator;

    private BoxCollider2D box;
    private Vector2 originalSize;

    private Vector2 originalSprite;

    GameObject isaaac;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        originalSize = box.size;
        originalSprite = box.offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveRight)
        {
            transform.Translate(2 * Time.deltaTime * speed, 0, 0);
        }
        else
        {
            transform.Translate(-2 * Time.deltaTime * speed, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pipe" || collision.gameObject.tag == "Enemie") {
            if (moveRight) { 
                moveRight = false;
            }
            else {
                moveRight = true;
            }

        }

        if (collision.gameObject.tag == "Player") {
            if (transform.position.y < collision.transform.position.y)
            {
                int playerLayer = LayerMask.NameToLayer("Player");
                int enemyLayer = LayerMask.NameToLayer("Enemie");

                Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
                isBated = true;
                float visualY = originalSize.y * 0.37f;
                speed = 0;
                animator.SetBool("beated", isBated);
                box.size = new Vector2(originalSize.x, visualY);
                box.offset = new Vector2(originalSprite.x, originalSprite.y - (originalSize.y - visualY) / 2f);
                //Collider2D[] colliders = FindObjectsOfType<Collider2D>();

                //foreach (Collider2D col in colliders)
                //{
                //    if (!col.CompareTag("Ground"))
                //    {
                //        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col, true);
                //    }
                //}
                //Invoke("Death", 1);
            }
            else {
                Isaac_controller.death = true;
            }
        }
    }

    private void Death() {
        Destroy(gameObject);
    }
}
