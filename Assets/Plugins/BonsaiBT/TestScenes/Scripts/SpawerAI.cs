
using UnityEngine;
using UnityEngine.Networking;

public class SpawerAI : NetworkBehaviour {

    public GameObject turretPrefab;
    public Vector2 spawnPos;

    public override void OnStartServer()
    {
        var turret = GameObject.Instantiate(turretPrefab);
        turret.transform.position = spawnPos;

        var turret2 = GameObject.Instantiate(turretPrefab);
        turret2.transform.position = spawnPos * 0.5f;


        NetworkServer.Spawn(turret);
        NetworkServer.Spawn(turret2);
    }
}
