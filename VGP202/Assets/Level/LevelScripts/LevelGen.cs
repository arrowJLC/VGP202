using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using Unity.VisualScripting;

public class TileLevelGen : MonoBehaviour
{
    public enum levelSpeed { slow = 3, normal = 4, fast = 5, insane = 7, wild = 10}

    InputManager inputManager;

    //levelSpeed currentSpeed = levelSpeed.slow;

    [Header("Level Generation Settings")]
    public int scrollSpeed = (int)levelSpeed.slow;

    [Header("Increse Speed")]
    private float gameTime = 0;
    private bool oneMin = false;


    [Header("Melter Spawn")]
    public Transform snowMelter;
    public int randomN;

    [Header("Chunk Settings")]
    public GameObject chunkPrefab;

    public int chunkWidth = 10;

    [Tooltip("Prefab for ground tile (as GameObject)")]
    public GameObject groundPrefab;
    public GameObject roofPrefab;

    [Tooltip("Prefab for obstacle tile (as GameObject)")]
    public GameObject obstaclePrefab;
    //public Obstacle obstaclePrefab;

    [Range(0, 1)]
    public float obstacleChance = 0.1f;

    [Header("Object Pool Settings")]
    public int initialPoolSize = 5;
    public int maxPoolSize = 10;

    private ObjectPool<GameObject> chunkPool;
    private List<GameObject> activeChunks = new List<GameObject>();
    private Camera mainCamera;
    private float screenLeftEdgeWorldX;
    private float screenRightEdgeWorldX;
    private float nextSpawnPosX = 0f;

    private float screenLeftEdgeTop;
    private float screenRightEdgeTop;
    private float nextSpawnPosTop = 0f;

    [Header("Experimental Settings")]
    [SerializeField] public List<GameObject> obstaclePrefabs;
    [SerializeField] private float obstacleSChance = 0.3f;
    [SerializeField] private float obstacleCooldown = 2f; // Seconds between spawns

    private float lastObstacleTime = 0f;

    void Start()
    {
        mainCamera = Camera.main;
        inputManager = gameObject.GetComponent<InputManager>();

        if (chunkPrefab == null || groundPrefab == null || obstaclePrefab == null)
        {
            Debug.LogError("Assign all prefab references in the inspector.");
            return;
        }
        

        chunkPool = new ObjectPool<GameObject>
        (
            createFunc: CreateNewChunk,
            actionOnGet: PositionAndActivate,
            actionOnRelease: DeactivateChunk,
            actionOnDestroy: DestroyChunk,
            collectionCheck: false,
            defaultCapacity: initialPoolSize,
            maxSize: maxPoolSize
        );

        CalculateScreenBounds();

        while (nextSpawnPosX < screenRightEdgeWorldX + chunkWidth)
        {
            chunkPool.Get();
        }

        while (nextSpawnPosTop < screenLeftEdgeTop +  chunkWidth)
        {
            chunkPool.Get();
        }
        
        //randomN = Random.Range(10, 40);
        // snowMelter = GameObject.FindWithTag("Enemy");
    }

    private void FixedUpdate()
    {
        gameTime += Time.deltaTime;
       // Debug.Log("Time: " + Mathf.FloorToInt(gameTime));

        if(!oneMin && gameTime >= 30f && gameTime <= 60f)
        {
            oneMin = true;
            scrollSpeed = (int)levelSpeed.normal;
            //spawnMelter();
            StartCoroutine(melterSpawn());
        }
        if (gameTime >= 61f && gameTime <= 180f)
        {
            scrollSpeed = (int)levelSpeed.fast;
        }
        if (gameTime >= 181f && gameTime <= 300f)
        {
            scrollSpeed = (int)levelSpeed.insane;
            //StartCoroutine(melterSpawn());
        }

        if (gameTime >= 301f)
        {
            scrollSpeed = (int)levelSpeed.wild;
        }
    }

    IEnumerator melterSpawn()
    {
        while (true)
        {
            randomN = Random.Range(10, 40);
            yield return new WaitForSeconds(randomN);
            spawnMelter();
        }
    }
    private void spawnMelter()
    {
        //if (gameTime == 181f)
        //{
            Transform spawn = snowMelter;
            Instantiate(spawn);
        //}
    }

    private void DeactivateChunk(GameObject chunk)
    {
        chunk.SetActive(false);
    }

    private void DestroyChunk(GameObject chunk)
    {
        Destroy(chunk);
    }

    private GameObject CreateNewChunk()
    {
        GameObject chunk = Instantiate(chunkPrefab, transform);
        chunk.SetActive(false);
        return chunk;
    }

    private void PositionAndActivate(GameObject chunk)
    {
        chunk.transform.position = new Vector3(nextSpawnPosX, 0f, 0f);
        chunk.transform.position = new Vector3(nextSpawnPosTop, 0f, 0f);

        chunk.SetActive(true);
        activeChunks.Add(chunk);
        nextSpawnPosX += chunkWidth;
        nextSpawnPosTop += chunkWidth;

        // Clear existing children
        foreach (Transform child in chunk.transform)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < chunkWidth; x++)
        {
            Vector3 groundPos = new Vector3(x, -2.3f, 0f);
            GameObject ground = Instantiate(groundPrefab, chunk.transform);
            ground.transform.localPosition = groundPos;

            Vector3 roofPos = new Vector3(x, 9.6f, 0f);
            GameObject roof = Instantiate(roofPrefab, chunk.transform);
            roof.transform.localPosition = roofPos;

            GenerateObstacle(chunk, x);
            
        }
    }

    //private void GenerateObstacle(GameObject chunk, int x)
    //{
    //    //instead of just returning when the first obstacle spawned - you should time out obstacles from spawning - possiblity skipping the loop where this function is called
    //    //You could also have a obstacle timeout value that gets reset everytime an obstacle spawns.
    //    //You could also have rules based on different obstacles
    //    //Add different obstalces

    //    //This is where you should select different obstacles to spawn - and check for your spawn rules to ensure the game is playable
    //    if (Random.value < obstacleChance)
    //    {
    //        Vector3 obstacleBotPos = new Vector3(x, -4f, 0f);
    //        Vector3 obstaclePosTop = new Vector3(x, 4.1f, 0f);
    //        GameObject obstacle = Instantiate(obstaclePrefab, chunk.transform);


    //        obstacle.transform.localPosition = (Random.Range(0, 2) == 0) ? obstacleBotPos : obstaclePosTop;

    //        return;
    //    }

    //    //if (Random.value < obstacleChance)
    //    //{
    //    //    Vector3 obstaclePosTop = new Vector3(x, 4.1f, 0f);
    //    //    GameObject obstacle = Instantiate(obstaclePrefab, chunk.transform);
    //    //    obstacle.transform.localPosition = obstaclePosTop;
    //    //}
    //}

    private void GenerateObstacle(GameObject chunk, int x)
    {
        float timeSinceLast = Time.time - lastObstacleTime;

        // Enforce obstacle cooldown
        if (timeSinceLast < obstacleCooldown)
            return;

        // Check if this position should spawn an obstacle
        if (Random.value < obstacleSChance)
        {
            // Select a random obstacle from the list
            GameObject selectedObstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];

            // Position choices
            Vector3 obstacleBotPos = new Vector3(x, -4f, 0f);
            Vector3 obstacleTopPos = new Vector3(x, 4.1f, 0f);

            // Optional: Add logic here to enforce obstacle rules (e.g., don't block both top & bottom consecutively)

            // Randomly decide to spawn on top or bottom
            Vector3 spawnPosition = (Random.Range(0, 2) == 0) ? obstacleBotPos : obstacleTopPos;

            GameObject obstacle = Instantiate(selectedObstacle, chunk.transform);
            obstacle.transform.localPosition = spawnPosition;

            // Reset cooldown timer
            lastObstacleTime = Time.time;
        }
    }

    //private void GenerateObstacle(GameObject chunk, int x)
    //{
    //    float timeSinceLast = Time.time - lastObstacleTime;

    //    if (timeSinceLast < obstacleCooldown)
    //        return;

    //    if (Random.value < obstacleChance)
    //    {
    //        GameObject selectedObstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];

    //        Vector3 obstacleBotPos = new Vector3(x, -4f, 0f);
    //        Vector3 obstacleTopPos = new Vector3(x, 4.1f, 0f);

    //        bool isTop = Random.Range(0, 2) == 0;

    //        Vector3 spawnPosition = isTop ? obstacleTopPos : obstacleBotPos;

    //        GameObject obstacle = Instantiate(selectedObstacle, chunk.transform);
    //        obstacle.transform.localPosition = spawnPosition;

    //        // Rotate top-spawning obstacle 180 degrees
    //        if (isTop)
    //        {
    //            obstacle.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
    //        }

    //        lastObstacleTime = Time.time;
    //    }
    //}


    private void CalculateScreenBounds()
    {
        float screenHeight = mainCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * mainCamera.aspect;
        screenLeftEdgeWorldX = mainCamera.transform.position.x - screenWidth / 2f;
        screenRightEdgeWorldX = mainCamera.transform.position.x + screenWidth / 2f;

        screenLeftEdgeTop = mainCamera.transform.position.x - screenWidth / 1f;
        screenRightEdgeTop = mainCamera.transform.position.x + screenWidth / 1f;

    }

    void Update()
    {
        float deltaScroll = scrollSpeed * Time.deltaTime;

        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = activeChunks[i];
            chunk.transform.position -= new Vector3(deltaScroll, 0f, 0f);
        }

        nextSpawnPosX -= deltaScroll;
        nextSpawnPosTop -= deltaScroll;

        if (activeChunks.Count > 0 && (activeChunks[0].transform.position.x + chunkWidth) < screenLeftEdgeWorldX)
        {
            GameObject chunkToRemove = activeChunks[0];
            activeChunks.RemoveAt(0);
            chunkPool.Release(chunkToRemove);
        }

        if (activeChunks.Count > 0 && (activeChunks[0].transform.position.x + chunkWidth) < screenLeftEdgeTop)
        {
            GameObject chunkToRemove = activeChunks[0];
            activeChunks.RemoveAt(0);
            chunkPool.Release(chunkToRemove);
        }

        GameObject rightMostChunk = (activeChunks.Count > 0) ? activeChunks[activeChunks.Count - 1] : null;
        float rightMostEdgeX = (rightMostChunk != null) ? rightMostChunk.transform.position.x + chunkWidth : nextSpawnPosX;
        float rightMostTop = (rightMostChunk != null) ? rightMostChunk.transform.position.x + chunkWidth : nextSpawnPosTop;

        if (rightMostEdgeX < screenRightEdgeWorldX + chunkWidth)
        {
            chunkPool.Get();
        }

        if (rightMostTop < screenRightEdgeTop + chunkWidth)
        {
            chunkPool.Get();
        }
    }

  
}





















//[System.Serializable]
//public class ObstacleType
//{
//    public GameObject prefab;
//    public float minDistanceBetween = 2f; // Can be used for spacing logic
//    public bool canSpawnTop;
//    public bool canSpawnBottom;
//}




////using UnityEngine;


////public class LevelGen : Singleton<LevelGen>
////{
////    public Transform startingPoint;
////    public int startingpieces;
////    public ObjectScroll[] piecesPrefabs;

////    public System.Collections.Generic.List<ObjectScroll> currentPieces = new System.Collections.Generic.List<ObjectScroll>();

////    private void Start()
////    {
////        int randNum = Random.Range(0, piecesPrefabs.Length);
////        ObjectScroll firstPiece = Instantiate(piecesPrefabs[randNum], startingPoint.position, Quaternion.identity);

////        currentPieces.Add(firstPiece);

////        for(int i = 0; i < startingpieces; i++)
////        {
////            spawnLevelPiece();
////        }
////    }

////    public void spawnLevelPiece()
////    {
////        //think of a weighted algo in order to decide which piece to place next instead of by random

////        //walker algo
////        //wave form collaspe

////        int randNum = Random.Range(0, piecesPrefabs.Length);

////        Vector2 prevPos = currentPieces[currentPieces.Count - 1].transform.position;

////        ObjectScroll newPiece = Instantiate(piecesPrefabs[randNum]);

////        Vector2 newSpawnPos = new Vector2(prevPos.x + currentPieces[currentPieces.Count - 1].GetExtent() + newPiece.GetExtent(), prevPos.y);

////        newPiece.transform.position = newSpawnPos;
////        currentPieces.Add(newPiece);
////    }
////}

//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Pool;
//using UnityEngine.Tilemaps;

//public class TileLevelGen : MonoBehaviour
//{
//    [Header("Level Generation Settings")]
//    public float scrollSpeed = 5f; //speed of the scrolling

//    [Header("Chunk Settings")]
//    public GameObject chunkPrefab; // prefab for the chunk
//    public int chunkWidth = 10; // width of the chunk
//    public Transform obstacleTile; // tile for the obstacle
//    public Transform groundTile; // tile for the ground TODO: MAYBE REMOVE THIS
//    public Transform obstaclePrefab;
//    public Transform groundPrefab;

//    [Range(0, 1)]
//    public float obstacleChance = 0.1f; // chance of an obstacle being placed in a tile

//    [Header("Object Pool Settings")]
//    public int initialPoolSize = 5; // initial size of the object pool
//    public int maxPoolSize = 10; // max size of the object pool

//    private ObjectPool<GameObject> chunkPool; // object pool for the chunks
//    private List<GameObject> activeChunks = new List<GameObject>(); // list of active chunks
//    private Camera mainCamera;
//    private float screenLeftEdgeWorldX;
//    private float screenRightEdgeWorldX;
//    private float nextSpawnPosX = 0f; // x position of the next chunk to spawn

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        mainCamera = Camera.main; // get the main camera
//        if (chunkPrefab == null)
//        {
//            Debug.LogError("Chunk prefab is not assigned in the inspector.");
//            return;
//        }

//        Tilemap chunkTilemap = chunkPrefab.GetComponentInChildren<Tilemap>();
//        if (chunkTilemap == null)
//        {
//            Debug.LogError("Chunk prefab does not have a Tilemap component.");
//            return;
//        }

//        chunkPool = new ObjectPool<GameObject>(
//            createFunc: CreateNewChunk, // create a new chunk
//            actionOnGet: PositionAndActivate, // activate the chunk
//            actionOnRelease: DeactivateChunk, // deactivate the chunk
//            actionOnDestroy: DestroyChunk, // destroy the chunk
//            collectionCheck: false,
//            defaultCapacity: initialPoolSize,// initial size of the pool
//            maxSize: maxPoolSize // max size of the pool
//        );

//        CalculateScreenBounds();

//        while (nextSpawnPosX < screenRightEdgeWorldX + chunkWidth)
//        {
//            chunkPool.Get(); // get a chunk from the pool
//        }
//    }

//    private void DeactivateChunk(GameObject chunk)
//    {
//        chunk.SetActive(false); // deactivate the chunk
//    }

//    private void DestroyChunk(GameObject chunk) => Destroy(chunk); // destroy the chunk

//    private GameObject CreateNewChunk()
//    {
//        GameObject chunk = Instantiate(chunkPrefab, transform); // instantiate a new chunk
//        chunk.SetActive(false); // deactivate the chunk

//        if (chunk.GetComponentInChildren<Tilemap>() == null)
//        {
//            Debug.LogError("Chunk prefab does not have a Tilemap component.");
//            Destroy(chunk);
//            return null;
//        }

//        return chunk;
//    }

//    private void PositionAndActivate(GameObject chunk)
//    {
//        chunk.transform.position = new Vector3(nextSpawnPosX, 0f, 0f); // set the position of the chunk
//        chunk.SetActive(true); // activate the chunk
//        activeChunks.Add(chunk); // add the chunk to the list of active chunks
//        nextSpawnPosX += chunkWidth; // update the position for the next chunk

//        Tilemap chunkTilemap = chunk.GetComponentInChildren<Tilemap>();

//        chunkTilemap.ClearAllTiles(); // clear all tiles in the chunk

//        //for (int x = 0; x < chunkWidth; x++)
//        //{
//        //    Vector3Int tilePos = new Vector3Int(x, -5, 0); // position of the tile in the chunk
//        //    chunkTilemap.SetTile(tilePos, groundTile); // set the ground tile

//        //    // Randomly place obstacles in the chunk
//        //    if (Random.value < obstacleChance)
//        //    {
//        //        Vector3Int obstaclePos = new Vector3Int(x, -4, 0); // position of the obstacle tile
//        //        chunkTilemap.SetTile(obstaclePos, obstacleTile); // set the obstacle tile
//        //    }
//        //}

//        for (int x = 0; x < chunkWidth; x++)
//        {
//            Vector3 groundPos = new Vector3(x, -5, 0); // World position for the ground object
//            Instantiate(groundPrefab, groundPos, Quaternion.identity, chunkParent); // Spawn ground

//            // Randomly place obstacles in the chunk
//            if (Random.value < obstacleChance)
//            {
//                Vector3 obstaclePos = new Vector3(x, -4, 0); // World position for the obstacle
//                Instantiate(obstaclePrefab, obstaclePos, Quaternion.identity, chunkPrefab); // Spawn obstacle
//            }
//        }


//        for (int x = 0; x < chunkWidth; x++)
//        {
//            Vector3 groundPos = new Vector3(x, -5, 0); // position of the ground in the chunk
//            Instantiate(groundTile, groundPos, Quaternion.identity, chunkTilemap); // instantiate ground prefab

//            // Randomly place obstacles in the chunk
//            if (Random.value < obstacleChance)
//            {
//                Vector3 obstaclePos = new Vector3(x, -4, 0); // position of the obstacle
//                Instantiate(obstacleTile, obstaclePos, Quaternion.identity, chunkTilemap); // instantiate obstacle prefab
//            }
//        }

//    }

//    private void CalculateScreenBounds()
//    {
//        float screenHeight = mainCamera.orthographicSize * 2f; // get the height of the screen
//        float screenWidth = screenHeight * mainCamera.aspect; // get the width of the screen
//        screenLeftEdgeWorldX = mainCamera.transform.position.x - screenWidth / 2f; // get the left edge of the screen
//        screenRightEdgeWorldX = mainCamera.transform.position.x + screenWidth / 2f; // get the right edge of the screen
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        float deltaScroll = scrollSpeed * Time.deltaTime; // scroll the chunks
//        for (int i = activeChunks.Count - 1; i >= 0; i--)
//        {
//            GameObject chunk = activeChunks[i];
//            chunk.transform.position -= new Vector3(deltaScroll, 0f, 0f); // move the chunk to the left
//        }

//        nextSpawnPosX -= deltaScroll; // update the position for the next chunk

//        // Check if the leftmost chunk is off-screen
//        if (activeChunks.Count > 0 && (activeChunks[0].transform.position.x + chunkWidth) < screenLeftEdgeWorldX)
//        {
//            GameObject chunkToRemove = activeChunks[0]; // get the leftmost chunk
//            activeChunks.RemoveAt(0); // remove it from the list of active chunks
//            chunkPool.Release(chunkToRemove); // release it back to the pool
//        }

//        GameObject rightMostChunk = (activeChunks.Count > 0) ? activeChunks[activeChunks.Count - 1] : null; // get the rightmost chunk
//        float rightMostEdgeX = (rightMostChunk != null) ? rightMostChunk.transform.position.x + chunkWidth : nextSpawnPosX; // get the rightmost edge of the chunks

//        // Check if we need to spawn a new chunk
//        if (rightMostEdgeX < screenRightEdgeWorldX + chunkWidth)
//        {
//            chunkPool.Get(); // get a new chunk from the pool
//        }
//    }
//}