using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    private bool isOpen, isOpenable;
    private Vector3 location;
    public GameObject openChest;

    private GameObject player;
    private CommandReceiver commandReceiver;

    //bool canOpen;


    void Start()
    {
        isOpen = false;
        isOpenable = false;
        location = transform.position;

        player = GameObject.FindWithTag("Player");
        commandReceiver = player.GetComponent<CommandReceiver>();
    }

    void Update()
    {
        if (!isOpen && isOpenable && commandReceiver.canOpen)
        {
           
            GameObject.Destroy(gameObject);
            GameObject.Instantiate(openChest, location, Quaternion.identity);
            GameManager.Instance.ChestOpened();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isOpenable = true;
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isOpenable = false;
    }


}
