using UnityEngine;

public class ManagerCaller : MonoBehaviour
{
    private void Awake()
    {
        if (EventManager.Instance)
        {
            Debug.Log("Call EventManager");
        }
    }
}
