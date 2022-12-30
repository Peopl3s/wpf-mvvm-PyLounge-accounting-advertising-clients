using System.Data.Common;

namespace WPFRJCode.Extensions
{
    /// <summary>
    /// Расширение для класса DbCommand
    /// </summary>
    public static class ParameterExtension
    {
        /// <summary>
        /// Добавляет параметр parameterName со знаничением parameterValue в команду БД command
        /// </summary>
        public static void AddParameterWithValue<T>(this DbCommand command, string parameterName, T parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            command.Parameters.Add(parameter);
        }
    }
}
