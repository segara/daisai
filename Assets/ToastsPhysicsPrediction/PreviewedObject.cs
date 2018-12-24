using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewedObject : MonoBehaviour {

    public static List<PreviewedObject> instances = new List<PreviewedObject>();
    public GameObject ghostObject;
    public bool drawLine;
    public bool drawGhosts;
    public Vector3 startSpeed;
    private LineRenderer line;
    private Vector3 zeroPosition;
    private Vector3 zeroSpeed;
    private Vector3 zeroAngularSpeed;
    private Quaternion zeroRot;
    private Rigidbody RB;

    // Use this for initialization
    void Start () {
        instances.Add(this);
        RB = GetComponent<Rigidbody>();
        if (RB.velocity == Vector3.zero)
        {
            RB.velocity = startSpeed;
        }
        if (drawLine)
        {
            line = GetComponent<LineRenderer>();
            line.positionCount = PhysicsPreview.Iterations * PhysicsPreview.InBetweenFrameStepsValue;
            Vector3[] pos = new Vector3[line.positionCount];
            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = transform.position;
            }
            line.SetPositions(pos);
            line.enabled = true;
        }
	}

    public static void SetAllGhostPositions(int i)
    {
        for (int j = 0; j < instances.Count; j++)
        {
            if (instances[j] == null)
            {
                instances.RemoveAt(j);
            }
        }
        foreach (PreviewedObject inst in instances)
        {
            if (inst != null)
            {
                inst.SetGhost(i);
            }
        }
    }

    public static void ResetZeroPositions()
    {
        foreach (PreviewedObject inst in instances)
        {
            if (inst != null)
            {
                inst.ResetZero();
            }
        }
    }

    public static void DrawLine(int i)
    {
        foreach (PreviewedObject inst in instances)
        {
            if (inst != null)
            {
                if (inst.drawLine)
                {
                    inst.line.SetPosition(i, inst.transform.position);
                }
            }
        }
    }

    public static void ActivateLine(bool on)
    {
        foreach (PreviewedObject inst in instances)
        {
            if (inst != null)
            {
                if (inst.drawLine)
                {
                    inst.line.enabled = on;
                }
            }
        }
    }

    public void ResetZero()
    {
        transform.position = zeroPosition;
        RB.velocity = zeroSpeed;
        RB.angularVelocity = zeroAngularSpeed;
        transform.rotation = zeroRot;
    }

    public void SetGhost(int i)
    {
        float alpha = (1 - (i / (float)PhysicsPreview.Iterations)) * 0.6f;
        if (i == 0)
        {
            alpha = 1;
            zeroPosition = transform.position;
            zeroSpeed = RB.velocity;
            zeroAngularSpeed = RB.angularVelocity;
            zeroRot = transform.rotation;
        }
        if (i != PhysicsPreview.Iterations - 1 && drawGhosts)
        {
            GhostPreview g = Instantiate(ghostObject, transform.position, transform.rotation).GetComponent<GhostPreview>();
            g.Init(alpha, GetComponent<MeshRenderer>(), GetComponent<MeshFilter>().mesh, transform);
            g.transform.localScale = transform.localScale;
        }
    }
}
