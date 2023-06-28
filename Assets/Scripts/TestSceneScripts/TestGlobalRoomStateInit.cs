using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGlobalRoomStateInit : MonoBehaviour {
    private void Awake() {
        GlobalRoomState.player = FindFirstObjectByType<PlayerMovement>().gameObject;
    }
}
