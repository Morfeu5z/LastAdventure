using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

    public GameObject ObjectToDestroy;
    private GameObject PrefExplo;

    void Start () {
        PrefExplo = (GameObject)Resources.Load("Prefabs/Explode", typeof(GameObject));
        if(!ObjectToDestroy) ObjectToDestroy = gameObject;
    }

    public void ExplodeMe()
    {
        Vector3 newVec = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Destroy(Instantiate(PrefExplo, newVec, Quaternion.identity), 1f);
        Destroy(ObjectToDestroy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ExplodeMe();
        }
    }
}
