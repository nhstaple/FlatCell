using System.Collections.Generic;
using UnityEngine;

namespace Utils.InputManager
{
    public enum EInput_Type
    {
        Error = -1,
        MouseAndKeyboard = 0,
        XboxOne,
        PS4
    }

    public class InputManager : MonoBehaviour
    {
        [SerializeField] public bool PC_DEBUG = false;
        [SerializeField] public bool XBOX_DEBUG = false;
        [SerializeField] public bool PS4_DEBUG = false;
        [SerializeField] public EInput_Type joystickType;
        private bool disconnected = false;

        private bool oldPC;
        private bool oldXBOX;
        private bool oldPS4;

        private float consolePollRate = 1f;
        private float consoleCounter = 0f;

        private void checkJoySticks()
        {
            string[] joysticks = Input.GetJoystickNames();
            Dictionary<EInput_Type, bool> detected = new Dictionary<EInput_Type, bool>();
            detected.Add(EInput_Type.MouseAndKeyboard, false);
            detected.Add(EInput_Type.XboxOne, false);
            detected.Add(EInput_Type.PS4, false);

            // If there is no mouse, ie on a console!
            if (Input.mousePresent)
            {
                detected[EInput_Type.MouseAndKeyboard] = true;
            }
            else
            {
                detected[EInput_Type.MouseAndKeyboard] = false;
            }

            foreach (string gamepad in joysticks)
            {
                if (!string.IsNullOrEmpty(gamepad) && gamepad.Contains("Xbox One"))
                {
                    if (disconnected)
                    {
                        disconnected = false;
                        UnityEngine.Debug.Log("reconnected!" + gamepad);
                    }
                    detected[EInput_Type.XboxOne] = true;
                }
                else if (!string.IsNullOrEmpty(gamepad) && gamepad.Contains("PS4"))
                {
                    if (disconnected)
                    {
                        disconnected = false;
                        UnityEngine.Debug.Log("reconnected!" + gamepad);
                    }
                    detected[EInput_Type.PS4] = true;
                }
                else
                {
                    detected[EInput_Type.XboxOne] = false;
                    detected[EInput_Type.PS4] = false;
                }
            }

            foreach (KeyValuePair<EInput_Type, bool> detect in detected)
            {
                if (detect.Value)
                {
                    joystickType = detect.Key;
                }
            }

            if (PC_DEBUG)
            {
                UnityEngine.Debug.Log("overriding pc controls");
                joystickType = EInput_Type.MouseAndKeyboard;
            }
            else if (XBOX_DEBUG && detected[EInput_Type.XboxOne])
            {
                UnityEngine.Debug.Log("overriding Xbox One controls");
                joystickType = EInput_Type.XboxOne;
            }
            else if (XBOX_DEBUG && !detected[EInput_Type.XboxOne])
            {
                UnityEngine.Debug.Log("Error- no Xbox One controller detected");
                joystickType = EInput_Type.Error;
            }
            else if (PS4_DEBUG && detected[EInput_Type.PS4])
            {
                UnityEngine.Debug.Log("overriding PS4 controls");
                joystickType = EInput_Type.PS4;
            }
            else if (PS4_DEBUG && !detected[EInput_Type.PS4])
            {
                UnityEngine.Debug.Log("Error- no PS4 controller detected");
                joystickType = EInput_Type.Error;
            }
        }

        private void checkDebugFlags()
        {
            if(oldPC != PC_DEBUG)
            {
                UnityEngine.Debug.Log("Set PC Flag!");
                oldPC = PC_DEBUG;
                checkJoySticks();
            }
            if (oldXBOX != XBOX_DEBUG)
            {
                UnityEngine.Debug.Log("Set Xbox One Flag!");
                oldXBOX = XBOX_DEBUG;
                checkJoySticks();
            }
            if (oldPS4 != PS4_DEBUG)
            {
                UnityEngine.Debug.Log("Set PS4 Flag!");
                oldPS4 = PS4_DEBUG;
                checkJoySticks();
            }
        }

        public void Start()
        {
            checkJoySticks();
            oldPC = PC_DEBUG;
            oldXBOX = XBOX_DEBUG;
            oldPS4 = PS4_DEBUG;
        }

        public void Update()
        {
            consoleCounter += Time.deltaTime;

            checkDebugFlags();
            
            if (disconnected)
            {
                checkJoySticks();
            }

            string[] temp = Input.GetJoystickNames();
            //Check whether array contains anything
            if (temp.Length > 0)
            {
                //Iterate over every element
                for (int i = 0; i < temp.Length; ++i)
                {
                    //Check if the string is empty or not
                    if (!string.IsNullOrEmpty(temp[i]))
                    {
                        //Not empty, controller temp[i] is connected
                        // UnityEngine.Debug.Log("Controller " + i + " is connected using: " + temp[i]);
                    }
                    else
                    {
                        //If it is empty, controller i is disconnected
                        //where i indicates the controller number
                        UnityEngine.Debug.Log("Controller: " + i + " is disconnected.");
                        disconnected = true;
                        checkJoySticks();
                    }
                }
            }

            switch (joystickType)
            {
                case EInput_Type.MouseAndKeyboard:
                    if(consoleCounter >= consolePollRate)
                    {
                        consoleCounter = 0;
                        UnityEngine.Debug.Log("Recieving input from Mouse and Keyboard");
                    }
                    break;
                case EInput_Type.XboxOne:
                    if (consoleCounter >= consolePollRate)
                    {
                        consoleCounter = 0;
                        UnityEngine.Debug.Log("Recieving input from Xbox one");
                    }
                    break;
                case EInput_Type.PS4:
                    if (consoleCounter >= consolePollRate)
                    {
                        consoleCounter = 0;
                        UnityEngine.Debug.Log("Recieving input form PS4");
                    }
                    break;
                default:
                    checkJoySticks();
                    break;
            }

        }

        public bool GetBoost()
        {
            return Input.GetButton("Boost");
        }

        public bool GetFire1(Vector3 LookDir)
        {
            switch(joystickType)
            {
                case EInput_Type.MouseAndKeyboard:
                    if(Input.GetButton("Fire1"))
                    {
                        return true;
                    }
                    break;

                case EInput_Type.XboxOne:
                    if(LookDir.magnitude != 0)
                    {
                        return true;
                    }
                    break;

                // TODO
                case EInput_Type.PS4:
                    break;

                default:
                    return false;
            }
            return false;
        }

        public bool GetFire2()
        {
            switch (joystickType)
            {
                case EInput_Type.MouseAndKeyboard:
                    if (Input.GetButton("Fire2"))
                    {
                        return true;
                    }
                    break;

                case EInput_Type.XboxOne:
                    if (Input.GetAxis("XboxLT") != 0)
                    {
                        return true;
                    }
                    break;

                // TODO
                case EInput_Type.PS4:
                    break;

                default:
                    return false;
            }
            return false;
        }
    }
}
