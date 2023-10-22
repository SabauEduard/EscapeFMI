using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rigidBody;
    public float speed = 10.0f;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        rigidBody.velocity = new Vector3(xMove * speed, rigidBody.velocity.y, zMove * speed);
    }
}
