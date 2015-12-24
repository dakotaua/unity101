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
	private bool isDamaged = false;
	private float attackTimer = 0;
	private float damagedTimer = 0.333f;
	private float attackCooldown = 0.292f;
	private Vector2 damageVector = new Vector2(-800,300);
    public Transform groundCheck;
    public LayerMask whatIsGround;

	// game stats
	public int curHealth;
	public int maxHealth = 50;

	// components
	public Collider2D attackTrigger;
	private Rigidbody2D rigidBody;
	private Animator animator;
	private Renderer spriteRenderer;

    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		attackTrigger.enabled = false;
		spriteRenderer = GetComponent<Renderer> ();

		curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update() {
		
		handlePlayerHealth ();
		handleJumpInput ();
		handleAttackInput ();
    }

    // this Update is called once per physics step
    void FixedUpdate() {
        checkGrounded();
		handleMovementInput();
		resolveAttackState ();
		resolveDamagedState ();
		resetValues ();
    }

    void OnTriggerEnter2D(Collider2D other) {

		// poc - falling off level resets scene state
        if (other.tag.Equals("worldPit")) {
			SceneManager.LoadScene("test_scene");
        }
    }

	private void handleMovementInput() {

		if (isDamaged) {
			return;
		}

		float horizontalInput = Input.GetAxis("Horizontal");
		rigidBody.velocity = new Vector2 (horizontalInput * maxSpeed, rigidBody.velocity.y);
		animator.SetFloat("speed", Mathf.Abs(horizontalInput));
		Flip(horizontalInput);
		
	}

	private void resolveAttackState() {

		if (isDamaged) {
			return;
		}

		if (isGrounded && isAttacking) {
			if (attackTimer > 0) {
				attackTimer -= Time.deltaTime;
			} else {
				isAttacking = false;
				attackTrigger.enabled = false;
			}
		}
	}

	private void resolveDamagedState() {

		if (isDamaged) {
			if (damagedTimer > 0) {
				damagedTimer -= Time.deltaTime;
			} else {
				isDamaged = false;
				animator.SetBool ("isDamaged", false);
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
		
	/*
	 * method invokation is more expensive than boolean evaluation. should we move the boolean checks
	 * up into the Update() method? If we want to customize the rules that govern (for instance) when
	 * jump may be invoked (isGrounded not checked on double-jump), they can't live in the Update() call.
	 */
	private void handleJumpInput() {
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
			rigidBody.AddForce(new Vector2(0, jumpForce));
		}
	}
		
	private void handleAttackInput() {

		if (isDamaged) {
			return;
		}

		// ground attack
		if (Input.GetKeyDown (KeyCode.K) && !isAttacking) {
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

	private void handlePlayerHealth() {

		if (curHealth > maxHealth)
			curHealth = maxHealth;

		if (curHealth <= 0)
			die ();

	}

	public void applyDamage(int damageValue) {

		if (isDamaged == true) {
			return;
		}

		isDamaged = true;
		curHealth -= damageValue;
		animator.SetTrigger ("dmgAnimTrigger");
		animator.SetBool ("isDamaged", true);
		StartCoroutine(doDamageBlinks(3f, 0.2f));
		rigidBody.velocity = Vector2.zero;
		rigidBody.AddForce (damageVector);
	}

	public void die() {
		Destroy (gameObject);
		SceneManager.LoadScene("test_scene");
	}

	IEnumerator doDamageBlinks(float duration, float blinkTime) {
		Debug.Log ("entered doDamageBlinks");
		while (duration > 0f) {
			Debug.Log ("Duration of blink is: " + duration);
			duration -= Time.deltaTime*15;	// unclear why seconds -= deltaTime doesnt work

			//toggle renderer
			spriteRenderer.enabled = !spriteRenderer.enabled;

			//wait for a bit
			yield return new WaitForSeconds(blinkTime);
		}

		//make sure renderer is enabled when we exit
		spriteRenderer.enabled = true;
	}

}
