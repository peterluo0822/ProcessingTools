﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProcessingTools.Tagger {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class XmlNodesSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static XmlNodesSettings defaultInstance = ((XmlNodesSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new XmlNodesSettings())));
        
        public static XmlNodesSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("institution")]
        public string BiorepositoriesInstitutionContentType {
            get {
                return ((string)(this["BiorepositoriesInstitutionContentType"]));
            }
            set {
                this["BiorepositoriesInstitutionContentType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("dwc:collectionCode")]
        public string BiorepositoriesCollectionCodeContentType {
            get {
                return ((string)(this["BiorepositoriesCollectionCodeContentType"]));
            }
            set {
                this["BiorepositoriesCollectionCodeContentType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("collection")]
        public string BiorepositoriesCollectionContentType {
            get {
                return ((string)(this["BiorepositoriesCollectionContentType"]));
            }
            set {
                this["BiorepositoriesCollectionContentType"] = value;
            }
        }
    }
}
