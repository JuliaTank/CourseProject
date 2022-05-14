using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    //Fields-----------------------------------------------------------------------
    //UI related
    private int points = 0;
    public int pointsToCollect = 0;
    //movement
    private Rigidbody rb;
    private Rigidbody rbO;
    private bool inAir = false;
    private bool bigJump = false;
    private float vertical;
    private float horizontal;
    [SerializeField]private float speed;
    [SerializeField] private GameObject barkObj;
    //audio
    [SerializeField] private AudioClip bark;
    [SerializeField] private AudioClip chomp;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip boing;
    [SerializeField] private AudioClip slap;
    private AudioSource source;
    //doors
    [SerializeField] private GameObject door;
    //animator
    private Animator anim;
    //UI - I know I should have done separate script for that 
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI snacks;

    [SerializeField] private TextMeshProUGUI flyingTip;
    [SerializeField] private UnityEngine.UI.Image victoryImg;
    [SerializeField] private UnityEngine.UI.Image defeatImg;

    [SerializeField] private TextMeshProUGUI friendTip;
    //light
    [SerializeField] private Light light;
    //navigation 
    private String currentScene;

    private int currentlevel;
    //other
    private bool specialMushroomPower;
    [SerializeField] private GameObject specialMushroom;

    public delegate void WinDelegate();

    public static event WinDelegate winEvent;
    //Functions-------------------------------------------------------------------
    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        pointsToCollect = PlayerPrefs.GetInt("PickUpsNo");

       currentScene = SceneManager.GetActiveScene().name;
       currentlevel = PlayerPrefs.GetInt("level");
       winEvent += LoadNewLevel;
        
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        snacks.text = "Snacks: 0 / " + pointsToCollect;
        flyingTip.text = "";
        if (friendTip != null)
        {
            friendTip.text = "";
        }
        victoryImg.enabled = false;
        defeatImg.enabled = false;
        specialMushroomPower = false;

    }
    private void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && !inAir)
        {
            if (!specialMushroomPower)
            {
                inAir = true;
            }
            rb.AddForce(Vector3.up*7f,ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            var barkObjj = Instantiate(barkObj);//, transform.position,Quaternion.identity);
            barkObjj.transform.position = transform.position + transform.forward;
            barkObjj.transform.rotation = transform.rotation;
            rbO = barkObjj.GetComponent<Rigidbody>();
            rbO.AddForce(transform.forward * 15f, ForceMode.Impulse);
            Destroy(barkObjj, 3f);
            source.PlayOneShot(bark);
        
        }
    }
    
    

    private void FixedUpdate()
    {
        anim.SetFloat("Speed",vertical);
        if (transform.position.y < 8.5f || healthSlider.value == 0)
        { 
            //GAME OVER -restart the level: reload whole scene, reset points, adjust UI itd
            Debug.Log("YOU LOST!");
            StartCoroutine(Die());

        }

        if (vertical != 0f && !bigJump)
        {
            rb.MovePosition(transform.position + transform.forward * (vertical * speed * Time.deltaTime));
            
        }else if (vertical != 0f && bigJump)
        {
            rb.MovePosition(transform.position + transform.forward * (vertical * speed* 2.5f * Time.deltaTime));
        }


        if (horizontal != 0f)
        {
            rb.MoveRotation(rb.rotation*Quaternion.Euler(Vector3.up*(50f*horizontal*Time.deltaTime)));
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
            source.PlayOneShot(chomp, 3f);
            points++;
            snacks.text = "Snacks " + points + " / " + pointsToCollect;
            Debug.Log("points: " + points);
            Destroy(other.gameObject);
            if (points == pointsToCollect)
            {
                Debug.Log("YOU WON!!, GO TO THE NEXT LEVEL :D");
                StartCoroutine(Victory());
                source.PlayOneShot(winSound, 8f);
                Destroy(door);
                if (currentScene.Equals("Level1"))
                {
                    PlayerPrefs.SetInt("level", 2);
                }else if (currentScene.Equals("Level2"))
                {
                    PlayerPrefs.SetInt("level", 3);
                }
                
            }
        }

    }

    IEnumerator Die()
    {
        //show die message for 3s and then reload page
        defeatImg.enabled = true;
        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Victory()
    {
        victoryImg.enabled = true;
        yield return new WaitForSeconds(2f);
        victoryImg.enabled = false;
    }
    

    IEnumerator ShowTip()
    {
        flyingTip.text = "Press space multiple times to start flying :)";
        yield return new WaitForSeconds(3f);
        flyingTip.text = "";
    }
    IEnumerator ShowFriendTip()
    {
        friendTip.text = "Hey, my name is Friend. Be careful, there are vets that will try to attack you, but don't worry, you have super barking power and will defeat them easily (left Ctrl). Good luck! ";
        yield return new WaitForSeconds(7f);
        friendTip.text = "";
    }

    void RespawnMushroom()
    {
        Instantiate(specialMushroom);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag.Equals("Platform"))
        {
            specialMushroomPower = false;
            inAir = false;
            bigJump = false;
        }

        if (other.collider.tag.Equals("Enemy"))
        {
            healthSlider.value -= 0.1f;
            source.PlayOneShot(slap,6f);
            rb.AddForce((Vector3.back)*6f,ForceMode.Impulse);
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
            rb.AddForce((Vector3.up+ transform.forward) *15f,ForceMode.Impulse);
            source.PlayOneShot(boing,3f);
        }

        if (other.collider.tag.Equals("SpecialMushroom"))
        {
            source.PlayOneShot(chomp, 3f);
            StartCoroutine(ShowTip());
            Destroy(other.collider.gameObject);
            specialMushroomPower = true;
            //respawn mushroom after 
            Invoke("RespawnMushroom",5);
        }

        if (other.collider.tag.Equals("Portal"))
        {
            if (winEvent != null)
            {
                winEvent();
            }
        }

        if (other.collider.tag.Equals("Friend"))
        {
            StartCoroutine(ShowFriendTip());
        }
    }

    private void LoadNewLevel()
    {
        if (currentScene.Equals("Level1"))
        {
            PlayerPrefs.SetInt("PickUpsNo",30);
            SceneManager.LoadScene("Level2");
                
        }else if (currentScene.Equals("Level2")) {
                
            PlayerPrefs.SetInt("PickUpsNo",42);
            SceneManager.LoadScene("Level3");
                
        }else if (currentScene.Equals("Level3"))
        {
            SceneManager.LoadScene("Menu");
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

