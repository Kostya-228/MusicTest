using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyTrigger : MonoBehaviour
{

    public List<GameObject> inZone = new List<GameObject>();
    public bool isGamer;
    public bool DestrOnClick;
    public bool HPLose = false;
    public int KeyNum;
    public HPController controller;

    private void Start()
    {
        controller = HPController.instance;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isGamer)
        {
            inZone.Add(other.gameObject);
            foreach (GameObject obj in inZone)
            {
                Destroy(obj);
                if (HPLose && controller != null) controller.UpdateLose();
            }
            inZone = new List<GameObject>();
        }
    }

    private void OnMouseDown()
    {
        if (DestrOnClick)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inZone.Remove(other.gameObject);
    }

    private void Update()
    {
        if (inZone.Count > 0)
            if (Input.GetKeyDown((KeyCode)48 + KeyNum))
            {
                foreach (GameObject obj in inZone)
                {
                    Destroy(obj);
                    //if (controller != null) controller.UpdateWin();
                }
                inZone = new List<GameObject>();
            }
    }



}
