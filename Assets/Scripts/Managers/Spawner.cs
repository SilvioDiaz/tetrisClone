using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour{

    public Shape[] m_allShapes;

    Shape getRandomShape(){
        int i = Random.Range(0,m_allShapes.Length);
        return m_allShapes[i];
    }

    public Shape SpawnShape(){
        Shape shape = null;
        shape = Instantiate(getRandomShape(),transform.position,Quaternion.identity) as Shape;
        return shape;
    }

    // Start is called before the first frame update
    void Start(){
        Vector2 originalVector = new Vector2(4.3f,1.3f);
        Vector2 newVector = VectorF.Round(originalVector);        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
