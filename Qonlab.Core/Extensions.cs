using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Qonlab.Core {
    public static class Extensions {
        [DebuggerStepThrough]
        public static T ToEnum<T>( this string s ) {
            return ( T ) Enum.Parse( typeof( T ), s );
        }

        [DebuggerStepThrough]
        public static string ToString( this IEnumerable source, string separator ) {
            return string.Join( separator, ( from s in source.Cast<object>()
                                             where s != null
                                             select s.ToString() ).ToArray() );
        }

        [DebuggerStepThrough]
        public static string ToSeparatedString<TSource>( this IEnumerable<TSource> source, Func<TSource, string> elementToString, string separator = "," ) {
            return source.Aggregate( string.Empty, ( result, value ) => result += elementToString( value ) + separator, result => result.RemoveLast( separator.Length ) );
        }

        [DebuggerStepThrough]
        public static string ToSeparatedString( this IEnumerable<string> source, string separator = "," ) {
            return source.Aggregate( string.Empty, ( result, value ) => result += value + separator, result => result.RemoveLast( separator.Length ) );
        }

        [DebuggerStepThrough]
        public static void Times<T>( this int count, T element, Action<T> execute ) {
            for ( int i = 0; i < count; i++ )
                execute( element );
        }

        [DebuggerStepThrough]
        public static void Times( this int count, Action execute ) {
            for ( int i = 0; i < count; i++ )
                execute();
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> ListItemsOf<T>( this int count, T resultValue ) {
            for ( int i = 0; i < count; i++ )
                yield return resultValue;
        }

        /// <summary>
        /// returns the current string duplicated for the given number of requested duplicates
        /// </summary>
        /// <param name="s">e.g. test</param>
        /// <param name="numberOfDuplicates">e.g. 2</param>
        /// <param name="separator">e.g. ;</param>
        /// <returns>e.g. test;test</returns>
        [DebuggerStepThrough]
        public static string Duplicate( this string s, int numberOfDuplicates, string separator ) {
            var result = string.Empty;
            for ( int i = 0; i < numberOfDuplicates; i++ )
                result += s + separator;
            result.RemoveLast( separator.Length );
            return result;
        }

        [DebuggerStepThrough]
        public static string Format( this string s, params object[] args ) {
            return string.Format( s, args );
        }

        [DebuggerStepThrough]
        public static string ToCamelCase( this string name ) {
            name = CultureInfo.InvariantCulture.TextInfo.ToTitleCase( name );
            name = Regex.Replace( name, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled );

            return name;
        }

        [DebuggerStepThrough]
        public static string SeparateCamelCase( this string s ) {
            return Regex.Replace( s, "([A-Z])", " $1", RegexOptions.Compiled ).Trim();
        }

        [DebuggerStepThrough]
        public static string RemoveLast( this string s, int length ) {
            return s.Length >= length ? s.Remove( s.Length - length, length ) : string.Empty;
        }

        [DebuggerStepThrough]
        public static string ReplaceFirst( this string s, string oldValue, string newValue ) {
            var indexOfOldValue = s.IndexOf( oldValue );
            return s.Substring( 0, indexOfOldValue ) +
                   newValue +
                   s.Substring( indexOfOldValue + oldValue.Length, s.Length - ( indexOfOldValue + oldValue.Length ) );
        }

        [DebuggerStepThrough]
        public static string ReplaceFirst( this string s, string oldValue, string newValue, int indexOfOldValue ) {
            return s.Substring( 0, indexOfOldValue ) +
                   newValue +
                   s.Substring( indexOfOldValue + oldValue.Length, s.Length - ( indexOfOldValue + oldValue.Length ) );
        }

        [DebuggerStepThrough]
        public static string ReplaceLastOccurrence( this string source, string find, string replace ) {
            int place = source.LastIndexOf( find );

            if ( place == -1 )
                return source;

            string result = source.Remove( place, find.Length ).Insert( place, replace );
            return result;
        }

        [DebuggerStepThrough]
        public static string[] Split( this string s, string splitter ) {
            return s.Split( splitter.ToCharArray() );
        }

        /// <summary>
        /// Parse a string to any other type including nullable types.
        /// </summary>
        /// <typeparam name="T">target type</typeparam>
        /// <param name="value">the current string instance</param>
        /// <returns>casted value</returns>
        [DebuggerStepThrough]
        public static T Parse<T>( this string value ) {
            // Get default value for type so if string
            // is empty then we can return default value.
            T result = default( T );
            if ( !string.IsNullOrEmpty( value ) ) {
                // we are not going to handle exception here
                // if you need SafeParse then you should create
                // another method specially for that.
                TypeConverter tc = TypeDescriptor.GetConverter( typeof( T ) );
                result = ( T ) tc.ConvertFrom( value );
            }
            return result;
        }

        [DebuggerStepThrough]
        public static int ToInt( this object o ) {
            if ( o is int ) {
                return ( int ) o;
            } else {
                try {
                    return int.Parse( o.ToString() );
                } catch ( FormatException ex ) {
                    throw new Exception( string.Format( "object '{0}' could not be converted to an int", o ), ex );
                }
            }

        }

        [DebuggerStepThrough]
        public static long ToMinutes( this DateTime dt ) {
            return ( long ) new TimeSpan( dt.Ticks ).TotalMinutes;
        }

        [DebuggerStepThrough]
        public static string ToShortString( this TimeSpan ts ) {
            int multiplier = 1;
            if ( ts.Ticks < 0 )
                multiplier = -1;

            DateTime dt = new DateTime( ts.Ticks * multiplier );
            return ( multiplier == -1 ? "- " : string.Empty ) + ( ( dt.Day - 1 ) + ( ( dt.Month - 1 ) * 30 ) ) + " d " + ( dt.ToString( "HH:mm:ss" ) == "00:00:00" ? string.Empty : dt.ToString( "HH:mm:ss" ) );
        }

        [DebuggerStepThrough]
        public static byte[] ToByte( this string s, Encoding encoding ) {
            Encoding encoder = null;

            if ( encoding == Encoding.UTF8 )
                encoder = new UTF8Encoding();
            else if ( encoding == Encoding.ASCII )
                encoder = new ASCIIEncoding();
            else if ( encoding == Encoding.Default )
                encoder = Encoding.Default;
            else
                throw new NotSupportedException();


            if ( !string.IsNullOrEmpty( s ) )
                return encoder.GetBytes( s );
            else
                return new byte[] { };
        }

        [DebuggerStepThrough]
        public static byte[] ToByte( this Stream s ) {
            byte[] bytes = null;
            using ( BinaryReader br = new BinaryReader( s ) ) {
                bytes = br.ReadBytes( ( int ) s.Length );
            }
            return bytes;
        }

        [DebuggerStepThrough]
        public static string ToString( this byte[] b, Encoding encoding ) {
            return new StreamReader( new MemoryStream( b ), encoding ).ReadToEnd();
        }

        [DebuggerStepThrough]
        public static string LamdbaToHumanReadableString( this Expression exp ) {
            var expString = exp.ToString();

            var parameter = Regex.Match( expString, "[^=]+" ).Value.Trim();
            var lambdaStripped = Regex.Replace( expString, @"value[(][^)]+[)].", string.Empty );
            return Regex.Replace( lambdaStripped, @"[^(]+[(]", string.Empty ).RemoveLast( 1 ).Replace( parameter + ".", string.Empty );
        }

        [DebuggerStepThrough]
        public static string ToISO8601DateTime( this DateTime dt ) {
            return dt.ToString( "yyyy-MM-ddTHH:mm:ss.fff" );
        }

        [DebuggerStepThrough]
        public static T Use<T>( this T item, Action<T> work ) {
            work( item );
            return item;
        }

        [DebuggerStepThrough]
        public static IEnumerable<int> GetAllForeignKeyIds( this DataTable dt, string fkName ) {
            foreach ( DataRow row in dt.Rows )
                if ( row[ fkName ] != DBNull.Value )
                    yield return row[ fkName ].ToInt();
        }

        [DebuggerStepThrough]
        public static IEnumerable<int> GetAllIds( this DataTable dt ) {
            foreach ( DataRow row in dt.Rows )
                yield return row[ "Id" ].ToInt();
        }

        [DebuggerStepThrough]
        public static void AddDelta<T>( this HashSet<T> hs, IEnumerable<T> values ) {
            values.ForEach( v => {
                if ( !hs.Contains( v ) )
                    hs.Add( v );
            } );
        }

        [DebuggerStepThrough]
        public static string ToMD5Hash( this string s ) {
            if ( ( s == null ) || ( s.Length == 0 ) ) {
                return string.Empty;
            }

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes( s );
            byte[] result = md5.ComputeHash( textToHash );

            return BitConverter.ToString( result );
        }

        [DebuggerStepThrough]
        public static string ToCSharpTypeName( this Type t, bool includeGenericParameterConstraints = false ) {
            if ( !t.IsGenericType ) {
                switch ( t.Name ) {
                    case "Boolean":
                        return "bool";
                    case "Byte":
                        return "byte";
                    case "Char":
                        return "char";
                    case "Decimal":
                        return "decimal";
                    case "Double":
                        return "double";
                    case "Int16":
                        return "short";
                    case "Int32":
                        return "int";
                    case "Int64":
                        return "long";
                    case "Single":
                        return "float";
                    case "String":
                        return "string";
                    case "Object":
                        return "object";
                    default:
                        return t.Name;
                }
            } else {
                var genericType = t.GetGenericArguments().Aggregate( t.ToCSharpTypeNameForGeneric() + "<", ( c, n ) => c += n.ToCSharpTypeName() + ", ", res => res.RemoveLast( 2 ) + ">" );

                if ( includeGenericParameterConstraints )
                    genericType += t.GetGenericArguments().Where( arg => arg.IsGenericParameter ).Select( arg => new { GenericArgument = arg.Name, Constraint = arg.GetGenericParameterConstraints().Aggregate( string.Empty, ( c, n ) => c += n.Name + ", ", res => res.RemoveLast( 2 ) ) } ).Aggregate( string.Empty, ( c, n ) => c += string.Format( " where {0} : {1}", n.GenericArgument, n.Constraint ) );

                return genericType;
            }
        }

        [DebuggerStepThrough]
        public static string ToCSharpTypeNameForGeneric( this Type t ) { return t.Name.Substring( 0, t.Name.IndexOf( '`' ) ); }

        [DebuggerStepThrough]
        public static IEnumerable<string> GetNamespaces( this Type t ) {
            yield return t.Namespace;

            if ( t.IsGenericType )
                foreach ( var arg in t.GetGenericArguments() )
                    foreach ( var s in GetNamespaces( arg ) )
                        yield return s;
        }

        [DebuggerStepThrough]
        public static T As<T>( this object o ) { return ( T ) o; }

        [DebuggerStepThrough]
        public static string AssemblyDescription( this Type t ) {
            var assemblyOfType = Assembly.GetAssembly( t );
            var assemblyDescriptionAttributes = assemblyOfType.GetCustomAttributes( typeof( AssemblyDescriptionAttribute ), false );
            if ( assemblyDescriptionAttributes.Count() > 0 ) {
                var assemblyDescriptionObject = assemblyDescriptionAttributes.First();
                if ( assemblyDescriptionObject is AssemblyDescriptionAttribute ) {
                    var assemblyDescription = assemblyDescriptionObject as AssemblyDescriptionAttribute;
                    return assemblyDescription.Description;
                }
            }

            return string.Empty;
        }

        [DebuggerStepThrough]
        public static string AssemblyVersion( this Type t ) {
            var assemblyOfType = Assembly.GetAssembly( t );
            var assemblyDescriptionAttributes = assemblyOfType.GetCustomAttributes( typeof( AssemblyVersionAttribute ), false );
            if ( assemblyDescriptionAttributes.Count() > 0 ) {
                var assemblyVersionObject = assemblyDescriptionAttributes.First();
                if ( assemblyVersionObject is AssemblyVersionAttribute ) {
                    var assemblyVersion = assemblyVersionObject as AssemblyVersionAttribute;
                    return assemblyVersion.Version;
                }
            }

            return string.Empty;
        }

        [DebuggerStepThrough]
        public static Stream LoadResourceAsStream( this Assembly a, string resourcePathInAssembly ) {
            var documentStream = a.GetManifestResourceStream( resourcePathInAssembly );
            if ( documentStream != null ) {
                return documentStream;
            } else {
                throw new Exception( string.Format( "Resource '{0}' not found in assembly '{1}'", resourcePathInAssembly, a.FullName ) );
            }
        }

        [DebuggerStepThrough]
        public static byte[] ToByteArray( this Stream s ) {
            using ( MemoryStream ms = new MemoryStream() ) {
                s.CopyTo( ms );
                return ms.ToArray();
            }
        }

        [DebuggerStepThrough]
        public static string LoadResourceAsString( this Assembly a, string resourcePathInAssembly ) {
            var documentStream = a.LoadResourceAsStream( resourcePathInAssembly );
            var documentString = new StreamReader( documentStream ).ReadToEnd();
            return documentString;
        }

        [DebuggerStepThrough]
        public static string Shorten( this string s, int maxLength, string continuation = "..." ) {
            if ( s.Length <= maxLength ) {
                return s;
            } else {
                var shortenedS = s.Substring( 0, maxLength ) + continuation;
                return shortenedS;
            }
        }

        [DebuggerStepThrough]
        public static string ToReadableString( this object o ) {
            var type = o.GetType();
            var properties = type.GetProperties();
            var propertyValues = properties.ToDictionary( k => k.Name, v => v.GetValue( o, null ) );

            return string.Format( "{0}[{1}]", type.Name, propertyValues.ToSeparatedString( k => string.Format( "{0}:{1}", k.Key, k.Value is IList || k.Value is ICollection ? ( k.Value as IEnumerable ).Cast<object>().ToSeparatedString( enumerableElement => enumerableElement != null ? enumerableElement.ToString() : "null" ) : k.Value ) ) );
        }

        [DebuggerStepThrough]
        public static string Slugify( string text ) {
            IdnMapping idnMapping = new IdnMapping();
            text = idnMapping.GetAscii( text );

            text = RemoveAccent( text ).ToLower();

            //  Remove all invalid characters.  
            text = Regex.Replace( text, @"[^a-z0-9\s-]", "" );

            //  Convert multiple spaces into one space
            text = Regex.Replace( text, @"\s+", " " ).Trim();

            //  Replace spaces by underscores.
            text = Regex.Replace( text, @"\s", "_" );

            return text;
        }

        [DebuggerStepThrough]
        public static string RemoveAccent( string text ) {
            byte[] bytes = Encoding.GetEncoding( "Cyrillic" ).GetBytes( text );

            return Encoding.ASCII.GetString( bytes );
        }
    }
}

