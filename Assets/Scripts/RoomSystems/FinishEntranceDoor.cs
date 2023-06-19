using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishEntranceDoor : MonoBehaviour, IAdjacentDoor
{
    public void OnPlayerEnter(GameObject player) {
        Debug.Log("Location finish");

        /* Code for ending the zone/level/game */
    }
}
