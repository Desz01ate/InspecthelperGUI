﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InspecthelperGUI.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("InspecthelperGUI.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @echo off
        ///if %1.==. GOTO ErrHandler
        ///taskkill -f -im %1.exe &gt; null
        ///GOTO :End
        ///:ErrHandler
        ///echo Invalid or missing parameter.
        ///:End
        ///exit /b.
        /// </summary>
        internal static string KillBatchCommand {
            get {
                return ResourceManager.GetString("KillBatchCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] nvidiaInspector {
            get {
                object obj = ResourceManager.GetObject("nvidiaInspector", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] nvidiaProfileInspector {
            get {
                object obj = ResourceManager.GetObject("nvidiaProfileInspector", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #header &quot;please do a &apos;pip install psutil&apos; as it should solve most problem&quot;
        ///try:
        ///	import os.path,os,sys,time
        ///	import psutil
        ///	import thread
        ///	from gtts import gTTS #google text-to-speech api
        ///	import playsound #mp3 player
        ///except:
        ///	os.system(&quot;color 04&quot;) #RED
        ///	for i in range(5):
        ///		print(&quot;Some modules are missing. Please restart inspecthelper after the modules installation&quot;)
        ///		time.sleep(0.5)
        ///	os.system(&quot;color 07&quot;) #DEFAULT 
        ///	os.system(&quot;C:\Python27\Scripts\pip.exe install psutil&quot;)
        ///	os.system(&quot;C:\Pyth [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Pysrc {
            get {
                return ResourceManager.GetString("Pysrc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to C:\Windows\_CustomCommand\inspecthelper.settings.
        /// </summary>
        internal static string SettngsFilePath {
            get {
                return ResourceManager.GetString("SettngsFilePath", resourceCulture);
            }
        }
    }
}
