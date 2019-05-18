using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourGhost : MonoBehaviour {

    private GameObject hero;
    private GameObject ghost;
    private Transform startPoint;
    private Vector3   randomTarget;

    public Animator GhostAnimator;
    private string switcher = "fly";
    private int direction = 0;

    //Smoof
    private float smoothTime;
    private Vector3 velocity = Vector3.zero;

    private float PlayerLookRight;
    private float distance;

    private GameObject AudioGhostPref;

    void Start() {
        hero = GameObject.Find("Hero/CameraPoint");
        ghost = transform.GetChild(0).gameObject;
        AudioGhostPref = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioGhost", typeof(GameObject));
        startPoint = transform;
        GhostAnimator = ghost.transform.GetChild(0).gameObject.GetComponent<Animator>();
        randomTarget = transform.position;
        RandomTarget();
    }

    private void FixedUpdate()
    {
        switch (switcher)
        {
            case "setDirection":
                {
                    direction = (hero.transform.position.x < ghost.transform.position.x) ? 1 : -1;
                    PlayerLookRight = hero.transform.parent.transform.localScale.x;
                    Flip(direction);
                    switcher = "wait";
                    break;
                }
            case "wait":
                {
                    if (Mathf.Abs(hero.transform.position.x - ghost.transform.position.x) < 1)
                    {
                        switcher = "go";
                        distance = 20;
                    }
                    if (hero.transform.parent.transform.localScale.x != PlayerLookRight)
                    {
                        switcher = "go";
                        direction = (direction == -1) ? 1 : -1;
                        distance = 10;
                        Flip();
                    }
                    break;
                }
            case "go":
                {
                    if (Mathf.Abs(hero.transform.position.x - ghost.transform.position.x) > distance)
                    {
                        switcher = "follow";
                        GhostAnimator.SetBool("Hide", false);
                        Instantiate(AudioGhostPref, transform, false);
                    }
                    break;
                }
            case "follow":
                {
                    MakeMove("Player");
                    if (Mathf.Abs(hero.transform.position.x - ghost.transform.position.x) > 80)
                    {
                        switcher = "runAway";
                        smoothTime = 3f;
                    }
                    if ((hero.transform.parent.transform.localScale.x > 0 && direction == -1) || (hero.transform.parent.transform.localScale.x < 0 && direction == 1))
                    {
                        switcher = "runAway";
                        smoothTime = 3f;
                        Flip();
                        GhostAnimator.SetBool("Hide", true);
                    }
                    break;
                }
            case "runAway":
                {
                    switcher = "fly";
                    GhostAnimator.SetBool("Hide", true);
                    RandomTarget();
                    break;
                }
            case "fly":
                {
                    MakeMove("Fly");
                    bool hide = (Random.Range(0, 10) >= 5) ? true : false;
                    GhostAnimator.SetBool("Hide", hide);
                    if (Mathf.Abs(randomTarget.x - ghost.transform.position.x) < 0.1f && Mathf.Abs(randomTarget.y - ghost.transform.position.y) < 0.1f)
                    {
                        RandomTarget();
                        GhostAnimator.SetBool("Hide", false);
                    }
                    break;
                }
        }
    }

    public void RandomTarget(){
        float rdx = 10;
        float rdy = 3;
        float x = randomTarget.x + Random.Range(rdx * -1, rdx);
        float y = randomTarget.y + Random.Range(rdy * -1, rdy);
        randomTarget = new Vector3(x, y, randomTarget.z);
    }

    public void MakeMove(string target = "Player")
    {
        switch (target)
        {
            case "Player":
                {
                    smoothTime = 0.8f;
                    Vector3 targetPosition = hero.transform.TransformPoint(new Vector3(0, 0, 0));
                    ghost.transform.position = Vector3.SmoothDamp(ghost.transform.position, targetPosition, ref velocity, smoothTime);
                    break;
                }
            case "StartPoint":
                {
                    smoothTime -= 0.01f;
                    Vector3 targetPosition = startPoint.TransformPoint(new Vector3(0, 0, 0));
                    ghost.transform.position = Vector3.SmoothDamp(ghost.transform.position, targetPosition, ref velocity, smoothTime);
                    break;
                }
            case "Fly":
                {
                    smoothTime = 1.5f;
                    Vector3 targetPosition = randomTarget;
                    ghost.transform.position = Vector3.SmoothDamp(ghost.transform.position, targetPosition, ref velocity, smoothTime);
                    break;
                }
        }
    }


    public void Flip(int mustBe = 0)
    {
        if ((mustBe == -1 && ghost.transform.localScale.x >= 0) || (mustBe == 1 && ghost.transform.localScale.x < 0) || mustBe == 0)
        {
            mustBe = -1;
        }
        else
        {
            mustBe = 1;
        }
        Vector3 newVector = ghost.transform.localScale;
        newVector.x *= mustBe;
        ghost.transform.localScale = newVector;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GhostAnimator.SetBool("Hide", true);
            switcher = "setDirection";
        }
    }
}
