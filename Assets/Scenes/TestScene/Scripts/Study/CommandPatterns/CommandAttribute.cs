namespace TestScene.Study
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute : System.Attribute
    {
        public string CommandName { get; }
        public CommandAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }
}