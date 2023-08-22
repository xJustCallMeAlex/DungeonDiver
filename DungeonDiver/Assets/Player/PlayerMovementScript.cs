using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public int moveSpeed = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            gameObject.transform.Translate(Vector2.down * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            gameObject.transform.Translate(Vector2.up * Time.deltaTime * moveSpeed);
        }

    }
}
