using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture2D currentMask;
    private Texture2D newMask;
    private Renderer crenderer;
    void Start()
    {
        crenderer = GetComponent<Renderer>();
        currentMask = (Texture2D) crenderer.material.GetTexture("_PaintMask");
        newMask = currentMask;
        ResetCanvas();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)) {
           PaintMask(250,250,100,100,Color.red);
        }
    }

    public void PaintMask(int x, int y, int width, int height, Color color){
        Color[] cArray = new Color[width*height];
        for(int i = 0; i < cArray.Length; i++) {
            cArray[i]=color;
        }
        this.newMask.SetPixels(x,y,width,height,cArray, 0);
        this.newMask.Apply();
        this.crenderer.material.mainTexture = newMask;
    
    }

    public void ResetCanvas(){
        Color[] cArray = new Color[512*512];
        for(int i = 0; i < cArray.Length; i++) {
            cArray[i]=Color.black;
        }
        this.newMask.SetPixels(0,0,512,512,cArray, 0);
        this.newMask.Apply();
        this.crenderer.material.mainTexture = newMask;
    }

    private void OnDestroy() {
        ResetCanvas();
    }

}
