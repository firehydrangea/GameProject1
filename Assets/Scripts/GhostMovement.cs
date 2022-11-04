using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    //to do: code timers for different ghost behavior modes,
    //code multiple ghost AIs,
    //code automatic shutoff for hasOrb after time interval
    //(although this may be handled by a different script),
    //test movement more comprehensively.

    //in the unity project, the ghost has several attached game objects,
    //top, bottom, left, and right. Those aren't currently used in the code
    //but I didn't bother to delete them in case they come in handy.

    public float speed = 4f;
    //should maybe set speed for pacman faster than speed for ghosts

    public string direction = "";
    public string lastValidDirection = "";
    public Vector2 dirVec = Vector2.zero;
    public bool blockUp = false;
    public bool blockDown = false;
    public bool blockLeft = false;
    public bool blockRight = false;
    private Renderer renderer;
    private bool isInvisible;

    public bool hasOrb = false;
    public int numLives = 3;
    public string mode = "scatter";
    //-1 (false) for chase
    //0 (false) for scatter
    //1 (true) for frighten
    //scatter and chase are differentiated by what is passed to getDirection
    //corner for scatter, player for chase
    public float time = 0;


    public GameObject corner;
    //corner is the ghost's target when in scatter mode.
    //Currently, there is only one ghost and it is green.
    //its target is in the upper left corner.

    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        //direction = "up";
        //lastValidDirection = "right";
        direction = getDirection(corner.transform.position);
        lastValidDirection = direction;
        renderer = this.GetComponent<Renderer>();
        isInvisible = false;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (!hasOrb)
        {
            speed = 4.0f;
            if (time > 7)
            {
                mode = "chase";
                Debug.Log(mode);
            }
            else
            {
                mode = "scatter";
                Debug.Log(mode);
            }
        }
        else
        {
            speed = 2.0f;
            mode = "frighten";
            Debug.Log(mode);
        }
        dirVec = Vector2.zero;

        if (direction == "up" && !blockUp)
        {
            dirVec.y = 1;
        }
        if (direction == "down" && !blockDown)
        {
            dirVec.y = -1;
        }
        if (direction == "left" && !blockLeft)
        {
            dirVec.x = -1;
        }
        if (direction == "right" && !blockRight)
        {
            dirVec.x = 1;
        }
        if (dirVec == Vector2.zero)
        {
            if (mode == "chase")
            {
                direction = getDirection(Player.transform.position);
            }
            else
            {
                direction = getDirection(corner.transform.position);
            }
        }
        direction.Normalize();

        Vector3 newPosition = new Vector3(speed * dirVec.x * Time.deltaTime, speed * dirVec.y * Time.deltaTime, 0);
        this.transform.position += newPosition;

        transform.position *= WrapObject();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
        blockUp = false;
        blockDown = false;
        blockLeft = false;
        blockRight = false;
        if (gameObject.tag == "Maze")
        {
            if (direction == "up")
            {
                blockUp = true;
            }
            else if (direction == "down")
            {
                blockDown = true;
            }
            else if (direction == "left")
            {
                blockLeft = true;
            }
            else
            {
                blockRight = true;
            }
            direction = lastValidDirection;
            if (mode == "chase")   //chase mode
            {
                //direction = getDirection(Player.transform.position); //no player in project yet so commented out

            }
            else  //scatter mode and frighten mode; frighten mode doesn't use input variable and is versatile
            {
                direction = getDirection(corner.transform.position);

            }
        }
        if (gameObject.tag == "Player")
        {
            if (hasOrb)
            {
                //Destroy(this);
                //instead of destroying ghost and instantiating new one, could
                //set ghost's position to home and set directions to default:
                this.transform.position = new Vector3(0, 0, 0);
                //direction = "up";
                //lastValidDirection="right";
            }
            else
            {
                //Destroy(gameObject);
                gameObject.transform.position = new Vector3(0, 0, 0);
                numLives = numLives - 1;
                Debug.Log(numLives);
                Debug.Log(numLives);
                Debug.Log(numLives);
                Debug.Log(numLives);
                Debug.Log(numLives);

            }
        }
    }
    private Vector2 WrapObject()
    {
        if (renderer.isVisible)
        {
            isInvisible = false;
            return new Vector2(1, 1);
        }

        if (isInvisible)
        {
            return new Vector2(1, 1);
        }

        float xFix = 1;
        float yFix = 1;
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0 || pos.x > 1)
            xFix = -1;
        if (pos.y < 0 || pos.y > 1)
            yFix = -1;
        isInvisible = true;
        return new Vector2(xFix, yFix);
    }

    public string getDirection(Vector2 target)
    {
        Vector2 distance = new Vector2(target.x - transform.position.x, target.y - transform.position.y);
        //Vector2 distance = Vector2.zero; //this line is only to get rid of error in above line temporarily.
        string[] validDirections = new string[4];

        if (!blockUp && lastValidDirection != "down")
        {
            validDirections[0] = "up";
        }
        if (!blockDown && lastValidDirection != "up")
        {
            validDirections[1] = "down";
        }
        if (!blockLeft && lastValidDirection != "right")
        {
            validDirections[2] = "left";
        }
        if (!blockRight && lastValidDirection != "left")
        {
            validDirections[3] = "right";
        }
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {

        }
        if (mode == "frighten")
        {
            int choice = Random.Range(0, 3);
            while (validDirections[choice] == null)
            {
                ++choice;
                choice = choice % 4;
            }
            lastValidDirection = direction;
            return validDirections[choice];
        }
        else
        {
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                if (distance.x > 0 && !blockRight)
                {
                    direction = "right";
                }
                else if (distance.x < 0 && !blockLeft)
                {
                    direction = "left";
                }
                else if (distance.y > 0 && !blockUp)
                {
                    direction = "up";
                }
                else
                {
                    direction = "down";
                }
            }
            else
            {
                if (distance.y > 0 && !blockUp)
                {
                    direction = "up";
                }
                else if (distance.y < 0 && !blockDown)
                {
                    direction = "down";
                }
                else if (distance.x > 0 && !blockRight)
                {
                    direction = "right";
                }
                else
                {
                    direction = "left";
                }
            }
            return direction; //here to prevent error while coding in progress

            //return direction; //here to prevent error while coding in progress
        }


    }
}
