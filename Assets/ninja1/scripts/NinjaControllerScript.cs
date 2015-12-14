using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NinjaControllerScript : MonoBehaviour {

	// character attributes
	[SerializeField]
    private float maxSpeed = 20f;
	[SerializeField]
	private float jumpForce = 700f;
    
	// mechanical properties
	private bool facingRight = true;
    private bool isGrounded = false;
	private bool isAttacking = false;
	private float attackTimer = 0;
	private float attackCooldown = 0.292f;
    public Transform groundCheck;
    public LayerMask whatIsGround;

	// components
	public Collider2D attackTrigger;
	private Rigidbody2D rigidBody;
	private Animator animator;

    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		attackTrigger.enabled = false;
    }

    // Update is called once per frame
    void Update() {
		setAnimatorInputs ();
		handleJump ();
		handleAttackInput ();
    }

    // this Update is called once per physics step
    void FixedUpdate() {
        checkGrounded();
		resolveMovement();
		executeAttack ();
		resetValues ();
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (other.tag.Equals("worldPit")) {
			SceneManager.LoadScene("test_scene");
        }
		if (other.tag.Equals("enemy")) {

			// determine height	
		}
			
    }

	private void resolveMovement() {

		float horizontalInput = Input.GetAxis("Horizontal");
		rigidBody.velocity = new Vector2 (horizontalInput * maxSpeed, rigidBody.velocity.y);
		animator.SetFloat("speed", Mathf.Abs(horizontalInput));
		Flip(horizontalInput);
		
	}

	private void executeAttack() {


		if (isGrounded && isAttacking) {
			if (attackTimer > 0) {
				attackTimer -= Time.deltaTime;
			} else {
				isAttacking = false;
				attackTrigger.enabled = false;
			}
		}
	}

	private void Flip(float horizontalInput) {

		if (horizontalInput > 0 && !facingRight || horizontalInput < 0 && facingRight) {
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
    }

	private void checkGrounded() {
	
		isGrounded = rigidBody.IsTouchingLayers (whatIsGround);
		animator.SetBool ("grounded", isGrounded);
		animator.SetFloat ("vSpeed", rigidBody.velocity.y);
	}

	private void setAnimatorInputs() {

		animator.SetBool ("hInput", false);
		if (Input.GetAxis("Horizontal") != 0)
			animator.SetBool ("hInput", true);		
	}

	/*
	 * method invokation is more expensive than boolean evaluation. should we move the boolean checks
	 * up into the Update() method? If we want to customize the rules that govern (for instance) when
	 * jump may be invoked (isGrounded not checked on double-jump), they can't live in the Update() call.
	 */
	private void handleJump() {
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
			rigidBody.AddForce(new Vector2(0, jumpForce));
		}
	}
		
	private void handleAttackInput() {

		// ground attack
		if (Input.GetKeyDown (KeyCode.K) && !isAttacking) {
			Debug.Log ("Handling Attack Input - Success!");
			if (isGrounded) {
				isAttacking = true;
				attackTimer = attackCooldown;
				attackTrigger.enabled = true;
				animator.SetTrigger ("gAttack");
			}
		}
	}

	private void resetValues() {
	}
}
