using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;
using UnityEngine.EventSystems;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private MovementScript pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatisGrappleable;
    public LineRenderer lr;
    public Item grapple;
    private PlayerInventory playerInventory;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    private bool grappling;
    private Vector3 grapplePoint;
    public float overshootYAxis;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    


    private void Start()
    {
        pm = GetComponent<MovementScript>();
        playerInventory = GetComponent<PlayerInventory>();
        grapple.Id = 4;//defines the grapple item
    }

    private void StartGrapple()
    {
        
        grappling = true;
        pm.freeze = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatisGrappleable))
        {//starts grapple
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {//cancels grapple (object not in range to grapple)
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }
        
    private void ExecuteGrapple()
    {
        pm.freeze = false; //freezes the movement of the player

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis; //if grapple is below the player
        pm.JumpToPosition(grapplePoint, highestPointOnArc);

    }



    public void StopGrapple()
    {
        pm.freeze = false;//player able to move again
        grappling = false;

        grapplingCdTimer = grapplingCd;// cooldown starts
        
        lr.enabled = false;


        
    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey) && playerInventory.InEquipment(grapple)) //if there is a grapple in
                                                                                  //inventory
        {
            StartGrapple();

        }
        if (grapplingCdTimer>0)
        {
            grapplingCdTimer -= Time.deltaTime;//cooldown gets less per frame that the player is not grappling
        }
    }

    private void LateUpdate()
    {
        if(grappling)
        {
            lr.SetPosition(0, gunTip.position);//grapple visuals
        }
    }

    

}

