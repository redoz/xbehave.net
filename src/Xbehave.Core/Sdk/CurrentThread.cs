namespace Xbehave.Sdk
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the currently executing thread.
    /// </summary>
    public static class CurrentThread
    {
        [ThreadStatic]
        private static List<IStepDefinition> stepDefinitions;

        /// <summary>
        /// Allows step definition for the currently executing thread.
        /// </summary>
        /// <returns>An object which disallows step definition for the currently executing thread.</returns>
        public static IDisposable AllowStepDefinition()
        {
            stepDefinitions = new List<IStepDefinition>();

            return new StepDefinitionDisallower();
        }

        /// <summary>
        /// Add a step definition to the currently executing thread.
        /// </summary>
        /// <param name="item">The step definition.</param>
        /// <exception cref="InvalidOperationException">Step definition is currently disallowed.</exception>
        public static void Add(IStepDefinition item)
        {
            if (stepDefinitions == null)
            {
                throw new InvalidOperationException("Step definition is currently disallowed.");
            }

            stepDefinitions.Add(item);
        }

        /// <summary>
        /// Gets the step definitions for the currently executing thread.
        /// </summary>
        public static IEnumerable<IStepDefinition> StepDefinitions =>
            stepDefinitions ?? Enumerable.Empty<IStepDefinition>();

        private sealed class StepDefinitionDisallower : IDisposable
        {
            public void Dispose() => stepDefinitions = null;
        }
    }
}
