using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
abstract public class Action : MonoBehaviour
{
    abstract public void Do_Action(List<GameObject> targets);
}
