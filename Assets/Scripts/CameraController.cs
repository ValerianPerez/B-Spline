using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// The speed multiplicator
    /// </summary>
    public float speed = 5;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Translate(new Vector3(x, 0, z));

        if (Input.GetMouseButton(1))
        {
            float horizontal = Input.GetAxis("Mouse X");
            transform.Rotate(0, horizontal, 0);
            float vertical = Input.GetAxis("Mouse Y");
            transform.Rotate(-vertical, 0, 0);


            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }        
    }
}
