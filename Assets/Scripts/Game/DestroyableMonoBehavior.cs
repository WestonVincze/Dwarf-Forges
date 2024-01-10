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

        /*
        //Old code to pass destruction upwards, caused issues.. Needs to determine if parent should be informed in a more robust fashion
        if(gameObject.transform.parent)
        {
            Destroy(gameObject.transform.parent, time);
        }
        else
        {
            Destroy(gameObject, time);
        }
        */
    }
}
