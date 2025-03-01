using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Switching : MonoBehaviour
{
    public Canvas player2inventory; 
    public GameObject character1;
    public GameObject character2;
    public bool p1;
    public bool p2;

    public Canvas healthbar1;
    public Canvas healthbar2;
    
    public Cinemachine.CinemachineFreeLook activeCameraFreeLook;
    public Cinemachine.CinemachineVirtualCamera activeCameraCombat;

    void Start()
    {
        healthbar1.enabled = true;
        healthbar2.enabled = false;
        p1 =true; 
        p2 =false;
        Toggle();
        Toggle();
        player2inventory.enabled = false;
        //bug with the inventory being enabled at the start of the scene
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            Toggle();
        }
    }
    void Toggle()
    {

        p1 = !p1; 
        p2 = !p2;
        if(p1 == false)
        {
            healthbar1.enabled = false;
            healthbar2.enabled = true;
            Components(character1, false);
            Cameras(character2);
            Components(character2, true);
        }
        else
        {
            healthbar1.enabled = true;
            healthbar2.enabled = false;
            Components(character2, false);
            Cameras(character1);
            Components(character1, true);
        }

    }
    private void Components(GameObject character, bool state)
    {
        character.GetComponent<Animator>().SetBool("isGrounded", true);//disable all animations except idle
        character.GetComponent<Animator>().SetBool("isJumping", false);
        character.GetComponent<Animator>().SetBool("isFalling", false);
        character.GetComponent<Animator>().SetBool("isMoving", false);

        character.GetComponent<MovementScript>().enabled = state;
        //character.GetComponent<CharacterController>().enabled = state;
        character.GetComponent<ThrowingBehaviour>().enabled = state;
        character.GetComponent<Grappling>().enabled = state;
        character.GetComponent<CombatCamera>().enabled = state;
        character.GetComponent<CombatCamToggleButton>().enabled = state;
        character.GetComponent<PlayerInventory>().enabled = state;
    }
    private void Cameras(GameObject character)
    {
        activeCameraCombat.Follow = character.transform.Find("CombatLookAt");
        activeCameraCombat.LookAt = character.transform.Find("CombatLookAt");

        activeCameraFreeLook.Follow = character.transform;
        activeCameraFreeLook.LookAt = character.transform;

        activeCameraCombat.enabled = true;
        activeCameraFreeLook.enabled = false;

    }

}
