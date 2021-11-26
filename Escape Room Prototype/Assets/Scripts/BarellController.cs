using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaptainCoder.TileBuilder;

public class BarellController : MonoBehaviour, ITileObject
{
    public char TextChar => '8';

    private MeshExploder MeshExploder
    {
        get => this.transform.Find("Mesh").GetComponent<MeshExploder>();
    }
    
    public void Explode()
    {
        GameObject rv = this.MeshExploder.Explode();
        MeshExploder.gameObject.SetActive(false);
    }

    public void Interact()
    {
        this.Explode();
    }
}
