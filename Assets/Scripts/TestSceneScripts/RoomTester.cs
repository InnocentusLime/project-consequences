using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class RoomTester : MonoBehaviour {
    [Header("Scenes")]
    public SceneAsset scene1;
    public SceneAsset scene2;

    private Room[] rooms;
    private ExitDoor[] exitDoors;
    private IEntranceDoor[] entranceDoors;

    // Start is called before the first frame update
    void Start() {
        SceneManager.LoadScene(scene1.name, LoadSceneMode.Additive);
        SceneManager.LoadScene(scene2.name, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update() {
        if (rooms == null) {
            Room room1 = Room.FindInScene(SceneManager.GetSceneAt(1));
            Room room2 = Room.FindInScene(SceneManager.GetSceneAt(2));

            if (room1 == null || room2 == null) {
                return;
            }

            rooms = new[] { room1, room2 };
            room1.GetComponentInChildren<ExitDoor>().adjacentDoor = room2.GetComponentInChildren<IEntranceDoor>();
            room2.GetComponentInChildren<ExitDoor>().adjacentDoor = GetComponent<IEntranceDoor>();

            rooms[0].GetComponentInChildren<IEntranceDoor>().Enter();

            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            rooms[0].GetComponentInChildren<ExitDoor>().Invoke("Interact", 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            rooms[1].GetComponentInChildren<ExitDoor>().Invoke("Interact", 0.0f);
        }
    }
}
