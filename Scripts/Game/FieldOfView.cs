using UnityEngine;

public class FieldOfView : MonoBehaviour {
    [SerializeField] private LayerMask layerMask; // Set this to your "Obstacles" layer
    private Mesh mesh;
    private float fov = 90f;
    private float viewDistance = 50f;
    private int rayCount = 50; 

    private void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate() {
        float angle = 0f;
        float angleStep = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero; // Origin point

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++) {
            Vector3 direction = GetVectorFromAngle(angle);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDistance, layerMask);

            if (hit.collider == null) {
                vertices[vertexIndex] = direction * viewDistance;
            } else {
                vertices[vertexIndex] = transform.InverseTransformPoint(hit.point);
            }

            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleStep; // Sweeping clockwise
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
    }

    private Vector3 GetVectorFromAngle(float angle) {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}