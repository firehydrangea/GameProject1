using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{
    // Start is called before the first frame update

    //public Manager manager;
    public int presentPoints = 100;
    private AudioSource waka;

    void Start()
    {
        waka = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            if (!waka.isPlaying)
            {
                waka.PlayOneShot(waka.clip, 0.05f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if (gameObject.tag == "Player")
        {
            //manager.incrementScore(presentPoints);
            
            Destroy(this.gameObject);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.tag == "Player" && !waka.isPlaying)
        {
            waka.PlayOneShot(waka.clip, 0.5f);
        }
    }
}
