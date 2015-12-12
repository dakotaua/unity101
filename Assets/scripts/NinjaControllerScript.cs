using UnityEngine;
using System.Collections;

public class NinjaControllerScript : MonoBehaviour {

	// character attributes
    public float maxSpeed = 20f;
	public float accelRate = 2f;
	public float deccelRate = 3.0f;
	public float jumpForce = 700f;
    
	// mechanical properties
	private bool facingRight = true;
    private bool isGrounded = false;
    public Transform groundCheck;
    public LayerMask whatIsGround;


	// components
	private Rigidbody2D rigidBody;
	private Animator animator;

    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

		animator.SetBool ("hInput", false);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rigidBody.AddForce(new Vector2(0, jumpForce));
        }
			
		if (Input.GetAxis("Horizontal") != 0)
			animator.SetBool ("hInput", true);
    }

    // this Update is called once per physics step
    void FixedUpdate() {
        checkGrounded();
		resolveMovement();
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.tag.Equals("worldPit")) {
            Application.LoadLevel("test_scene");
        }
    }




	private void resolveMovement() {

		float horizontalInput = Input.GetAxis("Horizontal");
		float newAccel = 0;
		if (horizontalInput != 0) {
			newAccel = horizontalInput * accelRate;
			newAccel += rigidBody.velocity.x;
			if (Mathf.Abs (newAccel) > maxSpeed) { 
				newAccel = horizontalInput * maxSpeed;
			}
			rigidBody.velocity = new Vector2(newAccel, rigidBody.velocity.y);

		} else {
			if (rigidBody.velocity.x > 0)
				newAccel = deccelRate * -1;
			else
				newAccel = deccelRate;
			newAccel += rigidBody.velocity.x;
			if (Mathf.Abs(newAccel) > Mathf.Abs(deccelRate)) {
				rigidBody.velocity = new Vector2(newAccel, rigidBody.velocity.y);
			} else {
				rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
			}
		}
				
		animator.SetFloat("speed", Mathf.Abs(horizontalInput));

		if (horizontalInput > 0 && !facingRight)
			Flip();
		else if (horizontalInput < 0 && facingRight)
			Flip();
	}

    private void Flip() {

        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	private void checkGrounded() {
	
		isGrounded = rigidBody.IsTouchingLayers (whatIsGround);
		animator.SetBool ("grounded", isGrounded);
		animator.SetFloat ("vSpeed", rigidBody.velocity.y);
	}

   
		
}
