using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGun : MonoBehaviour {

    private GameObject PrefSmoke;
    private GameObject PrefBullet;
    private GameObject hero;
    private ControllerPlayer c_hero;
    private GameObject myCamera;
    private GameObject audioShoot;
    private Transform gun;

    public float EndBulletWay = 50;
    private float correct;
    private bool canShoot = true;
	// Use this for initialization
	void Start () {
        hero = GameObject.Find("Hero");
        c_hero = GameObject.Find("Hero").GetComponent<ControllerPlayer>();
        PrefSmoke = (GameObject)Resources.Load("Prefabs/Gun_smoke", typeof(GameObject));
        PrefBullet = (GameObject)Resources.Load("Prefabs/Bullet", typeof(GameObject));
        myCamera = GameObject.Find("MainCamera/Camera");
        audioShoot = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioShoot", typeof(GameObject));
    }

    private void Update()
    {
        if (c_hero.equipActive)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;

            // Press Space to attack
            if (Input.GetButton("Attack1") && canShoot && c_hero.keyboardOn)
            {
                transform.GetComponent<Animator>().SetTrigger("Shoot");
                if (c_hero.move == 0 && c_hero.crouch == false)
                {
                    hero.GetComponent<Animator>().SetTrigger("Shoot");
                }
            }
        }

        if (c_hero.move != 0) GetComponent<Animator>().SetBool("Run", true);
        else GetComponent<Animator>().SetBool("Run", false);

        if (c_hero.crouch) GetComponent<Animator>().SetBool("Crouch", true);
        else GetComponent<Animator>().SetBool("Crouch", false);
    }

    public void Shoot()
    {
        correct = 3;
        if (hero.transform.localScale.x < 0) correct *= -1;
        canShoot = false;
        Vector3 newVec = new Vector3(transform.position.x + correct, transform.position.y, transform.position.z);
        GameObject tmp = Instantiate(PrefBullet, newVec, Quaternion.identity);
        tmp.GetComponent<ControllerBullet>().setBulletWay(EndBulletWay);
        if (hero.transform.localScale.x < 0)
        {
            Vector3 newVector = tmp.transform.localScale;
            newVector.x *= -1;
            tmp.transform.localScale = newVector;
            newVec = new Vector3(transform.position.x - correct, transform.position.y, transform.position.z);
            tmp.transform.position = newVec;
        }

        newVec = new Vector3(transform.position.x +1f, transform.position.y +0.5f, transform.position.z);
        tmp = Instantiate(PrefSmoke, newVec, Quaternion.identity);
        if (hero.transform.localScale.x < 0)
        {
            Vector3 newVector = tmp.transform.localScale;
            newVector.x *= -1;
            tmp.transform.localScale = newVector;
            newVec = new Vector3(transform.position.x - 1f, transform.position.y + 0.5f, transform.position.z);
            tmp.transform.position = newVec;
        }

        Destroy(tmp, 0.5f);
        if (c_hero.grounded) myCamera.GetComponent<Animator>().SetTrigger("shootShakeLite");
        else myCamera.GetComponent<Animator>().SetTrigger("shootShake");
        Destroy(Instantiate(audioShoot, transform.position, Quaternion.identity), 1f);
    }

    public void EndShoot()
    {
        canShoot = true;
    }
}
