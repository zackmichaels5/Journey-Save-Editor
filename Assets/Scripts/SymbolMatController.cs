using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolMatController : MonoBehaviour
{
    private float alpha = 0;

    private float alphaGoal = 0;

    public float speed = 2;
    public float vanishSpeed = 4;

    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        //Update symbol material alpha
        mat.SetFloat("Alpha", alpha);
    }

    // Update is called once per frame
    void Update()
    {
        //Change alpha to move towards goal alpha and set the material alpha
        if (alpha > alphaGoal)
        {
            alpha = Mathf.Clamp(alpha - vanishSpeed * Time.deltaTime, alphaGoal, 1);
            mat.SetFloat("Alpha", alpha);
        }
        else if (alpha < alphaGoal)
        {
            alpha = Mathf.Clamp(alpha + speed * Time.deltaTime, 0, alphaGoal);
            mat.SetFloat("Alpha", alpha);
        }
    }

    //Set the goal alpha
    public void SetAlpha(float al)
    {
        alphaGoal = al;
    }
}
