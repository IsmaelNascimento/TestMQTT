using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Speech Recognizer Dialog Controller (Use dialog of Google)
    ///･Note: Callback from Android to Unity is received under 'GameObject.name'. That is, it is unique within the hierarchy. 
    /// </summary>
    public class SpeechRecognizerDialogController : MonoBehaviour
    {
        //Inspector Settings
        public string message = "";                     //Dialog message

        //Callbacks
        [Serializable] public class ResultHandler : UnityEvent<string[]> { }    //recognization words
        public ResultHandler OnResult;                  //Callback when recognization success

        [Serializable] public class ErrorHandler : UnityEvent<string> { }       //error state messate
        public ErrorHandler OnError;

#region Properties and Local values Section

        //Properties
        private static bool isSupportedRecognizer = false;  //Cached supported Speech Recognizer (Because Recognizer shares one, it is static).
        private static bool isSupportedChecked = false;     //Already checked (Because Recognizer shares one, it is static).

        //Whether the device supports Speech Recognizer.
        public bool IsSupportedRecognizer {
            get {
                if (!isSupportedChecked)
                {
#if UNITY_EDITOR
                    isSupportedRecognizer = true;       //For Editor (* You can rewrite it as you like.)
#elif UNITY_ANDROID
                    isSupportedRecognizer = AndroidPlugin.IsSupportedSpeechRecognizer();
#endif
                    isSupportedChecked = true;
                }
                return isSupportedRecognizer;
            }
        }

        //Whether necessary permissions are granted.
        public bool IsPermissionGranted {
            get {
#if UNITY_EDITOR
                return true;    //For Editor (* You can rewrite it as you like.)
#elif UNITY_ANDROID
                return AndroidPlugin.CheckPermission("android.permission.RECORD_AUDIO");
#endif
            }
        }

#endregion

        // Use this for initialization
        private void Start()
        {
            if (!IsSupportedRecognizer)
            {
                if (OnError != null)
                    OnError.Invoke("Not supported: Speech Recognizer");
            }
            if (!IsPermissionGranted)
            {
                if (OnError != null)
                    OnError.Invoke("Permission denied: RECORD_AUDIO");
            }
        }

        // Update is called once per frame
        //private void Update()
        //{

        //}


        //Show dialog
        public void Show()
        {
            if (!IsSupportedRecognizer || !IsPermissionGranted)
                return;
#if UNITY_EDITOR
            Debug.Log("ShowSpeechRecognizer called");
#elif UNITY_ANDROID
            AndroidPlugin.ShowSpeechRecognizer(
                gameObject.name, "ReceiveResult",
                message);
#endif
        }


        //Returns value when speech recognition succeeds.
        private void ReceiveResult(string result)
        {
            if (string.IsNullOrEmpty(result))
                return;

            if (OnResult != null)
                OnResult.Invoke(result.Split('\n'));
        }
    }
}
