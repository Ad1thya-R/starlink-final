using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace MarchingCubes.Examples
{
    /// <summary>
    /// The terrain deformer which modifies the terrain
    /// </summary>
    public class TerrainDeformer : MonoBehaviour
    {
        /// <summary>
        /// Does the left mouse button add or remove terrain
        /// </summary>
        [Header("Terrain Deforming Settings")]
        [SerializeField] private bool leftClickAddsTerrain = true;

        /// <summary>
        /// How fast the terrain is deformed
        /// </summary>
        [SerializeField] private float deformSpeed = 0.0001f;

        /// <summary>
        /// How far the deformation can reach
        /// </summary>
        [SerializeField] private float deformRange = 3f;

        /// <summary>
        /// How far away points the player can deform
        /// </summary>
        [SerializeField] private float maxReachDistance = Mathf.Infinity;

        /// <summary>
        /// Which key must be held down to flatten the terrain
        /// </summary>
        [Header("Flattening")]
        [SerializeField] private KeyCode flatteningKey = KeyCode.LeftControl;

        /// <summary>
        /// The world the will be deformed
        /// </summary>
        [Header("Player Settings")]
        [SerializeField] private World world;

        /// <summary>
        /// The game object that the deformation raycast will be casted from
        /// </summary>
        [SerializeField] private Transform playerCamera;

        /// <summary>
        /// Is the terrain currently being flattened
        /// </summary>
        private bool _isFlattening;

        /// <summary>
        /// The point where the flattening started
        /// </summary>
        private float3 _flatteningOrigin;

        /// <summary>
        /// The normal of the flattening plane
        /// </summary>
        private float3 _flatteningNormal;

        private UIManager _uiManager;
        private Player _player;
        private void Awake()
        {
            _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
            _player = GameObject.Find("Player").GetComponent<Player>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (deformSpeed <= 0)
            {
                Debug.LogWarning("Deform Speed must be positive!");
                return;
            }

            if (deformRange <= 0)
            {
                Debug.LogWarning("Deform Range must be positive");
                return;
            }

            if (_uiManager.GetSelectedButton() == "terrain" && _player.IsDead() == false)
            {
                if (Input.GetKey(flatteningKey))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Vector3 startP = playerCamera.position;
                        Vector3 destP = startP + playerCamera.forward;
                        Vector3 direction = destP - startP;

                        var ray = new Ray(startP, direction);

                        if (!Physics.Raycast(ray, out RaycastHit hit, maxReachDistance))
                        {
                            return;
                        }

                        _isFlattening = true;

                        _flatteningOrigin = hit.point;
                        _flatteningNormal = hit.normal;
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        _isFlattening = false;
                    }
                }

                if (Input.GetKeyUp(flatteningKey))
                {
                    _isFlattening = false;
                }

                if (Input.GetMouseButton(0))
                {
                    FindObjectOfType<AudioManager>().Play("Digging");
                    if (_isFlattening)
                    {
                        FlattenTerrain();
                    }
                    else
                    {
                        RaycastToTerrain(leftClickAddsTerrain);
                    }
                }
                else if (Input.GetMouseButton(1))
                {
                    FindObjectOfType<AudioManager>().Play("Digging");
                    RaycastToTerrain(!leftClickAddsTerrain);
                }
                else
                {
                    FindObjectOfType<AudioManager>().StopPlaying("Digging");
                }
            }
            else
            {
                FindObjectOfType<AudioManager>().StopPlaying("Digging");
            }
            
        }

        /// <summary>
        /// Get a point on the flattening plane and flatten the terrain around it
        /// </summary>
        private void FlattenTerrain()
        {
            var result = Utils.PlaneLineIntersection(_flatteningOrigin, _flatteningNormal, playerCamera.position, playerCamera.forward, out float3 intersectionPoint);
            if (result != PlaneLineIntersectionResult.OneHit) { return; }

            int intRange = (int)math.ceil(deformRange);
            for (int x = -intRange; x <= intRange; x++)
            {
                for (int y = -intRange; y <= intRange; y++)
                {
                    for (int z = -intRange; z <= intRange; z++)
                    {
                        int3 localPosition = new int3(x, y, z);
                        float3 offsetPoint = intersectionPoint + localPosition;

                        float distance = math.distance(offsetPoint, intersectionPoint);
                        if (distance > deformRange)
                        {
                            continue;
                        }

                        int3 densityWorldPosition = (int3)offsetPoint;
                        float density = (math.dot(_flatteningNormal, densityWorldPosition) - math.dot(_flatteningNormal, _flatteningOrigin)) / deformRange;
                        float oldDensity = world.GetDensity(densityWorldPosition);

                        world.SetDensity((density + oldDensity) * 0.8f, densityWorldPosition);
                    }
                }
            }
        }

        /// <summary>
        /// Tests if the player is in the way of deforming and edits the terrain if the player is not.
        /// </summary>
        /// <param name="addTerrain">Should terrain be added or removed</param>
        private void RaycastToTerrain(bool addTerrain)
        {
            var ray = new Ray(playerCamera.position, playerCamera.forward);

            if (!Physics.Raycast(ray, out RaycastHit hit, maxReachDistance)) { return; }
            Vector3 hitPoint = hit.point;

            if (addTerrain)
            {
                Collider[] hits = Physics.OverlapSphere(hitPoint, deformRange / 2f * 0.8f);
                if (hits.Any(h => h.CompareTag("Player"))) { return; }
            }

            EditTerrain(hitPoint, addTerrain, deformSpeed, deformRange);
        }

        /// <summary>
        /// Deforms the terrain in a spherical region around the point
        /// </summary>
        /// <param name="point">The point to modify the terrain around</param>
        /// <param name="addTerrain">Should terrain be added or removed</param>
        /// <param name="deformSpeed">How fast the terrain should be deformed</param>
        /// <param name="range">How far the deformation can reach</param>
        private void EditTerrain(Vector3 point, bool addTerrain, float deformSpeed, float range)
        {
            int buildModifier = addTerrain ? 1 : -1;

            int hitX = Mathf.RoundToInt(point.x);
            int hitY = Mathf.RoundToInt(point.y);
            int hitZ = Mathf.RoundToInt(point.z);

            int intRange = Mathf.CeilToInt(range);

            for (int x = -intRange; x <= intRange; x++)
            {
                for (int y = -intRange; y <= intRange; y++)
                {
                    for (int z = -intRange; z <= intRange; z++)
                    {
                        int offsetX = hitX - x;
                        int offsetY = hitY - y;
                        int offsetZ = hitZ - z;

                        var offsetPoint = new int3(offsetX, offsetY, offsetZ);
                        float distance = math.distance(offsetPoint, point);
                        if (distance > range)
                        {
                            continue;
                        }

                        float modificationAmount = deformSpeed / distance * buildModifier;

                        float oldDensity = world.GetDensity(offsetPoint);
                        float newDensity = Mathf.Clamp(oldDensity - modificationAmount, -1, 1);

                        world.SetDensity(newDensity, offsetPoint);
                    }
                }
            }
        }
    }
}