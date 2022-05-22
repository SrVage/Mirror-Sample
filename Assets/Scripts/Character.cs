using System;
using Interfaces;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(CharacterController))]
public abstract class Character : NetworkBehaviour
{
    protected Action OnUpdateAction { get; set; }
    protected abstract FireAction fireAction { get; set; }

    [SyncVar] protected Vector3 serverPosition;
    [SyncVar] protected Quaternion serverRotation;
    private RaycastHit[] _shootHits;

    protected virtual void Initiate()
    {
        OnUpdateAction += Movement;
        _shootHits = new RaycastHit[10];
    }

    private void Update()
    {
        OnUpdate();
    }

    private void OnUpdate()
    {
        OnUpdateAction?.Invoke();
    }

    [Command]
    protected void CmdUpdatePosition(Vector3 position)
    {
        serverPosition = position;
    }

    [Command]
    protected void CmdUpdateRotation(Quaternion rotation)
    {
        serverRotation = rotation;
    }

    [Command]
    protected void CmdShootEvent(Vector3 startPoint, Vector3 endPoint)
    {
        Physics.RaycastNonAlloc(new Ray(startPoint, endPoint-startPoint), _shootHits, ShootingMaxDistance);
        foreach (var shootHit in _shootHits)
        {
            if (shootHit.collider != null)
            {
                if ((shootHit.collider.transform.position-startPoint).sqrMagnitude<1)
                    continue;
                foreach (var script in shootHit.collider.GetComponents<MonoBehaviour>())
                {
                    if (script is IDamage damage)
                        damage.Damage(10);
                }
            }
        }
    }

    private const float ShootingMaxDistance = 100f;

    public abstract void Movement();
}
