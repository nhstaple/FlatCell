
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Controller.Player;

namespace Utils.UIManager
{

    public class UIManager : MonoBehaviour
    {
        GameObject player;
        public PlayerController controller;

        public GameObject GetPlayerObject()
        {
            if (player == null)
            {
                GetPlayer();
            }
            return player;
        }

        public PlayerController GetPlayerController()
        {
            if(controller == null)
            {
                GetPlayer();
            }
            return controller;
        }

        void GetPlayer()
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
            }
            if (controller == null)
            {
                controller = player.GetComponent<PlayerController>();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            GetPlayer();
        }

        // Update is called once per frame
        void Update()
        {
            GetPlayer();
        }
    }
}