﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DEEP.AI;

namespace DEEP.spawn
{
    public class spawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> entitieList = new List<GameObject>();
        public GameObject startingPoint;
        public int i = 0;
        float time = 2;
        
        void Start()
        {
            spawnObjects();
        }

        void Update(){
            time -= Time.deltaTime;
            if(time <= 0){
                spawnObject(i);
            
                i = (i+1)% entitieList.Count;
                time = 2;
            }

        }

        public void spawnObject(int id){

            if (id < 0 || id > entitieList.Count)
            {
                Debug.Log("Invalid Id");
                return;
            }

            GameObject instance = Instantiate(entitieList[id],this.transform);
            instance.GetComponent<EnemyAISystem>().addPatrolPoint(startingPoint);
            instance.GetComponent<EnemyAISystem>().ResetPatrol();
        }

        public void spawnNObject(int id, int n){

            if (id < 0 || id > entitieList.Count)
            {
                Debug.Log("Invalid Id");
                return;
            }

            for (int i = 0; i < n; i++)
            {
                spawnObject(id);    
            }
        }

        public void spawnObjects(){
            for (int i = 0; i < entitieList.Count; i++)
            {
                spawnObject(i);
            }
        }

    }
    
}
