using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class One_Shot_FX : MonoBehaviour
{
    [SerializeField]
    private float lifespan;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die",lifespan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die(){
        Destroy(this.gameObject);
    }

}
