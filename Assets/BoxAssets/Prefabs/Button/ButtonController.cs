using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public List<Activable> controls;

    void ApplyActivate(Activable control) {
        control.activate();
    }

    void ApplyDeactivate(Activable control) {
        control.deactivate();
    }

    void OnTriggerEnter (Collider col)
    {
        Debug.Log("Button down for: " + col.tag);
        if (col.GetComponent<Activator>()) {
            controls.ForEach(ApplyActivate);
        }
    }

    void OnTriggerExit (Collider col)
    {
        Debug.Log("Button up for: " + col.tag);
        if (col.GetComponent<Activator>()) {
            controls.ForEach(ApplyDeactivate);
        }
    }
}
