using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCamera : MonoBehaviour
{
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camFollowPos;//serialized field so can be seen in editor


    private void Update()
    {
        xAxis.Update(Time.deltaTime); //time.delta time means the frame updates are consistent
                                      //and therefore doesnt move faster if you are getting more frames
        yAxis.Update(Time.deltaTime);
    }
    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }
}
