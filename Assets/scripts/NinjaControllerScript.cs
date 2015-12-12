using UnityEngine;
using System.Collections;

public class NinjaControllerScript : MonoBehaviour {

    public float maxSpeed = 10f;
    private bool facingRight = true;
    private Rigidbody2D rigidBody;
    private Animator animator;
    public float jumpForce = 700f;
    private bool isGrounded = false;
    public Transform groundCheck;
    public LayerMask whatIsGround;


    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rigidBody.AddForce(new Vector2(0, jumpForce));
        }
    }

    // this Update is called once per physics step
    void FixedUpdate() {

        float move = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(move * maxSpeed, rigidBody.velocity.y);
        animator.SetFloat("speed", Mathf.Abs(move));

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
    }

    void Flip() {

        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
