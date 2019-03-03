using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody2D theRB;
    [SerializeField] float moveSpeed = 1f;

    Animator myAnim;

    public static PlayerController instance;

    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    public string areaTransitionName;

    // Use this for initialization
    void Start () {
        if (instance == null) {
            instance = this;
        } else {
            if (instance != this) {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);

        theRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, Input.GetAxisRaw("Vertical") * moveSpeed);
        myAnim.SetFloat("moveX", theRB.velocity.x);
        myAnim.SetFloat("moveY", theRB.velocity.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1){
            myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }

        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
        Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
        transform.position.z
        );
    }

    public void SetBounds(Vector3 botLeft, Vector3 topRight){
        bottomLeftLimit = botLeft + new Vector3(.5f, .5f, 0f);
        topRightLimit = topRight + new Vector3(-1f, -1f, 0f);
    }
}
