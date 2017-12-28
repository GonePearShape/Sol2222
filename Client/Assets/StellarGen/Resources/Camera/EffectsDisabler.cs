using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Script that enables or disable visual effects.
 */
public class EffectsDisabler : MonoBehaviour
{	
	void Update ()
    {
        //Enable / Disable specified components
        if (Input.GetKeyDown(controle_key))
        {
            foreach(MonoBehaviour component in this.components)
            {
                component.enabled = !component.enabled;
            }
        }
	}

    //******************************************************************

    /**
     * List of components to be activated/deactivated.
     */
    public List<MonoBehaviour> components = new List<MonoBehaviour>();

    /**
     * Activation control key.
     */
    public KeyCode controle_key = KeyCode.F1;
}
