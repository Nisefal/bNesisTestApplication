using System;

/// <summary>
/// To clasify exceptions
/// </summary>
namespace TestTask.Exceptions
{
    /// <summary>
    /// thrown when static variable is null
    /// </summary>
    public class StaticVariableNotInitializedException : Exception
    {
        new public readonly string Message;

        public StaticVariableNotInitializedException()
        {
            Message = "Some static variables were not initialized";
        }
    }


    internal class NotCreatedException : Exception
    {
        public NotCreatedException()
        {
        }

        public NotCreatedException(string message) : base(message)
        {
        }

    }
}
