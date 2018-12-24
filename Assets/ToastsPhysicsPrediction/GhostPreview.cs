using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class GhostPreview : MonoBehaviour {
    
    public static List<GhostPreview> instances;
    private MeshRenderer rend;

    // Use this for initialization
    public void Init (float alpha, MeshRenderer meshy, Mesh m, Transform trans) {
        GetComponent<MeshFilter>().mesh = m;
        rend = GetComponent<MeshRenderer>();
        instances.Add(this);
        rend.materials = meshy.materials;
        for (int i = 0; i < rend.materials.Length; i++){
            Color c = rend.materials[i].color;
            rend.materials[i].color = new Color(c.r,c.g,c.b,alpha);
        }
    }

    public static void RemoveAllGhosts()
    {
        if (instances == null) { return; }
        for (int i = 0; i < instances.Count; i++)
        {
            if (instances[i] != null)
            {
                for (int j = 0; j < instances[i].rend.materials.Length; j++)
                {
                    Destroy(instances[i].rend.materials[j]);
                }
                Destroy(instances[i].gameObject);
            }
        }
        for (int i = 0; i < instances.Count; i++)
        {
            if (instances[i] == null)
            {
                instances.RemoveAt(i);
            }
        }
    }
}
