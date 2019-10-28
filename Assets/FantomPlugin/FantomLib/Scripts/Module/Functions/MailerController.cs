using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FantomLib
{
    /// <summary>
    /// Mailer Controller
    /// </summary>
    public class MailerController : MonoBehaviour
    {
        //Inspector Settings
        public string mailAddress = "xxx@example.com";
        public string subject = "Title";                //mail title
        [Multiline] public string body = "Message";     //mail body


        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        //private void Update()
        //{

        //}


        //Show Mailer with local values
        public void Show()
        {
            //string uri = "mailto:" + mailAddress + "?subject=" + WWW.EscapeURL(subject)  + "&body=" + WWW.EscapeURL(body);
            string uri = "mailto:" + mailAddress;   //when using extra[], query[]
#if UNITY_EDITOR
            Debug.Log(name + ".Show : uri = " + uri);
#elif UNITY_ANDROID
            //AndroidPlugin.StartActionURI("android.intent.action.SENDTO", uri);
            string[] extra = { "android.intent.extra.SUBJECT", "android.intent.extra.TEXT" };
            string[] query = { subject, body };
            AndroidPlugin.StartActionURI("android.intent.action.SENDTO", uri, extra, query);
#endif
        }

        //Set values dynamically and show Mailer (current values will be overwritten)
        public void Show(string mailAddress)
        {
            this.mailAddress = mailAddress;
            Show();
        }

        //Set values dynamically and show Mailer (current values will be overwritten)
        public void Show(string mailAddress, string subject)
        {
            this.mailAddress = mailAddress;
            this.subject = subject;
            Show();
        }

        //Set values dynamically and show Mailer (current values will be overwritten)
        public void Show(string mailAddress, string subject, string body)
        {
            this.mailAddress = mailAddress;
            this.subject = subject;
            this.body = body;
            Show();
        }
    }
}
