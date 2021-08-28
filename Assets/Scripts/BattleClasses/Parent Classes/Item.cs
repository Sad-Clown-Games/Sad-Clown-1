using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Item : Action
{
    
    #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
    #else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
    #endif
    override abstract public void Do_Action();
}
