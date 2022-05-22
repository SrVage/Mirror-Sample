using System.ComponentModel.Design.Serialization;
using Interfaces;
using Mirror;
using UnityEngine;

public class PlayerHealth:NetworkBehaviour, IDamage
{
    [SerializeField] private int _health = 100;

    [SyncVar] private int _serverHealth;

    public void Damage(int damage)
    {
        if (!isServer)
            return;
        if (_health > damage)
        {
            _health -= damage;
        }
        else
        {
            _health = 0;
            RpcDeath();
        }
        RpcUpdateHealth(_health);
    }

    [ClientRpc]
    private void RpcDeath()
    {
        if (hasAuthority)
            FindObjectOfType<NetworkManager>().StopClient();
       // OnStopClient();
    }

    [ClientRpc]
    private void RpcUpdateHealth(int health)
    {
        _serverHealth = health;
    }
}