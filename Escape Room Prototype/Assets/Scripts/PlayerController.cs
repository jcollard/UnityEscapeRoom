using UnityEngine;
using CaptainCoder.TileBuilder;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public int X;
    public int Y;

    [SerializeField]
    private TileSide _Facing = TileSide.North;
    public TileSide Facing
    {
        get => this._Facing;
        set
        {
            if (value == TileSide.Top || value == TileSide.Bottom || value == _Facing)
            {
                return;
            }
            this._Facing = value;
            this.UpdatePosition();
        }
    }

    private static readonly Dictionary<TileSide, (int, int)> MoveLookup = new Dictionary<TileSide, (int, int)>();
    private static readonly Dictionary<TileSide, (float, float)> PositionLookup = new Dictionary<TileSide, (float, float)>();
    private static readonly Dictionary<TileSide, Quaternion> RotationLookup = new Dictionary<TileSide, Quaternion>();

    static PlayerController()
    {
        MoveLookup[TileSide.North] = (0, 1);
        MoveLookup[TileSide.East] = (1, 0);
        MoveLookup[TileSide.South] = (0, -1);
        MoveLookup[TileSide.West] = (-1, 0);

        PositionLookup[TileSide.North] = (0, -5);
        PositionLookup[TileSide.South] = (0, 5);
        PositionLookup[TileSide.East] = (-5, 0);
        PositionLookup[TileSide.West] = (5, 0);

        RotationLookup[TileSide.North] = Quaternion.Euler(0, 0, 0);
        RotationLookup[TileSide.East] = Quaternion.Euler(0, 90, 0);
        RotationLookup[TileSide.South] = Quaternion.Euler(0, 180, 0);
        RotationLookup[TileSide.West] = Quaternion.Euler(0, 270, 0);
    }

    public void Start()
    {
        UpdatePosition();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Forward"))
        {

        }
    }

    public void MoveForward()
    {
        (int offX, int offZ) = MoveLookup[this.Facing];
        // TODO: Check current tile and make sure you can't move through a wall
        this.X += offX;
        this.Y += offZ;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        (float offX, float offZ) = PositionLookup[this.Facing];
        this.transform.position = new Vector3(this.X * 10 + offX, 5, this.Y * 10 + offZ);
        this.transform.localRotation = RotationLookup[this.Facing];
    }

}