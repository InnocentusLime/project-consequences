using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class InteractEvent : UnityEvent<GameObject> {
}

public class Interactable : MonoBehaviour {
    public UnityEvent<GameObject> interactionEvent;

    private void Awake() {
        interactionEvent ??= new InteractEvent();
    }
}
