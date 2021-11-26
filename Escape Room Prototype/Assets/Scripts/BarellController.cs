using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaptainCoder.TileBuilder;

public class BarellController : MonoBehaviour, ITileObject
{
    public char TextChar => '8';

    public int X, Y;
    
    public (int x, int y) Position 
    {
        get => (X, Y); 
        set { 
            X = value.x;
            Y = value.y;
        }
    }

    private MeshExploder MeshExploder
    {
        get => this.transform.Find("Mesh").GetComponent<MeshExploder>();
    }
    
    public void Explode()
    {
        Debug.Log("Explode Barrel?");
        if (PlayerController.INSTANCE.Position != this.Position)
        {
            Debug.Log($"Player: {PlayerController.INSTANCE.Position}, Barrel: {this.Position}");
            return;
        }
        GameObject rv = this.MeshExploder.Explode();
        MeshExploder.gameObject.SetActive(false);
    }

    public void Interact()
    {
        Debug.Log("In Barrel Interact");
        this.Explode();
    }
}
