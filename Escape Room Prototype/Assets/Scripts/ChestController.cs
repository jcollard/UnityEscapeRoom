using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{

    private Transform Lid
    {
        get => this.transform.Find("Lid");
    }

    private Vector3 EndPosition = new Vector3(0, 2.5f, 0.5f);
    private Quaternion EndRotation = Quaternion.Euler(-45, 180, 0);

    private Vector3 StartPosition = new Vector3(0, 1.5f, 0);
    private Quaternion StartRotation = Quaternion.Euler(0, 180, 0);
    public float OpenSpeed = 0.5f;
    private bool IsFinished = false;
    public bool IsOpen
    {
        get => StartTime > 0;
    }
    private float StartTime = -1f;
    private float EndTime = -1f;

    public void Update()
    {
        if (IsFinished || !IsOpen)
        {
            return;
        }
        float percent = (Time.time - StartTime) / (EndTime - StartTime);
        Lid.localPosition = Vector3.Lerp(StartPosition, EndPosition, percent);
        Lid.localRotation = Quaternion.Lerp(StartRotation, EndRotation, percent);
        if (percent >= 1)
        {
            IsFinished = true;
        }
    }

    public void Open()
    {
        Debug.Log("Called open?");
        if (!this.IsOpen)
        {
            this.StartTime = Time.time;
            this.EndTime = Time.time + this.OpenSpeed;
        }
    }



}
