using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Slayer : MonoBehaviour {

    private Mesh mesh;
    private List<int> newTriangles = new List<int>();
    private List<Vector3> newVertices = new List<Vector3>();

    private Vector3[] verts;
    private int[] trian;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
            GetComponent<MeshCollider>().enabled = true;
        else
        {
            GetComponent<MeshCollider>().enabled = false;
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Damageable")
        {
            mesh = collision.gameObject.GetComponent<MeshFilter>().mesh;
          //  verts = mesh.vertices;
         //   trian = mesh.triangles;
            foreach(ContactPoint contact in collision.contacts)
            {
                newVertices.Add(new Vector3(contact.point.x, contact.point.y, contact.point.z));
                Debug.Log(contact.point);
            }
        }
    }

    void OnCollisionaStay(Collision collision)
    { 
 /*       if (collision.gameObject.tag == "Damageable")
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                newVertices.Add(new Vector3(contact.point.x, contact.point.y, contact.point.z));
            }
        }*/
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Damageable")
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                newVertices.Add(new Vector3(contact.point.x, contact.point.y, contact.point.z));
            }
        }
        newTriangles.Add(0);
        newTriangles.Add(1);
        newTriangles.Add(2);
        newTriangles.Add(0);
        newTriangles.Add(2);
        newTriangles.Add(3);

        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        newVertices.Clear();
        newTriangles.Clear();

        Debug.Log("Exit");
    }
}
