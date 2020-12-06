using UnityEngine;

public class RandomModel : MonoBehaviour
{
    public GameObject[] Models;
    private void Awake()
    {
        //Models[Random.Range(0, Models.Length)].SetActive(true);
    }
}
