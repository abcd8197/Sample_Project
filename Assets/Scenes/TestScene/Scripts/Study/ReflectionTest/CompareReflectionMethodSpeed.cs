using System;
using System.Reflection;
using System.Diagnostics;
using UnityEngine;

// Performance Test Script

public class CompareReflectionMethodSpeed : MonoBehaviour
{
    public class MyClass
    {
        public void MyMethod()
        {

        }
    }

    private void Start()
    {
        MyClass myObj = new MyClass();
        MethodInfo methodInfo = typeof(MyClass).GetMethod("MyMethod");
        Stopwatch stopwatch = Stopwatch.StartNew();

        // ������ �÷��� ���� �� �޸� �Ҵ� �ʱ�ȭ
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // ���� ȣ��
        long memAlloc_Before = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
        for (int i = 0; i < 1000000; i++)
        {
            myObj.MyMethod();
        }
        stopwatch.Stop();
        long memAlloc_After = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
        UnityEngine.Debug.Log($"Direct call time: {stopwatch.ElapsedMilliseconds} ms");
        UnityEngine.Debug.Log($"Direct Call Memory Allocation: {memAlloc_After - memAlloc_Before}");

        memAlloc_Before = memAlloc_After;

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        stopwatch.Restart();

        // Reflection ȣ��
        for (int i = 0; i < 1000000; i++)
        {
            methodInfo.Invoke(myObj, null);
        }
        stopwatch.Stop();
        memAlloc_After = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
        UnityEngine.Debug.Log($"Reflection call time: {stopwatch.ElapsedMilliseconds} ms");
        UnityEngine.Debug.Log($"Reflection Call Memory Allocation: {memAlloc_After - memAlloc_Before}");
    }
}
