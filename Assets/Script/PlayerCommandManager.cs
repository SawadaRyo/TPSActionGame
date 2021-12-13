using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCommandManager : MonoBehaviour
{
    [SerializeField] ItemInspector itemInspector;
    bool command;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(command && other.gameObject.tag == "Item")
        {
            //itemInspector.Items
        }
    }
}
