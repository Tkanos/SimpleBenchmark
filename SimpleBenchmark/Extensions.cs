using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SimpleBenchmark
{
    internal static class Extensions
    {
        /// <summary>
        /// This does absolutely nothing, but 
        /// allows us to consume a property value without having
        /// a useless local variable. It should end up being JITted
        /// away completely, so have no effect on the results.
        /// </summary>
        /// <remarks>Borrowed from NodaTime benchmarks!! <see cref="https://code.google.com/p/noda-time/source/browse/src/NodaTime.Benchmarks/Framework/Extensions.cs" /></remarks>
        internal static void Consume<T>(this T value)
        {
        }

        /// <summary>
        /// Consumes as no inlining.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <remarks>see <see cref="http://stackoverflow.com/questions/9600965/what-does-this-attribute-do-methodimploptions-noinlining-or-what-is-inlining" /> </remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static T ConsumeNoInlining<T>(this T item)
        {
            return item;
        }

        // 
        /// <summary>
        /// Consumes the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dummy">The dummy.</param>
        /// <returns></returns>
        /// <remarks>Borrowed from LambdaMicrobenchmarking - <see cref="https://github.com/biboudis/LambdaMicrobenchmarking/blob/master/LambdaMicrobenchmarking/Compiler.cs#L11"/></remarks>
        static public T ConsumeValue<T>(this T dummy)
        {
            return dummy;
        }

    }
}
