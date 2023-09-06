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
        if(gameObject.transform.parent)
        {
            Destroy(gameObject.transform.parent, time);
        }
        else
        {
            Destroy(gameObject, time);
        }
    }
}
