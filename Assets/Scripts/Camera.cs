using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public Transform left, right;
    private float leftWidth, rightWidth;
    public float speed = 0.15f;

    public Transform player;
    public Vector3 offset = new Vector3(1, 0.65f, -10);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
    }
}
