using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Torches : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private SO_FireSpecs allFires;

    private ParticleSystem pS;
    private bool stopped = false;

    private void Awake()
    {
        pS = GetComponent<ParticleSystem>();
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        //Debug.Log(cameraTransform.name);

        StartCoroutine(FlameRndColor());
        StartCoroutine(FlameRndEmission());
    }

    private void Update()
    {
        var cameraInRange = CameraInRange();
        if (!stopped && !cameraInRange)
        {
            stopped = true;
            pS.Pause();
            StopAllCoroutines();
            return;
        }
        else if (stopped && cameraInRange)
        {
            stopped = false;
            pS.Play();
            StartCoroutine(FlameRndColor());
            StartCoroutine(FlameRndEmission());
        }
    }

    private bool CameraInRange()
    {
        var cameraPos = cameraTransform.position;
        var cameraDistance = Vector3.Distance(transform.position, cameraPos);
        var toReturn = (cameraDistance < allFires.CameraRenderDistance) ? true : false;
        //Debug.Log(Mathf.Round(cameraDistance));
        return toReturn;
    }

    private IEnumerator FlameRndColor()
    {
        while (true)
        {
            var rndGreen = (byte)Random.Range(allFires.RndColorRange.x, allFires.RndColorRange.y);
            var nextColor = new Color32(255, rndGreen, 0, 1);
            allFires.FireMaterial.DOColor(nextColor, allFires.FireLerpDuration);
            var nextChange = Random.Range(allFires.RndColorIntervalRange.x, allFires.RndColorIntervalRange.y);
            //Debug.Log($"Green changed to: {Mathf.Round(rndGreen)}! Next action in: {Mathf.Round(nextChange)}");
            yield return new WaitForSeconds(nextChange);
        }
    }

    private IEnumerator FlameRndEmission()
    {
        while (true)
        {
            var nextEmissionRate = Random.Range(allFires.RndEmissionRange.x, allFires.RndEmissionRange.y);
            pS.emissionRate = Mathf.Lerp(pS.emissionRate, nextEmissionRate, allFires.FireLerpDuration);
            var nextChange = Random.Range(allFires.RndEmissionIntervalRange.x, allFires.RndEmissionIntervalRange.y);
            //Debug.Log($"Emission changed! Next action in: {Mathf.Round(nextChange)}");
            yield return new WaitForSeconds(nextChange);
        }
    }
}
