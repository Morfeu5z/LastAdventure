using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerPlayer : MonoBehaviour
{
    // Keyboard
    [HideInInspector]
    public bool keyboardOn = true;

    // Player config
    [HideInInspector]
    public Rigidbody2D body;
    [HideInInspector]
    public Animator animator;

    // Ground contact
    private GameObject PlayerShadow;
    private GameObject Feet;
    private Transform groundContact;
    private LayerMask groundLayer;
    private float groundRadius = 0.2f;
    public bool grounded = false;

    // Player move
    private float moveSpeed     = 14f;
    [HideInInspector]
    public float move           = 10f;
    private bool goRight        = true;
    private bool blockerLeft    = false;
    private bool blockerRight   = false;

    // Player jump
    private float jumpForce     = 15f;
    private float jumpTime      = 0.3f;
    private float jumpClock;
    private bool doubleJump      = false;
    private string jumpMode      = "landing";

    private bool inDialog = true;

    // Dust player animatoration
    private GameObject DustUpPrefab;
    private GameObject DustDownPrefab;
    private GameObject DustDirectionPrefab;
    private float DestroyTime = 0.6f;

    //Dash
    private int DashMaker = 0;
    public float timerDash = 0;
    public GameObject DashPrefab;
    public GameObject ExplodePrefab;

    // Down Dash
    [HideInInspector]
    public GameObject DashDustParticlePrefab;
    [HideInInspector]
    public bool dashDown = false;
    public bool crouch   = false;

    // Attack and Demage
    public bool PlayerGetDemage = false;
    public float ResistTime;
    private int collisionCount = 0;
    private GameObject Ghost;

    //Camera
    private GameObject mainCamera;

    //Game master
    private GameObject gameMaster;
    private Config config;


    //Audio
    private GameObject audioGrounded;
    private GameObject audioDemage;
    private GameObject audioDeath;
    private GameObject audioJump;
    private GameObject audioDoubleJump;
    private bool audioDeathBool = false;
    private AudioClip AudioFeetStep;


    //Eqiup
    public GameObject equip;
    [HideInInspector]
    public  bool equipActive = false;


    //------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------Start-
    //------------------------------------------------------------------------------------

    void Start()
    {
        // Initialize default parameters
        config              = GameObject.Find("Config").GetComponent<Config>();
        Transform Respown   = GameObject.Find("Respown").gameObject.transform;
        PlayerShadow    = GameObject.Find("Hero/shadow").gameObject;
        groundContact   = GameObject.Find("Hero/GroundContact").gameObject.transform;
        Feet            = GameObject.Find("Hero/Feet").gameObject;
        Ghost           = GameObject.Find("Hero/Ghost").gameObject;
        
        Ghost.SetActive(false);

        DustUpPrefab        = (GameObject)Resources.Load("prefabs/DustUp", typeof(GameObject));
        DustDownPrefab      = (GameObject)Resources.Load("prefabs/DustDown", typeof(GameObject));
        DustDirectionPrefab = (GameObject)Resources.Load("prefabs/DustDirection", typeof(GameObject));

        audioGrounded   = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_Tup", typeof(GameObject));
        audioDemage     = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioDemage", typeof(GameObject));
        audioDeath      = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioDeath", typeof(GameObject));
        audioJump       = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioJump", typeof(GameObject));
        audioDoubleJump = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioDoubleJump", typeof(GameObject));
        DashPrefab      = (GameObject)Resources.Load("Prefabs/HeroDash", typeof(GameObject));
        ExplodePrefab   = (GameObject)Resources.Load("Prefabs/Explode", typeof(GameObject));

        body        = GetComponent<Rigidbody2D>();
        animator    = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
        mainCamera  = GameObject.Find("MainCamera");
        AudioFeetStep = Feet.GetComponent<AudioSource>().clip;

        animator.SetBool("HaveWeapon", false);

        // Load start position
        if (config.portal)
        {
            GameObject spawn = GameObject.Find("Portals").gameObject.transform.GetChild(config.portalIndex).gameObject.transform.GetChild(0).gameObject;
            transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z);

            config.portal = false;
            if (spawn.transform.parent.position.x > spawn.transform.position.x) Direction();
        }
        else
        {
            if (config.LoadedData.scene == SceneManager.GetActiveScene().buildIndex && config.LoadedData.position[0] !=0 && config.LoadedData.position[0] != 0)
            {
                transform.position = new Vector3(config.LoadedData.position[0], config.LoadedData.position[1], 0);
            }
            else
            {
                transform.position = new Vector3(Respown.position.x, Respown.position.y, Respown.position.z);
            }
        }
        mainCamera.transform.position = transform.position;
    }


    //------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------UPDATE-
    //------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) config.QSave();
        if (Input.GetKeyDown(KeyCode.F8)) config.QLoad();

        grounded = Physics2D.OverlapCircle(groundContact.position, groundRadius, groundLayer);

        // Run animation
        if (grounded && move != 0) animator.SetBool("Run", true);
        else animator.SetBool("Run", false);

        // Crouch animation
        if (Input.GetButton("Crouch"))
        {
            crouch = true;
            animator.SetBool("Crouch", true);
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            animator.SetBool("Crouch", false);
        }

        // Dash Down
        if (Input.GetButtonDown("Crouch") && !dashDown) {
            if (!grounded && config.CheckActiveItem("RacketShoes"))
            {
                dashDown = true;
                body.velocity = Vector2.up * (jumpForce * -3);
            }
        }

        // Equip manager
        if (Input.GetButtonDown("Equip"))
        {
            if (grounded && !equipActive && keyboardOn && config.LoadedData.life > 0) OpenEquip();
            else if (equipActive) CloseEquip();
        }

        // Dash Ahead
        if (Input.GetButtonDown("Dash"))
        {
            if (config.CheckActiveItem("RacketShoes") && timerDash <= -2)
            {
                timerDash = 0.4f;
                DashMaker = 0;
                Destroy(Instantiate(audioDoubleJump, transform, false), 1f);
            }
        }


        // If in the air
        if (!grounded){
            animator.SetBool("Run", false);
            animator.SetBool("Jump", true);
            if(jumpMode == "stay") jumpMode = "jumping";
        }
        
        // Prepeare to jump
        if (grounded) if (jumpMode != "stay") if (jumpMode == "landing" || jumpMode == "double") Landing();

        // jump mode
        switch (jumpMode)
        {
            case "stay":
                if (Input.GetButton("Jump") && !inDialog)
                {
                    if (move == 0)
                    {
                        Destroy(Instantiate(DustUpPrefab, groundContact.position, Quaternion.identity), DestroyTime);
                    }
                    else
                    {
                        GameObject tmp = Instantiate(DustDirectionPrefab, groundContact.position, Quaternion.identity);
                        tmp.GetComponent<SpriteRenderer>().flipX = !goRight;
                        Destroy(tmp, DestroyTime);
                    }
                    Destroy(Instantiate(audioJump, transform, false), 0.3f);
                    jumpClock   = jumpTime;
                    jumpMode    = "jumping";
                }
                break;

            case "jumping":
                if (Input.GetButton("Jump") && jumpClock > 0)
                {
                    body.velocity = Vector2.up * jumpForce;
                }
                else
                {
                    if (config.CheckActiveItem("RacketShoes")) jumpMode = "double";
                    else jumpMode = "landing";
                }
                break;

            case "double":
                if (Input.GetButtonDown("Jump") && !doubleJump)
                {
                    Destroy(Instantiate(audioDoubleJump, transform, false), 1f);
                    body.velocity = Vector2.up * jumpForce * 3;
                    animator.SetTrigger("DoubleJump");
                    Destroy(Instantiate(DustDownPrefab, groundContact.position, Quaternion.identity), DestroyTime);
                    doubleJump = true;
                    jumpMode = "landing";
                }
                break;
        }


        if (Input.GetButtonDown("Cancel"))
        {
            if (equipActive == false)
            {
                config.Esc = true;
                GameObject.Find("SoundTrack").GetComponent<ControllerAudio>().Turn("off");
                config.SceneReload();
            }
            else if (equipActive) CloseEquip();
        }
    }


    //------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------FIXED UPDATE-
    //------------------------------------------------------------------------------------
    // Update is called once per frame
    void FixedUpdate()
    {
        // Get move
        if (Input.GetAxisRaw("Horizontal") > 0.4 && !blockerRight && keyboardOn && !crouch) move = moveSpeed;
        else 
        if (Input.GetAxisRaw("Horizontal") < -0.4 && !blockerLeft && keyboardOn && !crouch) move = moveSpeed * -1;
        else move = 0;

        if (!grounded) jumpClock -= Time.deltaTime;
        if (timerDash >= -5) timerDash -= Time.deltaTime;

        if (grounded)
        {
            if (timerDash > 0) DashAhead();
            else body.velocity = new Vector2(move, body.velocity.y);
        }
        else
        {
            PlayerShadow.GetComponent<SpriteRenderer>().enabled = false;
            if (timerDash > 0) DashAhead();
            else
            {
                if (body.gravityScale < 5f) body.gravityScale += 0.05f;
                body.velocity = new Vector2(move, body.velocity.y);
            }

            // If hero fall beyond map
            if (transform.position.y < -50) config.OnePunchDeath();
        }

        Resist();

        //movement and flip
        if (move > 0 && transform.localScale.x < 0)
            Direction();
        else if (move < 0 && transform.localScale.x > 0)
            Direction();
    }



    //------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------FUNCTION-
    //------------------------------------------------------------------------------------

    public void DashAhead()
    {
        int dir = (transform.localScale.x < 0) ? -1 : 1;
        body.velocity = new Vector2(dir * 100, body.velocity.y);
        // Explosion on start
        if (DashMaker == 0) Destroy(Instantiate(ExplodePrefab, transform.position, Quaternion.identity), 1f);
        // Space between dust
        DashMaker++;
        if (DashMaker > 3f)
        {
            DashMaker = 1;
            Destroy(Instantiate(DustDownPrefab, transform.position, Quaternion.identity), 1f);
        }
    }

    public void Landing() {
        jumpMode    = "stay";
        dashDown    = false;
        doubleJump  = false;
        body.gravityScale = 3;
        animator.SetBool("Jump", false);
        PlayerShadow.GetComponent<SpriteRenderer>().enabled = true;
        Destroy(Instantiate(audioGrounded, transform, false), 0.1f);
        Destroy(Instantiate(DustDownPrefab, groundContact.position, Quaternion.identity), DestroyTime);
    }


    public void OpenEquip()
    {
        inDialog = true;
        keyboardOn = false;
        equipActive = true;
        equip.SetActive(true);
        animator.SetBool("Equip", true);
        mainCamera.GetComponent<ControllerCamera>().eqFocus(true);
        Vector3 newVector = equip.transform.localScale;
        newVector.x = (transform.localScale.x < 0) ? -1 : 1;
        equip.transform.localScale = newVector;
        config.UpdateEquip();
    }
    public void CloseEquip()
    {
        mainCamera.GetComponent<ControllerCamera>().eqFocus(false);
        animator.SetBool("Equip", false);
        config.MessageBox(false);
        equip.SetActive(false);
        equipActive = false;
        keyboardOn = true;
        inDialog = false;
    }

    public void MovmentBlocker(int direction = 0) // move blocker
    {
        blockerLeft = false;
        blockerRight = false;
        if (direction == -1) blockerLeft = true;
        else if(direction == 1) blockerRight = true;
    }

    public void Direction() // Function with Direction Filp
    {
        goRight = !goRight;
        Vector3 newVector = transform.localScale;
        newVector.x *= -1;
        transform.localScale = newVector;
    }

    #region 
    public void playerCollision(GameObject collision, float jump_f) // Function with with controll player and enemy colision
    {
        if (!grounded)
        {
            Landing();
            doubleJump = false;
            animator.SetTrigger("DoubleJump");
            body.velocity = Vector2.up * jumpForce * jump_f;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 3f;
        }
    }
    #endregion

    void DeathAudio() // Audio when death
    {
        if (!audioDeathBool)
        {
            audioDeathBool = true;
            Instantiate(audioDeath, transform, false);
        }
    }

    #region // Function with Demage
    public void SetDemaged()
    {
        if (!PlayerGetDemage)
        {
            ResistTime = 2;
            config.DropLife();
            PlayerGetDemage = true;
            GameObject.Find("Canvas/Vignette").GetComponent<Animator>().SetTrigger("Demaged");
            if(config.LoadedData.life > 0) Destroy(Instantiate(audioDemage, transform, false), 0.3f);
        }
    }
    public void Resist() // Demage resist time
    {
        if (ResistTime > 0) ResistTime -= Time.deltaTime;
        else if (ResistTime <= 0 && ResistTime > -1)
        {
            ResistTime = -2;
            PlayerGetDemage = false;
            collisionCount = 0;
        }
    }
    #endregion

    #region // Function with death animation
    public void Death()
    {
        transform.tag = "Untagged";
        if (equipActive) CloseEquip();
        animator.SetBool("Dead", true);
        animator.SetTrigger("Death");
        PlayerGetDemage = false;
        keyboardOn = false;
        move = 0;
        Ghost.SetActive(true);
        equip.GetComponent<ControllerEquip>().Active(true, "_Dead_");
    }
    #endregion

    #region //Dead Animation
    public void DeathEffect()
    {
        GameObject.Find("Canvas/Vignette").GetComponent<Animator>().SetTrigger("LightDown");
        GameObject.Find("Canvas/Death").GetComponent<Animator>().SetTrigger("Death");
    }

    #endregion

    public void jumper(GameObject collision, float force) // Fly to sky jumper
    {
        if (collision.tag == "Jumper") body.velocity = Vector2.up * (jumpForce * force);
    }


    public void makeStepWithAnimation()
    {
        if(timerDash < 0) Feet.GetComponent<AudioSource>().Play();
    }

    public void stepSound(AudioClip StepSound)
    {
        Feet.GetComponent<AudioSource>().clip = StepSound;
    }
    public void stepSoundOriginal()
    {
        Feet.GetComponent<AudioSource>().clip = AudioFeetStep;
    }


    public void RunDialog(bool stop = false)
    {
        inDialog = stop;
    }



    //------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------TRIGGERS-
    //------------------------------------------------------------------------------------


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Demage")
        {
            // collisionCount is needed to React only once
            collisionCount++;
            if (collisionCount == 1) SetDemaged();
        }
        if (collision.name.Substring(0, 4) == "Zoom")
        {
            float RWzoom = 0.2f;
            if (int.Parse(collision.name.Substring(4, 2)) > 40) RWzoom = 0.5f; 
            mainCamera.GetComponent<ControllerCamera>().CamZoom(int.Parse(collision.name.Substring(4, 2)), RWzoom);
        }
    }

}
