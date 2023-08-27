using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotationSpeed;

    private Animator animat;

    // Start is called before the first frame update
    void Start()
    {
        animat = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movmentDir = new Vector2(horizontalInput, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movmentDir.magnitude);
        movmentDir.Normalize();

        transform.Translate(movmentDir * speed * inputMagnitude * Time.deltaTime, Space.World);

        if (movmentDir != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movmentDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            animat.SetFloat("Speed", 1);
        } else
        {
            animat.SetFloat("Speed", 0);
        }

        
    }
}
