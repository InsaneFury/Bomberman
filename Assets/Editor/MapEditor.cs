using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Propiedades del editor usando MapCreator
[CustomEditor(typeof(MapCreator))]
[CanEditMultipleObjects]
public class MapEditor : Editor
{
    GameObject wallPrefab;
    GameObject innerWallPrefab;
    GameObject destructableWallPrefab;

    MapCreator creator;
    int minGridSize = 5;
    bool scriptActive = false;
    float randomPercent;

    //Sobrescribiendo la funcion OnInspectorGUI para poder customizar el inspector de nuestro script
    public override void OnInspectorGUI()
    {
        //Dibuja el inspector default (osea las variables publicas y las cosas que muestra el script MapCreator)
        DrawDefaultInspector();
        //Hacemos referencia a mapcreator (target devuelve el inspector del script MapCreator)
        creator = (MapCreator)target;

        wallPrefab = creator.wall;
        innerWallPrefab = creator.innerWall;
        destructableWallPrefab = creator.destructableWall;
        randomPercent = creator.percent;

        //Esta funcion nos permite agregar multiples buttons al inspector y mantenerlos de manera horizontal
        //Border Walls GUI
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Border"))
        {
            if (!scriptActive)
            {
                BuildBorder();
            }
        }
        if (GUILayout.Button("Delete Border"))
        {
            if (!scriptActive)
            {
                DeleteBorder();
            }
        } 
        EditorGUILayout.EndHorizontal();
        //Inner walls GUI
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create InnerWalls"))
        {
            if (!scriptActive)
            {
                BuildInnerWalls();
            }
        }
        if (GUILayout.Button("Delete InnerWalls"))
        {
            if (!scriptActive)
            {
                DeleteInnerWalls();
            }
        }
        EditorGUILayout.EndHorizontal();
        //Destructable Walls GUI
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create DestructableWalls"))
        {
            if (!scriptActive)
            {
                BuildDestructableWalls();
            }
        }
        if (GUILayout.Button("Delete DestructableWalls"))
        {
            if (!scriptActive)
            {
                DeleteDestructableWalls();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    //Crea walls al borde del mapa
    void BuildBorder()
    {
        if (CantBuild())
        {
            return;
        }

        //Eliminamos si es que existen bordes anteriores
        DeleteBorder();

        //Reescalando el piso y re posicionando
        ResizeFloor();
        //Activamos el script
        scriptActive = true;

        //Creando walls en x z
        for (int i = 0; i < creator.gridSizeX; i++)
        {
            for (int j = 0; j < creator.gridSizeZ; j++)
            {
                //Recorremos X
                if (i == 0 || i == creator.gridSizeX - 1)
                {
                    GameObject wall = PrefabUtility.InstantiatePrefab(wallPrefab) as GameObject;
                    wall.transform.position = new Vector3(creator.start.x + i + creator.offset.x,
                        creator.start.y + creator.offset.y,
                        creator.start.z + j + creator.offset.z
                        );
                    wall.transform.parent = creator.outerWallHolder;
                }

                //Recorremos Z
                if (j == 0 || j == creator.gridSizeZ - 1)
                {
                    GameObject wall = PrefabUtility.InstantiatePrefab(wallPrefab) as GameObject;
                    wall.transform.position = new Vector3(creator.start.x + i + creator.offset.x,
                        creator.start.y + creator.offset.y,
                        creator.start.z + j + creator.offset.z
                        );
                    wall.transform.parent = creator.outerWallHolder;
                }
            }
        }

        //Desactivamos el script
        scriptActive = false;
    }

    //Destruye los walls al borde del mapa
    void DeleteBorder()
    {
        //Contamos los childs y usamos childs - 1 porque arranca en 0
        int childCount = creator.outerWallHolder.childCount;
        //Recoremos con un for y destruimos con immediate que nos permite destruir fuera de play
        for (int i = childCount-1; i >= 0; i--)
        {
            DestroyImmediate(creator.outerWallHolder.transform.GetChild(i).gameObject);
        }
    }

    //Crea walls internas del mapa
    void BuildInnerWalls()
    {
        if (CantBuild())
        {
            return;
        }

        scriptActive = true;
        DeleteInnerWalls();

        int dist = 2;

        for (int i = dist; i <= creator.gridSizeX-dist; i++)
        {
            for (int j = dist; j <= creator.gridSizeZ-dist; j++)
            {
                if ((i % dist) == 0 && (j % dist) == 0)
                {
                    GameObject wall = PrefabUtility.InstantiatePrefab(innerWallPrefab) as GameObject;
                    wall.transform.position = new Vector3(creator.start.x + i + creator.offset.x,
                        creator.start.y + creator.offset.y,
                        creator.start.z + j + creator.offset.z
                        );
                    wall.transform.parent = creator.innerWallHolder;
                }
            }
        }
        scriptActive = false;
    }

    //Destruye walls internas del mapa
    void DeleteInnerWalls()
    {
        //Contamos los childs y usamos childs - 1 porque arranca en 0
        int childCount = creator.innerWallHolder.childCount;
        //Recoremos con un for y destruimos con immediate que nos permite destruir fuera de play
        for (int i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(creator.innerWallHolder.transform.GetChild(i).gameObject);
        }
    }

    //Crea walls destruibles dentro del mapa de manera random
    void BuildDestructableWalls()
    {
        if (CantBuild())
        {
            return;
        }

        scriptActive = true;
        DeleteDestructableWalls();

        //Ints para evitar spawnear dentro de las paredes exteriores
        int startDist = 1;
        int endDist = 2;

        //int para calcular donde se spawnearon walls internas
        int innerWallPos = 2;

        for (int i = startDist; i <= creator.gridSizeX - endDist; i++)
        {
            for (int j = startDist; j <= creator.gridSizeZ - endDist; j++)
            {
                if ((i % innerWallPos) == 0 && (j % innerWallPos) == 0)
                {
                    continue;
                }
                else
                {
                    if ((Random.Range(0f, 1f)) <= randomPercent)
                    {
                        GameObject wall = PrefabUtility.InstantiatePrefab(destructableWallPrefab) as GameObject;
                        wall.transform.position = new Vector3(creator.start.x + i + creator.offset.x,
                            creator.start.y + creator.offset.y,
                            creator.start.z + j + creator.offset.z
                            );
                        wall.transform.parent = creator.destructablesHolder;
                    }    
                }
            }
        }
        scriptActive = false;
    }

    //Destruye todas las walls destruibles dentro del mapa
    void DeleteDestructableWalls()
    {
        //Contamos los childs y usamos childs - 1 porque arranca en 0
        int childCount = creator.destructablesHolder.childCount;
        //Recoremos con un for y destruimos con immediate que nos permite destruir fuera de play
        for (int i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(creator.destructablesHolder.transform.GetChild(i).gameObject);
        }
    }

    //Cambiamos el tamaño del piso segun nuestro tamaño de mapa
    void ResizeFloor()
    {
        //Calculamos la escala necesaria para el piso dividiendo por 10 ya que la escala es 1
        Vector3 scaler = new Vector3((float)creator.gridSizeX / 10, 1, (float)creator.gridSizeZ / 10);
        //Re escalado
        creator.floor.transform.localScale = scaler;
        //Re posicionando
        creator.floor.transform.position = new Vector3(creator.gridSizeX / 2, 0, creator.gridSizeZ / 2);
        //Ajustando material para que se vea correctamente
        /*creator.floor.transform.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale =
            new Vector2(creator.gridSizeX, creator.gridSizeZ);*/
    }

    //Chekeamos si podemos construir
    bool CantBuild()
    {
        //Checkea que el mapa sea mayor o igual al minimo estipulado para evitar conflictos
        if (creator.gridSizeX < minGridSize || creator.gridSizeZ < minGridSize)
        {
            Debug.LogWarning("Grid size needs to be bigger or equal to minGridSize = " + minGridSize);
            return true;
        }
        //Checkea que el grid size sea par para evitar conflictos
        if (creator.gridSizeX % 2 == 0 || creator.gridSizeZ % 2 == 0)
        {
            Debug.LogWarning("Grid size needs to be uneven number");
            return true;
        }
        return false;
    }
}

