using System.Collections;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(WaitForGameManager());
    }


    IEnumerator WaitForGameManager()
    {
        yield return new WaitUntil(() => GameManager.instance != null);
        GameManager.instance.AddDebugEnterAction(() => gameObject.SetActive(true));
        GameManager.instance.AddDebugExitAction(() => gameObject.SetActive(false));

        gameObject.SetActive(GameManager.instance.inDebugMode);
    }
}