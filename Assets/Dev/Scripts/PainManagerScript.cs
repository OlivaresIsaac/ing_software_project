using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture2D currentMask;
    private Texture2D newMask;
    private Renderer crenderer;
    public Texture2D[] splashTextures;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        crenderer = GetComponent<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentMask = (Texture2D) crenderer.material.GetTexture("_PaintMask");
        newMask = currentMask;
        ResetCanvas();

       

    

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = GetImageMousePositionOnImage();
            //TODO automatizar tamaño
         PaintMask((int)mousePos.x,(int)mousePos.y,256,256,Color.red);
           //PaintMask(0,0,256,256,Color.red);
        }
    }

    public void PaintMask(int x, int y, int width, int height, Color color){
        int xCentered = x;
        int yCentered = y;
        xCentered -= splashTextures[0].width/2;
        yCentered-= splashTextures[0].height/2;
        Color[] cArray = new Color[width*height];
        Color[] currentPixels = this.newMask.GetPixels(xCentered, yCentered, width, height, 0);
        Color[] splash = this.splashTextures[0].GetPixels();
        for(int i = 0; i < cArray.Length; i++) {
 
            if(splash[i].r >= 0.1) {
                cArray[i]=color;
            } else {
               
                cArray[i] = currentPixels[i];
            }
          
         }
        this.newMask.SetPixels(xCentered,yCentered,width,height,cArray, 0);
 

        this.newMask.Apply();
        this.crenderer.material.mainTexture = newMask;
      
    
    }

    public void ResetCanvas(){
      
        Vector2 canvasSize = new Vector2(currentMask.width, currentMask.height);
        Color[] cArray = new Color[(int)canvasSize.x*(int)canvasSize.y];
        for(int i = 0; i < cArray.Length; i++) {
            cArray[i]=Color.black;
        }
        this.newMask.SetPixels(0,0,(int)canvasSize.x,(int)canvasSize.y,cArray, 0);
        this.newMask.Apply();
        this.crenderer.material.mainTexture = newMask;
    }

    private void OnDestroy() {
        ResetCanvas();
    }

    private Vector2 GetImageMousePositionOnImage(){
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Sprite sprite = spriteRenderer.sprite;
        Rect rect =  sprite.textureRect;
        float x = pos.x-gameObject.transform.position.x;
        float y = pos.y-gameObject.transform.position.y;
        x *= sprite.pixelsPerUnit;
        y *= sprite.pixelsPerUnit;
        // x*= currentMask.width;
        // y*= currentMask.height;
        x+= rect.width/2;
        y+= rect.height/2;
        x += rect.x;
        y += rect.y;
        int realX = Mathf.FloorToInt(x);
        int realY = Mathf.FloorToInt(y);    
        return(new Vector2(x,y));
    }

}
