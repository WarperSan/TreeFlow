using TreeFlow.Core.Interfaces;

namespace TreeFlow.Extensions
{
    public static class INodeExtension
    {
        /// <inheritdoc cref="INode.ReadFromContext(string)"/>
        /// <param name="node">Node to start reading from</param>
        /// <param name="key"><inheritdoc cref="INode.ReadFromContext(string)"/></param>
        /// <typeparam name="T">Type of the value to read</typeparam>
        /// <remarks>
        /// If the value found is not an instance of <see cref="T"/>, <c>default(T)</c> will be returned
        /// </remarks>
        public static T ReadFromContext<T>(this INode node, string key)
        {
            var value = node.ReadFromContext(key);

            if (value is T typedValue)
                return typedValue;

            return default;
        }
    }
}