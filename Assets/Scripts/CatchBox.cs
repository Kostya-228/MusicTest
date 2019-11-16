using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class CatchBox : MonoBehaviour
{
    static bool isCreateCirle = false;
    GameObject CathedBox;
    public int KeyNum;
    public GameObject img;
    public Canvas cnv;
    public HPController Controller;

    private void Start()
    {
        Controller = HPController.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        CathedBox = other.gameObject;
        if (isCreateCirle)
        {
            GameObject obj = Instantiate(img, transform.position, Quaternion.identity);
            obj.transform.SetParent(cnv.transform);
            obj.transform.localScale = new Vector3(0.66f, 0.43f, 0.66f);
            obj.transform.Rotate(90, 0, 0);
            StartCoroutine(wait());
            IEnumerator wait()
            {
                for (int i = 0; i < 30; i++)
                {
                    yield return new WaitForSeconds(0.05f);
                    obj.transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
                }
                Destroy(obj);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == CathedBox)
            CathedBox = null;
    }

    private void Update()
    {
        if (CathedBox)
        {
            if (Input.GetKeyDown((KeyCode)48 + KeyNum))
            {
                //Destroy(CathedBox);
                //CathedBox = null;
                CathedBox.transform.SetParent(transform);
                CathedBox.transform.localPosition = new Vector3();
                CathedBox.GetComponent<Rigidbody>().isKinematic = false;
                CathedBox.GetComponent<Rigidbody>().useGravity = true;
                if(Controller != null) Controller.UpdateWin();
            }
        }
    }
}
