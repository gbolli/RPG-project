using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        [System.Serializable]
        struct CursorMapping {
            public CursorType type;
            public Texture2D texture;
            public Vector3 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshHitDistance = 1f;
        [SerializeField] float rayastRadius = 0.5f;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead()) {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject()) {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RayCastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this)) {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InteractWithMovement()  // raycast to move if not on enemy target (and cancel attack)
        {
            // RaycastHit hit;
            // bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    
                    GetComponent<Mover>().StartMoveAction(target, 1f);   // hit.point
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target) {
            target = new Vector3();

            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out raycastHit);

            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool isCloseEnoughToNavmesh = NavMesh.SamplePosition(raycastHit.point, out navMeshHit, maxNavMeshHitDistance, NavMesh.AllAreas);

            if (!isCloseEnoughToNavmesh) return false;

            target = navMeshHit.position;

            

            return true;
        }

        RaycastHit[] RayCastAllSorted() {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), rayastRadius);
            // array for sort
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);
            return hits;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type) {
            foreach (CursorMapping mapping in cursorMappings) {
                if (mapping.type == type) {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}