using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarellController : MonoBehaviour
{

    private MeshExploder MeshExploder
    {
        get => this.transform.Find("Mesh").GetComponent<MeshExploder>();
    }
    
    public void Explode()
    {
        GameObject rv = this.MeshExploder.Explode();
        MeshExploder.gameObject.SetActive(false);
    }
}
