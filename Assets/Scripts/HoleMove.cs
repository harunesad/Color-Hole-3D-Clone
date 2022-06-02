using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMove : MonoBehaviour
{

    public MeshFilter holeMesh;
    public MeshCollider holeMeshCol;

    Mesh mesh;
    List<int> holeVertices;
    List<Vector3> offset;
    int holeVerticesCount;

    public float radius;
    public Transform holeCenter;
    public Vector2 moveLimit;

    float x, y;
    Vector3 touch, targetpos;
    public float moveSpeed;
    void Start()
    {
        GameControl.isCompleted = false;
        GameControl.isMoving = false;

        holeVertices = new List<int>();
        offset = new List<Vector3>();

        mesh = holeMesh.mesh;

        FindHoleVertices();
    }
    void Update()
    {
        GameControl.isMoving = Input.GetMouseButton(0);

        if (!GameControl.isCompleted && GameControl.isMoving)
        {
            MoveHole();
            UpdateHolePos();
        }
    }
    void MoveHole()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        touch = Vector3.Lerp(holeCenter.position, holeCenter.position + new Vector3(x, 0, y), moveSpeed * Time.deltaTime);

        float posX = Mathf.Clamp(touch.x, -moveLimit.x, moveLimit.x);
        float posZ = Mathf.Clamp(touch.z, -moveLimit.y, moveLimit.y);
        targetpos = new Vector3(posX, touch.y, posZ);

        holeCenter.position = targetpos;
    }
    void UpdateHolePos()
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < holeVerticesCount; i++)
        {
            vertices[holeVertices[i]] = holeCenter.position + offset[i];
        }
        mesh.vertices = vertices;
        holeMesh.mesh = mesh;
        holeMeshCol.sharedMesh = mesh;
    }
    void FindHoleVertices()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float distance = Vector3.Distance(holeCenter.position, mesh.vertices[i]);

            if (distance < radius)
            {
                holeVertices.Add(i);
                offset.Add(mesh.vertices[i] - holeCenter.position);
            }
        }
        holeVerticesCount = holeVertices.Count;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(holeCenter.position, radius);
    }
}
