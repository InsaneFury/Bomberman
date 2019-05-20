using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : SingletonMonobehaviour<Portal>
{
    EnemyManager eManager;
    Material mat;
    public bool portalIsActive = false;
    public bool playerIsOnPortal = false;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        eManager = EnemyManager.Get();
    }

    void Update()
    {
        if (eManager.EnemysActiveInGame <= 0)
        {
            ActivePortal();
        }
        else
        {
            DisablePortal();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && portalIsActive)
        {
            playerIsOnPortal = true;
        }
    }

    void ActivePortal()
    {
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_Color", new Color32(0, 191, 168, 0));
        portalIsActive = true;
    }

    void DisablePortal()
    {
        mat.DisableKeyword("_EMISSION");
        mat.SetColor("_Color", new Color32(255, 8, 0, 0));
        portalIsActive = false;
    }
}
