using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2[,] coordsMatrix;
    private int[,] isPaintedMatrix;//0 not painted, 1 painted
    public Vector2Int canvasSize;
    public int blockPixelSize;//potencia de 2
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.coordsMatrix = initializeCoordsMatrix();
        this.isPaintedMatrix = initializeIsPaintedMatrix(this.canvasSize, this.blockPixelSize);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2[,] initializeCoordsMatrix(){
        Camera camera = Camera.main;
        // Vector3 upperLeftScreen = new Vector3(0, Screen.height, 0 );
        // Vector3 upperRightScreen = new Vector3(Screen.width, Screen.height, 0);
      
        // Vector3 lowerRightScreen = new Vector3(Screen.width, 0, 0);
    
    //Corner locations in world coordinates
        // Vector3 upperLeft = camera.ScreenToWorldPoint(upperLeftScreen);
        // Vector3 upperRight = camera.ScreenToWorldPoint(upperRightScreen);
        Vector3 lowerLeft =  camera.ViewportToWorldPoint(new Vector3(0,0,camera.nearClipPlane));
        // Vector3 lowerRight = camera.ScreenToWorldPoint(lowerRightScreen);
        print(lowerLeft);

        return null;
    }

    public int[,] initializeIsPaintedMatrix(Vector2Int canvasSize, int blockPixelSize){
        int unitsInX = canvasSize.x / blockPixelSize;
        int unitsInY = canvasSize.y / blockPixelSize;

        return new int[unitsInX,unitsInY];
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

    
}
