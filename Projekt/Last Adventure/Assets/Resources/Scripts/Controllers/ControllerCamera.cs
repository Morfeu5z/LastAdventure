using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCamera : MonoBehaviour
{

    private GameObject hero;
    private GameObject target;
    private Camera cam;
    private float defaultSize;
    private GameObject lastTarget;
    public float LastFocus = 20;
    private Vector2 velocity;
    private float X;
    private float Y;
    private float addToY        = 1f;
    private float SmoothX       = 0.4f;
    private float SmoothY       = 0.2f;
    private float RememberSmoothY= 0.2f;
    private float zoomSpeed     = 0.1f;
    private bool cameraResize    = true;
    private bool followX         = true;
    private bool followY         = true;
    private bool equipFocus      = false;
    public  bool UpdateBorder    = false;
    public  bool specialFocus    = false;

    // Use this for initialization
    void Start()
    {
        hero    = GameObject.Find("Hero/CameraPoint");
        target  = hero;
        cam     = transform.GetChild(0).GetComponent<Camera>();
        defaultSize = 30;
    }

    void FixedUpdate()
    {
        if (followX) X = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, SmoothX);
        if (followY) Y = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref velocity.y, SmoothY);
        transform.position = new Vector3(X, Y + addToY, transform.position.z);
        

        if (cameraResize)
        {
            if (defaultSize > cam.orthographicSize)
            {
                cam.orthographicSize += zoomSpeed;
                if (cam.orthographicSize >= defaultSize)
                {
                    zoomChanged();
                }
            }
            else if (defaultSize < cam.orthographicSize)
            {
                cam.orthographicSize -= zoomSpeed;
                if (cam.orthographicSize <= defaultSize)
                {
                    zoomChanged();
                }
            }
        }
    }


    public void zoomChanged()
    {
        cameraResize = false;
        if (specialFocus) specialFocus = false;
    }

    public void CamZoom(float zoom, float speed)
    {
        if(zoom < 20)
        {
            SmoothX = 0.12f;
            SmoothY = 0.12f;
        }
        zoomSpeed = speed;
        defaultSize = zoom;
        cameraResize = true;
    }

    public void Focus(GameObject newTarget, float newZoom, float speed)
    {
        if (!equipFocus)
        {
            target = newTarget;
            SmoothX = 0.8f;
            SmoothY = 0.8f;
            CamZoom(newZoom, speed);
        }
    }
    public void PlayerFocus(float newZoom, float speed)
    {
        specialFocus = true;
        target = hero;
        SmoothX = 0.4f;
        SmoothY = 0.2f;
        CamZoom(newZoom, speed);
    }

    public void eqFocus(bool eqf)
    {
        equipFocus = eqf;
        float localspeed = 0;
        if (equipFocus)
        {
            localspeed = Mathf.Abs(defaultSize - 20) / 30;
            if (target.tag != "Player")
            {
                lastTarget = target;
                target = hero;
            }
            LastFocus = defaultSize;
            CamZoom(20, localspeed);
            RememberSmoothY = SmoothY;
            SmoothY = 0.1f;
        }
        else
        {
            SmoothY = RememberSmoothY;
            localspeed = Mathf.Abs(LastFocus - 20) / 30;
            target = lastTarget;
            defaultSize = LastFocus;
            CamZoom(LastFocus, localspeed);
        }
    }
}
