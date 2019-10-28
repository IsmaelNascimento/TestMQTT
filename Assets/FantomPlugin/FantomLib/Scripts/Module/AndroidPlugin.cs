using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FantomLib
{
    /// <summary>
    /// Call the Android native plugin
    /// http://fantom1x.blog130.fc2.com/blog-entry-293.html
    /// http://fantom1x.blog130.fc2.com/blog-entry-273.html
    ///(*) "fantomPlugin.aar" is required 'Minimum API Level：Android 4.2 (API 17)' or higher.
    ///(*) When using text file reading / writing to storage, it is necessary to make it more than Android 4.4 (API 19).
    ///(*) In order to acquire the value of the sensor it is necessary to set it above the necessary API Level of each sensor.
    /// For details, refer to official document or comments such as sensor related method & constant values.
    /// https://developer.android.com/reference/android/hardware/Sensor.html#TYPE_ACCELEROMETER
    ///･When using Hardware Volume Control, Speech Recognize with dialog, Wifi Settings open, Bluetooth request enable,
    /// Text file read/write to External Storage, Gallery open, Media Scanner, Screen orientation change event, Sensor values, Confirm Device Credentials, QR Code Scanner,
    /// Rename "AndroidManifest-FullPlugin~.xml" to "AndroidManifest.xml".
    ///･Permission is necessary for AndroidManifest.xml (It is summarized in Permission_ReadMe.txt) depending on the function to use.
    /// https://developer.android.com/reference/android/Manifest.permission.html
    ///･Text to Speech is required the reading engine and voice data must be installed on the device.
    /// (Google Play)
    /// https://play.google.com/store/apps/details?id=com.google.android.tts
    /// https://play.google.com/store/apps/details?id=jp.kddilabs.n2tts  (Japanese)
    /// (Installation Text To Speech)
    /// http://fantom1x.blog130.fc2.com/blog-entry-275.html#fantomPlugin_TextToSpeech_install
    ///·If ZXing's QR Code Scanner application is not in the device, a dialog prompting installation will be displayed.
    /// (Google Play)
    /// https://play.google.com/store/apps/details?id=com.google.zxing.client.android
    /// (ZXing open source project)
    /// https://github.com/zxing/zxing
    /// ==========================================================
    ///･License of use library. etc
    /// This plugin includes deliverables distributed under the license of Apache License, Version 2.0.
    /// http://www.apache.org/licenses/LICENSE-2.0
    /// ZXing ("Zebra Crossing") open source project (google). [ver.3.3.2] (QR Code Scan)
    /// https://github.com/zxing/zxing
    /// ==========================================================
    /// </summary>
#if UNITY_ANDROID
    public static class AndroidPlugin
    {

        //Class full path of plug-in in Java
        public const string ANDROID_PACKAGE = "jp.fantom1x.plugin.android.fantomPlugin";
        public const string ANDROID_SYSTEM = ANDROID_PACKAGE + ".AndroidSystem";



        //==========================================================
        // etc functions

        /// <summary>
        /// Release of cashe etc.
        ///(*) It is better to call with "OnDestroy()" for each scene as much as possible.
        ///(*) When using "FullPluginOnUnityPlayerActivity", it is always invoked inside the plugin at the end of the application.
        /// </summary>
        public static void Release()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                androidSystem.CallStatic(
                    "release"
                );
            }

            HardKey.ReleaseCache();
        }



        /// <summary>
        /// Get API Level (Build.VERSION.SDK_INT) of the device
        /// https://developer.android.com/guide/topics/manifest/uses-sdk-element.html#ApiLevels
        /// https://developer.android.com/reference/android/os/Build.VERSION.html#SDK_INT
        ///
        /// デバイスの API Level (Build.VERSION.SDK_INT) を取得する
        /// </summary>
        /// <returns>API Level</returns>
        public static int GetAPILevel()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<int>(
                    "getBuildVersion"
                );
            }
        }



        /// <summary>
        /// Check if the application is installed
        ///･Application is specified by package name (= Returns whether the package name exists on the device).
        /// 
        /// アプリケーションがインストールされているを調べる
        ///・アプリケーションはパッケージ名で指定する（＝端末にパッケージ名が存在しているかを返す）。
        /// </summary>
        /// <param name="packageName">Package name (Application ID)</param>
        /// <returns>true = exist</returns>
        public static bool IsExistApplication(string packageName)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<bool>(
                            "isExistPackage",
                            context,
                            packageName
                        );
                    }
                }
            }
        }



        /// <summary>
        /// Get the label name (localized name) of the application
        /// 
        /// アプリケーション名（ローカライズされた名前）を取得する
        /// </summary>
        /// <param name="packageName">Package name (Application ID)</param>
        /// <returns>Application name</returns>
        public static string GetApplicationName(string packageName)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<string>(
                            "getApplicationName",
                            context,
                            packageName
                        );
                    }
                }
            }
        }



        /// <summary>
        /// Get version code of the application (Internal number that always increments [integer value])
        /// https://developer.android.com/studio/publish/versioning.html#appversioning
        /// 
        /// アプリのバージョン番号（常にインクリメントする内部番号[整数値]）を取得する
        /// https://developer.android.com/studio/publish/versioning.html#appversioning
        /// </summary>
        /// <param name="packageName">Package name (Application ID)</param>
        /// <returns>Version Code / failure = 0</returns>
        public static int GetVersionCode(string packageName)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<int>(
                            "getVersionCode",
                            context,
                            packageName
                        );
                    }
                }
            }
        }



        /// <summary>
        /// Get the version name of the application (Character string used as the version number to be displayed to the user)
        /// https://developer.android.com/studio/publish/versioning.html#appversioning
        /// 
        /// アプリのバージョン名（ユーザーに表示するバージョン番号として使用される文字列）を取得する
        /// https://developer.android.com/studio/publish/versioning.html#appversioning
        /// </summary>
        /// <param name="packageName">Package name (Application ID)</param>
        /// <returns>Version Name / failure = ""(empty)</returns>
        public static string GetVersionName(string packageName)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<string>(
                            "getVersionName",
                            context,
                            packageName
                        );
                    }
                }
            }
        }





        //==========================================================
        //·Permissions used in fantomPlugin are as follows:
        // プラグインで利用するパーミッションは以下の通り：
        // android.permission.RECORD_AUDIO
        // android.permission.WRITE_EXTERNAL_STORAGE (or android.permission.READ_EXTERNAL_STORAGE : When read only)
        // android.permission.BLUETOOTH
        // android.permission.VIBRATE
        // android.permission.BODY_SENSORS
        //==========================================================

        /// <summary>
        /// Returns whether permission is granted.
        /// https://developer.android.com/reference/android/Manifest.permission.html
        ///·Use "Constant Value" in the developer manual for the permission string (eg: "android.permission.RECORD_AUDIO").
        /// 
        /// パーミッションが許可（付与）されているかどうかを返す
        ///・パーミッションの文字列はデベロッパーマニュアルの「Constant Value」を使う（例："android.permission.RECORD_AUDIO"）。
        /// https://developer.android.com/reference/android/Manifest.permission.html
        /// </summary>
        /// <param name="permission">permission string (eg: "android.permission.RECORD_AUDIO")</param>
        /// <returns>true = granted / false = denied (nothing)</returns>
        public static bool CheckPermission(string permission)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<bool>(
                            "checkPermission",
                            context,
                            permission
                        );
                    }
                }
            }
        }




        //==========================================================
        // Anrdoid Widget and oher

        /// <summary>
        /// Call Android Toast
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_Toast
        /// (Toast)
        /// https://developer.android.com/reference/android/widget/Toast.html#LENGTH_LONG
        /// </summary>
        /// <param name="message">Message string</param>
        /// <param name="longDuration">Display length : true = Toast.LENGTH_LONG / false = Toast.LENGTH_SHORT</param>
        public static void ShowToast(string message, bool longDuration = false)
        {
            if (string.IsNullOrEmpty(message))
                return;

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showToast",
                    context,
                    message,
                    longDuration ? 1 : 0
                );
            }));
        }



        /// <summary>
        /// Cancel if there is "Android Toast" being displayed. Ignored when not displayed.
        ///･Even when "AndroidPlugin.Release()" is called, it is executed on the native side.
        /// </summary>
        public static void CancelToast()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                androidSystem.CallStatic(
                    "cancelToast"
                );
            }
        }



        //==========================================================
        // Android Dialogs
        // https://developer.android.com/guide/topics/ui/dialogs.html
        // (AlertDialog)
        // https://developer.android.com/reference/android/app/AlertDialog.html
        //==========================================================


        /// <summary>
        /// Call Android "Yes/No" Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_AlertDialogYN
        ///･"Yes" -> yesValue / "No" -> noValue is returned as the argument of the callback.
        ///･When neither is pressed (clicking outside the dialog -> back to application), 
        /// nothing is returned (not doing anything).
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="yesCaption">String of "Yes" button</param>
        /// <param name="yesValue">Return value when "Yes" button</param>
        /// <param name="noCaption">String of "No" button</param>
        /// <param name="noValue">Return value when "No" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowDialog(string title, string message, string callbackGameObject, string callbackMethod,
            string yesCaption, string yesValue, string noCaption, string noValue, string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showDialog",
                    context,
                    title,
                    message,
                    callbackGameObject,
                    callbackMethod,
                    yesCaption,
                    yesValue,
                    noCaption,
                    noValue,
                    style
                );
            }));
        }


        /// <summary>
        /// Call Android "OK" Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_AlertDialogOK
        ///･When pressed the "OK" button or clicked outside the dialog (-> back to application) 
        /// return the same value (resultValue).
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="resultValue">Return value when "OK" button or closed dialog</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowDialog(string title, string message, string callbackGameObject, string callbackMethod,
            string okCaption, string resultValue, string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showDialog",
                    context,
                    title,
                    message,
                    callbackGameObject,
                    callbackMethod,
                    okCaption,
                    resultValue,
                    style
                );
            }));
        }



        /// <summary>
        /// Call Android "Yes/No" Dialog with CheckBox
        /// http://fantom1x.blog130.fc2.com/blog-entry-279.html#fantomPlugin_AlertDialogYNwithCheck
        ///･The check status is returned as a callback argument with ", CHECKED_TRUE" or ", CHECKED_FALSE"
        /// concatenated with the return value (yesValue / noValue).
        ///･When neither is pressed (clicking outside the dialog -> back to application),
        /// nothing is returned (not doing anything).
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="checkBoxText">Text string of check box</param>
        /// <param name="checkBoxTextColor">Text color of check box (0 = not specified: Color format is int32 (AARRGGBB: Android-Java))</param>
        /// <param name="defaultChecked">Initial state of check box (true = On / false = Off)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="yesCaption">String of "Yes" button</param>
        /// <param name="yesValue">Return value when "Yes" button</param>
        /// <param name="noCaption">String of "No" button</param>
        /// <param name="noValue">Return value when "No" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowDialogWithCheckBox(string title, string message, 
            string checkBoxText, int checkBoxTextColor, bool defaultChecked,
            string callbackGameObject, string callbackMethod,
            string yesCaption, string yesValue, string noCaption, string noValue, string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showDialogWithCheckBox",
                    context,
                    title,
                    message,
                    checkBoxText,
                    checkBoxTextColor,
                    defaultChecked,
                    callbackGameObject,
                    callbackMethod,
                    yesCaption,
                    yesValue,
                    noCaption,
                    noValue,
                    style
                );
            }));
        }


        /// <summary>
        /// Call Android "Yes/No" Dialog with CheckBox (Unity.Color overload)
        /// http://fantom1x.blog130.fc2.com/blog-entry-279.html#fantomPlugin_AlertDialogYNwithCheck
        ///･The check status is returned as a callback argument with ", CHECKED_TRUE" or ", CHECKED_FALSE"
        /// concatenated with the return value (yesValue / noValue).
        ///･When neither is pressed (clicking outside the dialog -> back to application),
        /// nothing is returned (not doing anything).
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="checkBoxText">Text string of check box</param>
        /// <param name="checkBoxTextColor">Text color of check box (Color.clear = not specified: Not clear color)</param>
        /// <param name="defaultChecked">Initial state of check box (true = On / false = Off)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="yesCaption">String of "Yes" button</param>
        /// <param name="yesValue">Return value when "Yes" button</param>
        /// <param name="noCaption">String of "No" button</param>
        /// <param name="noValue">Return value when "No" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowDialogWithCheckBox(string title, string message,
            string checkBoxText, Color checkBoxTextColor, bool defaultChecked,
            string callbackGameObject, string callbackMethod,
            string yesCaption, string yesValue, string noCaption, string noValue, string style = "")
        {
            ShowDialogWithCheckBox(title, message, checkBoxText, checkBoxTextColor.ToIntARGB(), defaultChecked,
                callbackGameObject, callbackMethod, yesCaption, yesValue, noCaption, noValue, style);
        }



        /// <summary>
        /// Call Android "OK" Dialog with CheckBox
        /// http://fantom1x.blog130.fc2.com/blog-entry-279.html#fantomPlugin_AlertDialogOKwithCheck
        ///･The check status is returned as a callback argument with ", CHECKED_TRUE" or ", CHECKED_FALSE"
        /// concatenated with the return value (resultValue).
        ///･When pressed the "OK" button or clicked outside the dialog (-> back to application) 
        /// return the same value (resultValue).
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="checkBoxText">Text string of check box</param>
        /// <param name="checkBoxTextColor">Text color of check box (0 = not specified: Color format is int32 (AARRGGBB: Android-Java))</param>
        /// <param name="defaultChecked">Initial state of check box (true = On / false = Off)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="resultValue">Return value when "OK" button or the dialog is closed</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowDialogWithCheckBox(string title, string message, 
            string checkBoxText, int checkBoxTextColor, bool defaultChecked,
            string callbackGameObject, string callbackMethod,
            string okCaption, string resultValue, string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showDialogWithCheckBox",
                    context,
                    title,
                    message,
                    checkBoxText,
                    checkBoxTextColor,
                    defaultChecked,
                    callbackGameObject,
                    callbackMethod,
                    okCaption,
                    resultValue,
                    style
                );
            }));
        }


        /// <summary>
        /// Call Android "OK" Dialog with CheckBox (Unity.Color overload)
        /// http://fantom1x.blog130.fc2.com/blog-entry-279.html#fantomPlugin_AlertDialogOKwithCheck
        ///･The check status is returned as a callback argument with ", CHECKED_TRUE" or ", CHECKED_FALSE"
        /// concatenated with the return value (resultValue).
        ///･When pressed the "OK" button or clicked outside the dialog (-> back to application) 
        /// return the same value (resultValue).
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="checkBoxText">Text string of check box</param>
        /// <param name="checkBoxTextColor">Text color of check box (Color.clear = not specified: Not clear color)</param>
        /// <param name="defaultChecked">Initial state of check box (true = On / false = Off)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="resultValue">Return value when "OK" button or closed dialog</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowDialogWithCheckBox(string title, string message,
            string checkBoxText, Color checkBoxTextColor, bool defaultChecked,
            string callbackGameObject, string callbackMethod,
            string okCaption, string resultValue, string style = "")
        {
            ShowDialogWithCheckBox(title, message, checkBoxText, checkBoxTextColor.ToIntARGB(), defaultChecked,
                callbackGameObject, callbackMethod, okCaption, resultValue, style);
        }



        /// <summary>
        /// Call Android Selection Dialog (Return index or items string)
        /// http://fantom1x.blog130.fc2.com/blog-entry-274.html#fantomPlugin_SelectDialog
        ///･There is no confirmation button.
        ///･When "OK", the string of the choice (items) is returned as it is as the argument of the callback
        /// (or return index ([*]string type) with resultIsIndex=true).
        ///･When clicked outside the dialog (-> back to application) return nothing.
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="items">Choice strings (Array)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="resultIsIndex">Flag to set return value as index ([*]string type)</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSelectDialog(string title, string[] items, 
            string callbackGameObject, string callbackMethod, bool resultIsIndex = false, string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showSelectDialog",
                    context,
                    title,
                    items,
                    callbackGameObject,
                    callbackMethod,
                    resultIsIndex,
                    style
                );
            }));
        }



        /// <summary>
        /// Call Android Selection Dialog (Return result value for each item)
        /// http://fantom1x.blog130.fc2.com/blog-entry-274.html#fantomPlugin_SelectDialog
        ///･There is no confirmation button.
        ///･An element of the result array (resultValues) corresponding to the sequence of choices (items)
        /// is returned as the argument of the callback.
        ///･When clicked outside the dialog (-> back to application) return nothing.
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="items">Choice strings (Array)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="resultValues">The element of the selected index (items) becomes the return value</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSelectDialog(string title, string[] items, string callbackGameObject, string callbackMethod, string[] resultValues, string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showSelectDialog",
                    context,
                    title,
                    items,
                    callbackGameObject,
                    callbackMethod,
                    resultValues,
                    style
                );
            }));
        }




        /// <summary>
        /// Call Android Single Choice Dialog (Return index or items string)
        /// http://fantom1x.blog130.fc2.com/blog-entry-274.html#fantomPlugin_SingleChoiceDialog
        ///･When "OK", the string of the choice (items) is returned as it is as the argument of the callback
        /// (or return index ([*]string type) with resultIsIndex=true).
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="items">Choice strings (Array)</param>
        /// <param name="checkedItem">Initial value of index (0~n-1)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="changeCallbackMethod">Method name to callback of value chaged (it is in GameObject)</param>
        /// <param name="resultIsIndex">Flag to set return value as index ([*]string type)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSingleChoiceDialog(string title, string[] items, int checkedItem, 
            string callbackGameObject, string callbackMethod, string changeCallbackMethod, bool resultIsIndex, 
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showSingleChoiceDialog",
                    context,
                    title,
                    items,
                    checkedItem,
                    callbackGameObject,
                    callbackMethod,
                    changeCallbackMethod,
                    resultIsIndex,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }

        //(*) Argument difference overload
        /// <summary>
        /// Call Android Single Choice Dialog (Return index or items string)
        /// http://fantom1x.blog130.fc2.com/blog-entry-274.html#fantomPlugin_SingleChoiceDialog
        ///･When "OK", the string of the choice (items) is returned as it is as the argument of the callback
        /// (or return index ([*]string type) with resultIsIndex=true).
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="items">Choice strings (Array)</param>
        /// <param name="checkedItem">Initial value of index (0~n-1)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="resultIsIndex">Flag to set return value as index ([*]string type)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSingleChoiceDialog(string title, string[] items, int checkedItem, 
            string callbackGameObject, string callbackMethod, bool resultIsIndex = false, 
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowSingleChoiceDialog(title, items, checkedItem, callbackGameObject, callbackMethod, "", resultIsIndex, okCaption, cancelCaption, style);
        }


        /// <summary>
        /// Call Android Single Choice Dialog (Return result value for each item)
        /// http://fantom1x.blog130.fc2.com/blog-entry-274.html#fantomPlugin_SingleChoiceDialog
        ///･When "OK", the element of the result array (resultValues) corresponding to the sequence of choices (items)
        /// is returned as the argument of the callback.
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="items">Choice strings (Array)</param>
        /// <param name="checkedItem">Initial value of index (0~n-1)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="changeCallbackMethod">Method name to callback of value chaged (it is in GameObject)</param>
        /// <param name="resultValues">The element of the selected index (items) becomes the return value</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSingleChoiceDialog(string title, string[] items, int checkedItem,
            string callbackGameObject, string callbackMethod, string changeCallbackMethod, string[] resultValues,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showSingleChoiceDialog",
                    context,
                    title,
                    items,
                    checkedItem,
                    callbackGameObject,
                    callbackMethod,
                    changeCallbackMethod,
                    resultValues,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }

        //(*) Argument difference overload
        /// <summary>
        /// Call Android Single Choice Dialog (Return result value for each item)
        /// http://fantom1x.blog130.fc2.com/blog-entry-274.html#fantomPlugin_SingleChoiceDialog
        ///･When "OK", the element of the result array (resultValues) corresponding to the sequence of choices (items)
        /// is returned as the argument of the callback.
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="items">Choice strings (Array)</param>
        /// <param name="checkedItem">Initial value of index (0~n-1)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="resultValues">The element of the selected index (items) becomes the return value</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSingleChoiceDialog(string title, string[] items, int checkedItem,
            string callbackGameObject, string callbackMethod, string[] resultValues,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowSingleChoiceDialog(title, items, checkedItem, callbackGameObject, callbackMethod, "", resultValues, okCaption, cancelCaption, style);
        }



        /// <summary>
        /// Call Android Multi Choice Dialog (Return index or items string)
        /// http://fantom1x.blog130.fc2.com/blog-entry-274.html#fantomPlugin_MultiChoiceDialog
        ///･Return only those checked from multiple choices.
        ///･When "OK", the string of the choice is concatenated with a line feed ("\n") and returned as the argument of the callback
        /// (or return index ([*]string type) with resultIsIndex=true).
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="items">Choice strings (Array)</param>
        /// <param name="checkedItems">Initial state of checked (Array) (null = nothing)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="changeCallbackMethod">Method name to callback of value chaged (it is in GameObject)</param>
        /// <param name="resultIsIndex">Flag to set return value as index ([*]string type)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowMultiChoiceDialog(string title, string[] items, bool[] checkedItems,
            string callbackGameObject, string callbackMethod, string changeCallbackMethod, bool resultIsIndex,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showMultiChoiceDialog",
                    context,
                    title,
                    items,
                    checkedItems,
                    callbackGameObject,
                    callbackMethod,
                    changeCallbackMethod,
                    resultIsIndex,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }

        //(*) Argument difference overload
        /// <summary>
        /// Call Android Multi Choice Dialog (Return index or items string)
        /// http://fantom1x.blog130.fc2.com/blog-entry-274.html#fantomPlugin_MultiChoiceDialog
        ///･Return only those checked from multiple choices.
        ///･When "OK", the string of the choice is concatenated with a line feed ("\n") and returned as the argument of the callback
        /// (or return index ([*]string type) with resultIsIndex=true).
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="items">Choice strings (Array)</param>
        /// <param name="checkedItems">Initial state of checked (Array) (null = nothing)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="resultIsIndex">Flag to set return value as index ([*]string type)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowMultiChoiceDialog(string title, string[] items, bool[] checkedItems,
            string callbackGameObject, string callbackMethod, bool resultIsIndex = false,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowMultiChoiceDialog(title, items, checkedItems, callbackGameObject, callbackMethod, "", resultIsIndex, okCaption, cancelCaption, style);
        }


        /// <summary>
        /// Call Android Multi Choice Dialog (Return result value for each item)
        /// http://fantom1x.blog130.fc2.com/blog-entry-274.html#fantomPlugin_MultiChoiceDialog
        ///･Return only those checked from multiple choices.
        ///･When "OK", the elements of the result array (resultValues) corresponding to the sequence of choices (items)
        /// are concatenated with line feed ("\n") and returned as arguments of the callback.
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="items">Choice strings (Array)</param>
        /// <param name="checkedItems">Initial state of checked (Array) (null = nothing)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="changeCallbackMethod">Method name to callback of value chaged (it is in GameObject)</param>
        /// <param name="resultValues">The element of the selected index (items) becomes the return value</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowMultiChoiceDialog(string title, string[] items, bool[] checkedItems,
            string callbackGameObject, string callbackMethod, string changeCallbackMethod, string[] resultValues,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showMultiChoiceDialog",
                    context,
                    title,
                    items,
                    checkedItems,
                    callbackGameObject,
                    callbackMethod,
                    changeCallbackMethod,
                    resultValues,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }

        //(*) Argument difference overload
        /// <summary>
        /// Call Android Multi Choice Dialog (Return result value for each item)
        /// http://fantom1x.blog130.fc2.com/blog-entry-274.html#fantomPlugin_MultiChoiceDialog
        ///･Return only those checked from multiple choices.
        ///･When "OK", the elements of the result array (resultValues) corresponding to the sequence of choices (items)
        /// are concatenated with line feed ("\n") and returned as arguments of the callback.
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="items">Choice strings (Array)</param>
        /// <param name="checkedItems">Initial state of checked (Array) (null = nothing)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="resultValues">The element of the selected index (items) becomes the return value</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowMultiChoiceDialog(string title, string[] items, bool[] checkedItems,
            string callbackGameObject, string callbackMethod, string[] resultValues,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowMultiChoiceDialog(title, items, checkedItems, callbackGameObject, callbackMethod, "", resultValues, okCaption, cancelCaption, style);
        }



        /// <summary>
        /// Call Android Switch Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-280.html#fantomPlugin_SwitchDialog
        ///･Depending on the state of the switch, a dialog for acquiring the On/Off state of each item.
        ///･When "OK", the result (true/false) corresponding to the sequence of items is concatenated with a line feed ("\n")
        /// and returned as the argument of the callback.
        ///･When a key is set for each item, the state of the switch (true/false) is returned with '=' like "key=true", "key=false".
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        ///(*) When use the message string, Notice that it will not be displayed if the items overflow the dialog (-> message="").
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string ("" = nothing)</param>
        /// <param name="items">Item strings (Array)</param>
        /// <param name="itemKeys">Item keys (Array) (null = all nothing)</param>
        /// <param name="checkedItems">Initial state of the switches (Array) (null = all off)</param>
        /// <param name="itemsTextColor">Text color of items (0 = not specified: Color format is int32 (AARRGGBB: Android-Java))</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="changeCallbackMethod">Method name to callback of value chaged (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSwitchDialog(string title, string message, 
            string[] items, string[] itemKeys, bool[] checkedItems, int itemsTextColor,
            string callbackGameObject, string callbackMethod, string changeCallbackMethod,
            string okCaption, string cancelCaption, string style)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showSwitchDialog",
                    context,
                    title,
                    message,
                    items,
                    itemKeys,
                    checkedItems,
                    itemsTextColor,
                    callbackGameObject,
                    callbackMethod,
                    changeCallbackMethod,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }

        /// <summary>
        /// Call Android Switch Dialog (Unity.Color overload)
        /// http://fantom1x.blog130.fc2.com/blog-entry-280.html#fantomPlugin_SwitchDialog
        ///･Depending on the state of the switch, a dialog for acquiring the On/Off state of each item.
        ///･When "OK", the result (true/false) corresponding to the sequence of items is concatenated with a line feed ("\n")
        /// and returned as the argument of the callback.
        ///･When a key is set for each item, the state of the switch (true/false) is returned with '=' like "key=true", "key=false".
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        ///(*) When use the message string, Notice that it will not be displayed if the items overflow the dialog (-> message="").
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string ("" = nothing)</param>
        /// <param name="items">Item strings (Array)</param>
        /// <param name="itemKeys">Item keys (Array) (null = nothing)</param>
        /// <param name="checkedItems">Initial state of the switches (Array) (null = all off)</param>
        /// <param name="itemsTextColor">Text color of items (Color.clear = not specified: Not clear color)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="changeCallbackMethod">Method name to callback of value chaged (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSwitchDialog(string title, string message, 
            string[] items, string[] itemKeys, bool[] checkedItems, Color itemsTextColor,
            string callbackGameObject, string callbackMethod, string changeCallbackMethod,
            string okCaption, string cancelCaption, string style)
        {
            ShowSwitchDialog(title, message, items, itemKeys, checkedItems, itemsTextColor.ToIntARGB(), 
                callbackGameObject, callbackMethod, changeCallbackMethod, okCaption, cancelCaption, style);
        }

        //(*) Argument difference overload
        /// <summary>
        /// Call Android Switch Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-280.html#fantomPlugin_SwitchDialog
        ///･Depending on the state of the switch, a dialog for acquiring the On/Off state of each item.
        ///･When "OK", the result (true/false) corresponding to the sequence of items is concatenated with a line feed ("\n")
        /// and returned as the argument of the callback.
        ///･When a key is set for each item, the state of the switch (true/false) is returned with '=' like "key=true", "key=false".
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        ///(*) When use the message string, Notice that it will not be displayed if the items overflow the dialog (-> message="").
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string ("" = nothing)</param>
        /// <param name="items">Item strings (Array)</param>
        /// <param name="itemKeys">Item keys (Array) (null = all nothing)</param>
        /// <param name="checkedItems">Initial state of the switches (Array) (null = all off)</param>
        /// <param name="itemsTextColor">Text color of items (0 = not specified: Color format is int32 (AARRGGBB: Android-Java))</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSwitchDialog(string title, string message, 
            string[] items, string[] itemKeys, bool[] checkedItems, int itemsTextColor,
            string callbackGameObject, string callbackMethod, 
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowSwitchDialog(title, message, items, itemKeys, checkedItems, itemsTextColor, 
                callbackGameObject, callbackMethod, "", okCaption, cancelCaption, style);
        }

        //(*) Argument difference overload
        /// <summary>
        /// Call Android Switch Dialog (Unity.Color overload)
        /// http://fantom1x.blog130.fc2.com/blog-entry-280.html#fantomPlugin_SwitchDialog
        ///･Depending on the state of the switch, a dialog for acquiring the On/Off state of each item.
        ///･When "OK", the result (true/false) corresponding to the sequence of items is concatenated with a line feed ("\n")
        /// and returned as the argument of the callback.
        ///･When a key is set for each item, the state of the switch (true/false) is returned with '=' like "key=true", "key=false".
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        ///(*) When use the message string, Notice that it will not be displayed if the items overflow the dialog (-> message="").
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string ("" = nothing)</param>
        /// <param name="items">Item strings (Array)</param>
        /// <param name="itemKeys">Item keys (Array) (null = nothing)</param>
        /// <param name="checkedItems">Initial state of the switches (Array) (null = all off)</param>
        /// <param name="itemsTextColor">Text color of items (Color.clear = not specified: Not clear color)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSwitchDialog(string title, string message,
            string[] items, string[] itemKeys, bool[] checkedItems, Color itemsTextColor,
            string callbackGameObject, string callbackMethod,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowSwitchDialog(title, message, items, itemKeys, checkedItems, itemsTextColor.ToIntARGB(), 
                callbackGameObject, callbackMethod, "", okCaption, cancelCaption, style);
        }



        /// <summary>
        /// Call Android Slider Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-281.html#fantomPlugin_SliderDialog
        ///･When "OK", the result value corresponding to the sequence of items is concatenated with a line feed ("\n")
        /// and returned as the argument of the callback.
        ///･The result value follows the setting of the number of digits after the decimal point (digits) (it becomes an integer when 0).
        ///･When a key is set for each item, the value is returned with '=' like "key=3", "key=4.5".
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        ///(*) When use the message string, Notice that it will not be displayed if the items overflow the dialog (-> message="").
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string ("" = nothing)</param>
        /// <param name="items">Item strings (Array)</param>
        /// <param name="itemKeys">Item keys (Array) (null = all nothing)</param>
        /// <param name="defValues">Initial values (null = all 0 : 6 digits of integer part + 3 digits after decimal point)</param>
        /// <param name="minValues">Minimum values (null = all 0 : 6 digits of integer part + 3 digits after decimal point)</param>
        /// <param name="maxValues">Maximum values (null = all 100 : 6 digits of integer part + 3 digits after decimal point)</param>
        /// <param name="digits">Number of decimal places (0 = integer, 1~3 = after decimal point)</param>
        /// <param name="itemsTextColor">Text color of items (0 = not specified: Color format is int32 (AARRGGBB: Android-Java))</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="resultCallbackMethod">Method name to result callback when "OK" button pressed (it is in GameObject)</param>
        /// <param name="changeCallbackMethod">Method name to real-time callback when the value of the slider is changed (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSliderDialog(string title, string message, string[] items, string[] itemKeys, 
            float[] defValues, float[] minValues, float[] maxValues, int[] digits, int itemsTextColor,
            string callbackGameObject, string resultCallbackMethod, string changeCallbackMethod = "",
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showSeekBarDialog",
                    context,
                    title,
                    message,
                    items,
                    itemKeys,
                    defValues,
                    minValues,
                    maxValues,
                    digits,
                    itemsTextColor,
                    callbackGameObject,
                    resultCallbackMethod,
                    changeCallbackMethod,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }


        /// <summary>
        /// Call Android Slider Dialog (Unity.Color overload)
        /// http://fantom1x.blog130.fc2.com/blog-entry-281.html#fantomPlugin_SliderDialog
        ///･When "OK", the result value corresponding to the sequence of items is concatenated with a line feed ("\n")
        /// and returned as the argument of the callback.
        ///･The result value follows the setting of the number of digits after the decimal point (digits) (it becomes an integer when 0).
        ///･When a key is set for each item, the value is returned with '=' like "key=3", "key=4.5".
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        ///(*) When use the message string, Notice that it will not be displayed if the items overflow the dialog (-> message="").
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string ("" = nothing)</param>
        /// <param name="items">Item strings (Array)</param>
        /// <param name="itemKeys">Item keys (Array) (null = all nothing)</param>
        /// <param name="defValues">Initial values (null = all 0 : 6 digits of integer part + 3 digits after decimal point)</param>
        /// <param name="minValues">Minimum values (null = all 0 : 6 digits of integer part + 3 digits after decimal point)</param>
        /// <param name="maxValues">Maximum values (null = all 100 : 6 digits of integer part + 3 digits after decimal point)</param>
        /// <param name="digits">Number of decimal places (0 = integer, 1~3 = after decimal point)</param>
        /// <param name="itemsTextColor">Text color of items (Color.clear = not specified: Not clear color)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="resultCallbackMethod">Method name to result callback when "OK" button pressed (it is in GameObject)</param>
        /// <param name="changeCallbackMethod">Method name to real-time callback when the value of the slider is changed (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSliderDialog(string title, string message, string[] items, string[] itemKeys,
            float[] defValues, float[] minValues, float[] maxValues, int[] digits, Color itemsTextColor, 
            string callbackGameObject, string resultCallbackMethod, string changeCallbackMethod = "",
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowSliderDialog(title, message, items, itemKeys, 
                defValues, minValues, maxValues, digits, itemsTextColor.ToIntARGB(), 
                callbackGameObject, resultCallbackMethod, changeCallbackMethod, 
                okCaption, cancelCaption, style);
        }



        //==========================================================
        //Customizable Dialog

        public const string ANDROID_CUSTOM_DIALOG = ANDROID_PACKAGE + ".AndroidCustomDialog";

        /// <summary>
        /// Call Android Custom Dialog
        ///･Dialog where freely add Text, Switch, Slider, Toggle buttons and Dividing lines.
        ///･The return value is a pair of values ("key=value" + line feed ("\n")) or JSON format ("{"key":"value"}") for a key set for each item (resultIsJson=true:JSON).
        ///･The parameter (dialogItems) of each item (Widgets: DivisorItem, TextItem, SwitchItem, SliderItem, ToggleItem) in one array.
        ///･Generation of each widget is arranged in order from the top. Ignored if there are invalid parameters (or no dialog is generated)
        ///(*) When use the message string, Notice that it will not be displayed if the items overflow the dialog (-> message="").
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string ("" = nothing)</param>
        /// <param name="dialogItems">Parameters of each item (widget) (Array)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to result callback when "OK" button pressed (it is in GameObject)</param>
        /// <param name="resultIsJson">return value in: true=JSON format / false="key=value\n"</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowCustomDialog(string title, string message, DialogItem[] dialogItems,
            string callbackGameObject, string callbackMethod, bool resultIsJson = false,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            if (dialogItems == null || dialogItems.Length == 0)
                return;

            string[] jsons = dialogItems.Select(e => JsonUtility.ToJson(e)).ToArray();
            if (jsons == null || jsons.Length == 0)
                return;

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_CUSTOM_DIALOG);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showCustomDialog",
                    context,
                    title,
                    message,
                    jsons,
                    callbackGameObject,
                    callbackMethod,
                    resultIsJson,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }



        //==========================================================
        // Text Input Dialogs

        /// <summary>
        /// Call Android Single line text input Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-276.html#fantomPlugin_SingleLineTextDialog
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="text">Initial value of text string</param>
        /// <param name="maxLength">Character limit (0 = no limit)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSingleLineTextDialog(string title, string message, string text, int maxLength,
            string callbackGameObject, string callbackMethod,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showSingleLineTextDialog",
                    context,
                    title,
                    message,
                    text,
                    maxLength,
                    callbackGameObject,
                    callbackMethod,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }


        /// <summary>
        /// Call Android Single line text input Dialog (no message overload)
        /// http://fantom1x.blog130.fc2.com/blog-entry-276.html#fantomPlugin_SingleLineTextDialog
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="text">Initial value of text string</param>
        /// <param name="maxLength">Character limit (0 = no limit)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowSingleLineTextDialog(string title, string text, int maxLength,
            string callbackGameObject, string callbackMethod,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowSingleLineTextDialog(title, "", text, maxLength, callbackGameObject, callbackMethod, okCaption, cancelCaption, style);
        }



        /// <summary>
        /// Call Android Multi line text input Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-276.html#fantomPlugin_MultiLineTextDialog
        ///･Text entry to include line breaks. The line feed code of the return value is unified to "\n".
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="text">Initial value of text string</param>
        /// <param name="maxLength">Character limit (0 = no limit)</param>
        /// <param name="lines">Number of display lines</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowMultiLineTextDialog(string title, string message, string text, int maxLength, int lines,
            string callbackGameObject, string callbackMethod,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showMultiLineTextDialog",
                    context,
                    title,
                    message,
                    text,
                    maxLength,
                    lines,
                    callbackGameObject,
                    callbackMethod,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }


        /// <summary>
        /// Call Android Multi line text input Dialog (no message overload)
        /// http://fantom1x.blog130.fc2.com/blog-entry-276.html#fantomPlugin_MultiLineTextDialog
        ///･Text entry to include line breaks. The line feed code of the return value is unified to "\n".
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="text">Initial value of text string</param>
        /// <param name="maxLength">Character limit (0 = no limit)</param>
        /// <param name="lines">Number of display lines</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowMultiLineTextDialog(string title, string text, int maxLength, int lines,
            string callbackGameObject, string callbackMethod,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowMultiLineTextDialog(title, "", text, maxLength, lines, callbackGameObject, callbackMethod, okCaption, cancelCaption, style);
        }



        /// <summary>
        /// Call Android Numeric text input Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-277.html#fantomPlugin_NumericTextDialog
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="defValue">Initial value</param>
        /// <param name="maxLength">Character limit (0 = no limit) ([*]Including decimal point and sign)</param>
        /// <param name="enableDecimal">true=decimal possible / false=integer only</param>
        /// <param name="enableSign">Possible to input a sign ('-' or '+') at the beginning</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowNumericTextDialog(string title, string message, float defValue, int maxLength, bool enableDecimal, bool enableSign,
            string callbackGameObject, string callbackMethod,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showNumericTextDialog",
                    context,
                    title,
                    message,
                    defValue,
                    maxLength,
                    enableDecimal,
                    enableSign,
                    callbackGameObject,
                    callbackMethod,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }


        /// <summary>
        /// Call Android Numeric text input Dialog (no message overload)
        /// http://fantom1x.blog130.fc2.com/blog-entry-277.html#fantomPlugin_NumericTextDialog
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="defValue">Initial value</param>
        /// <param name="maxLength">Character limit (0 = no limit) ([*]Including decimal point and sign)</param>
        /// <param name="enableDecimal">true=decimal possible / false=integer only</param>
        /// <param name="enableSign">Possible to input a sign ('-' or '+') at the beginning</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowNumericTextDialog(string title, float defValue, int maxLength, bool enableDecimal, bool enableSign,
            string callbackGameObject, string callbackMethod,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowNumericTextDialog(title, "", defValue, maxLength, enableDecimal, enableSign, callbackGameObject, callbackMethod, okCaption, cancelCaption, style);
        }



        /// <summary>
        /// Call Android Alpha Numeric text input Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-277.html#fantomPlugin_AlphaNumericTextDialog
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="text">Initial value of text string</param>
        /// <param name="maxLength">Character limit (0 = no limit)。</param>
        /// <param name="addChars">Additional character lists such as symbols ("_-.@": each character, "" = nothing)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowAlphaNumericTextDialog(string title, string message, string text, int maxLength, string addChars,
            string callbackGameObject, string callbackMethod,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showAlphaNumericTextDialog",
                    context,
                    title,
                    message,
                    text,
                    maxLength,
                    addChars,
                    callbackGameObject,
                    callbackMethod,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }


        /// <summary>
        /// Call Android Alpha Numeric text input Dialog (no message overload)
        /// http://fantom1x.blog130.fc2.com/blog-entry-277.html#fantomPlugin_AlphaNumericTextDialog
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="text">Initial value of text string</param>
        /// <param name="maxLength">Character limit (0 = no limit)。</param>
        /// <param name="addChars">Additional character lists such as symbols ("_-.@": each character, "" = nothing)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowAlphaNumericTextDialog(string title, string text, int maxLength, string addChars,
            string callbackGameObject, string callbackMethod,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowAlphaNumericTextDialog(title, "", text, maxLength, addChars, callbackGameObject, callbackMethod, okCaption, cancelCaption, style);
        }



        /// <summary>
        /// Call Android Password text input Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-277.html#fantomPlugin_PasswordTextDialog
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="text">Initial value of text string</param>
        /// <param name="maxLength">Character limit (0 = no limit)。</param>
        /// <param name="numberOnly">true=numeric only</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowPasswordTextDialog(string title, string message, string text, int maxLength, bool numberOnly,
            string callbackGameObject, string callbackMethod,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showPasswordTextDialog",
                    context,
                    title,
                    message,
                    text,
                    maxLength,
                    numberOnly,
                    callbackGameObject,
                    callbackMethod,
                    okCaption,
                    cancelCaption,
                    style
                );
            }));
        }


        /// <summary>
        /// Call Android Password text input Dialog (no message overload)
        /// http://fantom1x.blog130.fc2.com/blog-entry-277.html#fantomPlugin_PasswordTextDialog
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="text">Initial value of text string</param>
        /// <param name="maxLength">Character limit (0 = no limit)。</param>
        /// <param name="numberOnly">true=numeric only</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="okCaption">String of "OK" button</param>
        /// <param name="cancelCaption">String of "Cancel" button</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowPasswordTextDialog(string title, string text, int maxLength, bool numberOnly,
            string callbackGameObject, string callbackMethod,
            string okCaption = "OK", string cancelCaption = "Cancel", string style = "")
        {
            ShowPasswordTextDialog(title, "", text, maxLength, numberOnly, callbackGameObject, callbackMethod, okCaption, cancelCaption, style);
        }



        //==========================================================
        // Picker Dialogs

        /// <summary>
        /// Call Android DatePicker Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-278.html#fantomPlugin_DataPickerDialog
        /// When pressed the "OK" button, the date is returned as a callback with the specified format (resultDateFormat).
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        /// (Date format [Android-Java])
        /// https://developer.android.com/reference/java/text/SimpleDateFormat.html
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="defaultDate">Initial value of date string (like "2017/01/31", "17/1/1")</param>
        /// <param name="resultDateFormat">Return date format (default: "yyyy/MM/dd"->"2017/01/03", ex: "yy-M-d"->"17-1-3" [Android-Java])</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowDatePickerDialog(string defaultDate, string resultDateFormat, 
            string callbackGameObject, string callbackMethod, string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showDatePickerDialog",
                    context,
                    defaultDate,
                    resultDateFormat,
                    callbackGameObject,
                    callbackMethod,
                    style
                );
            }));
        }


        /// <summary>
        /// Call Android TimePicker Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-278.html#fantomPlugin_TimePickerDialog
        /// When pressed the "OK" button, the time is returned as a callback with the specified format (resultTimeFormat).
        ///･When pressed the "Cancel" button or clicked outside the dialog (-> back to application) return nothing.
        /// (Time format [Android-Java])
        /// https://developer.android.com/reference/java/text/SimpleDateFormat.html
        /// (Theme)
        /// https://developer.android.com/reference/android/R.style.html#Theme
        /// </summary>
        /// <param name="defaultTime">Initial value of time string (like "0:00"~"23:59")</param>
        /// <param name="resultTimeFormat">Return time format (default: "HH:mm"->"03:05", ex: "H:mm"->"3:05" [Android-Java])</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
        /// <param name="style">Style applied to dialog (Theme: "android:Theme.DeviceDefault.Dialog.Alert", "android:Theme.DeviceDefault.Light.Dialog.Alert" or etc)</param>
        public static void ShowTimePickerDialog(string defaultTime, string resultTimeFormat,
            string callbackGameObject, string callbackMethod, string style = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showTimePickerDialog",
                    context,
                    defaultTime,
                    resultTimeFormat,
                    callbackGameObject,
                    callbackMethod,
                    style
                );
            }));
        }




        //==========================================================
        // Notification

        /// <summary>
        /// Call Android Notification
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_Notification
        ///(*) Icon in Unity is fixed with resource name "app_icon".
        ///･Put the duration in the order of the vibrator pattern array (off, on, off, on, ...) (unit: ms [millisecond = 1/1000 seconds])) / null = none
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="iconName">Icon resource name (Unity's default is "app_icon")</param>
        /// <param name="tag">Identification tag (The same tag is overwritten when notified consecutively)</param>
        /// <param name="showTimestamp">Add notification time display</param>
        /// <param name="vibratorPattern">Array of duration pattern / null = none</param>
        public static void ShowNotification(string title, string message, string iconName, string tag, 
            bool showTimestamp, long[] vibratorPattern)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showNotification",
                    context,
                    title,
                    message,
                    iconName,
                    tag,
                    showTimestamp,
                    vibratorPattern
                );
            }));
        }

        //(*) OneShot (one time only) vibrator overload
        /// <summary>
        /// Call Android Notification
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_Notification
        ///(*) Icon in Unity is fixed with resource name "app_icon".
        ///･Put the duration in the order of the vibrator pattern array (off, on, off, on, ...) (unit: ms [millisecond = 1/1000 seconds])) / null = none
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="iconName">Icon resource name (Unity's default is "app_icon")</param>
        /// <param name="tag">Identification tag (The same tag is overwritten when notified consecutively)</param>
        /// <param name="showTimestamp">Add notification time display</param>
        /// <param name="vibratorDuration">Duration of vibration</param>
        public static void ShowNotification(string title, string message, string iconName, string tag, 
            bool showTimestamp, long vibratorDuration)
        {
            long[] pattern = vibratorDuration > 0 ? new long[] { 0, vibratorDuration } : null;
            ShowNotification(title, message, iconName, tag, showTimestamp, pattern);
        }

        //(*) No vibrator overload
        /// <summary>
        /// Call Android Notification
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_Notification
        ///(*) Icon in Unity is fixed with resource name "app_icon".
        ///･Put the duration in the order of the vibrator pattern array (off, on, off, on, ...) (unit: ms [millisecond = 1/1000 seconds])) / null = none
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="iconName">Icon resource name (Unity's default is "app_icon")</param>
        /// <param name="tag">Identification tag (The same tag is overwritten when notified consecutively)</param>
        /// <param name="showTimestamp">Add notification time display</param>
        public static void ShowNotification(string title, string message, string iconName = "app_icon", string tag = "tag", 
            bool showTimestamp = true)
        {
            ShowNotification(title, message, iconName, tag, showTimestamp, null);
        }



        /// <summary>
        /// Call Android Notification (Tap to take action to URI)
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_NotificationToOpenURL
        ///･(Action: Constant Value)
        /// https://developer.android.com/reference/android/content/Intent.html#ACTION_VIEW
        ///(*) Icon in Unity is fixed with resource name "app_icon".
        ///･Put the duration in the order of the vibrator pattern array (off, on, off, on, ...) (ms [millisecond = 1/1000 seconds]) / null = none
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="iconName">Icon resource name (Unity's default is "app_icon")</param>
        /// <param name="tag">Identification tag (The same tag is overwritten when notified consecutively)</param>
        /// <param name="action">String of Action (ex: "android.intent.action.VIEW")</param>
        /// <param name="uri">URI to action (URL etc.)</param>
        /// <param name="showTimestamp">Add notification time display</param>
        /// <param name="vibratorPattern">Array of duration pattern / null = none</param>
        public static void ShowNotificationToActionURI(string title, string message, string iconName, string tag,
            string action, string uri, bool showTimestamp, long[] vibratorPattern)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showNotificationToActionURI",
                    context,
                    title,
                    message,
                    iconName,
                    tag,
                    action,
                    uri,
                    showTimestamp,
                    vibratorPattern
                );
            }));
        }

        //(*) OneShot (one time only) vibrator overload
        /// <summary>
        /// Call Android Notification (Tap to take action to URI)
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_NotificationToOpenURL
        ///･(Action: Constant Value)
        /// https://developer.android.com/reference/android/content/Intent.html#ACTION_VIEW
        ///(*) Icon in Unity is fixed with resource name "app_icon".
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="iconName">Icon resource name (Unity's default is "app_icon")</param>
        /// <param name="tag">Identification tag (The same tag is overwritten when notified consecutively)</param>
        /// <param name="action">String of Action (ex: "android.intent.action.VIEW")</param>
        /// <param name="uri">URI to action (URL etc.)</param>
        /// <param name="showTimestamp">Add notification time display</param>
        /// <param name="vibratorDuration">Duration of vibration (ms [millisecond = 1/1000 seconds])</param>
        public static void ShowNotificationToActionURI(string title, string message, string iconName, string tag,
            string action, string uri, bool showTimestamp, long vibratorDuration)
        {
            long[] pattern = vibratorDuration > 0 ? new long[] { 0, vibratorDuration } : null;
            ShowNotificationToActionURI(title, message, iconName, tag, action, uri, showTimestamp, pattern);
        }

        //(*) No vibrator overload
        /// <summary>
        /// Call Android Notification (Tap to take action to URI)
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_NotificationToOpenURL
        ///･(Action: Constant Value)
        /// https://developer.android.com/reference/android/content/Intent.html#ACTION_VIEW
        ///(*) Icon in Unity is fixed with resource name "app_icon".
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="iconName">Icon resource name (Unity's default is "app_icon")</param>
        /// <param name="tag">Identification tag (The same tag is overwritten when notified consecutively)</param>
        /// <param name="action">String of Action (ex: "android.intent.action.VIEW")</param>
        /// <param name="uri">URI to action (URL etc.)</param>
        /// <param name="showTimestamp">Add notification time display</param>
        public static void ShowNotificationToActionURI(string title, string message, string iconName = "app_icon", string tag = "tag",
            string action = "android.intent.action.VIEW", string uri = "", bool showTimestamp = true)
        {
            ShowNotificationToActionURI(title, message, iconName, tag, action, uri, showTimestamp, null);
        }



        /// <summary>
        /// Call Android Notification (Tap to take action to open URL)
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_NotificationToOpenURL
        ///･Put the duration in the order of the vibrator pattern array (off, on, off, on, ...) (unit: ms [millisecond = 1/1000 seconds])) / null = none
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="url">URL to open in browser</param>
        /// <param name="tag">Identification tag (The same tag is overwritten when notified consecutively)</param>
        /// <param name="showTimestamp">Add notification time display</param>
        /// <param name="vibratorPattern">Array of duration pattern / null = none</param>
        public static void ShowNotificationToOpenURL(string title, string message, string url, string tag, 
            bool showTimestamp, long[] vibratorPattern)
        {
            if (string.IsNullOrEmpty(url))
                return;

            ShowNotificationToActionURI(title, message, "app_icon", tag, "android.intent.action.VIEW", url, 
                showTimestamp, vibratorPattern);
        }

        //(*) OneShot (one time only) vibrator overload
        /// <summary>
        /// Call Android Notification (Tap to take action to open URL)
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_NotificationToOpenURL
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="url">URL to open in browser</param>
        /// <param name="tag">Identification tag (The same tag is overwritten when notified consecutively)</param>
        /// <param name="showTimestamp">Add notification time display</param>
        /// <param name="vibratorDuration">Duration of vibration</param>
        public static void ShowNotificationToOpenURL(string title, string message, string url, string tag, 
            bool showTimestamp, long vibratorDuration)
        {
            if (string.IsNullOrEmpty(url))
                return;

            long[] pattern = vibratorDuration > 0 ? new long[] { 0, vibratorDuration } : null;
            ShowNotificationToActionURI(title, message, "app_icon", tag, "android.intent.action.VIEW", url, 
                showTimestamp, pattern);
        }

        //(*) No vibrator overload
        /// <summary>
        /// Call Android Notification (Tap to take action to open URL)
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_NotificationToOpenURL
        /// </summary>
        /// <param name="title">Title string</param>
        /// <param name="message">Message string</param>
        /// <param name="url">URL to open in browser</param>
        /// <param name="tag">Identification tag (The same tag is overwritten when notified consecutively)</param>
        /// <param name="showTimestamp">Add notification time display</param>
        public static void ShowNotificationToOpenURL(string title, string message, string url, string tag = "tag",
            bool showTimestamp = true)
        {
            ShowNotificationToActionURI(title, message, "app_icon", tag, "android.intent.action.VIEW", url, 
                showTimestamp, null);
        }




        //==========================================================
        // Vibrator
        //(*) In API 26 (Android 8.0), this function is deprecated, so some devices may not be available.
        //(*) The following permission is necessary to use.
        // '<uses-permission android:name="android.permission.VIBRATE" />' in 'AndroidManifest.xml'
        //==========================================================

        //(*) Required: '<uses-permission android:name="android.permission.VIBRATE" />' in 'AndroidManifest.xml'
        /// <summary>
        /// Whether the devices supports a vibrator?
        ///(*) It has nothing to do with permissions.
        /// </summary>
        /// <returns>true = supported</returns>
        public static bool IsSupportedVibrator()
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<bool>(
                            "hasVibrator",
                            context
                        );
                    }
                }
            }
        }


        //(*) Required: '<uses-permission android:name="android.permission.VIBRATE" />' in 'AndroidManifest.xml'
        /// <summary>
        /// Vibrate the vibrator with a pattern.
        ///･Put the duration in the order of the vibrator pattern array (off, on, off, on, ...) (unit: ms [millisecond = 1/1000 seconds])) / null = none
        /// </summary>
        /// <param name="pattern">Array of duration pattern / null = none</param>
        /// <param name="isLoop">true = do loop</param>
        public static void StartVibrator(long[] pattern, bool isLoop = false)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        androidSystem.CallStatic(
                            "startVibrator",
                            context,
                            pattern,
                            isLoop
                        );
                    }
                }
            }
        }


        //(*) Required: '<uses-permission android:name="android.permission.VIBRATE" />' in 'AndroidManifest.xml'
        /// <summary>
        /// Vibrate the vibrator only once.
        /// </summary>
        /// <param name="duration">Duration of vibration (ms [millisecond = 1/1000 seconds])</param>
        public static void StartVibrator(long duration)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        androidSystem.CallStatic(
                            "startVibrator",
                            context,
                            duration
                        );
                    }
                }
            }
        }


        //(*) Required: '<uses-permission android:name="android.permission.VIBRATE" />' in 'AndroidManifest.xml'
        /// <summary>
        /// Interrupt vibration of the vibrator
        /// </summary>
        public static void CancelVibrator()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                androidSystem.CallStatic(
                    "cancelVibrator"
                );
            }
        }





        //==========================================================
        // Start something action

        //(*) No argument overload
        /// <summary>
        /// Start something Action
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_OpenURL
        ///･(Action: Constant Value)
        /// https://developer.android.com/reference/android/content/Intent.html#ACTION_VIEW
        /// https://developer.android.com/reference/android/provider/Settings.html#ACTION_ACCESSIBILITY_SETTINGS
        /// </summary>
        /// <param name="action">Starting of Action (ex: "android.settings.SETTINGS")</param>
        public static void StartAction(string action)
        {
            StartAction(action, "", "");
        }


        /// <summary>
        /// Start something Action
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_OpenURL
        ///･(Action: Constant Value)
        /// https://developer.android.com/reference/android/content/Intent.html#ACTION_VIEW
        /// https://developer.android.com/reference/android/provider/Settings.html#ACTION_ACCESSIBILITY_SETTINGS
        /// </summary>
        /// <param name="action">Starting of Action (ex: "android.intent.action.WEB_SEARCH")</param>
        /// <param name="extra">Parameter name to give to the Action (ex: "query")</param>
        /// <param name="query">Value to give to the Action</param>
        public static void StartAction(string action, string extra, string query)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startAction",
                    context,
                    action,
                    extra,
                    query
                );
            }));
        }


        /// <summary>
        /// Start Web Search
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_OpenURL
        ///･The search application differs depending on the setting of the system.
        ///･Same as StartAction("android.intent.action.WEB_SEARCH", "query", "keyword")
        /// </summary>
        /// <param name="query">Search keyword</param>
        public static void StartWebSearch(string query)
        {
            StartAction("android.intent.action.WEB_SEARCH", "query", query);
        }


        //(*) multiple parameter overload
        //※複数パラメタオーバーロード
        /// <summary>
        /// Start something Action
        ///･(Action: Constant Value)
        /// https://developer.android.com/reference/android/content/Intent.html#ACTION_VIEW
        /// https://developer.android.com/reference/android/provider/Settings.html#ACTION_ACCESSIBILITY_SETTINGS
        /// 
        /// 
        /// アクティビティからのアクション起動（戻値はなし）
        ///・アクションは以下を参照（Constant Value を使う）
        /// https://developer.android.com/reference/android/content/Intent.html#ACTION_VIEW
        /// https://developer.android.com/reference/android/provider/Settings.html#ACTION_ACCESSIBILITY_SETTINGS
        /// </summary>
        /// <param name="action">Starting of Action (ex: "android.intent.action.WEB_SEARCH")</param>
        /// <param name="extra">Parameter name to give to the Action (ex: "query")</param>
        /// <param name="query">Value to give to the Action</param>
        public static void StartAction(string action, string[] extra, string[] query)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startAction",
                    context,
                    action,
                    extra,
                    query
                );
            }));
        }



        /// <summary>
        /// Start action with Chooser (application selection widget)
        /// </summary>
        /// <param name="action">String of Action (ex: "android.intent.action.SEND")</param>
        /// <param name="extra">Parameter name to give to the Action (ex: "android.intent.extra.TEXT")</param>
        /// <param name="query">Value to give to the Action</param>
        /// <param name="mimetype">MIME Type (ex: "text/plain")</param>
        /// <param name="title">Title to display in Chooser (Empty -> "Select a application")</param>
        public static void StartActionWithChooser(string action, string extra, string query, string mimetype, string title)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startActionWithChooser",
                    context,
                    action,
                    extra,
                    query,
                    mimetype,
                    title
                );
            }));
        }


        /// <summary>
        /// Use Chooser (application selection widget) to send text to other applications.
        ///·Use it for sharing text (Twitter etc.).
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="title">Title to display in Chooser (Empty -> "Select a application")</param>
        public static void StartActionSendText(string text, string title)
        {
            StartActionWithChooser("android.intent.action.SEND", "android.intent.extra.TEXT", text, "text/plain", title);
        }


        //(*) multiple parameter overload
        //※複数パラメタオーバーロード
        /// <summary>
        /// Start action with Chooser (application selection widget)
        /// 
        /// Chooser（アプリ選択ウィジェット）でアクション起動する
        /// </summary>
        /// <param name="action">String of Action (ex: "android.intent.action.SEND")</param>
        /// <param name="extra">Parameter name to give to the Action (ex: "android.intent.extra.TEXT")</param>
        /// <param name="query">Value to give to the Action</param>
        /// <param name="mimetype">MIME Type (ex: "text/plain")</param>
        /// <param name="title">Title to display in Chooser (Empty -> "Select a application")</param>
        public static void StartActionWithChooser(string action, string[] extra, string[] query, string mimetype, string title)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startActionWithChooser",
                    context,
                    action,
                    extra,
                    query,
                    mimetype,
                    title
                );
            }));
        }




        /// <summary>
        /// Start Action to URI
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_OpenURL
        ///･(Action: Constant Value)
        /// https://developer.android.com/reference/android/content/Intent.html#ACTION_VIEW
        /// (ex)
        /// StartActionURI("android.intent.action.VIEW", "geo:37.7749,-122.4194?q=restaurants");   //Google Map (Search word: restaurants)
        /// StartActionURI("android.intent.action.VIEW", "google.streetview:cbll=29.9774614,31.1329645&cbp=0,30,0,0,-15");   //Street View
        /// StartActionURI("android.intent.action.SENDTO", "mailto:xxx@example.com");   //Launch mailer
        /// https://developers.google.com/maps/documentation/android-api/intents
        /// </summary>
        /// <param name="action">String of Action (ex: "android.intent.action.VIEW")</param>
        /// <param name="uri">URI to action (URL etc.)</param>
        public static void StartActionURI(string action, string uri)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startActionURI",
                    context,
                    action,
                    uri
                );
            }));
        }


        /// <summary>
        /// Open URL (Launch Browser)
        ///･The browser application differs depending on the setting of the system.
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_OpenURL
        ///･Same as StartActionURI("android.intent.action.VIEW", "URL")
        /// </summary>
        /// <param name="url">URL to open in browser</param>
        public static void StartOpenURL(string url)
        {
            StartActionURI("android.intent.action.VIEW", url);
        }


        /// <summary>
        /// Display specified applications on Google Play
        /// (*) Google Play must be installed.
        /// 
        /// Google Play で指定アプリケーションを表示する
        /// ※Google Play がインストールされている必要がある。
        /// </summary>
        /// <param name="packageName">Package name (Application ID)</param>
        public static void ShowMarketDetails(string packageName)
        {
            string uri = "market://details?id=" + packageName;
            StartActionURI("android.intent.action.VIEW", uri);
        }


        /// <summary>
        /// Search keywords on Google Play
        /// (*) Google Play must be installed.
        /// 
        /// Google Play でキーワード検索をする
        /// ※Google Play がインストールされている必要がある。
        /// </summary>
        /// <param name="keyword">Search keywords</param>
        public static void StartMarketSearch(string keyword)
        {
            string uri = "market://search?q=" + keyword;
            StartActionURI("android.intent.action.VIEW", uri);
        }


        //(*) multiple parameter overload
        //※複数パラメタオーバーロード
        /// <summary>
        /// Start Action to URI
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_OpenURL
        ///･(Action: Constant Value)
        /// https://developer.android.com/reference/android/content/Intent.html#ACTION_VIEW
        /// 
        /// 
        /// アクティビティからのURIへのアクション起動（戻値はなし）
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_OpenURL
        ///・アクションは以下を参照（Constant Value を使う）
        /// https://developer.android.com/reference/android/content/Intent.html#ACTION_VIEW
        /// </summary>
        /// <param name="action">String of Action (ex: "android.intent.action.VIEW")</param>
        /// <param name="uri">URI to action (URL etc.)</param>
        /// <param name="extra">Parameter name to give to the Action (ex: "android.intent.extra.TEXT")</param>
        /// <param name="query">Value to give to the Action</param>
        public static void StartActionURI(string action, string uri, string[] extra, string[] query)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startActionURI",
                    context,
                    action,
                    uri,
                    extra,
                    query
                );
            }));
        }





        //==========================================================
        // Media Scanner

        /// <summary>
        /// Scan (recognize) files with MediaScanner (single file)
        ///·Scan completed path name is returned in the callback.
        /// </summary>
        /// <param name="path">Scan target path (absolute path)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="completeCallbackMethod">Method name to call back completion</param>
        public static void StartMediaScanner(string path, string callbackGameObject, string completeCallbackMethod)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startActionMediaScanner",
                    context,
                    path,
                    callbackGameObject,
                    completeCallbackMethod
                );
            }));
        }


        //(*) No callback overload
        /// <summary>
        /// Scan (recognize) files with MediaScanner (single file)
        /// </summary>
        /// <param name="path">Scan target path (absolute path)</param>
        public static void StartMediaScanner(string path)
        {
            StartMediaScanner(path, "", "");
        }


        /// <summary>
        /// Scan (recognize) files with MediaScanner (multiple file)
        ///·Completion callback is sent each path.
        /// </summary>
        /// <param name="paths">Array of scan target path (absolute path)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="completeCallbackMethod">Method name to call back completion</param>
        public static void StartMediaScanner(string[] paths, string callbackGameObject, string completeCallbackMethod)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startActionMediaScanner",
                    context,
                    paths,
                    callbackGameObject,
                    completeCallbackMethod
                );
            }));
        }


        //(*) No callback overload
        /// <summary>
        /// Scan (recognize) files with MediaScanner (multiple file)
        /// </summary>
        /// <param name="paths">Array of scan target path (absolute path)</param>
        public static void StartMediaScanner(string[] paths)
        {
            StartMediaScanner(paths, "", "");
        }



        //==========================================================
        // Call system settings etc.

        /// <summary>
        /// Open Wifi system settings screen
        ///·Callbacks can basically be regarded as 'CLOSED_WIFI_SETTINGS' (closed) only (on / off will not be returned).
        /// </summary>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="resultCallbackMethod">Method name to callback the result</param>
        public static void OpenWifiSettings(string callbackGameObject, string resultCallbackMethod)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "openWifiSettings",
                    context,
                    callbackGameObject,
                    resultCallbackMethod
                );
            }));
        }


        //(*) No callback overload
        /// <summary>
        /// Open Wifi system settings screen (No callback overload)
        /// </summary>
        public static void OpenWifiSettings()
        {
            OpenWifiSettings("", "");
        }



        //(*) Permission Denial -> Required: '<uses-permission android:name="android.permission.BLUETOOTH" />' in 'AndroidManifest.xml'
        /// <summary>
        /// Start Bluetooth connection check and request
        ///·When current is on -> "SUCCESS_BLUETOOTH_ENABLE" is returned in the callback.
        ///·When current is off -> A connection request dialog appears
        /// -> "Yes" returns "SUCCESS_BLUETOOTH_ENABLE" for callback, "CANCEL_BLUETOOTH_ENABLE" on "No".
        ///·If "ERROR_BLUETOOTH_ADAPTER_NOT_AVAILABLE" returns in the callback, it can not be used in the system.
        ///(*) 'BLUETOOTH' permission is necessary for 'AndroidManifest.xml' (If it does not exist, "Permission Denial" appears in the callback).
        /// </summary>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="resultCallbackMethod">Method name to callback the result</param>
        public static void StartBluetoothRequestEnable(string callbackGameObject, string resultCallbackMethod)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startBluetoothRequestEnable",
                    context,
                    callbackGameObject,
                    resultCallbackMethod
                );
            }));
        }


        //(*) Required: '<uses-permission android:name="android.permission.BLUETOOTH" />' in 'AndroidManifest.xml'
        //(*) No callback overload
        /// <summary>
        /// Start Bluetooth connection check and request (No callback overload)
        ///·When current is on -> Do nothing.
        ///·When current is off -> A connection request dialog appears -> "Yes" turns on.
        ///(*) 'BLUETOOTH' permission is necessary for 'AndroidManifest.xml' (If it does not exist, "Permission Denial" appears in the callback).
        /// </summary>
        public static void StartBluetoothRequestEnable()
        {
            StartBluetoothRequestEnable("", "");  //コールバック無しオーバーロード
        }



        //==========================================================
        // Open system default application, use etc.

        //(*) API 19 (Android4.4) or higher is recommended
        //(*) When using External Storage, the following permission is required (unnecessary when "WRITE_EXTERNAL_STORAGE" exists).
        // '<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />' in 'AndroidManifest.xml'
        /// <summary>
        /// Open the gallery and get the path
        ///(*)API 19 (Android 4.4) or higher is recommended. Because paths that can be get by API Level are different, paths are not always returned.
        ///·When it succeeds, a string is returned in JSON format as '{"path":"(path)","width":(width),"height":(height)}' (It can be convert with JsonUtility).
        /// There is a possibility that the size (width, height) can not be get depending on the save condition (it becomes 0).
        ///·If it fails, the following error message is returned.
        /// CANCEL_GALLERY : Closed without selection
        /// ERROR_GALLERY_GET_PATH_FAILURE: Path get failed
        /// </summary>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="resultCallbackMethod">Method name to callback the result (absolute path)</param>
        /// <param name="errorCallbackMethod">Method name to callback error or cancel</param>
        public static void OpenGallery(string callbackGameObject, string resultCallbackMethod, string errorCallbackMethod)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "openGallery",
                    context,
                    callbackGameObject,
                    resultCallbackMethod,
                    errorCallbackMethod
                );
            }));
        }



        //==========================================================
        // Using the storage access framework
        //(*) API 19 (Android4.4) or higher
        // https://developer.android.com/guide/topics/providers/document-provider.html
        //==========================================================

        //Default charset encoding
        const string DEFAULT_ENCODING = "UTF-8";

        //(*) API 19 (Android4.4) or higher
        /// <summary>
        /// Select and load a text file with the storage access framework.
        ///(*) API 19 (Android4.4) or higher
        ///·When it succeeds, the text read in 'resultCallbackMethod' is returned.
        ///·If it fails, the following error message is returned.
        /// CANCEL_LOAD_TEXT : Closed without selection
        /// UnsupportedEncodingException : Unsupported character encoding
        /// </summary>
        /// <param name="encoding">Character encoding ("UTF-8", "Shift_JIS", "EUC-JP", "JIS" etc.)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="resultCallbackMethod">Method name to callback the result (loaded text)</param>
        /// <param name="errorCallbackMethod">Method name to callback error or cancel</param>
        public static void OpenStorageAndLoadText(string encoding, 
            string callbackGameObject, string resultCallbackMethod, string errorCallbackMethod)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "openDocumentAndLoadText",
                    context,
                    encoding,
                    callbackGameObject,
                    resultCallbackMethod,
                    errorCallbackMethod
                );
            }));
        }


        //(*) API 19 (Android4.4) or higher
        //(*) UTF-8 encoding overload
        /// <summary>
        /// Select and load a text file with the storage access framework (Character encoding : UTF-8).
        ///(*) API 19 (Android4.4) or higher
        ///·When it succeeds, the text read in 'resultCallbackMethod' is returned.
        ///·If it fails, the following error message is returned.
        /// CANCEL_LOAD_TEXT : Closed without selection
        /// UnsupportedEncodingException : Unsupported character encoding
        /// </summary>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="resultCallbackMethod">Method name to callback the result (loaded text)</param>
        /// <param name="errorCallbackMethod">Method name to callback error or cancel</param>
        public static void OpenStorageAndLoadText(string callbackGameObject, string resultCallbackMethod, string errorCallbackMethod)
        {
            OpenStorageAndLoadText(DEFAULT_ENCODING, callbackGameObject, resultCallbackMethod, errorCallbackMethod);
        }



        //(*) API 19 (Android4.4) or higher
        /// <summary>
        /// Select the directory in the storage access framework and save it.
        ///(*) API 19 (Android4.4) or higher
        ///·When it succeeds, the file name saved in 'resultCallbackMethod' is returned.
        ///·If it fails, the following error message is returned.
        /// CANCEL_SAVE_TEXT : Closed without selection
        /// UnsupportedEncodingException : Unsupported character encoding
        /// </summary>
        /// <param name="fileName">Default file name (ex: "NewDocument.txt". Does not include directory path)</param>
        /// <param name="text">Text to save</param>
        /// <param name="encoding">Character encoding ("UTF-8", "Shift_JIS", "EUC-JP", "JIS" etc.)</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="resultCallbackMethod">Method name to callback the result (the file name saved)</param>
        /// <param name="errorCallbackMethod">Method name to callback error or cancel</param>
        public static void OpenStorageAndSaveText(string fileName, string text, string encoding,
            string callbackGameObject, string resultCallbackMethod, string errorCallbackMethod)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "createDocumentAndSaveText",
                    context,
                    fileName,
                    text,
                    encoding,
                    callbackGameObject,
                    resultCallbackMethod,
                    errorCallbackMethod
                );
            }));
        }


        //(*) API 19 (Android4.4) or higher
        //(*) UTF-8 encoding overload
        /// <summary>
        /// Select the directory in the storage access framework and save it (Character encoding : UTF-8).
        ///(*) API 19 (Android4.4) or higher
        ///·When it succeeds, the file name saved in 'resultCallbackMethod' is returned.
        ///·If it fails, the following error message is returned.
        /// CANCEL_SAVE_TEXT : Closed without selection
        /// UnsupportedEncodingException : Unsupported character encoding
        /// </summary>
        /// <param name="fileName">Default file name (ex: "NewDocument.txt". Does not include directory path)</param>
        /// <param name="text">Text to save</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="resultCallbackMethod">Method name to callback the result (the file name saved)</param>
        /// <param name="errorCallbackMethod">Method name to callback error or cancel</param>
        public static void OpenStorageAndSaveText(string fileName, string text,
            string callbackGameObject, string resultCallbackMethod, string errorCallbackMethod)
        {
            OpenStorageAndSaveText(fileName, text, DEFAULT_ENCODING, callbackGameObject, resultCallbackMethod, errorCallbackMethod);
        }




        //==========================================================
        // Get status of External Storage
        //(*) Although it was an SD card before API 19 (Android4.4), it became virtual storage (internal storage) after API 19.
        //·before API 19 : "mnt/sdcard/" or "sdcard/" etc. (Depends on model)
        //·after  API 19 : "/storage/emulated/0" like (Depends on model(?))
        //(*) To write to External Storage, permission is necessary for 'AndroidManifest.xml'.
        // <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
        //(*) It can be "READ_EXTERNAL_STORAGE" for reading only.
        //==========================================================

        /// <summary>
        /// Returns whether the primary shared/external storage media is emulated.
        /// </summary>
        /// <returns>true = storage media is emulated</returns>
        public static bool IsExternalStorageEmulated()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<bool>(
                    "isExternalStorageEmulated"
                );
            }
        }

        /// <summary>
        /// Returns whether the primary shared/external storage media is physically removable.
        /// </summary>
        /// <returns>true if the storage device can be removed (such as an SD card), or false if the storage device is built in and cannot be physically removed.</returns>
        public static bool IsExternalStorageRemovable()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<bool>(
                    "isExternalStorageRemovable"
                );
            }
        }

        /// <summary>
        /// Check for External Storage mounted.
        /// </summary>
        /// <returns>true = mounted</returns>
        public static bool IsExternalStorageMounted()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<bool>(
                    "isExternalStorageMounted"
                );
            }
        }

        /// <summary>
        /// Check for External Storage mounted and readonly.
        /// </summary>
        /// <returns>true = mounted and readonly</returns>
        public static bool IsExternalStorageMountedReadOnly()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<bool>(
                    "isExternalStorageMountedReadOnly"
                );
            }
        }

        //(*) To write to External Storage, required '<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />' in 'AndroidManifest.xml'
        /// <summary>
        /// Check for External Storage mounted and read/write.
        /// </summary>
        /// <returns>true = mounted and read/write</returns>
        public static bool IsExternalStorageMountedReadWrite()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<bool>(
                    "isExternalStorageMountedReadWrite"
                );
            }
        }

        /// <summary>
        /// Returns the current state of the primary shared/external storage media.
        /// https://developer.android.com/reference/android/os/Environment.html#MEDIA_BAD_REMOVAL
        /// </summary>
        /// <returns>one of "unknown", "removed", "unmounted", "checking", "nofs", "mounted", "mounted_ro", "shared", "bad_removal", or "unmountable".</returns>
        public static string GetExternalStorageState()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageState"
                );
            }
        }

        /// <summary>
        /// Return the primary shared/external storage directory.
        /// </summary>
        /// <returns>returns directory path like "/storage/emulated/0"</returns>
        public static string GetExternalStorageDirectory()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageDirectory"
                );
            }
        }

        /// <summary>
        /// Returns the 'Alarms' directory of shared/external storage directory.
        /// </summary>
        /// <returns>returns directory path like "/storage/emulated/0/Alarms"</returns>
        public static string GetExternalStorageDirectoryAlarms()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageDirectoryAlarms"
                );
            }
        }

        /// <summary>
        /// Returns the 'DCIM' directory of shared/external storage directory.
        /// </summary>
        /// <returns>returns directory path like "/storage/emulated/0/DCIM"</returns>
        public static string GetExternalStorageDirectoryDCIM()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageDirectoryDCIM"
                );
            }
        }

        //(*) API 19 or higher (Note: before that all empty characters(""))
        /// <summary>
        /// Returns the 'Documents' directory of shared/external storage directory.
        /// (*) API 19 or higher (Note: before that all empty characters(""))
        /// </summary>
        /// <returns>returns directory path like "/storage/emulated/0/Documents"</returns>
        public static string GetExternalStorageDirectoryDocuments()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageDirectoryDocuments"
                );
            }
        }

        /// <summary>
        /// Returns the 'Downloads' directory of shared/external storage directory.
        /// </summary>
        /// <returns>returns directory path like "/storage/emulated/0/Downloads"</returns>
        public static string GetExternalStorageDirectoryDownloads()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageDirectoryDownloads"
                );
            }
        }

        /// <summary>
        /// Returns the 'Movies' directory of shared/external storage directory.
        /// </summary>
        /// <returns>returns directory path like "/storage/emulated/0/Movies"</returns>
        public static string GetExternalStorageDirectoryMovies()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageDirectoryMovies"
                );
            }
        }

        /// <summary>
        /// Returns the 'Music' directory of shared/external storage directory.
        /// </summary>
        /// <returns>returns directory path like "/storage/emulated/0/Music"</returns>
        public static string GetExternalStorageDirectoryMusic()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageDirectoryMusic"
                );
            }
        }

        /// <summary>
        /// Returns the 'Notifications' directory of shared/external storage directory.
        /// </summary>
        /// <returns>returns directory path like "/storage/emulated/0/Notifications"</returns>
        public static string GetExternalStorageDirectoryNotifications()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageDirectoryNotifications"
                );
            }
        }

        /// <summary>
        /// Returns the 'Pictures' directory of shared/external storage directory.
        /// </summary>
        /// <returns>returns directory path like "/storage/emulated/0/Pictures"</returns>
        public static string GetExternalStorageDirectoryPictures()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageDirectoryPictures"
                );
            }
        }

        /// <summary>
        /// Returns the 'Podcasts' directory of shared/external storage directory.
        /// </summary>
        /// <returns>returns directory path like "/storage/emulated/0/Podcasts"</returns>
        public static string GetExternalStorageDirectoryPodcasts()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageDirectoryPodcasts"
                );
            }
        }

        /// <summary>
        /// Returns the 'Ringtones' directory of shared/external storage directory.
        /// </summary>
        /// <returns>returns directory path like "/storage/emulated/0/Ringtones"</returns>
        public static string GetExternalStorageDirectoryRingtones()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<string>(
                    "getExternalStorageDirectoryRingtones"
                );
            }
        }




        //==========================================================
        // Speech Recognizer
        //(*) Required "uses-permission android:name="android.permission.RECORD_AUDIO" tag in "AndroidManifest.xml".
        //(*) Rename "AndroidManifest-FullPlugin~.xml" to "AndroidManifest.xml" 
        // http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_SpeechRecognizerDialog
        // http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_SpeechRecognizerListener
        //==========================================================


        //(*) Required: '<uses-permission android:name="android.permission.RECORD_AUDIO />' in 'AndroidManifest.xml'
        /// <summary>
        /// Does the system support speech recognizer?
        /// (*) It has nothing to do with permissions.
        /// </summary>
        /// <returns>true = supported</returns>
        public static bool IsSupportedSpeechRecognizer()
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<bool>(
                            "isSupportedSpeechRecognizer",
                            context
                        );
                    }
                }
            }
        }



        //(*) Required: '<uses-permission android:name="android.permission.RECORD_AUDIO />' in 'AndroidManifest.xml'
        /// <summary>
        /// Call Android Speech Recognizer Dialog
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_SpeechRecognizerDialog
        ///･The recognized strings are concatenated with line feed ("\n") and returned as arguments of the callback.
        /// If it fails, it returns an error code string (like "ERROR_").
        ///･(Error code string)
        /// https://developer.android.com/reference/android/speech/SpeechRecognizer.html#ERROR_AUDIO
        ///･Mainly the following error occurs:
        /// ERROR_NO_MATCH : Could not recognize it.
        /// ERROR_SPEECH_TIMEOUT ： Wait for speech time out.
        /// ERROR_RECOGNIZER_BUSY : System is busy (When going on continuously etc.)
        /// ERROR_INSUFFICIENT_PERMISSIONS : There is no permission tag "RECORD_AUDIO" in "AndroidManifest.xml".
        /// ERROR_NETWORK : Could not connect to the network.
        /// </summary>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="resultCallbackMethod">Method name to result callback (it is in GameObject)</param>
        /// <param name="message">Message string</param>
        public static void ShowSpeechRecognizer(string callbackGameObject, string resultCallbackMethod, string message = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showSpeechRecognizer",
                    context,
                    callbackGameObject,
                    resultCallbackMethod,
                    message
                );
            }));
        }



        //(*) Required: '<uses-permission android:name="android.permission.RECORD_AUDIO />' in 'AndroidManifest.xml'
        /// <summary>
        /// Call Android Speech Recognizer without Dialog (Receive events and results)
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_SpeechRecognizerListener
        ///･The recognized strings are concatenated with line feed ("\n") and returned as arguments of the callback.
        /// If it fails, it returns an error code string (like "ERROR_").
        ///･(Error code string)
        /// https://developer.android.com/reference/android/speech/SpeechRecognizer.html#ERROR_AUDIO
        ///･Mainly the following error occurs:
        /// ERROR_NO_MATCH : Could not recognize it.
        /// ERROR_SPEECH_TIMEOUT ： Wait for speech time out.
        /// ERROR_RECOGNIZER_BUSY : System is busy (When going on continuously etc.)
        /// ERROR_INSUFFICIENT_PERMISSIONS : There is no permission tag "RECORD_AUDIO" in "AndroidManifest.xml".
        /// ERROR_NETWORK : Could not connect to the network.
        /// </summary>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="resultCallbackMethod">Method name to callback when recognition is successful (it is in GameObject)</param>
        /// <param name="errorCallbackMethod">Method name to callback when recognition is failure or error (it is in GameObject)</param>
        /// <param name="readyCallbackMethod">Method name to callback when start waiting for speech recognition (Always "onReadyForSpeech" is returned) (it is in GameObject)</param>
        /// <param name="beginCallbackMethod">Method name to callback when the first voice entered the microphone (Always "onBeginningOfSpeech" is returned) (it is in GameObject)</param>
        public static void StartSpeechRecognizer(string callbackGameObject, string resultCallbackMethod, string errorCallbackMethod,
            string readyCallbackMethod = "", string beginCallbackMethod = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startSpeechRecognizer",
                    context,
                    callbackGameObject,
                    resultCallbackMethod,
                    errorCallbackMethod,
                    readyCallbackMethod,
                    beginCallbackMethod
                );
            }));
        }



        //(*) Required: '<uses-permission android:name="android.permission.RECORD_AUDIO />' in 'AndroidManifest.xml'
        /// <summary>
        /// Release speech recognizer instance (Also use it to interrupt)
        ///･Even when "AndroidPlugin.Release()" is called, it is executed on the native side.
        /// </summary>
        public static void ReleaseSpeechRecognizer()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                androidSystem.CallStatic(
                    "releaseSpeechRecognizer"
                );
            }
        }




        //==========================================================
        // Text To Speech
        //(*) Voice data must be installed on the terminal in order to use it.
        // The following are confirmed operation.
        // https://play.google.com/store/apps/details?id=com.google.android.tts
        // https://play.google.com/store/apps/details?id=jp.kddilabs.n2tts
        //==========================================================


        /// <summary>
        /// Call Android Text To Speech
        /// http://fantom1x.blog130.fc2.com/blog-entry-275.html#fantomPlugin_TextToSpeech
        ///･When it is available at the first startup, "SUCCESS_INIT" status is returned to the callback method (statusCallbackMethod).
        ///･(Error code string)
        /// https://developer.android.com/reference/android/speech/tts/TextToSpeech.html#ERROR
        /// Moreover, the following error occurs:
        /// ERROR_LOCALE_NOT_AVAILABLE : There is no voice data corresponding to the system language setting
        /// ERROR_INIT : Initialization failed
        /// ERROR_UTTERANCEPROGRESS_LISTENER_REGISTER : Event listener registration failed
        /// ERROR_UTTERANCEPROGRESS_LISTENER : Some kind of error of event acquisition
        ///･Interrupted event callback (stopCallbackMethod) can only be operated with API 23 (Android 6.0) or higher.
        /// However, it seems that the behavior varies depending on the system, so be careful.
        /// </summary>
        /// <param name="message">Reading text string</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="statusCallbackMethod">Method name to callback when returns status (including error)</param>
        /// <param name="startCallbackMethod">Method name to callback when start reading text (Always "onStart" is returned) (it is in GameObject)</param>
        /// <param name="doneCallbackMethod">Method name to callback when finish reading text (Always "onDone" is returned) (it is in GameObject)</param>
        /// <param name="stopCallbackMethod">Method name to callback when interrupted reading text (During playback is "INTERRUPTED". Other than always "onStop" is returned) (it is in GameObject)</param>
        public static void StartTextToSpeech(string message, string callbackGameObject = "", string statusCallbackMethod = "",
                                                string startCallbackMethod = "", string doneCallbackMethod = "", string stopCallbackMethod = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startTextToSpeech",
                    context,
                    message,
                    callbackGameObject,
                    statusCallbackMethod,
                    startCallbackMethod,
                    doneCallbackMethod,
                    stopCallbackMethod
                );
            }));
        }



        /// <summary>
        /// Initialize Text To Speech (First time only)
        /// http://fantom1x.blog130.fc2.com/blog-entry-275.html#fantomPlugin_TextToSpeech
        ///･When it is available at the first startup, "SUCCESS_INIT" status is returned to the callback method (statusCallbackMethod).
        ///･(Error code string)
        /// https://developer.android.com/reference/android/speech/tts/TextToSpeech.html#ERROR
        /// Moreover, the following error occurs:
        /// ERROR_LOCALE_NOT_AVAILABLE : There is no voice data corresponding to the system language setting
        /// ERROR_INIT : Initialization failed
        /// ERROR_UTTERANCEPROGRESS_LISTENER_REGISTER : Event listener registration failed
        /// ERROR_UTTERANCEPROGRESS_LISTENER : Some kind of error of event acquisition
        /// </summary>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="statusCallbackMethod">Method name to callback when returns status (including error)</param>
        public static void InitTextToSpeech(string callbackGameObject = "", string statusCallbackMethod = "")
        {
            StartTextToSpeech("", callbackGameObject, statusCallbackMethod);
        }
        [Obsolete("This method name is a typo. Please use 'InitTextToSpeech' instead.")]
        public static void InitSpeechRecognizer(string callbackGameObject = "", string statusCallbackMethod = "")
        {
            StartTextToSpeech("", callbackGameObject, statusCallbackMethod);
        }



        /// <summary>
        /// Interruption of text reading
        /// </summary>
        public static void StopTextToSpeech()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                androidSystem.CallStatic(
                    "stopTextToSpeech"
                );
            }
        }



        /// <summary>
        /// Get utterance Speed (1.0f: Normal speed)
        /// </summary>
        /// <returns>0.5f~1.0f~2.0f</returns>
        public static float GetTextToSpeechSpeed()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<float>(
                    "getTextToSpeechSpeed"
                );
            }
        }


        /// <summary>
        /// Set utterance Speed (1.0f: Normal speed)
        /// </summary>
        /// <param name="newSpeed">0.5f~1.0f~2.0f</param>
        /// <returns>Speed after setting : 0.5f~1.0f~2.0f</returns>
        public static float SetTextToSpeechSpeed(float newSpeed)
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<float>(
                    "setTextToSpeechSpeed",
                    newSpeed
                );
            }
        }


        /// <summary>
        /// Add utterance Speed (1.0f: Normal speed)
        /// </summary>
        /// <param name="addSpeed">0.5f~1.0f~2.0f</param>
        /// <returns>Speed after addition : 0.5f~1.0f~2.0f</returns>
        public static float AddTextToSpeechSpeed(float addSpeed)
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<float>(
                    "addTextToSpeechSpeed",
                    addSpeed
                );
            }
        }


        /// <summary>
        /// Reset utterance Speed (-> 1.0f: Normal speed)
        ///･Same as SetTextToSpeechSpeed(1f)
        /// </summary>
        public static float ResetTextToSpeechSpeed()
        {
            return SetTextToSpeechSpeed(1.0f);
        }



        /// <summary>
        /// Get utterance Pitch (1.0f: Normal pitch)
        /// </summary>
        /// <returns>0.5f~1.0f~2.0f</returns>
        public static float GetTextToSpeechPitch()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<float>(
                    "getTextToSpeechPitch"
                );
            }
        }


        /// <summary>
        /// Set utterance Pitch (1.0f: Normal pitch)
        /// </summary>
        /// <param name="newPitch">0.5f~1.0f~2.0f</param>
        /// <returns>Pitch after setting : 0.5f~1.0f~2.0f</returns>
        public static float SetTextToSpeechPitch(float newPitch)
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<float>(
                    "setTextToSpeechPitch",
                    newPitch
                );
            }
        }


        /// <summary>
        /// Add utterance Pitch (1.0f: Normal pitch)
        /// </summary>
        /// <param name="addPitch">Pitch to be added: 0.5f~1.0f~2.0f</param>
        /// <returns>Pitch after addition : 0.5f~1.0f~2.0f</returns>
        public static float AddTextToSpeechPitch(float addPitch)
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                return androidSystem.CallStatic<float>(
                    "addTextToSpeechPitch",
                    addPitch
                );
            }
        }


        /// <summary>
        /// Reset utterance Pitch (-> 1.0f: Normal Pitch)
        ///･Same as SetTextToSpeechPitch(1f)
        /// </summary>
        public static float ResetTextToSpeechPitch()
        {
            return SetTextToSpeechPitch(1.0f);
        }



        /// <summary>
        /// Release Text To Speech instance (Resource release)
        ///･Even when "AndroidPlugin.Release()" is called, it is executed on the native side.
        /// </summary>
        public static void ReleaseTextToSpeech()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                androidSystem.CallStatic(
                    "releaseTextToSpeech"
                );
            }
        }




        //========================================================================
        //Listening status
        //(*) Unreleased listening is a cause of memory leak, so it is better to always release it when not using it.
        //(*) Application 'Pause -> Resume' with 'remove -> set' is unnecessary (automatically stopped → resumed).
        //(*) It is released even when 'AndroidPlugin.Release()' (It is automatically released even when the application is quit).
        //========================================================================

        /// <summary>
        /// Start listening to the status of the battery
        ///(*) Callback registration becomes unique (always overwritten, the last registered one will be valid).
        ///(*) It is unnecessary in the application resume(return from pause)  (automatically restart).
        /// </summary>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback when returns status</param>
        public static void StartBatteryStatusListening(string callbackGameObject, string callbackMethod)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "startBatteryListening",
                    context,
                    callbackGameObject,
                    callbackMethod
                );
            }));
        }


        /// <summary>
        /// Stop listening to the status of the battery (release)
        ///(*) Unreleased listening is a cause of memory leak, so it is better to always release it when not using it.
        ///(*) It is unnecessary in the application pause (automatically stop).
        /// </summary>
        public static void StopBatteryStatusListening()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                androidSystem.CallStatic(
                    "stopBatteryListening"
                );
            }
        }




        //========================================================================
        // Get Android's configuration change
        //·The changed status is transmitted from Android when the configuration changes.
        //(*) Application 'Pause -> Resume' with 'remove -> set' is unnecessary (automatically stopped → resumed).
        //(*) It is released even when 'AndroidPlugin.Release()' (It is automatically released even when the application is quit).
        // (*Requied: rename and use 'AndroidManifest-FullPlugin_~.xml')
        //========================================================================

        /// <summary>
        /// Register screen rotation callback
        ///(*) Callback registration becomes unique (always overwritten, the last registered one will be valid).
        ///(*) The following attribute is required for "activity" tag of "AndroidManifest.xml" (* In the case of Unity, it is added by default).
        /// 'android:configChanges="orientation|screenSize"'
        ///·Normally, for applications that rotate the screen in four directions, add the following attributes to the 'activity' tag of 'AndroidManifest.xml'.
        /// 'android:screenOrientation="sensor"'
        ///(* Included by default in 'AndroidManifest-FullPlugin_Sensor.xml')
        /// https://developer.android.com/guide/topics/manifest/activity-element.html
        /// </summary>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback when returns status</param>
        public static void SetOrientationChangeListener(string callbackGameObject, string callbackMethod)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "setOrientationChangeListener",
                    context,
                    callbackGameObject,
                    callbackMethod
                );
            }));
        }


        /// <summary>
        /// Release screen rotation callback
        ///(*)'AndroidPlugin.Release()' is also released.
        /// </summary>
        public static void RemoveOrientationChangeListener()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                androidSystem.CallStatic(
                    "removeOrientationChangeListener"
                );
            }
        }




        //========================================================================
        // Get values of Android sensor
        //·The sensor can choose the acquisition interval, but the higher the speed, the higher the load, so the minimum speed (200 ms (5 fps)) is recommended.
        //·Input.acceleration, gyro, compass etc, those that can be obtained by Unity, it seems that the load is light.
        //(*) Note that the installation of each sensor differs depending on the device. It is better to filter with sensors that are available for Google Play.
        // (Using Google Play filters to target specific sensor configurations)
        //  https://developer.android.com/guide/topics/sensors/sensors_overview.html#sensors-configs
        //(*) Note that use of each sensor is also restricted by API Level (as noted in the 'SensorType' comment).
        //  https://developer.android.com/reference/android/hardware/Sensor.html#TYPE_ACCELEROMETER
        //  https://developer.android.com/guide/topics/manifest/uses-sdk-element.html#ApiLevels
        //(*) Depending on the sensor, permissions may be required (as noted in the 'SensorType' comment).
        //(*) Callback registration is unique for each sensor (always overwritten, the last one registered will be valid).
        //(*) Application 'Pause -> Resume' with 'remove -> set' is unnecessary (automatically stopped → resumed).
        //(*) It is released even when 'AndroidPlugin.Release()' (It is automatically released even when the application is quit).
        // (Requied: rename and use 'AndroidManifest-FullPlugin_~.xml')
        //
        //
        // Android のセンサーの値を取得する
        //・センサーは取得間隔を選べるが、速度が速いほど負荷が高くなるため、最低速度（200ms (5fps)）で良い。
        //・Input.acceleration, gyro, compass 等、Unity で取得できるものは、そちらの方が負荷が軽いと思われる。
        //※各センサーの搭載は端末の種類によって違うので注意。Google Play では利用できるセンサーでフィルタリングした方が良い。
        // (Using Google Play filters to target specific sensor configurations)
        //  https://developer.android.com/guide/topics/sensors/sensors_overview.html#sensors-configs
        //※各センサーの利用は API Level でも制限されるので注意（SensorType のコメントに記してある）。
        //  https://developer.android.com/reference/android/hardware/Sensor.html#TYPE_ACCELEROMETER
        //  https://developer.android.com/guide/topics/manifest/uses-sdk-element.html#ApiLevels
        //※センサーによってはパーミッションが必要になることがある（SensorType のコメントに記してある）。
        //※コールバックの登録はセンサーごとにユニークとなる（常に上書き。最後に登録したものが有効となる）。
        //※アプリケーションの Pause → Resume での削除→再登録は不要（自動的に停止→再開される）。
        // (※「AndroidManifest-FullPlugin_～.xml」をリネームして使う)
        //========================================================================

        /// <summary>
        /// Get supported for each sensor
        /// </summary>
        /// <param name="sensorType">Sensor type constant</param>
        /// <returns>true = supported</returns>
        public static bool IsSupportedSensor(int sensorType)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<bool>(
                            "isSupportedSensor",
                            context,
                            sensorType
                        );
                    }
                }
            }
        }

        //(*) SensorType overload
        /// <summary>
        /// Get supported for each sensor
        /// </summary>
        /// <param name="sensorType">Sensor type constant</param>
        /// <returns>true = supported</returns>
        public static bool IsSupportedSensor(SensorType sensorType)
        {
            return IsSupportedSensor((int)sensorType);
        }


        /// <summary>
        /// Register listener for each sensor
        ///·Sensor type constant
        /// https://developer.android.com/reference/android/hardware/Sensor.html#TYPE_ACCELEROMETER
        ///·Sensor delay constant (Detection Interval) Constant is preferably (3: Normal [200 ms]) (* to reduce load).
        /// https://developer.android.com/reference/android/hardware/SensorManager.html#SENSOR_DELAY_FASTEST
        ///(*) Callback registration is unique for each sensor (always overwritten, the last one registered will be valid).
        ///(*) Application 'Pause -> Resume' with 'remove -> set' is unnecessary (automatically stopped → resumed).
        ///(*) It is unnecessary in the application resume(return from pause)  (automatically restart).
        ///
        ///
        /// 各センサーのリスナー登録
        ///・センサー種類定数
        /// https://developer.android.com/reference/android/hardware/Sensor.html#TYPE_ACCELEROMETER
        ///・センサー速度(検出間隔)定数はなるべく（3: Normal[200ms]）が良い（※負荷を軽減するため）。
        /// https://developer.android.com/reference/android/hardware/SensorManager.html#SENSOR_DELAY_FASTEST
        ///※コールバックの登録はセンサーごとにユニークとなる（常に上書き。最後に登録したものが有効となる）。
        ///※アプリケーションの Resume(Pauseから復帰)では不要（自動的に再開される）。
        /// </summary>
        /// <param name="sensorType">Sensor type constant</param>
        /// <param name="sensorDelay">Sensor delay constant</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback when returns status</param>
        public static void SetSensorListener(int sensorType, int sensorDelay, string callbackGameObject, string callbackMethod)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "setSensorListener",
                    context,
                    sensorType,
                    sensorDelay,
                    callbackGameObject,
                    callbackMethod
                );
            }));
        }

        //(*) SensorType overload
        /// <summary>
        /// Register callback for each sensor
        ///·Sensor type constant
        /// https://developer.android.com/reference/android/hardware/Sensor.html#TYPE_ACCELEROMETER
        ///·Sensor delay constant (Detection Interval) Constant is preferably (3: Normal [200 ms]) (* to reduce load).
        /// https://developer.android.com/reference/android/hardware/SensorManager.html#SENSOR_DELAY_FASTEST
        ///(*) Callback registration is unique for each sensor (always overwritten, the last one registered will be valid).
        ///(*) Application 'Pause -> Resume' with 'remove -> set' is unnecessary (automatically stopped → resumed).
        ///(*) It is unnecessary in the application resume(return from pause)  (automatically restart).
        ///
        ///
        /// 各センサーのリスナー登録
        ///・センサー種類定数
        /// https://developer.android.com/reference/android/hardware/Sensor.html#TYPE_ACCELEROMETER
        ///・センサー速度(検出間隔)定数はなるべく（3: Normal[200ms]）が良い（※負荷を軽減するため）。
        /// https://developer.android.com/reference/android/hardware/SensorManager.html#SENSOR_DELAY_FASTEST
        ///※コールバックの登録はセンサーごとにユニークとなる（常に上書き。最後に登録したものが有効となる）。
        ///※アプリケーションの Resume(Pauseから復帰)では不要（自動的に再開される）。
        /// </summary>
        /// <param name="sensorType">Sensor type constant</param>
        /// <param name="sensorDelay">Sensor delay constant</param>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback when returns status</param>
        public static void SetSensorListener(SensorType sensorType, SensorDelay sensorDelay, string callbackGameObject, string callbackMethod)
        {
            SetSensorListener((int)sensorType, (int)sensorDelay, callbackGameObject, callbackMethod);
        }


        /// <summary>
        /// Release callback for each sensor
        ///(*)'AndroidPlugin.Release()' is also released.
        ///(*) It is unnecessary in the application pause (automatically stop).
        ///
        ///
        /// 各センサーの解除
        ///※AndroidPlugin.Release() でも解除される。
        ///※アプリケーションの Pause(一時停止)では不要（自動的に停止される）。
        /// </summary>
        /// <param name="sensorType">Sensor type constant</param>
        public static void RemoveSensorListener(int sensorType)
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                androidSystem.CallStatic(
                    "removeSensorListener",
                    sensorType
                );
            }
        }

        //SensorType overload
        /// <summary>
        /// Release callback for each sensor
        ///(*)'AndroidPlugin.Release()' is also released.
        ///(*) It is unnecessary in the application pause (automatically stop).
        ///
        ///
        /// 各センサーの解除
        ///※AndroidPlugin.Release() でも解除される。
        ///※アプリケーションの Pause(一時停止)では不要（自動的に停止される）。
        /// </summary>
        /// <param name="sensorType">Sensor type constant</param>
        public static void RemoveSensorListener(SensorType sensorType)
        {
            RemoveSensorListener((int)sensorType);
        }


        /// <summary>
        /// Release callback for all sensors
        ///(*)'AndroidPlugin.Release()' is also released.
        ///
        /// 
        /// 全センサーの解除
        ///※AndroidPlugin.Release() でも解除される。
        /// </summary>
        public static void ReleaseSensors()
        {
            using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
            {
                androidSystem.CallStatic(
                    "releaseSensors"
                );
            }
        }




        //==========================================================
        // Confirm Device Credentials (fingerprint, pattern, PIN, password, etc. depending on user device setting)
        //(*) API 21 (Android 5.0) or higher
        //
        // デバイス認証（指紋・パターン・PIN・パスワード等。端末の設定による）
        //※API 21 (Android 5.0) 以上
        //==========================================================

        /// <summary>
        /// Whether Confirm Device Credentials (fingerprint, pattern, PIN, password, etc. depending on user device setting) is available (API 21 or higher)
        ///・It is false when unavailable device or security setting is turned off.
        ///(*) It is always false when it is below API 21 (Android 5.0).
        /// 
        /// 
        /// デバイス認証（指紋・パターン・PIN・パスワード等。端末の設定による）が利用可能かどうかを取得する（API 21 以上）
        ///・利用できないデバイス、またはセキュリティ設定がオフになっているとき false となる。
        ///※API 21 (Android 5.0) より下のときは常に false になる。
        /// </summary>
        /// <returns>true = supported</returns>
        public static bool IsSupportedDeviceCredentials()
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<bool>(
                            "isSupportedCredentials",
                            context
                        );
                    }
                }
            }
        }


        /// <summary>
        /// Show Confirm Device Credentials screen (fingerprint, pattern, PIN, password, etc. depending on user device setting) and returns the authentication result. (API 21 or higher)
        ///·The following status string is returned in the result (JSON format).
        /// SUCCESS_CREDENTIALS : Authentication success
        /// UNAUTHORIZED_CREDENTIALS : Authentication failure or cancel.
        /// ERROR_NOT_SUPPORTED : Device authentication can not be used. Or security is turned off.
        ///·Session ID (string) should contain unique and random character string each time.
        ///(*) message (description character string) may not be reflected by the device.
        ///(*) Title and message are basically empty (because messages are set automatically by system language).
        ///
        ///
        /// デバイス認証（指紋・パターン・PIN・パスワード等。端末の設定による）を開き、結果を返す（API 21 以上）
        ///・結果は以下のステータス文字列が返る（JSON形式）。
        /// SUCCESS_CREDENTIALS : 認証成功。
        /// UNAUTHORIZED_CREDENTIALS : 認証失敗。またはキャンセル。
        /// ERROR_NOT_SUPPORTED : デバイス認証が利用できない。またはセキュリティがオフになっている。
        ///・セッションID文字列は毎回ユニークでランダムな文字列を含んだ方が良い。
        ///※message（メッセージ文字列）はデバイスによって反映されない場合がある。
        ///※title, message は基本的に空で良い（システム言語によって自動でメッセージが設定されるため）。
        /// </summary>
        /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
        /// <param name="callbackMethod">Method name to callback when returns status</param>
        /// <param name="sessionID">Unique SessionID string</param>
        /// <param name="title">Title string (When empty, it becomes default of the device)</param>
        /// <param name="message">Message string (When empty, it becomes default of the device)</param>
        public static void ShowDeviceCredentials(string callbackGameObject, string callbackMethod, string sessionID, string title = "", string message = "")
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showCredentials",
                    context,
                    callbackGameObject,
                    callbackMethod,
                    sessionID,
                    title,
                    message
                );
            }));
        }


        //Character string list (used for JSON)
        const string ASCII = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!#$%&*+-.=?@^_`|~";
        const int ASCII_LENGTH = 24;    //Random ASCII character string part length     //ランダムなアスキー文字列部分の長さ

        //Generate random SessionID string
        internal static string GenerateSessionID()
        {
            string s = "";
            for (int i = 0; i < ASCII_LENGTH; i++)
                s += ASCII.Substring(Random.Range(0, ASCII.Length), 1);
            return DateTime.Now.Ticks + " " + s;
        }




        /// <summary>
        /// Launch QR Code (Bar Code) Scanner to acquire text.
        /// Using ZXing ("Zebra Crossing") open source project (google). [ver.3.3.2]
        /// https://github.com/zxing/zxing
        /// (Apache License, Version 2.0)
        /// http://www.apache.org/licenses/LICENSE-2.0
        ///·Launch the QR Code Scanner application of ZXing and obtain the result text.
        ///·When cancellation or acquisition fails, it returns a empty character ("").
        ///·If ZXing's QR Code Scanner application is not in the device, a dialog prompting installation will be displayed.
        /// https://play.google.com/store/apps/details?id=com.google.zxing.client.android
        /// 
        /// 
        /// QRコード(バーコード)スキャナを起動してテキストを取得する。
        /// ZXing オープンソースプロジェクト（google）を利用。[ver.3.3.2]
        /// https://github.com/zxing/zxing
        /// (Apache License, Version 2.0)
        /// http://www.apache.org/licenses/LICENSE-2.0
        ///・ZXing の QRコードスキャナアプリを起動し、結果のテキストを取得する。
        ///・キャンセルまたは取得失敗したときなどは、空文字（""）を返す。
        ///・端末に ZXing の QRコードスキャナアプリが入ってない場合は、インストールを促すダイアログが表示される。
        /// https://play.google.com/store/apps/details?id=com.google.zxing.client.android
        /// </summary>
        /// <param name="callbackGameObject">コールバックするヒエラルキーの GameObject 名</param>
        /// <param name="callbackMethod">コールバックするメソッド名</param>
        public static void ShowQRCodeScanner(string callbackGameObject, string callbackMethod)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM);

            //Android の UIスレッド（メインスレッド）で動かす
            context.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                androidSystem.CallStatic(
                    "showQRCodeScannerZXing",
                    context,
                    callbackGameObject,
                    callbackMethod
                );
            }));
        }





        //==========================================================
        // Control of Hardware Volume settings

        /// <summary>
        /// Get Hardware Volume (Media volume only)
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_HardVolumeGetSet
        ///･0~max (max:15? Depends on system?)
        /// </summary>
        /// <returns>Current Hardware Volume</returns>
        public static int GetMediaVolume()
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<int>(
                            "getMediaVolume",
                            context
                        );
                    }
                }
            }
        }



        /// <summary>
        /// Get Maximum Hardware Volume (Media volume only)
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_HardVolumeGetSet
        ///･0~max (max:15? Depends on system?)
        /// </summary>
        /// <returns>Maximum Hardware Volume</returns>
        public static int GetMediaMaxVolume()
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<int>(
                            "getMediaMaxVolume",
                            context
                        );
                    }
                }
            }
        }



        /// <summary>
        /// Set Hardware Volume (Media volume only)
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_HardVolumeGetSet
        ///･0~max (max:15? Depends on system?)
        /// </summary>
        /// <param name="volume">new Hardware Volume</param>
        /// <param name="showUI">true=display system UI</param>
        /// <returns>Hardware Volume after setting</returns>
        public static int SetMediaVolume(int volume, bool showUI)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<int>(
                            "setMediaVolume",
                            context,
                            volume,
                            showUI
                        );
                    }
                }
            }
        }



        /// <summary>
        /// Add Hardware Volume (Media volume only)
        /// http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_HardVolumeGetSet
        ///･0~max (max:15? Depends on system?)
        /// </summary>
        /// <param name="addVol">Volume to be added</param>
        /// <param name="showUI">true=display system UI</param>
        /// <returns>Hardware Volume after addition</returns>
        public static int AddMediaVolume(int addVol, bool showUI)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass androidSystem = new AndroidJavaClass(ANDROID_SYSTEM))
                    {
                        return androidSystem.CallStatic<int>(
                            "addMediaVolume",
                            context,
                            addVol,
                            showUI
                        );
                    }
                }
            }
        }





        //========================================================================
        // Get Hardware button press event 
        //･Rename "AndroidManifest-FullPlugin~.xml" to "AndroidManifest.xml" when receive events of hardware volume button.
        //･Cache objects for Android access to monitor input.
        // Therefore, it is better to register the listener with "OnEnable()" and release it with "OnDisable()".
        // http://fantom1x.blog130.fc2.com/blog-entry-273.html#fantomPlugin_HardVolumeListener
        //========================================================================


        /// <summary>
        /// Get Hardware button press event 
        /// </summary>
        public static class HardKey
        {
            //Class full path of plug-in in Java
            public const string ANDROID_HARDKEY = ANDROID_PACKAGE + ".AndroidHardKey";


            //For Android's AndroidHardKey class acquisition
            private static AndroidJavaClass mAndroidHardKey;    //Cached Object

            private static AndroidJavaClass AndroidHardKey {
                get {
                    if (mAndroidHardKey == null)
                    {
                        mAndroidHardKey = new AndroidJavaClass(ANDROID_HARDKEY);
                    }
                    return mAndroidHardKey;
                }
            }


            /// <summary>
            /// Release cashed object
            ///･Also called "AndroidPlugin.Release()".
            /// </summary>
            public static void ReleaseCache()
            {
                if (mAndroidHardKey != null)
                {
                    mAndroidHardKey.Dispose();
                    mAndroidHardKey = null;
                }
            }


            /// <summary>
            /// Release all listeners
            ///･Also called "AndroidPlugin.Release()".
            /// </summary>
            public static void RemoveAllListeners()
            {
                RemoveKeyVolumeUpListener();
                RemoveKeyVolumeDownListener();
                ReleaseCache();
            }



            /// <summary>
            /// Register the callback (listener) when the hardware volume (media volume only) is increased.
            ///･Only one callback can be registered.
            /// </summary>
            /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
            /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
            /// <param name="resultValue">Return value when volume is raised</param>
            public static void SetKeyVolumeUpListener(string callbackGameObject, string callbackMethod, string resultValue)
            {
                AndroidHardKey.CallStatic(
                    "setKeyVolumeUpListener",
                    callbackGameObject,
                    callbackMethod,
                    resultValue
                );
            }



            /// <summary>
            /// Register the callback (listener) when the hardware volume (media volume only) is decreased.
            ///･Only one callback can be registered.
            /// </summary>
            /// <param name="callbackGameObject">GameObject name in hierarchy to callback</param>
            /// <param name="callbackMethod">Method name to callback (it is in GameObject)</param>
            /// <param name="resultValue">Return value when the volume is lowered</param>
            public static void SetKeyVolumeDownListener(string callbackGameObject, string callbackMethod, string resultValue)
            {
                AndroidHardKey.CallStatic(
                    "setKeyVolumeDownListener",
                    callbackGameObject,
                    callbackMethod,
                    resultValue
                );
            }



            /// <summary>
            /// Release the callback (listener) when the hardware volume (media volume only) is increased.
            ///･Also called "AndroidPlugin.Release()".
            /// </summary>
            public static void RemoveKeyVolumeUpListener()
            {
                AndroidHardKey.CallStatic(
                    "releaseKeyVolumeUpListener"
                );
            }



            /// <summary>
            /// Release the callback (listener) when the hardware volume (media volume only) is decreased.
            ///･Also called "AndroidPlugin.Release()".
            /// </summary>
            public static void RemoveKeyVolumeDownListener()
            {
                AndroidHardKey.CallStatic(
                    "releaseKeyVolumeDownListener"
                );
            }



            /// <summary>
            /// Set whether to control media volume by the smartphone itself by hardware buttons.
            ///･To disable the volume buttons on the smartphone, set it to 'false' if you want to operate only from the Unity side.
            ///･It is possible to receive the event and volume control with "SetMediaVolume()" or "AddMediaVolume()".
            /// </summary>
            /// <param name="enable">true=Volume operation on smartphone / false=Disable volume operation at the smartphone</param>
            public static void SetVolumeOperation(bool enable)
            {
                AndroidHardKey.CallStatic(
                    "setVolumeOperation",
                    enable
                );
            }


        }

    }
#endif



    //==========================================================
    // Item (widget) instance parameters class for Custom Dialog

    //Types of items (widgets)
    [Serializable]
    public enum DialogItemType
    {
        Divisor,
        Text,       //Android-TextView
        Switch,     //Android-Switch
        Slider,     //Android-SeekBar
        Toggle,     //Android-RadioButton
    }

    //Base class for parameters of each item
    [Serializable]
    public abstract class DialogItem
    {
        public string type;         //Types of items (widgets)
        public string key = "";     //Key to be associated with return value

        public DialogItem() { }

        public DialogItem(DialogItemType type, string key = "")
        {
            this.type = type.ToString();
            this.key = key;
        }

        public DialogItem Clone()
        {
            return (DialogItem)MemberwiseClone();
        }
    }

    //Parameters for Dividing Line
    [Serializable]
    public class DivisorItem : DialogItem
    {
        public float lineHeight = 1;    //Line width (unit: dp)
        public int lineColor = 0;       //Line color (0 = not specified: Color format is int32 (AARRGGBB: Android-Java))

        public DivisorItem(float lineHeight, int lineColor = 0)
            : base(DialogItemType.Divisor)
        {
            this.lineHeight = lineHeight;
            this.lineColor = lineColor;
        }

        //Unity.Color overload
        public DivisorItem(float lineHeight, Color lineColor)
            : this(lineHeight, XColor.ToIntARGB(lineColor)) { }

        public new DivisorItem Clone()
        {
            return (DivisorItem)MemberwiseClone();
        }
    }

    //Parameters for Text
    [Serializable]
    public class TextItem : DialogItem
    {
        public string text = "";        //Text string
        public int textColor = 0;       //Text color (0 = not specified: Color format is int32 (AARRGGBB: Android-Java))
        public int backgroundColor = 0; //Background color (0 = not specified: Color format is int32 (AARRGGBB: Android-Java))
        public string align = "";       //Text alignment ("" = not specified, "center", "right" or "left")

        public TextItem(string text, int textColor = 0, int backgroundColor = 0, string align = "")
            : base(DialogItemType.Text)
        {
            this.text = text;
            this.textColor = textColor;
            this.backgroundColor = backgroundColor;
            this.align = align;
        }

        //Unity.Color overload
        public TextItem(string text, Color textColor, Color backgroundColor, string align = "")
            : this(text, XColor.ToIntARGB(textColor), XColor.ToIntARGB(backgroundColor), align) { }

        public TextItem(string text, Color textColor)
            : this(text, XColor.ToIntARGB(textColor), 0, "") { }

        public new TextItem Clone()
        {
            return (TextItem)MemberwiseClone();
        }
    }

    //Parameters for Switch
    [Serializable]
    public class SwitchItem : DialogItem
    {
        public string text = "";        //Text string
        public bool defChecked;         //Initial state of switch (true = On / false = Off)
        public int textColor = 0;       //Text color (0 = not specified: Color format is int32 (AARRGGBB: Android-Java))
        public string changeCallbackMethod = ""; //Method name to real-time callback when the value of the switch is changed (it is in GameObject)

        public SwitchItem(string text, string key, bool defChecked, int textColor = 0, string changeCallbackMethod = "")
            : base(DialogItemType.Switch, key)
        {
            this.text = text;
            this.defChecked = defChecked;
            this.textColor = textColor;
            this.changeCallbackMethod = changeCallbackMethod;
        }

        //Unity.Color overload
        public SwitchItem(string text, string key, bool defChecked, Color textColor, string changeCallbackMethod = "")
            : this(text, key, defChecked, XColor.ToIntARGB(textColor), changeCallbackMethod) { }

        public new SwitchItem Clone()
        {
            return (SwitchItem)MemberwiseClone();
        }
    }

    //Parameters for Slider
    [Serializable]
    public class SliderItem : DialogItem
    {
        public string text = "";        //Text string
        public float value;             //Initial value
        public float min;               //Minimum value
        public float max;               //Maximum value
        public int digit;               //Number of decimal places (0 = integer, 1~3 = after decimal point)
        public int textColor = 0;       //Text color (0 = not specified: Color format is int32 (AARRGGBB: Android-Java))
        public string changeCallbackMethod = ""; //Method name to real-time callback when the value of the slider is changed (it is in GameObject)

        public SliderItem(string text, string key, float value, float min = 0, float max = 100, int digit = 0, int textColor = 0, string changeCallbackMethod = "")
            : base(DialogItemType.Slider, key)
        {
            this.text = text;
            this.value = value;
            this.min = min;
            this.max = max;
            this.digit = digit;
            this.textColor = textColor;
            this.changeCallbackMethod = changeCallbackMethod;
        }

        //Unity.Color overload
        public SliderItem(string text, string key, float value, float min, float max, int digit, Color textColor, string changeCallbackMethod = "")
            : this(text, key, value, min, max, digit, XColor.ToIntARGB(textColor), changeCallbackMethod) { }

        public new SliderItem Clone()
        {
            return (SliderItem)MemberwiseClone();
        }
    }

    //Parameters for Toggle (Group)
    [Serializable]
    public class ToggleItem : DialogItem
    {
        public string[] items;          //Item strings (Array)
        public string[] values;         //Value for each item (Array)
        public string defValue = "";    //Initial value ([*]Prioritize checkedItem)
        public int checkedItem;         //Initial index (nothing defValue or not found defValue)
        public int textColor = 0;       //Text color (0 = not specified: Color format is int32 (AARRGGBB: Android-Java))
        public string changeCallbackMethod = ""; //Method name to real-time callback when the value of the toggle is changed (it is in GameObject)

        //Initial value constructor
        public ToggleItem(string[] items, string key, string[] values, string defValue, int textColor = 0, string changeCallbackMethod = "")
            : base(DialogItemType.Toggle, key)
        {
            this.items = items;
            this.values = values;
            this.defValue = defValue;
            this.textColor = textColor;
            this.changeCallbackMethod = changeCallbackMethod;
        }

        //Initial index constructor
        public ToggleItem(string[] items, string key, string[] values, int checkedItem, int textColor = 0, string changeCallbackMethod = "")
            : base(DialogItemType.Toggle, key)
        {
            this.items = items;
            this.values = values;
            this.checkedItem = checkedItem;
            this.textColor = textColor;
            this.changeCallbackMethod = changeCallbackMethod;
        }

        //Unity.Color overload
        public ToggleItem(string[] items, string key, string[] values, string defValue, Color textColor, string changeCallbackMethod = "")
            : this(items, key, values, defValue, XColor.ToIntARGB(textColor), changeCallbackMethod) { }

        public ToggleItem(string[] items, string key, string[] values, int checkedItem, Color textColor, string changeCallbackMethod = "")
            : this(items, key, values, checkedItem, XColor.ToIntARGB(textColor), changeCallbackMethod) { }

        public new ToggleItem Clone()
        {
            ToggleItem obj = (ToggleItem)MemberwiseClone();
            obj.items = (string[])items.Clone();
            obj.values = (string[])values.Clone();
            return obj;
        }
    }


    //==========================================================
    // For data acquisition

    //For image information
    [Serializable]
    public class ImageInfo
    {
        public string path = "";
        public int width;
        public int height;

        public ImageInfo() { }

        public ImageInfo(string path, int width, int height) {
            this.path = path;
            this.width = width;
            this.height = height;
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);    //for debug
        }
    }

    //For battery status information
    [Serializable]
    public class BatteryInfo
    {
        public string timestamp;    //Time when information was obtained.           //情報を取得した時刻
        public int level;           //The remaining battery capacity.               //残量
        public int scale;           //Maximum amount of battery.                    //最大量
        public int percent;         //％（level/scale*100）(= UnityEngine.SystemInfo.batteryLevel*100)
        public string status;       //Charge state (= UnityEngine.BatteryStatus)    //充電状態を表す
        public string health;       //Battery condition.                            //コンディションを表す
        public float temperature;   //Battery temperature (℃).                     //バッテリ温度（℃）

        public override string ToString()
        {
            return JsonUtility.ToJson(this);    //for debug
        }
    }


    //For Confirm Device Credentials status
    //認証ステータス用
    [Serializable]
    public class CredentialInfo
    {
        public string status;      //Result status
        public string sessionID;   //Session ID (string)
    }




    //==========================================================
    // Contant Values etc.
    // 定数など

    /// <summary>
    /// Sensor Type constant (* value can not be changed)
    ///･Basically the type of sensor is defined as 'int' type, and the return value is defined as 'float[]'.
    /// If there is a newly added sensor that is not here, it seems that if you add an ID to 'SensorType' you can use it as it is (Except for TriggerEvent type).
    ///·Because plugin loads native calls, it seems better if it can be used to 'Input.acceleration', '.gyro' etc., by Unity built-in.
    ///(Sensor Type)
    /// https://developer.android.com/reference/android/hardware/Sensor.html#TYPE_ACCELEROMETER
    ///(Sensor Values)
    /// https://developer.android.com/reference/android/hardware/SensorEvent.html#values
    ///(Using Google Play filters to target specific sensor configurations)
    /// https://developer.android.com/guide/topics/sensors/sensors_overview.html#sensors-configs
    ///(Sensors Overview)
    /// https://developer.android.com/guide/topics/sensors/sensors_overview.html
    ///(API Level)
    /// https://developer.android.com/guide/topics/manifest/uses-sdk-element.html#ApiLevels
    /// 
    /// 
    /// センサー種類定数（※値の変更は不可）
    ///・基本的にセンサーの種類は"int型"で定義されており、戻り値は"float[]"で定義されている。
    /// ここには無い新しく追加されたセンサーがあった場合、"SensorType"にIDを追加すればそのまま利用できる場合がある（TriggerEvent タイプを除く）。
    ///・プラグインではネイティブ呼び出しの負荷がかかるため、Input.acceleration, .gyro 等、Unity で取得できるものは、そちらの方が良いと思われる。
    ///(センサーの種類)
    /// https://developer.android.com/reference/android/hardware/Sensor.html#TYPE_ACCELEROMETER
    ///(センサーの戻り値)
    /// https://developer.android.com/reference/android/hardware/SensorEvent.html#values
    ///(Google Play などには利用できるセンサーでフィルタリングした方が良い)
    /// https://developer.android.com/guide/topics/sensors/sensors_overview.html#sensors-configs
    ///(Sensors Overview)
    /// https://developer.android.com/guide/topics/sensors/sensors_overview.html
    ///(API Level)
    /// https://developer.android.com/guide/topics/manifest/uses-sdk-element.html#ApiLevels
    /// </summary>
    [Serializable]
    public enum SensorType
    {
        None = 0,                           //(dummy, not set)
        Accelerometer = 1,                  //[m/s^2] (≒ Input.acceleration * -10)              //加速度
        MagneticField = 2,                  //[uT] (= Input.compass.rawVector)                   //磁気
        Orientation = 3,                    //[degree] (deprecated in API level 20）             //方向 [度] (※API 20 で廃止）
        Gyroscope = 4,                      //[rad/s]                                            //角速度
        Light = 5,                          //[lux]                                              //照度
        Pressure = 6,                       //[hPa]                                              //気圧  
        Proximity = 8,                      //[cm]                                               //近接 
        Gravity = 9,                        //[m/s^2]                                            //重力
        LinearAcceleration = 10,            //[m/s^2] (= Accelerometer - Gravity)                //線形加速度
        RotationVector = 11,                //[vector] (*[0]~[2]:API 9, [3][4]:API 18 or higher) //デバイスの回転ベクトル
        RelativeHumidity = 12,              //[%] (*API 20 or higher)                            //湿度
        AmbientTemperature = 13,            //[℃]                                               //気温
        MagneticFieldUncalibrated = 14,     //[uT] (*API 18 or higher)                           //未較正の磁気
        GameRotationVector = 15,            //[vector] (*API 18 or higher)                       //地磁気を使用しない回転ベクトル
        GyroscopeUncalibrated = 16,         //[rad/s] (*API 18 or higher)                        //未較正の角速度
        SignificantMotion = 17,             //[1 only] (*API 18 or higher)                       //動作継続トリガ
        StepDetector = 18,                  //[1 only] (*API 19 or higher)                       //歩行トリガ
        StepCounter = 19,                   //[steps (system boot)] (*API 19 or higher)          //歩数 [通算歩数]
        GeomagneticRotationVector = 20,     //[vector] (*API 19 or higher)                       //地磁気の回転ベクトル
        HeartRate = 21,                     //[bpm](*API 20 or higher. Required permission：'android.permission.BODY_SENSORS') //毎分の心拍数
        Pose6DOF = 28,                      //[quaternion, translation] (*API 24 or higher)      //デバイスポーズ
        StationaryDetect = 29,              //[1 only] (*API 24 or higher)                       //静止検出トリガ
        MotionDetect = 30,                  //[1 only] (*API 24 or higher)                       //動作検出トリガ
        HeartBeat = 31,                     //[confidence=0~1] (*API 24 or higher)               //心拍ピーク検出
        LowLatencyOffbodyDetect = 34,       //[0 (device is off-body) or 1 (device is on-body)] (*API 26 or higher)  //デバイス装着検出
        AccelerometerUncalibrated = 35,     //[m/s^2] (*API 26 or higher)                        //未較正の加速度
    }

    /// <summary>
    /// Sensor detection speed constant (* value can not be changed)
    ///·Normal is recommended, since plug-in loads native calls and return value conversion.
    ///(*) In fact, it is received 'faster' (depending on the type of sensor).
    ///(Sensor Delay)
    /// https://developer.android.com/reference/android/hardware/SensorManager.html#SENSOR_DELAY_FASTEST
    /// 
    /// 
    /// センサーの検出速度定数（※値の変更は不可）
    ///・プラグインではネイティブ呼び出し・戻り値の変換の負荷がかかるため、Normal を推奨。
    ///※実際には「より速く」受信される（センサーの種類による）。
    ///(センサー速度)
    /// https://developer.android.com/reference/android/hardware/SensorManager.html#SENSOR_DELAY_FASTEST
    /// </summary>
    [Serializable]
    public enum SensorDelay
    {
        Fastest = 0,    //0ms [*Not recommended as it will result in high load]
        Game    = 1,    //20ms (50fps)
        UI      = 2,    //66.6ms (15fps)
        Normal  = 3,    //200ms (5fps) [*Recommended]
    }

    /// <summary>
    /// For acquisition sensor value
    /// 
    /// センサーの戻り値取得用
    /// </summary>
    [Serializable]
    public class SensorInfo
    {
        public int type;
        public float[] values;
    }

    /// <summary>
    /// General constants of sensor values
    /// 
    /// センサー値の一般的な定数
    /// 
    /// https://developer.android.com/reference/android/hardware/SensorManager.html#GRAVITY_DEATH_STAR_I
    /// </summary>
    public class SensorConstant
    {
        public class Light
        {
            // Maximum luminance of sunlight in lux
            public static readonly float SunlightMax  = 120000.0f;
            // luminance of sunlight in lux
            public static readonly float Sunlight     = 110000.0f;
            // luminance in shade in lux
            public static readonly float Shade        = 20000.0f;
            // luminance under an overcast sky in lux
            public static readonly float Overcast     = 10000.0f;
            // luminance at sunrise in lux
            public static readonly float Sunrise      = 400.0f;
            // luminance under a cloudy sky in lux
            public static readonly float Cloudy       = 100.0f;
            // luminance at night with full moon in lux
            public static readonly float Fullmoon     = 0.25f;
            // luminance at night with no moon in lux
            public static readonly float NoMoon       = 0.001f;
        }

        public class Gravity
        {
            // Standard gravity (g) on Earth. This value is equivalent to 1G
            public static readonly float Standard          = 9.80665f;
            // Sun's gravity in SI units (m/s^2)
            public static readonly float Sun               = 275.0f;
            // Mercury's gravity in SI units (m/s^2)
            public static readonly float Mercury           = 3.70f;
            // Venus' gravity in SI units (m/s^2)
            public static readonly float Venus             = 8.87f;
            // Earth's gravity in SI units (m/s^2)
            public static readonly float Earth             = 9.80665f;
            // The Moon's gravity in SI units (m/s^2)
            public static readonly float Moon              = 1.6f;
            // Mars' gravity in SI units (m/s^2)
            public static readonly float Mars              = 3.71f;
            // Jupiter's gravity in SI units (m/s^2)
            public static readonly float Jupiter           = 23.12f;
            // Saturn's gravity in SI units (m/s^2)
            public static readonly float Saturn            = 8.96f;
            // Uranus' gravity in SI units (m/s^2)
            public static readonly float Uranus            = 8.69f;
            // Neptune's gravity in SI units (m/s^2)
            public static readonly float Neptune           = 11.0f;
            // Pluto's gravity in SI units (m/s^2)
            public static readonly float Pluto             = 0.6f;
            // Gravity (estimate) on the first Death Star in Empire units (m/s^2)
            public static readonly float DeathStarInEmpire = 0.000000353036145f;
            // Gravity on the island
            public static readonly float TheIsland         = 4.815162342f;
        }

        public class MagneticField
        {
            // Maximum magnetic field on Earth's surface
            public static readonly float EarthMax = 60.0f;
            // Minimum magnetic field on Earth's surface
            public static readonly float EarthMin = 30.0f;
        }

        public class Pressure
        {
            // Standard atmosphere, or average sea-level pressure in hPa (millibar)
            public static readonly float StandardAtmosphere = 1013.25f;
        }
    }



    /// <summary>
    /// For 'action' input support
    ///(*) Actions have a request API Level, and some are available depending on the model of the device, others are not (sometimes it is duplicated).
    ///(*) You can add something that is not here.
    /// 
    /// アクション文字列の入力支援用
    ///※アクションは要求API Levelがあり、デバイスのモデルによっても利用できるものとそうでないものがある（重複してる場合もある）。
    ///※ここに無いものを追加しても構わない。
    /// </summary>
    [Serializable]
    public class ActionString
    {
        public static readonly string None = "(None)";   //default for display

        public static readonly string[] ConstantValues =
        {
            None, //dummy, not set. (*Do not change index:[0])

            //https://developer.android.com/reference/android/content/Intent.html#ACTION_VIEW
            "android.intent.action.VIEW",
            "android.intent.action.EDIT",
            "android.intent.action.WEB_SEARCH",
            "android.intent.action.SEND",
            "android.intent.action.SENDTO",
            "android.intent.action.CALL_BUTTON",
            "android.intent.action.DIAL",
            "android.intent.action.SET_WALLPAPER",
            "android.intent.action.MANAGE_NETWORK_USAGE",
            "android.intent.action.POWER_USAGE_SUMMARY",

            //https://developer.android.com/reference/android/provider/Settings.html#ACTION_ACCESSIBILITY_SETTINGS
            "android.settings.ACCESSIBILITY_SETTINGS",
            "android.settings.ADD_ACCOUNT_SETTINGS",
            "android.settings.AIRPLANE_MODE_SETTINGS",
            "android.settings.APN_SETTINGS",
            "android.settings.APPLICATION_DETAILS_SETTINGS",
            "android.settings.APPLICATION_DEVELOPMENT_SETTINGS",
            "android.settings.APPLICATION_SETTINGS",
            "android.settings.APP_NOTIFICATION_SETTINGS",   //API 26
            "android.settings.BATTERY_SAVER_SETTINGS",      //API 22
            "android.settings.BLUETOOTH_SETTINGS",
            "android.settings.CAPTIONING_SETTINGS",         //API 19
            "android.settings.CAST_SETTINGS",               //API 21
            "android.settings.CHANNEL_NOTIFICATION_SETTING",//API 26
            "android.settings.DATA_ROAMING_SETTINGS",
            "android.settings.DATE_SETTINGS",
            "android.settings.DEVICE_INFO_SETTINGS",
            "android.settings.DEVICE_INFO_SETTINGS",
            "android.settings.DREAM_SETTINGS",              //API 18
            "android.settings.HARD_KEYBOARD_SETTINGS",      //API 24
            "android.settings.HOME_SETTINGS",               //API 21
            "android.settings.IGNORE_BACKGROUND_DATA_RESTRICTIONS_SETTINGS",//API 24
            "android.settings.IGNORE_BATTERY_OPTIMIZATION_SETTINGS",        //API 23
            "android.settings.INPUT_METHOD_SETTINGS",
            "android.settings.INPUT_METHOD_SUBTYPE_SETTINGS",
            "android.settings.INTERNAL_STORAGE_SETTINGS",
            "android.settings.LOCALE_SETTINGS",
            "android.settings.LOCATION_SOURCE_SETTINGS",
            "android.settings.MANAGE_ALL_APPLICATIONS_SETTINGS",
            "android.settings.MANAGE_APPLICATIONS_SETTINGS",
            "android.settings.MANAGE_DEFAULT_APPS_SETTINGS",    //API 24
            "android.settings.action.MANAGE_OVERLAY_PERMISSION",//API 23
            "android.settings.MANAGE_UNKNOWN_APP_SOURCES",      //API 26
            "android.settings.action.MANAGE_WRITE_SETTINGS",    //API 23
            "android.settings.MEMORY_CARD_SETTINGS",
            "android.settings.NETWORK_OPERATOR_SETTINGS",
            "android.settings.NFCSHARING_SETTINGS",
            "android.settings.NFC_PAYMENT_SETTINGS",        //API 19
            "android.settings.NFC_SETTINGS",
            "android.settings.NIGHT_DISPLAY_SETTINGS",      //API 26
            "android.settings.ACTION_NOTIFICATION_LISTENER_SETTINGS",   //API 22
            "android.settings.NOTIFICATION_POLICY_ACCESS_SETTINGS",     //API 23
            "android.settings.ACTION_PRINT_SETTINGS",       //API 19
            "android.settings.PRIVACY_SETTINGS",
            "android.settings.QUICK_LAUNCH_SETTINGS",
            "android.search.action.SEARCH_SETTINGS",
            "android.settings.SECURITY_SETTINGS",
            "android.settings.SETTINGS",
            "android.settings.SHOW_REGULATORY_INFO",        //API 21
            "android.settings.SOUND_SETTINGS",
            "android.settings.SYNC_SETTINGS",
            "android.settings.USAGE_ACCESS_SETTING",        //API 21
            "android.settings.USER_DICTIONARY_SETTINGS",
            "android.settings.VOICE_INPUT_SETTINGS",        //API 21
            "android.settings.VPN_SETTINGS",                //API 24
            "android.settings.VR_LISTENER_SETTINGS",        //API 24
            "android.settings.WEBVIEW_SETTINGS",            //API 24
            "android.settings.WIFI_IP_SETTINGS",
            "android.settings.WIFI_SETTINGS",
            "android.settings.WIRELESS_SETTINGS",
            "android.settings.ZEN_MODE_PRIORITY_SETTINGS",  //API 26
        };
    }

}

