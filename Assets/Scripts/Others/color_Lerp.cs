using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class color_Lerp : MonoBehaviour
{
    public Color targetColor = new Color( 0, 1, 0, 1 );
    Material materialToChange;

    void Start( )
    {
        materialToChange = gameObject.GetComponent<Renderer>( ).material;
        StartCoroutine( LerpFunction( targetColor, 5 ) );
        
    }

    IEnumerator LerpFunction(Color endValue, float duration)
    {
        float time = 0;
        Color startValue = materialToChange.color;

        while(time < duration)
        {
            materialToChange.color = Color.Lerp( startValue, endValue, time / duration );
            time += Time.deltaTime;
            yield return null;
        }
        materialToChange.color = endValue;
    }
}
