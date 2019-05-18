using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBullet : MonoBehaviour {

    private float Fly = 2f;
    private GameObject PrefExplo;
    public GameObject PrefSparks;
    private float start;
    private float sparkPos;
    private float end = 100;
    public bool specialAmmo = false;
    public float demage = 1;
    public Color ColorStandardAmmo;
    public Color ColorSpecialAmmo;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("Hero").transform.position.x > GameObject.Find("Hero/Weapon/Gun(Clone)").transform.position.x) {
            Fly *= -1;
        }
        PrefExplo = (GameObject)Resources.Load("Prefabs/Explode", typeof(GameObject));
        PrefSparks = (GameObject)Resources.Load("Prefabs/BulletSparks", typeof(GameObject));

        if (GameObject.Find("Config").GetComponent<Config>().CheckActiveItem("specialAmmo")) {
            transform.GetComponent<SpriteRenderer>().color = ColorSpecialAmmo;
            specialAmmo = true;
        } else {
            transform.GetComponent<SpriteRenderer>().color = ColorStandardAmmo;
            specialAmmo = false;
        }
        start = transform.position.x;
        sparkPos = 0;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x + Fly, transform.position.y, transform.position.z);
        sparkPos++;
        if(Mathf.Abs(transform.position.x - start) > end) Explode();
        if (sparkPos > 1f)
        {
            //Debug.Log("Spark");
            sparkPos = 0;
            Vector3 newVec = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject tmp = Instantiate(PrefSparks, newVec, Quaternion.identity);

            if (specialAmmo)
            {
                tmp.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ColorSpecialAmmo;
            }
            else
            {
                tmp.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ColorStandardAmmo;
            }
            Destroy(tmp, 1f);
        }
	}

    public void setBulletWay(float x)
    {
        end = x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            switch (collision.tag)
            {
            case "Golem":
                demage = (specialAmmo) ? 4 : 1;
                if (specialAmmo) collision.GetComponent<BehaviourGolem>().DropLife(demage);
                Explode();
                break;

            case "Shild":
                Explode();
                break;

            case "Enemy":
                demage = (specialAmmo) ? 4 : 1;
                Explode();
                if(collision.name == "Slime") collision.GetComponent<BehaviourSlime>().DropLife(demage);
                break;

        }
        if (collision.gameObject.layer == 31)
        {
            Explode();
        }
    }

    public void Explode()
    {
        Vector3 newVec = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Destroy(Instantiate(PrefExplo, newVec, Quaternion.identity), 1f);
        Destroy(gameObject);
    }
}
