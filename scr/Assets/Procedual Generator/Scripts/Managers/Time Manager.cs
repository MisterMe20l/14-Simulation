using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private SO_Time time;
    [SerializeField] private Vector3 m_startValues; // x = Day; y = Hour; z = Minutes
    [SerializeField, Range(1, 100)] private float m_timeSpeed;


    private void Awake()
    {
        time.Day = (int)m_startValues.x;
        time.Hour = (int)m_startValues.y;
        time.Minute = (int)m_startValues.z;

        StartCoroutine(CountingTime());
    }

    private void Update()
    {
        Time.timeScale = m_timeSpeed;
    }

    private IEnumerator CountingTime()
    {
        while (true)
        {
            time.Minute += 1;
            if (time.Minute == 60)
            {
                time.Minute = 0;
                time.Hour += 1;
            }
            if (time.Hour == 24)
            {
                time.Hour = 0;
                time.Day += 1;
            }
            yield return new WaitForSeconds(.5f);
        }
    }
}
