﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Localization.GameSession {
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
    public class GameSessionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal GameSessionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Localization.GameSession.GameSessionMessages", typeof(GameSessionMessages).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Код подключения должен быть не пустым.
        /// </summary>
        public static string JoinCodeEmpty {
            get {
                return ResourceManager.GetString("JoinCodeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось подключиться к игре.
        /// </summary>
        public static string JoiningError {
            get {
                return ResourceManager.GetString("JoiningError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сейчас не ваша очередь делать ход.
        /// </summary>
        public static string NotYourTurn {
            get {
                return ResourceManager.GetString("NotYourTurn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У вас уже есть активная игра.
        /// </summary>
        public static string SessionAlreadyCreated {
            get {
                return ResourceManager.GetString("SessionAlreadyCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось создать игру.
        /// </summary>
        public static string SessionFailedAtCreation {
            get {
                return ResourceManager.GetString("SessionFailedAtCreation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to В этой игре уже достаточно участников.
        /// </summary>
        public static string SessionIsFull {
            get {
                return ResourceManager.GetString("SessionIsFull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Игра не найдена.
        /// </summary>
        public static string SessionNotFound {
            get {
                return ResourceManager.GetString("SessionNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы уже находитесь в этой игре.
        /// </summary>
        public static string YouAlreadyJoined {
            get {
                return ResourceManager.GetString("YouAlreadyJoined", resourceCulture);
            }
        }
    }
}
