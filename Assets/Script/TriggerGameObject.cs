using UnityEngine;

public class TriggerGameObject : MonoBehaviour
{
    public GameObject gamePrefab;
    private ITriggerObject triggerObject;
    public bool triggerByEntrance;

    private void Start()
    {
        if (triggerObject == null)
        {
            triggerObject = gamePrefab.GetComponent<ITriggerObject>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerObject?.TriggerEnter(other.gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        triggerObject?.TriggerStay(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        triggerObject?.TriggerExit(other.gameObject);
    }
}
