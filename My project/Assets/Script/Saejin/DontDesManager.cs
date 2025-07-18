using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDesManager : MonoBehaviour
{
    public static DontDesManager Instance { get; private set; }
    [SerializeField] RegionManager regionPrefab;
    public GameObject manager;
    public GameObject mapScreen;
    public GameObject grid;
    private StageNode stageNode;
    private GameObject tileManager;
    private RegionController tile;
    private string lastUnloaded;
    private Camera tileCam;
    private Vector3 camPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(manager);
            DontDestroyOnLoad(mapScreen);

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        else
        {
            Destroy(gameObject);
            Destroy(mapScreen);
        }
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MapScene")
        {
            mapScreen.SetActive(true);
            EnterStage es = stageNode.GetComponent<EnterStage>();
            es.SetLineColor(stageNode);
        }
        if (lastUnloaded.EndsWith("EliteBattleScene"))
        {
            Destroy(grid);
            Destroy(tileManager);
        }

        if (lastUnloaded.EndsWith("BattleScene"))
        {
            if (grid == null)
            {
                var tilePrefab = GameObject.Find("Tiles");
                if (tilePrefab != null)
                {
                    grid = tilePrefab;
                    DontDestroyOnLoad(grid);
                }
            }
            else
            {
                foreach (var root in scene.GetRootGameObjects())
                {
                    if (root.name == "Tiles" && root != grid)
                    {
                        Destroy(root);
                        break;
                    }
                }
            }

            if (tileManager == null)
            {
                var tmPrefab = GameObject.Find("RegionManager");
                if (tmPrefab != null)
                {
                    tileManager = tmPrefab;
                    DontDestroyOnLoad(tileManager);
                }
            }
            else
            {
                foreach (var root in scene.GetRootGameObjects())
                {
                    if (root.name == "RegionManager" && root != tileManager)
                    {
                        Destroy(root);
                        break;
                    }
                }
            }
            var cam = GameObject.Find("Main Camera");
            tileCam = cam.GetComponent<Camera>();
            tileCam.transform.position = camPosition;
            grid.SetActive(true);
            var tManager = tileManager.GetComponent<RegionManager>();
            tManager.SetNextTile(tile);
        }
        else if (scene.name.EndsWith("FieldScene"))
        {
            Debug.Log("Field ȣ��");
            var regionManager = GameObject.Find("RegionManager");
            tileManager = regionManager;
            var tManager = tileManager.GetComponent<RegionManager>();

            if (scene.name.StartsWith("Felix"))
            {
                Debug.Log("Felix");
                tManager.stageType = 0;
            }
            else if (scene.name.StartsWith("Phobia"))
            {
                Debug.Log("Phobia");
                tManager.stageType = 1;
            }
            else if (scene.name.StartsWith("Odium"))
            {
                tManager.stageType = 2;
                Debug.Log("Odium");
            }
            else if (scene.name.StartsWith("Amare"))
            {
                tManager.stageType = 3;
                Debug.Log("Amare");
            }
            else if (scene.name.StartsWith("Irascor"))
            {
                tManager.stageType = 4;
                Debug.Log("Irascor");
            }
            else if (scene.name.StartsWith("Lacrima"))
            {
                tManager.stageType = 5;
                Debug.Log("Lacrima");
            }
            else if (scene.name.StartsWith("Havet"))
            {
                tManager.stageType = 6;
                Debug.Log("Havet");
            }
            mapScreen.SetActive(false);
            DontDestroyOnLoad(tileManager);
        }

        if (scene.name.EndsWith("BattleScene"))
        {
            grid.SetActive(false);
        }
    }
    public void OnSceneUnloaded(Scene scene)
    {
        lastUnloaded = scene.name;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void SetStage(StageNode node)
    {
        stageNode = node;
    }

    public void SetTile(RegionController target, RegionManager m)
    {
        tile = target;
    }

    public void setCamPosition(Vector3 cp)
    {
        camPosition = cp;
    }

    public void setDesGrid()
    {
        grid = GameObject.Find("Tiles");
        DontDestroyOnLoad(grid);
    }
}
