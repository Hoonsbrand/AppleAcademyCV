﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    public AudioClip appleSE;
    public AudioClip bombSE;
    AudioSource aud;
    GameObject director;


    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("GameDirector");
        this.aud = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Apple")
        {
            director.GetComponent<GameDirector>().GetApple();
            this.aud.PlayOneShot(this.appleSE);
        }
        else
        {
            director.GetComponent<GameDirector>().GetBomb();
            this.aud.PlayOneShot(this.bombSE);
        }
        Destroy(other.gameObject);
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                float x = Mathf.RoundToInt(hit.point.x); // 정수화 
                float z = Mathf.RoundToInt(hit.point.z);
                transform.position = new Vector3(x, 0.0f, z);
            }
        }
    }
}
