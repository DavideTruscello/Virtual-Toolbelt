using Microsoft.Xaml.Behaviors;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Virtual_Toolbelt.Utilities
{
    //used in place of InvokeCommandAction to pass a parameter to the binded Command.
    //Since InvokeCommandAction is sealed and cannot be inherited from the class
    //inherits from InvokeCommandAction's parent and to work properly must replicate
    //InvokeCommandAction's behavior
    public class InteractiveCommand : TriggerAction<DependencyObject>
    {
        //executes the binded command with the parameter if resolved
        protected override void Invoke(object parameter)
        {
            if (base.AssociatedObject != null)
            {
                ICommand command = this.ResolveCommand();
                if ((command != null) && command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                }
            }
        }

        private ICommand ResolveCommand()
        {
            ICommand command = null;
            //verifies if the command is already setted
            if (this.Command != null)
            {
                return this.Command;
            }
            //if not sets it if it exists by looking all the the PropertyInfos of the AssociatedObject
            //and looks for an ICommand with the same name as the one specified in the binding. Once found
            //it is returned
            if (base.AssociatedObject != null)
            {
                foreach (PropertyInfo info in base.AssociatedObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (typeof(ICommand).IsAssignableFrom(info.PropertyType) && string.Equals(info.Name, this.CommandName, StringComparison.Ordinal))
                    {
                        command = (ICommand)info.GetValue(base.AssociatedObject, null);
                    }
                }
            }
            return command;
        }

        //to make it compatible with Freezable Objects
        private string commandName;
        public string CommandName
        {
            get
            {
                base.ReadPreamble();
                return this.commandName;
            }
            set
            {
                if (this.CommandName != value)
                {
                    base.WritePreamble();
                    this.commandName = value;
                    base.WritePostscript();
                }
            }
        }

        //access the DependencyProperty as ICommand
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        //holds the command inside a CommandProperty to allow binding
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(InteractiveCommand), new UIPropertyMetadata(null));
    }
}