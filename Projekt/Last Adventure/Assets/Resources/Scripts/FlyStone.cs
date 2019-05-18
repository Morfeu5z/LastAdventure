using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyStone : MonoBehaviour {

    private float Height = 0.1f;
    private float Speed = 0.0001f;
    private float ZeroHeight = 0;

    private void Start()
    {
        Vector3 newVec = transform.localPosition;
        newVec.y += Random.Range(0, Height) * Random.Range(-1,1);
        transform.localPosition = newVec;
        ZeroHeight = transform.localPosition.y;
    }

    private void FixedUpdate()
    {
        Vector3 newVec = transform.localPosition;
        newVec.y += Speed;
        transform.localPosition = newVec;
        if (transform.localPosition.y > ZeroHeight + Height)
        {
            Speed *= -1;
        }
        else if (transform.localPosition.y < ZeroHeight - Height)
        {
            Speed *= -1;
        }
	}
}
