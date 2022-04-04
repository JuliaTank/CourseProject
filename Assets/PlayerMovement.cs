using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;


public class PlayerMovement : MonoBehaviour
{
    //TODO: Bark attack, enemies, UI:(replay level, levels(lock/unlock), pause/play,volume) win/loose, moving between scenes/levels), 3rd level 
    //Fields-----------------------------------------------------------------------
    //UI related
    private int points = 0;

    public int pointsToCollect = 0;//has to be set by scene itself
    //movement
    private Rigidbody rb;
    private bool inAir = false;
    private bool bigJump = false;
    private float vertical;
    private float horizontal;
    [SerializeField]private float speed;
    //audio
    [SerializeField] private AudioClip bark;
    [SerializeField] private AudioClip chomp;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip boing;
    [SerializeField] private GameObject barkObj;
    private AudioSource source;
    //doors
    [SerializeField] private GameObject door;
    //animator
    private Animator anim;
    
    //Functions-------------------------------------------------------------------
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        pointsToCollect = 16;
    }
    private void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && !inAir)
        {
            inAir = true;
            rb.AddForce(Vector3.up*7f,ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            barkObj = Instantiate(barkObj, transform);
            source.PlayOneShot(bark);
        }
    }

    private void FixedUpdate()
    {
        anim.SetFloat("Speed",vertical);
        if (transform.position.y < 8.5f)
        { 
            //GAME OVER -restart the level: reload whole scene, reset points, adjust UI itd
            Debug.Log("YOU LOST!");
        }

        if (vertical != 0f && !bigJump)
        {
            rb.MovePosition(transform.position + transform.forward * (vertical * speed * Time.deltaTime));
            //transform.Translate(Vector3.right * (vertical * 5f * Time.deltaTime));
        }else if (vertical != 0f && bigJump)
        {
            rb.MovePosition(transform.position + transform.forward * (vertical * speed* 3 * Time.deltaTime));
        }


        if (horizontal != 0f)
        {
            rb.MoveRotation(rb.rotation*Quaternion.Euler(Vector3.up*(50f*horizontal*Time.deltaTime)));
            //transform.Rotate(Vector3.up*(50f*horizontal*Time.deltaTime));
        }
    }

    private void Land()
    {
        inAir = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Food"))
        {
            source.PlayOneShot(chomp,3f);
            points++;
            Debug.Log("points: "+points);
            Destroy(other.gameObject);
            if (points == pointsToCollect)
            {
                Debug.Log("YOU WON!!");
                Destroy(door);
            }
        }

       
    }
    
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag.Equals("Platform"))
        {
            inAir = false;
            bigJump = false;
        }

        if (other.collider.tag.Equals("MovingPlatform"))
        {
            inAir = false;
            gameObject.transform.SetParent(other.transform);
                
        }
        if (other.collider.tag.Equals("Mushroom"))
        {
            inAir = true;
            bigJump = true;
            rb.AddForce((Vector3.up+ transform.forward) *20f,ForceMode.Impulse);
            source.PlayOneShot(boing,3f);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.tag.Equals("MovingPlatform"))
        {
            other.transform.DetachChildren();
        }
    }
}

