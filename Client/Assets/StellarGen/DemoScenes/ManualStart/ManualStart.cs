using UnityEngine;
using System.Collections;

/**
 * This script shows how to manually start StellarGen.
 */
public class ManualStart : MonoBehaviour
{
    void Awake()
    {
        //Starts StellarGen.
        GameObject.FindGameObjectWithTag("StellarGenController").GetComponent<StellarGen.StellarGenController>().Setup();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), ":: StellarGen Demo ::");
        GUI.Label(new Rect(10, 40, 400, 30), "Press DELETE to stop StellarGen.");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            //Stop StellarGen.        
            GameObject.FindGameObjectWithTag("StellarGenController").GetComponent<StellarGen.StellarGenController>().Terminate();
        }
    }
}
