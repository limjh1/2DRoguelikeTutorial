using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    //최소 최대값이 들어가는 클래스
    public class  Count
    {
        public int minimum;
        public int maximum;
        
        //생성자
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    //게임판 사이즈
    public int columns = 8;
    public int rows = 8;

    //게임 내부에 존재하는 음식과 벽의 개수
    //Count라는 곳을 생성하고 foodCount를 할당
    public Count foodCount = new Count(1, 5);
    public Count wallCount = new Count(5, 9);
    
    //게임 내부에 존재하는 오브젝트 프리팹들을 담고있는 변수
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] enemyTiles;

    //랜덤한 위치값을 정하기 위한 리스트
    List<Vector3> gridPosition = new List<Vector3>();

    //바닥 타일을 가지고 있는 오브젝트
    Transform boardHolder;
    
    //모든 위치값을 담고있는 그릇에서 하나씩 빼서 랜덤값으로 리턴한다
    Vector3 RandomPosition()
    {
        //인덱스
        int randomIndex = Random.Range(0, gridPosition.Count);
        //위치
        Vector3 randomPosition = gridPosition[randomIndex];
        gridPosition.RemoveAt(randomIndex);

        return randomPosition;
    }

    //랜덤한 위치를 얻기위해 위치값들을 모두 담고있는 그릇을 초기화한다
    void InitializeList()
    {
        gridPosition.Clear();

        for (int i = 1; i < columns-1; i++)
        {
            for (int j = 1; j < rows-1; j++)
            {
                gridPosition.Add(new Vector3(i, j, 0));
            }
        }
    }

    //기본 바닥 타일을 까는 함수
    void BoardSetup()
    {
        //바닥을 담고있는 Board라는 게임오브젝트 생성 후 
        boardHolder = new GameObject("Board").transform;
        for (int i = -1; i < columns+1; i++)
        {
            for (int j = -1; j < rows+1; j++)
            {
                //모든 칸이 비어있지 않게 floorTiles들 중 랜덤한 타일을 선택한다
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                //위치값이 테두리라면 outerWallTiles들 중 랜덤한 타일을 선택한다
                if (i == -1 || i == columns || j == -1 || j == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                //선택한 타일을 게임상에 소환한다
                GameObject instance = Instantiate(toInstantiate, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                //소환한 타일을 Board라는 게임오브젝트 안에 넣는다
                instance.transform.SetParent(boardHolder);
            }
            
        }
    }

    //오브젝트를 랜덤한 위치에 Instantiate하는 함수
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();

            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        int enemyCount = (int)Mathf.Log(level, 2f); // level이 3일때 1 이상이 된다. (3라운드부터 적이 나옴)
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0), Quaternion.identity);

    }


}
