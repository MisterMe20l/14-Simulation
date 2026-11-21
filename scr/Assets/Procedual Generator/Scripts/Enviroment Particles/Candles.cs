using UnityEngine;

public class Candles : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private SO_FireSpecs allFires;

    private ParticleSystem pS;
    private bool stopped = false;

    private void Awake()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        pS = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        var cameraInRange = CameraInRange();
        if (!stopped && !cameraInRange)
        {
            stopped = true;
            pS.Pause();
            return;
        }
        else if (stopped && cameraInRange)
        {
            stopped = false;
            pS.Play();
        }
    }

    private bool CameraInRange()
    {
        var cameraPos = cameraTransform.position;
        var cameraDistance = Vector3.Distance(transform.position, cameraPos);
        var toReturn = (cameraDistance < allFires.CameraRenderDistance) ? true : false;
        return toReturn;
    }
}
