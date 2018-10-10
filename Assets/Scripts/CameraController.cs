using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// The speed multiplicator
    /// </summary>
    public float speed = 5;

    /// <summary>
    /// The sphere grabbed
    /// </summary>
    private GameObject grabbedObject;

    /// <summary>
    /// The offset when grabbing
    /// </summary>
    private Vector3 grabbedOffset;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Translate(new Vector3(x, 0, z));

        //Right click
        if (Input.GetMouseButton(1))
        {
            float horizontal = Input.GetAxis("Mouse X");
            transform.Rotate(0, horizontal, 0);
            float vertical = Input.GetAxis("Mouse Y");
            transform.Rotate(-vertical, 0, 0);


            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }
        //Left click press
        else if (Input.GetMouseButtonDown(0))        
        {
            RaycastHit hit;
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Point"))
                {
                    grabbedObject = hit.collider.gameObject;
                }
            }
        }
        //Left click released
        else if (Input.GetMouseButtonUp(0))
        {
            grabbedObject = null;
        }

        if (grabbedObject)
        {
            //TODO : Moving object
        }
    }
}
