﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace plugin.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.13.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string refreshToken {
            get {
                return ((string)(this["refreshToken"]));
            }
            set {
                this["refreshToken"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool separateTagBindings {
            get {
                return ((bool)(this["separateTagBindings"]));
            }
            set {
                this["separateTagBindings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("genres;mood")]
        public string recordingTagBindings {
            get {
                return ((string)(this["recordingTagBindings"]));
            }
            set {
                this["recordingTagBindings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("genres")]
        public string releaseTagBindings {
            get {
                return ((string)(this["releaseTagBindings"]));
            }
            set {
                this["releaseTagBindings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("genres")]
        public string releaseGroupTagBindings {
            get {
                return ((string)(this["releaseGroupTagBindings"]));
            }
            set {
                this["releaseGroupTagBindings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("synthpop;synth-pop\r\nuplifting trance;melodic trance\r\nalt-pop;alternative pop")]
        public string findReplace {
            get {
                return ((string)(this["findReplace"]));
            }
            set {
                this["findReplace"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool tagSubmitIsDestructive {
            get {
                return ((bool)(this["tagSubmitIsDestructive"]));
            }
            set {
                this["tagSubmitIsDestructive"] = value;
            }
        }
    }
}
