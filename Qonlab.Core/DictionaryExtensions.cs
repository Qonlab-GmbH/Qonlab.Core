using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Qonlab.Core {
    public static class DictionaryExtensions {
        [DebuggerStepThrough]
        public static TValue GetValueOrDefault<TKey, TValue>( this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue ) {
            TValue value;
            return dictionary.TryGetValue( key, out value ) ? value : defaultValue;
        }

        [DebuggerStepThrough]
        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TSource, TKey, TValue>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector ) {
            var d = new SortedDictionary<TKey, TValue>();

            foreach ( var item in source )
                d.Add( keySelector( item ), valueSelector( item ) );

            return d;
        }

        [DebuggerStepThrough]
        public static SortedDictionary<TKey, List<TSource>> ToSortedDictionaryList<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector ) {
            var d = new SortedDictionary<TKey, List<TSource>>();

            foreach ( var item in source ) {
                if ( !d.ContainsKey( keySelector( item ) ) )
                    d.Add( keySelector( item ), new List<TSource>() );
                d[ keySelector( item ) ].Add( item );
            }

            return d;
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, List<TSource>> ToDictionaryList<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector ) {
            var d = new Dictionary<TKey, List<TSource>>();

            foreach ( var item in source ) {
                if ( !d.ContainsKey( keySelector( item ) ) )
                    d.Add( keySelector( item ), new List<TSource>() );
                d[ keySelector( item ) ].Add( item );
            }

            return d;
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, List<TValue>> ToDictionaryList<TSource, TKey, TValue>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector ) {
            var d = new Dictionary<TKey, List<TValue>>();

            foreach ( var item in source ) {
                if ( !d.ContainsKey( keySelector( item ) ) )
                    d.Add( keySelector( item ), new List<TValue>() );
                d[ keySelector( item ) ].Add( valueSelector( item ) );
            }

            return d;
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, HashSet<TValue>> ToDictionaryHashSet<TSource, TKey, TValue>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector ) {
            var d = new Dictionary<TKey, HashSet<TValue>>();

            foreach ( var item in source ) {
                if ( !d.ContainsKey( keySelector( item ) ) )
                    d.Add( keySelector( item ), new HashSet<TValue>() );
                d[ keySelector( item ) ].Add( valueSelector( item ) );
            }

            return d;
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, Dictionary<TKey2, TValue>> ToDictionaryDictionary<TSource, TKey, TKey2, TValue>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TKey2> keySelector2, Func<TSource, TValue> valueSelector ) {
            var d = new Dictionary<TKey, Dictionary<TKey2, TValue>>();

            foreach ( var item in source ) {
                TKey key = keySelector( item );
                TKey2 key2 = keySelector2( item );

                if ( !d.ContainsKey( key ) )
                    d.Add( key, new Dictionary<TKey2, TValue>() );

                d[ key ].Add( key2, valueSelector( item ) );
            }

            return d;
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, Dictionary<TKey2, List<TValue>>> ToDictionaryDictionaryList<TSource, TKey, TKey2, TValue>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TKey2> keySelector2, Func<TSource, TValue> valueSelector ) {
            var d = new Dictionary<TKey, Dictionary<TKey2, List<TValue>>>();

            foreach ( var item in source ) {
                TKey key = keySelector( item );
                TKey2 key2 = keySelector2( item );

                if ( !d.ContainsKey( key ) )
                    d.Add( key, new Dictionary<TKey2, List<TValue>>() );
                if ( !d[ key ].ContainsKey( key2 ) )
                    d[ key ].Add( key2, new List<TValue>() );

                d[ key ][ key2 ].Add( valueSelector( item ) );
            }

            return d;

        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, TValue> ToMultipleKeyDictionary<TSource, TKey, TValue>( this IEnumerable<TSource> source, Func<TSource, List<TKey>> keySelector, Func<TSource, TValue> valueSelector ) {
            var d = new Dictionary<TKey, TValue>();

            foreach ( var item in source ) {
                List<TKey> keys = keySelector( item );

                TValue value = valueSelector( item );

                foreach ( var key in keys )
                    d[ key ] = value;
            }

            return d;
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, List<TValue>> ToMultipleKeyDictionaryList<TSource, TKey, TValue>( this IEnumerable<TSource> source, Func<TSource, List<TKey>> keySelector, Func<TSource, TValue> valueSelector ) {
            var d = new Dictionary<TKey, List<TValue>>();

            foreach ( var item in source ) {
                List<TKey> keys = keySelector( item );

                TValue value = valueSelector( item );

                foreach ( var key in keys ) {
                    if ( !d.ContainsKey( key ) )
                        d.Add( key, new List<TValue>() );

                    d[ key ].Add( value );
                }
            }

            return d;
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, Dictionary<TKey2, TValue>> ToMultipleKeyDictionaryDictionary<TSource, TKey, TKey2, TValue>( this IEnumerable<TSource> source, Func<TSource, List<TKey>> keySelector, Func<TSource, TKey2> keySelector2, Func<TSource, TValue> valueSelector ) {
            var d = new Dictionary<TKey, Dictionary<TKey2, TValue>>();

            foreach ( var item in source ) {
                List<TKey> keys = keySelector( item );
                TKey2 key2 = keySelector2( item );

                TValue value = valueSelector( item );

                foreach ( var key in keys ) {
                    if ( !d.ContainsKey( key ) )
                        d.Add( key, new Dictionary<TKey2, TValue>() );

                    d[ key ].Add( key2, value );
                }
            }

            return d;
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, Dictionary<TKey2, Dictionary<TKey3, List<TSource>>>> To3xDictionaryList<TSource, TKey, TKey2, TKey3>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TKey2> keySelector2, Func<TSource, TKey3> keySelector3 ) {
            return source.To3xDictionaryList( keySelector, keySelector2, keySelector3, s => s );
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, Dictionary<TKey2, Dictionary<TKey3, List<TValue>>>> To3xDictionaryList<TSource, TKey, TKey2, TKey3, TValue>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TKey2> keySelector2, Func<TSource, TKey3> keySelector3, Func<TSource, TValue> valueSelector ) {
            var d = new Dictionary<TKey, Dictionary<TKey2, Dictionary<TKey3, List<TValue>>>>();

            foreach ( var item in source ) {
                TKey key = keySelector( item );
                TKey2 key2 = keySelector2( item );
                TKey3 key3 = keySelector3( item );

                if ( !d.ContainsKey( key ) )
                    d.Add( key, new Dictionary<TKey2, Dictionary<TKey3, List<TValue>>>() );
                if ( !d[ key ].ContainsKey( key2 ) )
                    d[ key ].Add( key2, new Dictionary<TKey3, List<TValue>>() );
                if ( !d[ key ][ key2 ].ContainsKey( key3 ) )
                    d[ key ][ key2 ].Add( key3, new List<TValue>() );

                d[ key ][ key2 ][ key3 ].Add( valueSelector( item ) );
            }

            return d;
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, Dictionary<TKey2, Dictionary<TKey3, List<TSource>>>> To3xMultiKeyDictionaryList<TSource, TKey, TKey2, TKey3>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TKey2> keySelector2, Func<TSource, List<TKey3>> keySelector3 ) {
            return source.To3xMultiKeyDictionaryList( keySelector, keySelector2, keySelector3, s => s );
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, Dictionary<TKey2, Dictionary<TKey3, List<TValue>>>> To3xMultiKeyDictionaryList<TSource, TKey, TKey2, TKey3, TValue>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TKey2> keySelector2, Func<TSource, List<TKey3>> keySelector3, Func<TSource, TValue> valueSelector ) {
            var d = new Dictionary<TKey, Dictionary<TKey2, Dictionary<TKey3, List<TValue>>>>();

            foreach ( var item in source ) {
                TKey key = keySelector( item );
                TKey2 key2 = keySelector2( item );
                List<TKey3> key3s = keySelector3( item );

                if ( !d.ContainsKey( key ) )
                    d.Add( key, new Dictionary<TKey2, Dictionary<TKey3, List<TValue>>>() );
                if ( !d[ key ].ContainsKey( key2 ) )
                    d[ key ].Add( key2, new Dictionary<TKey3, List<TValue>>() );

                foreach ( var key3 in key3s ) {
                    if ( !d[ key ][ key2 ].ContainsKey( key3 ) )
                        d[ key ][ key2 ].Add( key3, new List<TValue>() );

                    d[ key ][ key2 ][ key3 ].Add( valueSelector( item ) );
                }
            }

            return d;
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, Dictionary<TKey2, Dictionary<TKey3, Dictionary<TKey4, List<TSource>>>>> To4xMultiKeyDictionaryList<TSource, TKey, TKey2, TKey3, TKey4>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TKey2> keySelector2, Func<TSource, TKey3> keySelector3, Func<TSource, List<TKey4>> keySelector4 ) {
            return source.To4xMultiKeyDictionaryList( keySelector, keySelector2, keySelector3, keySelector4, s => s );
        }

        [DebuggerStepThrough]
        public static Dictionary<TKey, Dictionary<TKey2, Dictionary<TKey3, Dictionary<TKey4, List<TValue>>>>> To4xMultiKeyDictionaryList<TSource, TKey, TKey2, TKey3, TKey4, TValue>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TKey2> keySelector2, Func<TSource, TKey3> keySelector3, Func<TSource, List<TKey4>> keySelector4, Func<TSource, TValue> valueSelector ) {
            var d = new Dictionary<TKey, Dictionary<TKey2, Dictionary<TKey3, Dictionary<TKey4, List<TValue>>>>>();

            foreach ( var item in source ) {
                TKey key = keySelector( item );
                TKey2 key2 = keySelector2( item );
                TKey3 key3 = keySelector3( item );
                List<TKey4> keys4 = keySelector4( item );

                if ( !d.ContainsKey( key ) )
                    d.Add( key, new Dictionary<TKey2, Dictionary<TKey3, Dictionary<TKey4, List<TValue>>>>() );
                if ( !d[ key ].ContainsKey( key2 ) )
                    d[ key ].Add( key2, new Dictionary<TKey3, Dictionary<TKey4, List<TValue>>>() );
                if ( !d[ key ][ key2 ].ContainsKey( key3 ) )
                    d[ key ][ key2 ].Add( key3, new Dictionary<TKey4, List<TValue>>() );
                foreach ( var key4 in keys4 ) {
                    if ( !d[ key ][ key2 ][ key3 ].ContainsKey( key4 ) )
                        d[ key ][ key2 ][ key3 ].Add( key4, new List<TValue>() );

                    d[ key ][ key2 ][ key3 ][ key4 ].Add( valueSelector( item ) );
                }
            }

            return d;
        }
    }
}
