using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    private void Start() {
        Disable();
    }

    public void Enable() {
        // Works well enough for now
        gameObject.SetActive(true);
    }

    public void Disable() {
        // Works well enough for now
        gameObject.SetActive(false);
    }
}
