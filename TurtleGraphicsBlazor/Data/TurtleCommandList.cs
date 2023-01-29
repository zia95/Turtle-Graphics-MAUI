using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleGraphicsBlazor.Data
{
    public class TurtleCommandList
    {
        public enum CommandTypes
        {
            None,


            Forward,
            Backward,
            Repeat,
            PenColor,
            Left,
            Right,

            
            End,
            PenUp,
            PenDown,
        }
        public static bool CheckIfCommandRequiresParameter(CommandTypes commandType )
        {
            return commandType is >= CommandTypes.Forward and <= CommandTypes.Right;
        }

        public record TurtleCommand(CommandTypes Command, int Parameter)
        {
            public bool IsValid 
            { 
                get 
                {
                    if(Command is >= CommandTypes.Forward and <= CommandTypes.PenDown)
                    {
                        return (Parameter > 0 || !CheckIfCommandRequiresParameter(Command));
                    }
                    return false;
                } 
            }
        }

        public int Id { get; set; } = DefaultId;
        public const int DefaultId = -1;
        public string ListName { get; set; } = "unnamed";
        public TurtleCommandList(string name) => ListName = name;
        public List<TurtleCommand> Commands { get; init; } = new List<TurtleCommand>();

        public bool Add(CommandTypes Command, int Parameter = 0)
        {
            if(Parameter > 0 || !CheckIfCommandRequiresParameter(Command))
            {
                Commands.Add(new TurtleCommand(Command, Parameter));
                return true;
            }
            return false;
        }

        public bool AreAllRepeatCommandValid()
        {
            int t = 0;
            foreach(var command in Commands)
            {
                if (command.Command == CommandTypes.Repeat) t++;
                if(command.Command == CommandTypes.End) t--;

                if (t == 2) return false;
            }
            return t is 0;
        }


        public bool IsValid()
        {
            if(string.IsNullOrWhiteSpace(ListName)) return false;

            if(!AreAllRepeatCommandValid()) return false;

            foreach(var command in Commands) 
            {
                if(!command.IsValid) return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"Name:{ListName}, Items:{Commands.Count}";
        }
    }
}
