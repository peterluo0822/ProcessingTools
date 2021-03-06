﻿namespace ProcessingTools.Tagger.Commands.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using ProcessingTools.Tagger.Commands.Contracts.Commands;

    public static class CommandExtensions
    {
        public static string GetDescriptionMessageForCommand(this ITaggerCommand command)
        {
            var type = command.GetType();

            return $"\n\t{type.GetDescriptionMessageForCommand()}\n";
        }

        public static string GetDescriptionMessageForCommand(this Type type)
        {
            string message = type.GetCustomAttribute<DescriptionAttribute>(false)?.Description;

            if (string.IsNullOrWhiteSpace(message))
            {
                var name = Regex.Replace((string)type.FullName, @".*?([^\.]+)\Z", "$1");
                name = Regex.Replace(name, @"Command\Z", string.Empty);

                message = Regex.Replace(name, "(?=[A-Z])", " ").Trim();
            }

            return message;
        }
    }
}
