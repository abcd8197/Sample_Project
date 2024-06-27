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

        // 가비지 컬렉션 실행 및 메모리 할당 초기화
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // 직접 호출
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

        // Reflection 호출
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
