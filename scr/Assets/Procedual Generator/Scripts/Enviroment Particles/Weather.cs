using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    [SerializeField] private SO_Time clock;
    [SerializeField] private float m_routineTime;
    private ParticleSystem weatherPS;

    private bool startWindStarted;
    private bool endtWindStarted;

    private void Start()
    {
        weatherPS = GetComponent<ParticleSystem>();
        weatherPS.startSpeed = 5f;
        weatherPS.emissionRate = 0f;
    }

    private void Update()
    {
        if (!startWindStarted && clock.Hour == 22)
        {
            startWindStarted = true;
            StartCoroutine(StartWindRoutine());
        }
        if (!endtWindStarted && clock.Hour == 2)
        {
            endtWindStarted = true;
            StartCoroutine(EndtWindRoutine());
        }
    }

    private IEnumerator StartWindRoutine()
    {
        var timer = 0f;
        while (timer < m_routineTime)
        {
            timer++;
            weatherPS.emissionRate++;
            weatherPS.startSpeed += .2f;
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator EndtWindRoutine()
    {
        var timer = 0f;
        while (timer < m_routineTime)
        {
            timer++;
            weatherPS.emissionRate--;
            weatherPS.startSpeed -= .2f;
            yield return new WaitForSeconds(1);
        }
    }
}
