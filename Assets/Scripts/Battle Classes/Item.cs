using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Item : Action
{
    override abstract public void Do_Action(List<GameObject> targets);
}
