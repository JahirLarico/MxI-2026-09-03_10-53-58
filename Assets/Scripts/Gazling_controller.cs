using UnityEngine;

public class Gazling_controller : MonoBehaviour
{

    public float speed = 0.6f;
    public bool moveRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            if (transform.position.y < collision.transform.position.y) {
                Destroy(gameObject);
            } 
        }
    }
}
