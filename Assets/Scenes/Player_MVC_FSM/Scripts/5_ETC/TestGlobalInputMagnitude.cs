using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGlobalInputMagnitude : MonoBehaviour
{
    private TMPro.TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = this.GetComponent<TMPro.TextMeshProUGUI>();
    }
}
