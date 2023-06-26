using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BulletHitEvent : UnityEvent<GameObject> {
}

public class Hittable : MonoBehaviour {
    public BulletHitEvent bulletHitEvent;

    private void Awake() {
        bulletHitEvent ??= new BulletHitEvent();
    }
}
