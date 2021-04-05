using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement: MonoBehaviour
{
    public Transform player;
    public float shiftDistX, shiftDistY;
    public float shiftTime = 0.4f;
    private float px, py, cx, cy, cz;
    private bool moving;
    // Start is called before the first frame update
    void Start() {
        cx = transform.position.x;
        cy = transform.position.y;
        cz = transform.position.z;
        moving = false;
    }

    // Update is called once per frame
    void Update() {
        px = player.position.x;
        py = player.position.y;
        if(!moving && px - cx > shiftDistX) {
            shiftRight();
        }
        if(!moving && cx - px > shiftDistX) {
            shiftLeft();
        }
        if(!moving && py - cy > shiftDistY) {
            shiftUp();
        }
        if(!moving && cy - py > shiftDistY) {
            shiftDown();
        }
    }

    private void shiftRight() {
        StartCoroutine(shift(new Vector3(cx + 2 * shiftDistX, cy, cz)));
    }

    private void shiftLeft() {
        StartCoroutine(shift(new Vector3(cx - 2 * shiftDistX, cy, cz)));
    }

    private void shiftUp() {
        StartCoroutine(shift(new Vector3(cx, cy + 2 * shiftDistY, cz)));
    }

    private void shiftDown() {
        StartCoroutine(shift(new Vector3(cx, cy - 2 * shiftDistY, cz)));
    }

    IEnumerator shift(Vector3 destination) {
        moving = true;
        Vector3 velocity = Vector3.zero;
        float startTime = Time.time;
        Vector3 fakeTarget = (destination - transform.position) * .7f + destination;
        while(Time.time - startTime < shiftTime) {
            transform.position = Vector3.SmoothDamp(transform.position, fakeTarget, ref velocity, shiftTime);
            yield return 0;
        }
        transform.position = destination;
        cx = destination.x;
        cy = destination.y;
        moving = false;
    }
}
