using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLIneRenderer : MonoBehaviour
{
    LineRenderer line;

    [SerializeField] Texture[] textures;

    int animationStep;

    [SerializeField] float fps;
    float fpsCounter;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        fps = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        fpsCounter += Time.deltaTime;

        if(fpsCounter >= 1f/fps)
        {
            animationStep++;
            if(animationStep == textures.Length)
            {
                animationStep = 0;
            }

            line.material.SetTexture("_MainTex", textures[animationStep]);

            fpsCounter = 0;
        }
    }
}
