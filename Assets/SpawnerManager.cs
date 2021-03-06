using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public List<CarSpawn> carSpawners;
    public List<NPCSpawner> NPCSpawners;
    public List<PathMovement> NPCs;
    public List<CarPathMovement> Cars;

    private bool npcsEnabled = true;
    private bool carsEnabled = true;

    public void EnableCars(bool enabled)
    {
        carsEnabled = enabled;
        foreach(CarSpawn cs in carSpawners)
        {
            if(enabled)
            {
                StartCoroutine(cs.SpawnCar());
                cs.gameObject.SetActive(true);
            }
            else
            {
                cs.StopAllCoroutines();
                cs.gameObject.SetActive(false);
            }
        }
        if(!enabled)
        {
            RemoveCars();
        }
    }

    public void EnableNPCs(bool enabled)
    {
        npcsEnabled = enabled;
        foreach(NPCSpawner ns in NPCSpawners)
        {
            if(enabled)
            {
                ns.gameObject.SetActive(true);
                StartCoroutine(ns.SpawnNPC());
            }
            else
                ns.gameObject.SetActive(false);
            {
                ns.StopAllCoroutines();
            }
        }
        if(!enabled)
        {
            RemoveNPCs();
        }
    }

    public void RemoveCars()
    {
        Cars.Clear();
        Cars.AddRange(FindObjectsOfType<CarPathMovement>());
        foreach(CarPathMovement car in Cars)
        {
            if (car.crossing != null)
            {
                BoxCollider collider = car.GetComponentsInChildren<BoxCollider>().First((box) => box.name == "CarHitbox");
                car.crossing.OnTriggerExit(collider);
            }
            Destroy(car.gameObject);
        }
    }

    public void RemoveNPCs()
    {
        NPCs.Clear();
        NPCs.AddRange(FindObjectsOfType<PathMovement>());
        foreach (PathMovement npc in NPCs)
        {
            if (npc.crossing != null)
            {
                npc.crossing.OnTriggerExit(npc.GetComponent<CapsuleCollider>());
            }
            Destroy(npc.gameObject);
        }
    }

    public void AdjustMaxNPCS(int maxNPCs)
    {
        foreach(NPCSpawner ns in NPCSpawners)
        {
            ns.maxNPCSpawn = maxNPCs / NPCSpawners.Count();
        }
    }

    public void AdjustBusynessLevel(float mode)
    {
        RemoveCars();
        RemoveNPCs();
        switch (mode)
        {
            case (1):
                SetSpawnerLimits(0, 0, 0, 0, 0);
                break;
            case (2):
                SetSpawnerLimits(10, 20, 10, 20, 3);
                break;
            case (3):
                SetSpawnerLimits(7, 15, 5, 15, 8);
                break;
            case (4):
                SetSpawnerLimits(5, 10, 3, 8, 15);
                break;
            default:
                break;
        }
    }

    private void SetSpawnerLimits(int carMin, int carMax, int npcMin, int npcMax, int npcSpawnMax)
    {
        foreach (CarSpawn cs in carSpawners)
        {
            cs.spawnTimeMin = carMin;
            cs.spawnTimeMax = carMax;
            cs.StopAllCoroutines();
            if (carsEnabled)
            {
                StartCoroutine(cs.SpawnCar());
            }
        }

        foreach (NPCSpawner ns in NPCSpawners)
        {
            ns.spawnTimeMin = npcMin;
            ns.spawnTimeMax = npcMax;
            ns.maxNPCSpawn = npcSpawnMax;
            ns.StopAllCoroutines();
            if (npcsEnabled)
            {
                StartCoroutine(ns.SpawnNPC());
            }
        }
    }
}
