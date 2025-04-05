using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    [SerializeField] GameObject prefabEffectMatch3;
    private float timeEndAni = 0.6f;
    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
    }
    public void SpawnEffectMatch3(Transform positionSpawn)
    {
        GameObject obj = Instantiate(prefabEffectMatch3, positionSpawn.transform.position, Quaternion.identity);
        Destroy(obj, timeEndAni);
    }
    public void SpawnEffectMatch3(int x, int y)
    {
        GameObject obj = Instantiate(prefabEffectMatch3, new Vector2(x, y), Quaternion.identity);
        Destroy(obj, timeEndAni);
    }
    public void SpawnEffectMatch3(Vector2Int pos, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            GameObject obj = Instantiate(prefabEffectMatch3, new Vector2(pos.x, pos.y), Quaternion.identity);
            Destroy(obj, timeEndAni);
        }
    }
}