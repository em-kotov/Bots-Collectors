using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public Action ReadyToBeRemoved;

    public void CreateFort(Fort original, Bot bot)
    {
        Fort fort = Instantiate(original, transform.position, Quaternion.identity);
        fort.InitializeOnFlagCreate(bot);
        ReadyToBeRemoved?.Invoke();
    }
}
