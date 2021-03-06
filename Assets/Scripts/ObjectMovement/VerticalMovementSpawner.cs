using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VerticalMovementUltil;

public class VerticalMovementSpawner : MonoBehaviour
{
    private readonly List<GameObject> objects = new List<GameObject>();
    private bool scriptIsWorking;
    private float currentSpeed;
    private int currentNumberOfObjects;
    private float currentHeight;
    private float currentRadius;
    private MeshTypes currentType;

    public MeshTypes meshType;
    public int seed;
    public List<Mesh> meshes;
    public bool IsRunning;
    public bool customObjectsEnabler;    

    [Range(1, 25)]
    public int numberOfObjects;
    
    [Range(2, 9)]
    public float radius;
    
    [Range(0.1f, 4f)]
    public float speed;
    
    [Range(0, 7.5f)]
    public float height;

    private float defaultRadius;
    private float defaultSpeed;

    public Vector3 Center { get; private set; }

    private void Start()
    {
        defaultRadius = radius;
        defaultSpeed = speed;
    }

    void InitializeScript()
    {
        Random.InitState(seed);
        Center = new Vector3(0, height, 0);
        currentSpeed = speed;
        currentNumberOfObjects = numberOfObjects;
        currentHeight = height;
        currentRadius = radius;
        currentType = meshType;
        SpawnShapesAroundCenter(numberOfObjects, defaultRadius);
    }

    public void Update()
    {
        if (scriptIsWorking)
        {
            if (speed != currentSpeed)
            {
                foreach (var obj in objects)
                    obj.GetComponent<VerticalMovementAdjuster>().speed = speed;
                currentSpeed = speed;
            }
            if (height != currentHeight)
            {
                foreach (var obj in objects)
                    obj.GetComponent<VerticalMovementAdjuster>().height = height;
                currentHeight = height;
            }
            if (radius != currentRadius)
            {
                foreach (var obj in objects)
                    obj.GetComponent<VerticalMovementAdjuster>().radius = radius;
                currentRadius = radius;
            }
            if (numberOfObjects != currentNumberOfObjects)
            {
                ResetScene();
                currentNumberOfObjects = numberOfObjects;
            }
            if (!IsRunning)
            {
                DestroyAllObjects();
                scriptIsWorking = false;
            }
            if (meshType != currentType)
            {
                ResetScene();
                currentType = meshType;
            }
        }
        else if (!scriptIsWorking && IsRunning)
        {
            InitializeScript();
            scriptIsWorking = true;
        }
    }

    public void SpawnShapesAroundCenter(int num, float radius)
    {
        for (int i = 0; i < num; i++)
        {
            var spawnDir = CalculateSpawnDirection(i, num);
            var spawnPos = Center + spawnDir * radius;
            GameObject obj = CreateMeshes(spawnPos, Center, transform, meshType, meshes);
            SetVerticalAdjusterScriptParameters(obj, radius, num, Center, speed, height);
            obj.name = $"{i + 1}";
            objects.Add(obj);
        }
    }

    void ResetScene()
    {
        DestroyAllObjects();
        InitializeScript();
    }

    void DestroyAllObjects()
    {
        objects.ForEach((obj) => Destroy(obj));
        objects.Clear();
    }

    public void Play(int nrOfObjects, float speed, float distance, MeshTypes meshType, int seed)
    {
        this.seed = seed;
        this.numberOfObjects = nrOfObjects;
        InitializeScript();
        this.speed = this.defaultSpeed * speed;
        this.radius = this.defaultRadius * distance;
        this.meshType = meshType;
        IsRunning = true;
        scriptIsWorking = true;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void ChangeNumberOfObjects(int nrOfObjects)
    {
        numberOfObjects = nrOfObjects;
    }

    public void ChangeObjectSpeed(float speed)
    {
        this.speed = this.defaultSpeed * speed;
    }

    public void ChangeObjectRadius(float radius)
    {
        this.radius = this.defaultRadius * radius;
    }
}
