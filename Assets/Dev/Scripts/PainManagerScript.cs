using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture2D currentMask;
    private Texture2D newMask;
    public Texture2D backgroundImage;
    private Renderer crenderer;
    private SpriteRenderer spriteRenderer;
    public int xOffset;
    public int yOffset;

    public GridManagerScript grid;
    void Start()
    {
        crenderer = GetComponent<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentMask = (Texture2D) crenderer.material.GetTexture("_PaintMask");
        newMask = currentMask;
        ResetCanvas();

        // Camera camera = Camera.main;
        // print(camera.pixelWidth);    
        //print(closestDownMultiple(330.5f, grid.blockPixelSize));
       

    

        
    }

    // Update is called once per frame
    void Update()
    {
       detectPaint();
    }

    public void detectPaint(){
        if(Input.GetMouseButtonDown(0)) {
            //Vector2 mousePos = GetImageMousePositionOnImage();
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos = worldCoordsToImageCoords(clickPos.x, clickPos.y);
            // for now
            //pos = GetImageMousePositionOnImage();
            // for now

            int xIndex = closestDownMultiple((pos.x-xOffset),this.grid.blockPixelSize, true);
            int yIndex = closestDownMultiple((pos.y-yOffset),this.grid.blockPixelSize, false);
            // no esta funcionando el closest
           // string msg = "la posicion en y es "+pos.y.ToString()+" y su yIndex es "+yIndex.ToString();
           // print(msg);
            Vector2Int maxMatrixSize = this.grid.getMatrixDimensions(this.grid.canvasSize, this.grid.blockPixelSize);
            bool areIndexesValid = (xIndex >= 0) && (yIndex >= 0) && (xIndex <maxMatrixSize.x && (yIndex < maxMatrixSize.y));
            if(areIndexesValid) {
                Vector2Int finalPos =  mapIndexToCoord(xIndex,yIndex);
                PaintMask(finalPos.x,finalPos.y,this.grid.blockPixelSize,this.grid.blockPixelSize );
            } else {
                
                print("Se intentó pintar fuera del grid en x="+pos.x.ToString() + " y=" +pos.y.ToString());
            }
           
           //PaintMask(0,0,256,256,Color.red);
           //PaintMask(getLowerLeftCoords().x,getLowerLeftCoords().y,128,128);
           
        }
    }

    public void PaintMask(int x, int y, int width, int height){
        // int xCentered = x;
        // int yCentered = y;
        // xCentered -= splashTextures[0].width/2;
        // yCentered-= splashTextures[0].height/2;
        //Color[] cArray = new Color[width*height];
        //Color[] currentPixels = this.newMask.GetPixels(xCentered, yCentered, width, height, 0);
        Color[] imagePixels = this.backgroundImage.GetPixels(x, y, width, height, 0);
        //Color[] splash = this.splashTextures[Random.Range(0,this.splashTextures.Length)].GetPixels();
        // for(int i = 0; i < cArray.Length; i++) {
 
        //     if(splash[i].r >= 0.1) {
        //         cArray[i]=imagePixels[i];
        //     } else {
               
        //         cArray[i] = currentPixels[i];
        //     }
          
        //  }
        this.newMask.SetPixels(x,y,width,height,imagePixels, 0);
 

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

// se borrara GetImageMousePositionOnImage
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

    public Vector2Int getLowerLeftCoords(){
        Camera camera = Camera.main;
        Vector3 pos = camera.ViewportToWorldPoint(new Vector3(0,0,camera.nearClipPlane));
        Sprite sprite = spriteRenderer.sprite;
        Rect rect =  sprite.textureRect;
        float x = pos.x-gameObject.transform.position.x;
        float y = pos.y-gameObject.transform.position.y;
        x *= sprite.pixelsPerUnit;
        y *= sprite.pixelsPerUnit;
        x+= rect.width/2;
        y+= rect.height/2;
        x += rect.x;
        y += rect.y;
        int realX = Mathf.FloorToInt(x);
        int realY = Mathf.FloorToInt(y); 

        return new Vector2Int(realX, realY);
    }

    public Vector2 worldCoordsToImageCoords(float worldX,float worldY) {
        Sprite sprite = spriteRenderer.sprite;
        Rect rect =  sprite.textureRect;
        float x = worldX-gameObject.transform.position.x;
        float y = worldY-gameObject.transform.position.y;
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

    public int closestDownMultiple(float num, int blockPixelSize, bool isX){
        // returns the index of the closest rounded down blockPixelSize multiple
        int previousMultiple = 0;
        int nextMultiple = 0;
        int maxIteration = (grid.canvasSize.y /blockPixelSize)-1;
        for(int i = 0; i<= maxIteration;i++){
            previousMultiple = nextMultiple;
            nextMultiple = blockPixelSize*(i+1);
            if((previousMultiple <= num)&&(num < nextMultiple)){
                if (isX){
                    return i;
                } else {
                    return maxIteration-i+1;
                }
               
            }
           
        }
        return -1;
    }

    public Vector2Int mapIndexToCoord(int Xindex, int YIndex){
        return this.grid.getCoordsMatrix()[YIndex, Xindex];
    }



}
