using UnityEngine;
using System.Collections;
using Puzzle;
using UI;

public class FloatingPlatform : MonoBehaviour
{
    public Vector3[] positions;
    public Vector3[] rotations;
    public float moveSpeed = 3;
    public int CurrentPoint = 0;
    public float WaitTime = 3;
    public float tolarance = 0.1f;
    public GameUI playerUI;

    private PuzzleDevice puzzout;
    private Rigidbody rigid;
    private Vector3 dir;
    private float wait;

    private void Start()
    {
        puzzout = GetComponent<PuzzleDevice>();
        rigid = GetComponent<Rigidbody>();
        wait = 0;

        puzzout.active = false;
    }

    void Update() {
        if (playerUI != null && !playerUI.Active) {
            rigid.velocity = Vector3.zero;
            return;
        }

        if (puzzout.active == true) {
            dir = (positions[CurrentPoint] - transform.position).normalized;

            if (wait >= WaitTime)  {
                rigid.velocity = dir * moveSpeed * Time.deltaTime;
                rigid.constraints = RigidbodyConstraints.FreezeRotation;
            } else {
                rigid.constraints = RigidbodyConstraints.FreezeAll;
                wait += Time.deltaTime;
            }

        } else  // Freeze when not active
            rigid.constraints = RigidbodyConstraints.FreezeAll;

        if ((transform.position - positions[CurrentPoint]).magnitude <= tolarance)
        {
            CurrentPoint = (CurrentPoint + 1) % positions.Length;
            dir = Vector3.zero;
            rigid.velocity = Vector3.zero;
            wait = 0;
        }
        

        puzzout.active = puzzout.IsEnabled();
    }
}