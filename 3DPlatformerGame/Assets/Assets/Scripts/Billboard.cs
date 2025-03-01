using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera _camera;
    private void Start()
    {
        _camera = Camera.main; //seperate variable holding camera.main as if this was in update
                               //it takes up a lot of uneccesary processing power to find camera.main
                               //every frame
    }
    private void LateUpdate()
    {
        transform.forward= _camera.transform.forward;
    }
}
