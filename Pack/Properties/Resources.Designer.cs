﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pack.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Pack.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 {
        ///  &quot;app&quot;: &quot;com.tencent.autoreply&quot;,
        ///  &quot;desc&quot;: &quot;&quot;,
        ///  &quot;view&quot;: &quot;autoreply&quot;,
        ///  &quot;ver&quot;: &quot;0.0.0.1&quot;,
        ///  &quot;prompt&quot;: &quot;[来自仙域的诏令]&quot;,
        ///  &quot;meta&quot;: {
        ///    &quot;metadata&quot;: {
        ///      &quot;title&quot;: &quot;游戏菜单&quot;,
        ///      &quot;buttons&quot;: [
        ///        {
        ///          &quot;slot&quot;: 1,
        ///          &quot;action_data&quot;: &quot;个人信息&quot;,
        ///          &quot;name&quot;: &quot;个人信息&quot;,
        ///          &quot;action&quot;: &quot;notify&quot;
        ///        },
        ///        {
        ///          &quot;slot&quot;: 1,
        ///          &quot;action_data&quot;: &quot;加入地图&quot;,
        ///          &quot;name&quot;: &quot;加入地图&quot;,
        ///          &quot;action&quot;: &quot;notify&quot;
        ///        }
        ///      ],
        ///      &quot;type&quot;: &quot;guest&quot;,
        ///      &quot; [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string JSON_QuickButton_Help {
            get {
                return ResourceManager.GetString("JSON_QuickButton_Help", resourceCulture);
            }
        }
    }
}
