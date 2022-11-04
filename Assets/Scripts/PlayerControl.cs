using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //private SpriteRenderer turning;

    public float speed = 0.01f;
    public float lookSpeed = 180f;

    private float rotationAmount = 0;
    private float actualSpeed = 0;

    private Vector2 direction;
    public float lerpConstant = 0.001f;
    private float localSpeed;
    private bool turning = false;
    private GhostMovement ghosting;
    private bool wall = false;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        GhostMovement ghostMovement = GetComponent<GhostMovement>();
        this.ghosting = ghostMovement;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            
            direction.y = 1;
            direction.x = 0;
            
            //StartCoroutine(rotatePac());
            //localSpeed += speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //turning = false;
            direction.x = 1;
            direction.y = 0;
            
            //StartCoroutine(rotatePac());
            //localSpeed += speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
            direction.y = 0;
            //localSpeed += speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1;
            direction.x = 0;
            //localSpeed += speed;
        }
        if (direction != Vector2.zero)
        {
            direction.Normalize();
        }
        else
        {
            return;
        }

        //Mathf.Clamp(localSpeed, 0, 40);
        //localSpeed = Mathf.Lerp(localSpeed, 0, 0.1f);
        //Vector3 newPosition = new Vector3(localSpeed * direction.x * Time.deltaTime, localSpeed * direction.y * Time.deltaTime, 0);
        Vector3 newPosition = new Vector3(speed * direction.x * Time.deltaTime, speed * direction.y * Time.deltaTime, 0);
        this.transform.position += newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if (gameObject.tag == "PowerUp")
        {
            //ghosting.hasOrb = true;
        }
        if(gameObject.tag == "Maze")
        {
            Vector3 newPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            this.transform.position = newPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(gameObject.tag == "Maze")
        {
            wall = false;
        }
    }

    IEnumerator rotatePac()
    {
        if (!turning)
        {
            transform.Rotate(Vector3.forward * -90);
            turning = true;
        }
        yield return new WaitForSeconds(1);
        turning = false;
    }
}
