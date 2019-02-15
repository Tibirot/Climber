using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Utility;

namespace SA.Android.Utilities
{
    public class AN_JavaBridge
    {

        private readonly Dictionary<string, AndroidJavaClass> m_classes = new Dictionary<string, AndroidJavaClass>();


        //--------------------------------------
        //  Initialization
        //--------------------------------------

        public AN_JavaBridge() {
            //Registring the message handler
            CallStatic("com.stansassets.core.utility.AN_UnityBridge", "RegisterMessageHandler");
        }


        //--------------------------------------
        //  Public Methods
        //--------------------------------------


        public void CallStatic(string javaClassName, string methodName, params object[] args) {
            var javaClass = GetJavaClass(javaClassName);

            List<object> argsuments = new List<object>();
            foreach (object p in args) {
                argsuments.Add(ConverObjectData(p));
            }

            LogCommunication(javaClassName, methodName, argsuments);

            if (Application.isEditor) { return; }
            javaClass.CallStatic(methodName, argsuments.ToArray());
        }

        public T CallStatic<T>(string javaClassName, string methodName, params object[] args) {
            var javaClass = GetJavaClass(javaClassName);

            List<object> argsuments = new List<object>();
            foreach (object p in args) {
                argsuments.Add(ConverObjectData(p));
            }


            LogCommunication(javaClassName, methodName, argsuments);

            if (Application.isEditor) { return default(T);}
            var result =  javaClass.CallStatic<T>(methodName, argsuments.ToArray());
            AN_Logger.LogCommunication("[Sync] Sent to Unity ->: " + result);
            return result;
        }

        public R CallStaticWithCallback<R,T>(string javaClassName, string methodName, Action<T> callback, params object[] args) {

            AndroidJavaClass javaClass = GetJavaClass(javaClassName);

            List<object> argsuments = new List<object>();

            foreach (object p in args) {
                argsuments.Add(ConverObjectData(p));
            }


            LogCommunication(javaClassName, methodName, argsuments);
            argsuments.Add(AN_MonoJavaCallback.ActionToJavaObject<T>(callback));

            if (Application.isEditor) { return default(R); }
            return javaClass.CallStatic<R>(methodName, argsuments.ToArray());
        }

        public void CallStaticWithCallback<T>(string javaClassName, string methodName, Action<T> callback, params object[] args) {

            AndroidJavaClass javaClass = GetJavaClass(javaClassName);

            List<object> argsuments = new List<object>();

            foreach(object p in args) {
                argsuments.Add(ConverObjectData(p));
            }


            LogCommunication(javaClassName, methodName, argsuments);
            argsuments.Add(AN_MonoJavaCallback.ActionToJavaObject<T>(callback));

            if (Application.isEditor) { return; }
            javaClass.CallStatic(methodName, argsuments.ToArray());
        }


        //--------------------------------------
        //  Private Methods
        //--------------------------------------

        private string LogArgsuments(List<object> argsuments) {
            string log = string.Empty;
            foreach(var p in argsuments) {
                if(log != string.Empty) {
                    log += " | ";
                }

                log += p.ToString();
            }

            return log;
        }

        private void LogCommunication(string className, string methodName, List<object> argsuments) {

            string strippedClassName = SA_PathUtil.GetExtension(className);
            strippedClassName = strippedClassName.Substring(1);
            string argsumentsLog = LogArgsuments(argsuments);
            if(!string.IsNullOrEmpty(argsumentsLog)) {
                argsumentsLog = " :: " + argsumentsLog;
            }
            AN_Logger.LogCommunication("Sent to Java -> " + strippedClassName + "." + methodName + argsumentsLog);
        }


        private object ConverObjectData(object param) {
            if (param.GetType().Equals(typeof(string))) {
                return  param.ToString();
            } else if (param.GetType().Equals(typeof(bool))) {
                return param;
            } else if (param.GetType().Equals(typeof(int))) {
                return param;
            } else if (param.GetType().Equals(typeof(long))) {
                return param;
            } else if (param.GetType().Equals(typeof(float))) {
                return param;
            } else if (param.GetType().Equals(typeof(Texture2D))) {
                return (param as Texture2D).ToBase64String();
            } else {
                return JsonUtility.ToJson(param);
            }
        }

        private AndroidJavaClass GetJavaClass(string javaClassName) {

            if (Application.isEditor) {
                return null;
            }

            if (m_classes.ContainsKey(javaClassName)) {
                return m_classes[javaClassName];
            } else {
                var javaClass = new AndroidJavaClass(javaClassName);
                m_classes.Add(javaClassName, javaClass);
                return javaClass;
            }
        }
    }
}