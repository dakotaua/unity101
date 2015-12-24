using UnityEngine;
using System.Collections;

public class MummyControllerScript : MonoBehaviour
{

    public float shambleSpeed = 5f;
    public float MAX_SHAMBLE_TIME = 5f;
    private int facing;
    private float shambleTime;

	// components
	private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start() {
        facing = 1;
        shambleTime = 0;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        resolveMovement();
    }

    void fixedUpdate() {

    }

    private void resolveMovement() {

        shambleTime += Time.deltaTime;
        if (shambleTime > MAX_SHAMBLE_TIME)
        {
            Flip();
            shambleTime = 0;
        }
        rigidBody.velocity = new Vector2(facing * shambleSpeed, rigidBody.velocity.y);

    }

    private void Flip()
    {
        facing *= -1;
        transform.localScale = new Vector2(transform.localScale.x *-1, transform.localScale.y);
    }
}
