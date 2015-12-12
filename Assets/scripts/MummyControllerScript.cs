using UnityEngine;
using System.Collections;

public class MummyControllerScript : MonoBehaviour
{

    public float shambleSpeed = 5f;
    public int MAX_SHAMBLE_COUNT = 3000;
    public float MAX_SHAMBLE_TIME = 5f;
    private int facing;
    private Rigidbody2D rigidBody;
    private float shambleTime;

    // Use this for initialization
    void Start()
    {
        facing = 1;
        shambleTime = 0;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        print("Mummy update");
        resolveMovement();
    }

    void fixedUpdate() {

        print("Mummy fixedUpdate");
    }

    private void resolveMovement() {

        shambleTime += Time.deltaTime;
        if (shambleTime > MAX_SHAMBLE_TIME)
        {
            print("Mummy reached shambleCount. About face!");
            Flip();
            shambleTime = 0;
        }
        print("setting mummy x velocity to " + facing * shambleSpeed);
        rigidBody.velocity = new Vector2(facing * shambleSpeed, rigidBody.velocity.y);

    }

    private void Flip()
    {
       // print("Flip - localScale currently: " + transform.localScale.x);
       // print("Flip - facing currently: " + transform.
        facing *= -1;
        transform.localScale = new Vector2(transform.localScale.x *-1, transform.localScale.y);
    }
}
