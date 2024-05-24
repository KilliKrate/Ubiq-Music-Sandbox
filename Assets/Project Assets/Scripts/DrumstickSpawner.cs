using MessageTypes;
using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.Spawning;
using UnityEngine;

public class DrumstickSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public NetworkScene scene;

    protected NetworkSpawnManager manager;

    void Start()
    {
        manager = scene.GetComponentInChildren<NetworkSpawnManager>();
        manager.OnSpawned.AddListener(OnSpawned);
    }
    
    private void OnSpawned(GameObject go, IRoom room, IPeer peer, NetworkSpawnOrigin origin)
    {
        if (go.CompareTag("Drumstick") && origin == NetworkSpawnOrigin.Remote)
        {
            SyncedTransform st = go.GetComponent<SyncedTransform>();
            st.Start();
            st.SyncLastPosition();
        }
    }

    public void Spawn(int index)
    {
        if (manager)
        {
            GameObject go = manager.SpawnWithPeerScope(manager.catalogue.prefabs[index]);
            spawnPoint.GetPositionAndRotation(out var pos, out var rot);
            go.transform.SetPositionAndRotation(pos, rot);
        }
    }
}
