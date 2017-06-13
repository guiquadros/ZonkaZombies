using UnityEngine;
using ZonkaZombies.Characters.Enemy.EnemyIA;

namespace ZonkaZombies.Characters.Enemy
{
    public class VisionRayDebugger : MonoBehaviour
    {
        [SerializeField]
        private GenericEnemy _genericEnemyBehavior;
         
        [SerializeField]
        private float _fieldOfViewSize = 5f;

        private Mesh _mesh;
        private float _angle;

        private void Awake()
        {
            _mesh = new Mesh();
        }

        private void BuildMesh()
        {
            _angle = _genericEnemyBehavior.FieldOfViewAngle * 2f;

            Vector3 v0 = Vector3.zero;
            Vector3 v1 = new Vector3(Mathf.Cos(Mathf.Deg2Rad * _angle), 0, Mathf.Sin(Mathf.Deg2Rad * _angle)) * _fieldOfViewSize;
            Vector3 v2 = new Vector3(-Mathf.Cos(Mathf.Deg2Rad * _angle), 0, Mathf.Sin(Mathf.Deg2Rad * _angle)) * _fieldOfViewSize;

            _mesh.vertices = new[] { v0, v1, v2 };
            _mesh.triangles = new[] { 0, 1, 2 };

            _mesh.normals = new[] { Vector3.zero, Vector3.zero, Vector3.zero };
        }


        private void OnDrawGizmos()
        {
            if (_mesh == null || _genericEnemyBehavior == null) return;

            BuildMesh();

            Gizmos.DrawMesh(_mesh, _genericEnemyBehavior.transform.position, _genericEnemyBehavior.transform.rotation, Vector3.one);
        }
    }
}