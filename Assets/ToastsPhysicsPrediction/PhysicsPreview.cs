using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPreview : MonoBehaviour {
    
    public int iterations = 10;
    public float timeStep = 0.5f;
    private int inBetweenFrameSteps = 10;
    private static int InBetweenFrameSteps = 10; //1frame 
    public static int Iterations = 10   ;
    private static float TimeStep = 0.5f;
    public static int InBetweenFrameStepsValue { get { return InBetweenFrameStepsValue; } }
    
    private static PhysicsPreview instance;
    public bool startInFreeze;
    public static bool timeIsFreezed;
    public static bool physcisAreRunning;
    public static Action delegateAfterFinishRolling;

    // Use this for initialization
    void Awake () {
        //Set custom values
        Iterations = iterations;
        TimeStep = timeStep;
        InBetweenFrameSteps = inBetweenFrameSteps;


        Physics.autoSimulation = false;
        instance = this;

        timeIsFreezed = startInFreeze;

        if (!startInFreeze)
        {
            SetPhysicsStart();
        }

        StartCoroutine(cleanGarbage());
	}

    public static void Preview()
    {
        for (int i = 0; i < Iterations; i++)
        {
            PreviewedObject.SetAllGhostPositions(i);
            for (int j = 0; j < InBetweenFrameSteps; j++)
            {
                PreviewedObject.DrawLine(j + (InBetweenFrameSteps * i));
                Physics.Simulate(TimeStep/InBetweenFrameSteps);
            }
        }
        PreviewedObject.ResetZeroPositions();
        PreviewedObject.ActivateLine(true);
    }

    public static void SetPhysicsStop()
    {
        timeIsFreezed = true;
        physcisAreRunning = false;
        instance.StopAllCoroutines();
    }

    public static void SetPhysicsStart()
    {
        if (physcisAreRunning)
        {
            Debug.Log("Warning! SetPhysicsStart() is called although the physics are already running!!");
            return;
        }
        timeIsFreezed = false;
        physcisAreRunning = true;
        instance.StartCoroutine(instance.PhysicsLoop());
    }

    public static void SetPhysicsSimulateOnlyStart()
    {
        if (physcisAreRunning)
        {
            Debug.Log("Warning! SetPhysicsStart() is called although the physics are already running!!");
            return;
        }
        timeIsFreezed = false;
        physcisAreRunning = true;
        instance.StartCoroutine(instance.PhysicsSimulLoop());
    }

    public static void SetPhysicsSimulatePlayStart()
    {
        if (physcisAreRunning)
        {
            Debug.Log("Warning! SetPhysicsStart() is called although the physics are already running!!");
            return;
        }
        timeIsFreezed = false;
        physcisAreRunning = true;
        instance.StartCoroutine(instance.PhysicsLoop());
    }
    public static void PhysicsRecordPlayStart()
    {
        instance.StartCoroutine(instance.DiceRecordPlay());
    }

    public static void RemoveGhosts()
    {
        PreviewedObject.ActivateLine(false);
        GhostPreview.RemoveAllGhosts();
        GhostPreview.instances = new List<GhostPreview>();
    }

    IEnumerator cleanGarbage()
    {
        while (true)
        {
            System.GC.Collect();
            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator PhysicsSimulLoop()
    {
        bool initRolling = false;

        yield return null;

        while (physcisAreRunning)
        {

            Physics.Simulate(TimeStep / InBetweenFrameSteps);

            if (!initRolling && !Dice.IsRollingUpdate())
            {
               
            }
            else
                initRolling = true;

            if (initRolling)
            {
                if (!Dice.IsRollingUpdate())
                {
                    Debug.Log("dice stop");

                    if (delegateAfterFinishRolling != null)
                    {
                       
                        delegateAfterFinishRolling.Invoke();
                    }
                       
                    break;
                }
                else
                {
                    Dice.SetDataAllDice();
                    Debug.Log("rolling");
                }
            }
        }

        yield return null;
    }
    IEnumerator PhysicsLoop()
    {
        float timer = 0;
        float interval = (TimeStep / InBetweenFrameSteps);
        while (physcisAreRunning)
        {
            timer += Time.deltaTime;

            while (timer >= interval)
            {
                timer -= interval;
                Physics.Simulate(interval);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator DiceRecordPlay()
    {
        int playIndex = 0;
        int stopCount = 0;
        ArrayList rollingDie = Dice.GetAllDice();
        while (playIndex<500)
        {
            foreach (RollingDie item in rollingDie)
            {
                if (item.SetSyncWithRecord(playIndex) == false)
                    stopCount++;
            }
          
            yield return new WaitForSeconds(Time.deltaTime);

            if (stopCount == rollingDie.Count)
                break;

            playIndex++;
        }
        yield return null;
    }
}
