using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class RoomTester : MonoBehaviour {
    [Header("Scenes")]
    public GameObject player;
    public SceneAsset scene1;
    public SceneAsset scene2;

    private bool roomsReady;
    private FinishEntranceDoor finishEntranceDoor;

    // Start is called before the first frame update
    void Start() {
        finishEntranceDoor = GetComponent<FinishEntranceDoor>();
        SceneManager.LoadScene(scene1.name, LoadSceneMode.Additive);
        SceneManager.LoadScene(scene2.name, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update() {
        // FIXME ugly. We need a better working loader for the final game anyway.
        if (!roomsReady) {
            Room room1 = Room.FindInScene(SceneManager.GetSceneAt(1));
            Room room2 = Room.FindInScene(SceneManager.GetSceneAt(2));

            if (room1 == null || room2 == null) {
                return;
            }

            // Connect the rooms
            room1.exitDoor.playerLeaveEvent.AddListener(room2.entranceDoor.OnPlayerLeaveOldRoom);
            room2.exitDoor.playerLeaveEvent.AddListener(finishEntranceDoor.OnPlayerEnter);

            // Start the first room
            room1.entranceDoor.OnPlayerLeaveOldRoom(player);

            roomsReady = true;
        }
    }
}
