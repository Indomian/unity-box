using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activable : MonoBehaviour
{
    protected bool _active = false;

    public virtual void activate() {
        _active = true;
    }

    public virtual void deactivate() {
        _active = false;
    }

    public virtual void Start() {
         if (_active) {
            activate();
        } else {
            deactivate();
        }
    }
}
