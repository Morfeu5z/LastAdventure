using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    public bool RandomOrder = false;
    public float speed = 0.002f;
    private Vector3 pos;
    private float reduct = 0;
    private Vector3 start, end;

    private void Start()
    {
        reduct = Random.Range(0, 0.01f);
        start = transform.parent.transform.GetChild(0).gameObject.transform.position;
        end = transform.parent.transform.GetChild(1).gameObject.transform.position;
        if (RandomOrder == true) transform.GetComponent<SpriteRenderer>().sortingOrder = orderLayer();
    }

    // Update is called once per frame
    void Update() {
        pos = new Vector3(transform.position.x + speed + reduct, transform.position.y, transform.position.z);
        if (transform.position.x > end.x)
        {
            pos = new Vector3(start.x, transform.position.y, transform.position.z);
            if(RandomOrder == true) transform.GetComponent<SpriteRenderer>().sortingOrder = orderLayer();
        }
        transform.position = pos;
    }

    public int orderLayer(){
        reduct = Random.Range(0, 0.01f);
        int order = Random.Range(0, 6);
        if (order == 0 || order == 2 || order == 10)
        {
            return -20;
        }
        else
        {
            return 20;
        }
    }
}
