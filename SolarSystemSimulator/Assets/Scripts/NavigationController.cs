using UnityEngine;
using UnityEngine.Serialization;

public class NavigationController : MonoBehaviour
{
    [SerializeField] private Transform _virtualBoxTarget;
    [SerializeField] private Vector3 _virtualBoxSize;
    
    void Start()
    {
        CreateVirtualBox();
    }

    private void CreateVirtualBox()
    {
        
    }
}
