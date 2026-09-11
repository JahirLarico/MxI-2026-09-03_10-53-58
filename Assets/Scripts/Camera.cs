using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //public Transform player;
    //public Vector3 offset = new Vector3(1, 0.65f, -10);

    public Transform target;
    public Transform leftBound, rightBound;
    private float leftBoundWitdh, rightBoundWitdh;
    public float smoothDampTime = 0.15f;
    private Vector3 smoothDamVelocity = Vector3.zero;
    private float camWitdh, camHeight, levelMinX, levelMaxX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camHeight = Camera.main.orthographicSize * 2;
        camWitdh = camHeight * Camera.main.aspect;

        leftBoundWitdh = leftBound.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        rightBoundWitdh= rightBound.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;

        levelMinX = leftBound.position.x + leftBoundWitdh + (camWitdh / 2);
        levelMaxX = rightBound.position.x - rightBoundWitdh - (camWitdh / 2);

    }

    // Update is called once per frame
    void Update()
    {
        float targetX = Mathf.Max(levelMinX, Mathf.Min(levelMaxX, target.position.x));
        float x = Mathf.SmoothDamp(transform.position.x, targetX, ref smoothDamVelocity.x, smoothDampTime);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        //transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
    }
}
