namespace CodeHub.Web.Infrastructure.Filters
{
    using System;
    using System.ComponentModel.DataAnnotations;
    
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UserNameAllowedSymbolsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var username = value as string;

            return username != null && this.ValidateSymbolsInUsername(username);
        }

        private bool ValidateSymbolsInUsername(string username)
        {
            for (int i = 0; i < username.Length; i++)
            {
                if (!char.IsLetterOrDigit(username[i]) && username[i] != '_')
                {
                    return false;
                }
            }

            return true;
        }
    }
}