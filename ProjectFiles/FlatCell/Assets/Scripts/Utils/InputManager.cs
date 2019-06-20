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

    public enum EDPad_Type
    {
        None = 0,
        Up,
        Right,
        Down,
        Left
    }

    public class DPad
    {
        public bool UpPressed = false;
        public bool DownPressed = false;
        public bool LeftPressed = false;
        public bool RightPressed = false;
    }

    public class InputManager : MonoBehaviour
    {
        [SerializeField] public bool SupressDebugMessage = true;
        [SerializeField] public bool InvertedStick = false;
        [SerializeField] public bool PC_DEBUG = false;
        [SerializeField] public bool XBOX_DEBUG = false;
        [SerializeField] public bool PS4_DEBUG = false;
        [SerializeField] public EInput_Type joystickType;
        private bool disconnected = false;
        DPad dpad = new DPad();

        private bool oldPC;
        private bool oldXBOX;
        private bool oldPS4;
        private bool DEBUG_DPAD = false;
        private bool DEBUG_GETDPAD = true;

        private float consolePollRate = 1f;
        private float consoleCounter = 0f;

        // https://pastebin.com/yJunNcEc
        private float screenH;
        private float screenW;

        private float oldLength = 0;

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
            string[] joysticks = Input.GetJoystickNames();
            if(joysticks.Length > oldLength)
            {
                UnityEngine.Debug.Log("Gamepad connected!");
                oldLength = joysticks.Length;
                checkJoySticks();
            }

            if (oldPC != PC_DEBUG)
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
            screenH = Screen.height / 2;
            screenW = Screen.width / 2;
            oldPC = PC_DEBUG;
            oldXBOX = XBOX_DEBUG;
            oldPS4 = PS4_DEBUG;
        }

        public void Update()
        {
            consoleCounter += Time.deltaTime;

            GetDPad();
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
                        if (!SupressDebugMessage)
                        {
                            UnityEngine.Debug.Log("Controller: " + i + " is disconnected.");
                        }
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
                        if (!SupressDebugMessage)
                        {
                            UnityEngine.Debug.Log("Recieving input from Mouse and Keyboard");
                        }
                    }
                    break;
                case EInput_Type.XboxOne:
                    if (consoleCounter >= consolePollRate)
                    {
                        consoleCounter = 0;
                        if (!SupressDebugMessage)
                        {
                            UnityEngine.Debug.Log("Recieving input from Xbox one");
                        }       
                    }
                    break;
                case EInput_Type.PS4:
                    if (consoleCounter >= consolePollRate)
                    {
                        consoleCounter = 0;
                        if (!SupressDebugMessage)
                        {
                            UnityEngine.Debug.Log("Recieving input form PS4");
                        }
                    }
                    break;
                default:
                    checkJoySticks();
                    break;
            }

        }

        public void GetSticks(ref Vector3 Movement, ref Vector3 Look)
        {
            // The left stick / WASD.
            Movement = new Vector3(Input.GetAxis("Horizontal"), 0f,
                                   Input.GetAxis("Vertical"));

            switch (joystickType)
            {
                case EInput_Type.MouseAndKeyboard:
                    // The mouse.
                    Look = new Vector3(Input.mousePosition.x - screenW, 0f,
                                       Input.mousePosition.y - screenH);
                    break;

                case EInput_Type.XboxOne:
                    Look = new Vector3(Input.GetAxis("RightStickX"), 0f,
                                       Input.GetAxis("RightStickY"));
                    if (!InvertedStick)
                    {
                        Look.z *= -1;
                    }
                    break;

                // TODO
                case EInput_Type.PS4:
                    break;

                default:
                    break;
            }
            Look.Normalize();
        }

        public EDPad_Type GetDPad()
        {
            switch(joystickType)
            {
                // TODO get arrow keys up or down
                case EInput_Type.MouseAndKeyboard:
                    // Up on dpad.
                    if (!dpad.UpPressed && Input.GetButton("MenuUp"))
                    {
                        if(DEBUG_GETDPAD)
                        {
                            UnityEngine.Debug.Log("Pressed D-Pad Up on Keyboard!");
                        }

                        dpad.UpPressed = true;
                    }
                    else if (!Input.GetButton("MenuUp"))
                    {
                        dpad.UpPressed = false;
                    }

                    
                    // Down on dpad.
                    if (!dpad.DownPressed && Input.GetButton("MenuDown"))
                    {
                        if (DEBUG_GETDPAD)
                        {
                            UnityEngine.Debug.Log("Pressed D-Pad Down on Keyboard!");
                        }
                       
                        dpad.DownPressed = true;
                    }
                    else if (!Input.GetButton("MenuDown"))
                    {
                        dpad.DownPressed = false;
                    }
                    
                    // TODO
                    // Menu Right & Menu Left

                    break;

                case EInput_Type.XboxOne:
                    // Up/Down on dpad.
                    if (!dpad.UpPressed && Input.GetAxis("XboxDUp") > 0)
                    {
                        if (DEBUG_GETDPAD)
                        {
                            UnityEngine.Debug.Log("Pressed D-Pad Up on Xbox One controller!");
                        }
                    
                        dpad.UpPressed = true;
                    }
                    else if (!dpad.DownPressed && Input.GetAxis("XboxDUp") < 0)
                    {
                        if (DEBUG_GETDPAD)
                        {
                            UnityEngine.Debug.Log("Pressed D-Pad Down on Xbox One controller!");
                        }
                      
                        dpad.DownPressed = true;
                    }
                    else if (Input.GetAxis("XboxDUp") == 0)
                    {
                        dpad.UpPressed = false;
                        dpad.DownPressed = false;
                    }

                    // Left/right on dpad
                    if (!dpad.RightPressed && Input.GetAxis("XboxDRight") > 0)
                    {
                        if (DEBUG_GETDPAD)
                        {
                            UnityEngine.Debug.Log("Pressed D-Pad Left on Xbox One controller!");
                        }
                                       
                        dpad.RightPressed = true;
                    }
                    else if (!dpad.LeftPressed && Input.GetAxis("XboxDRight") < 0)
                    {
                        if (DEBUG_GETDPAD)
                        {
                            UnityEngine.Debug.Log("Pressed D-Pad Left on Xbox One controller!");
                        }

                        dpad.LeftPressed = true;
                    }
                    else if(Input.GetAxis("XboxDRight") == 0)
                    {
                        dpad.RightPressed = false;
                        dpad.LeftPressed = false;
                    }
                    break;

                // TODO get PS4 DPad buttons
                case EInput_Type.PS4:
                    break;

                default:
                    break;
            }

            EDPad_Type ret = EDPad_Type.None;

            if (dpad.UpPressed)
            {
                ret |= EDPad_Type.Up;
            }
            if (dpad.RightPressed)
            {
                ret |= EDPad_Type.Right;
            }
            if (dpad.DownPressed)
            {
                ret |= EDPad_Type.Down;
            }
            if (dpad.LeftPressed)
            {
                ret |= EDPad_Type.Left;
            }

            if((ret & EDPad_Type.Up) == EDPad_Type.Up && DEBUG_DPAD)
            {
                UnityEngine.Debug.Log("Pressed D-Pad Up!");
            }
            if ((ret & EDPad_Type.Right) == EDPad_Type.Right && DEBUG_DPAD)
            {
                UnityEngine.Debug.Log("Pressed D-Pad Right!");
            }
            if ((ret & EDPad_Type.Down) == EDPad_Type.Down && DEBUG_DPAD)
            {
                UnityEngine.Debug.Log("Pressed D-Pad Down!");
            }
            if ((ret & EDPad_Type.Left) == EDPad_Type.Left && DEBUG_DPAD)
            {
                UnityEngine.Debug.Log("Pressed D-Pad Up!");
            }


            return ret;
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

        // TODO
        // 
    }
}
