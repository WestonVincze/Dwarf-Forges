using UnityEngine;
using UnityEngine.Events;

public abstract class DestroyableMonoBehavior : MonoBehaviour
{
    public UnityAction OnDestroyEvent;

    private void OnDestroy()
    {
       OnDestroyEvent?.Invoke();
       OnDestroyEvent = null;
    }

    public void Destroy(float time = 0)
    {
        Destroy(gameObject, time);
    }
}
