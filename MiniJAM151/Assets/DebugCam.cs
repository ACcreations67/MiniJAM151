using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCam : MonoBehaviour
{

    [SerializeField] float sensitivity = 3;
    [SerializeField] float speed = .5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.Euler((new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * sensitivity) + transform.rotation.eulerAngles);
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * speed;
        }
    }
}
