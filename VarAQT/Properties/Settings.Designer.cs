﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VarAQT.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.3.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("127.0.0.1")]
        public string VaraCommandClientIP {
            get {
                return ((string)(this["VaraCommandClientIP"]));
            }
            set {
                this["VaraCommandClientIP"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8300")]
        public int VaraCommandClientPort {
            get {
                return ((int)(this["VaraCommandClientPort"]));
            }
            set {
                this["VaraCommandClientPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8100")]
        public int VaraKissClientPort {
            get {
                return ((int)(this["VaraKissClientPort"]));
            }
            set {
                this["VaraKissClientPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool VaraMonitorEnabled {
            get {
                return ((bool)(this["VaraMonitorEnabled"]));
            }
            set {
                this["VaraMonitorEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("127.0.0.1")]
        public string VaraMonitorCommandClientIP {
            get {
                return ((string)(this["VaraMonitorCommandClientIP"]));
            }
            set {
                this["VaraMonitorCommandClientIP"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8350")]
        public int VaraMonitorCommandClientPort {
            get {
                return ((int)(this["VaraMonitorCommandClientPort"]));
            }
            set {
                this["VaraMonitorCommandClientPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8100")]
        public int VaraMonitorKissClientPort {
            get {
                return ((int)(this["VaraMonitorKissClientPort"]));
            }
            set {
                this["VaraMonitorKissClientPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int BeaconInterval {
            get {
                return ((int)(this["BeaconInterval"]));
            }
            set {
                this["BeaconInterval"] = value;
            }
        }
    }
}
