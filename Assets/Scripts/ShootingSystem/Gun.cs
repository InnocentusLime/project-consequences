using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Gun : MonoBehaviour {
    public bool allowedToShoot = true;
    private bool isCoolDowned;
    public float coolDownDuration = 0.8f;
    public GameObject bullet;
    private Camera cam;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
        Assert.IsNotNull(cam);
    }

    // Update is called once per frame
    private void Update() {
        if (!Input.GetMouseButtonDown(0) || !allowedToShoot || isCoolDowned) {
            return;
        }

        Vector2 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 player = transform.localPosition;
        float angle = Vector2.SignedAngle(Vector2.up, mouse - player);
        Instantiate(bullet, player, Quaternion.AngleAxis(angle, Vector3.forward));
        isCoolDowned = true;
        StartCoroutine(CoolDownRoutine(coolDownDuration));
    }

    private IEnumerator CoolDownRoutine(float duration) {
        yield return new WaitForSeconds(duration);
        isCoolDowned = false;
    }
}
