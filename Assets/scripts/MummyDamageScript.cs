using UnityEngine;
using System.Collections;

public class MummyDamageScript : MonoBehaviour {

	[SerializeField]
	private int mummyTouchDamage = 10;
	private Collider2D mummyDamageTrigger;

	// Use this for initialization
	void Start () {
		mummyDamageTrigger = GetComponent<Collider2D> ();
		mummyDamageTrigger.enabled = true;
		Debug.Log ("Mummy damage script init");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll) {

		if (coll.gameObject.tag == "damageable") {
			NinjaControllerScript ninScript = coll.gameObject.GetComponent<NinjaControllerScript> ();
			ninScript.applyDamage (mummyTouchDamage);
		}
	}
}