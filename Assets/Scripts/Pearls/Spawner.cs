using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private int _poolCapacity = 30;
    [SerializeField] private int _poolMaxSize = 40;

    protected ObjectPool<T> Pool;

    private void Awake()
    {
        Pool = new ObjectPool<T>(
            createFunc: () => OnCreate(),
            actionOnGet: item => OnGet(item),
            actionOnRelease: item => OnRelease(item),
            actionOnDestroy: item => Destroy(item.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private T OnCreate()
    {
        return Instantiate(_prefab);
    }

    protected virtual void OnGet(T item)
    {
        item.gameObject.SetActive(true);
    }

    protected virtual void OnRelease(T item)
    {
        item.gameObject.SetActive(false);
    }

    protected void SetItemPosition(T item, Vector3 position)
    {
        item.transform.position = position;
    }
}