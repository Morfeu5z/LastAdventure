using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourJellyFish : MonoBehaviour {

    public float MaxDistance = 10;
    public float MinDistance = 2;
    public float Scale = 1;
    public float ProportionX = 1;
    public float Speed = 0.1f;
    public bool OnlyBack = false;
    private float DistanceRandom;
    private float SpeedCopy;
    private string switcher = "Fly";
    private GameObject body;

    // Use this for initialization
    void Start () {
        body = transform.parent.gameObject;
        RandomProp();
        SpeedCopy = Speed;
        body.transform.localPosition = new Vector3(body.transform.localPosition.x, body.transform.localPosition.y + Random.Range(0, DistanceRandom), 0);
    }
	
	// Update is called once per frame
	void Update () {
        switch (switcher)
        {
            case "Fly":
                if (Speed > 0.0001f) Speed -= 0.00005f;
                if (body.transform.localPosition.y > DistanceRandom)
                {
                    switcher = "Fall";
                    transform.GetComponent<Animator>().SetBool("Fly", false);
                    Speed = SpeedCopy * -1;
                }
                break;
            case "Fall":
                if (body.transform.localPosition.y <= 0)
                {
                    switcher = "Fly";
                    transform.GetComponent<Animator>().SetBool("Fly", true);
                    Speed = SpeedCopy;
                    DistanceRandom = Random.Range(MinDistance, MaxDistance);
                    RandomProp();
                }
                break;
        }
        body.transform.localPosition = new Vector3(body.transform.localPosition.x, body.transform.localPosition.y + Speed, 0);
    }

    public void RandomProp()
    {
        int order = (Random.Range(0, 10) < 5) ? -25 : 25;
        if (OnlyBack) order = -25;
        transform.GetComponent<SpriteRenderer>().sortingOrder = order;

        Color col = transform.GetComponent<SpriteRenderer>().color;
        col.a = (order < 0) ? Random.Range(0.05f, 0.3f) : Random.Range(0.2f, 0.5f);
        transform.GetComponent<SpriteRenderer>().color = col;

        float xy = (order < 0) ? Random.Range(1.5f, 3) : Random.Range(0.2f, 1.5f);

        DistanceRandom = (xy < 1.5f) ? Random.Range(MinDistance*xy, MaxDistance*xy) : Random.Range(MinDistance, MaxDistance);

        body.transform.parent.transform.localScale = new Vector3((xy * Scale) * ProportionX, xy * Scale, 0);
    }

    public void ResetSpeed()
    {
        Speed = SpeedCopy;
    }
}
