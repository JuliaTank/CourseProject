using UnityEngine;

public class Movement : MonoBehaviour {

    [SerializeField]
    private float rotationalSpeed;

    [SerializeField]
    private float moveSpeed;

    private float vertical;
    private float horizontal;
    private bool jump;
    private Animator anim;
    private Rigidbody rb;
    private bool grounded;

    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int jumpHash = Animator.StringToHash("Jump");

    private void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        jump = Input.GetButtonDown("Jump");
        
        if (jump && !anim.GetCurrentAnimatorStateInfo(0).IsName("BaseLayer.Jump")) {
            anim.SetTrigger(jumpHash);
        }
    }

    private void FixedUpdate() {
        anim.SetFloat(speedHash, vertical);

        var trans = transform;
        rb.MovePosition(trans.position + vertical * moveSpeed * 0.1f * trans.forward);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0f, horizontal * rotationalSpeed, 0f)));
    }
}