using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ItemInspector : MonoBehaviour
{
    [SerializeField] Transform player;

    bool command = false;
    ItemCommand itemCommand;
    ItemInspector itemInspector;
    GameObject itemPrefab;
    Text itemMnew;
    Dictionary<int,Object> items = new Dictionary<int, Object>();
    public Dictionary<int,Object> Items { get => items; set => items = value; }

    private void Start()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (command)
        {
            if (itemCommand == ItemCommand.USE)
            {
                //itemInspector.Items.Remove();
            }
            else if (itemCommand == ItemCommand.PUT)
            {
                Instantiate(itemPrefab, player);
                //itemInspector.Items.Remove();
            }
            else if (itemCommand == ItemCommand.DESTORY)
            {
                //itemInspector.Items.Remove();
            }
        }
    }
    void OnCommand(InputValue inputValue)
    {
        command = inputValue.isPressed;
    }
    enum ItemCommand
    {
        USE,
        DESTORY,
        PUT
    }
}
