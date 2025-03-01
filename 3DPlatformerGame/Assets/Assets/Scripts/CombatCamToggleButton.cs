using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCamToggleButton : MonoBehaviour
{
    private Switching switching;
    public MonoBehaviour combatcamscript;
    public MonoBehaviour throwingscript;
    public MonoBehaviour grapplingscript;
    public CinemachineFreeLook firstCamera;
    public CinemachineVirtualCamera secondCamera;

    public bool CombatCamActive = true;

    void Start()
    {
        // Ensure both cameras are initially enabled or disabled as needed
        if (firstCamera != null)
            firstCamera.enabled = false;

        if (secondCamera != null)
            secondCamera.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {//toggles between cinemachine freelook and virtual cameras
            ToggleCameras();
        }
    }

    public void ToggleCameras()
    {
        CombatCamActive = !CombatCamActive;
        //disables the scrit which cannot be used in third person camera mode
        firstCamera.enabled = !firstCamera.enabled;
        combatcamscript.enabled = !combatcamscript.enabled;
        throwingscript.enabled = !throwingscript.enabled;
        grapplingscript.enabled = !grapplingscript.enabled;
        secondCamera.enabled = !secondCamera.enabled;
    }


}