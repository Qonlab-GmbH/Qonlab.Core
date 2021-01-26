using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Qonlab.Core.ExtendedList;

namespace Qonlab.Core {
    public static class CollectionExtension {
        [DebuggerStepThrough]
        public static void ForEach<T>( this IEnumerable<T> source, Action<T> action ) {
            foreach ( var item in source )
                action( item );
        }

        [DebuggerStepThrough]
        public static SortedList<TKey, TSource> ToSortedList<TKey, TSource>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector ) {
            var list = new SortedList<TKey, TSource>();
            source.ToList().ForEach( i => list.Add( keySelector( i ), i ) );
            return list;
        }

        [DebuggerStepThrough]
        public static SortedList<T, T> ToSortedList<T>( this IEnumerable<T> source ) {
            var list = new SortedList<T, T>();
            source.ToList().ForEach( i => list.Add( i, i ) );
            return list;
        }

        [DebuggerStepThrough]
        public static TSource PickRandom<TSource>( this IEnumerable<TSource> source ) {
            var list = source.ToList();

            if ( list.Count == 0 )
                return default( TSource );

            if ( list.Count == 1 )
                return list[ 0 ];

            return list[ new Random().Next( list.Count ) ];
        }

        [DebuggerStepThrough]
        public static IEnumerable<List<T>> Partition<T>( this IList<T> source, int size ) {
            for ( int i = 0; i < Math.Ceiling( source.Count / ( double ) size ); i++ )
                yield return new List<T>( source.Skip( size * i ).Take( size ) );
        }

        [DebuggerStepThrough]
        public static List<TResult> SelectToList<TSource, TResult>( this IEnumerable<TSource> source, params Func<TSource, TResult>[] selectors ) {
            return source.Aggregate( new List<TResult>(), ( c, n ) => {
                selectors.ForEach( s => {
                    var v = s( n );
                    if ( typeof( TResult ).IsValueType || v != null )
                        c.Add( v );
                } );
                return c;
            } );
        }

        [DebuggerStepThrough]
        public static IList<TResult> Split<TSource, TResult>( this List<TSource> inputList, Func<IList<TSource>, TResult> resultSelector, int splitSize ) {
            var outputList = new List<TResult>();

            for ( int i = 0; i < inputList.Count; i += splitSize ) {
                var split = inputList.GetRange( i, Math.Min( splitSize, inputList.Count - i ) );
                var splitAsResult = resultSelector( split );
                outputList.Add( splitAsResult );
            }

            return outputList;
        }

        [DebuggerStepThrough]
        public static ExtendedList<TSource> ToExtendedList<TSource>( this IEnumerable<TSource> source ) {
            return new ExtendedList<TSource>( source );
        }

        [DebuggerStepThrough]
        public static HashSet<TKey> ToHashSet<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector ) {
            var hs = new HashSet<TKey>();

            foreach ( var item in source )
                hs.Add( keySelector( item ) );

            return hs;
        }

        [DebuggerStepThrough]
        public static HashSet<TKey> ToHashSet<TSource, TKey>( this IEnumerable<TSource> source, params Func<TSource, TKey>[] keySelectors ) {
            var hs = new HashSet<TKey>();

            foreach ( var key in source.SelectMany( item => keySelectors.Select( ks => ks( item ) ).Where( key => !hs.Contains( key ) ) ) )
                hs.Add( key );

            return hs;
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>( this IDictionary<TKey, TValue> d, Func<TValue, TValue> cloneValue = null ) {
            var cloned = d.ToDictionary( k => k.Key, v => cloneValue != null ? cloneValue( v.Value ) : v.Value );
            return cloned;
        }
    }
}
