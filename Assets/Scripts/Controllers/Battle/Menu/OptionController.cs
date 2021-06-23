using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class OptionController : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Battle_Option> options;
    public GameObject option_prefab;
    public float x_offset;
    public float y_offset;

    abstract public void Set_Options();
}
