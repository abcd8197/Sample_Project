namespace TestScene.Study
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class CommandManager : UnityEngine.MonoBehaviour
    {
        #region ## Singleton ##
        private static CommandManager sInstance = null;
        public static CommandManager Instance
        {
            get
            {
                if (sInstance == null)
                {
                    sInstance = new UnityEngine.GameObject("_Command").AddComponent<CommandManager>();
                    DontDestroyOnLoad(sInstance.gameObject);
                }

                return sInstance;
            }
        }
        #endregion

        private Dictionary<string, Action> commandDictionary = new Dictionary<string, Action>();

        public void RegisterCommand(string commandName, Action command)
        {
            if (!commandDictionary.ContainsKey(commandName))
                commandDictionary.Add(commandName, command);
        }

        public void ExecuteCommand(string commandName)
        {
            if (commandDictionary.TryGetValue(commandName, out Action action))
            {
                action?.Invoke();
            }
        }

        public void RegisterAllCommand(object obj)
        {
            var methods = obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(CommandAttribute), false);

                foreach (var attribute in attributes)
                {
                    var commandAttribute = attribute as CommandAttribute;
                    if (commandAttribute != null)
                    {
                        RegisterCommand(commandAttribute.CommandName, (Action)Delegate.CreateDelegate(typeof(Action), obj, method));
                    }
                }

            }

        }
    }
}