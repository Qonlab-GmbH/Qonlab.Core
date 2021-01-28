using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Qonlab.Core {
    public static class ReflectionExtensions {
        [DebuggerStepThrough]
        public static MethodInfo GetMethodInfo<T>( Expression<Action<T>> methodOrMember ) {
            var method = methodOrMember.Body as MethodCallExpression;

            if ( method != null ) {
                return method.Method;
            }

            var member = methodOrMember.Body as MemberExpression;
            if ( member != null )
                Console.WriteLine( member.Member );

            throw new ArgumentException( "Expression is not a method nor a member", "methodOrMember" );
        }

        [DebuggerStepThrough]
        public static MethodInfo GetMethodInfo<TBase, TReturn>( Expression<Func<TBase, TReturn>> methodOrMember ) {
            var method = methodOrMember.Body as MethodCallExpression;

            if ( method != null ) {
                return method.Method;
            }

            var member = methodOrMember.Body as MemberExpression;
            if ( member != null )
                Console.WriteLine( member.Member );

            throw new ArgumentException( "Expression is not a method nor a member", "methodOrMember" );
        }

        public enum PropertySelectionType {
            FirstProperty,
            LastProperty,
            AllProperties
        }

        [DebuggerStepThrough]
        public static string PropertyFunctionToPropertyName<T>( Expression<Func<T, object>> propertySelector, PropertySelectionType propertySelectionType = PropertySelectionType.FirstProperty ) {
            return PropertyFunctionToPropertyName<T, object>( propertySelector, propertySelectionType );
        }

        [DebuggerStepThrough]
        public static IList<MemberInfo> PropertyFunctionToPropertyInfo<T>( Expression<Func<T, object>> propertySelector, PropertySelectionType propertySelectionType = PropertySelectionType.FirstProperty ) {
            return PropertyFunctionToPropertyInfo<T, object>( propertySelector, propertySelectionType );
        }

        [DebuggerStepThrough]
        public static string PropertyFunctionToPropertyName<TSource, TProperty>( Expression<Func<TSource, TProperty>> propertySelector, PropertySelectionType propertySelectionType = PropertySelectionType.FirstProperty ) {
            return PropertyFunctionToPropertyInfo<TSource, TProperty>( propertySelector, propertySelectionType ).ToSeparatedString( p => p.Name, separator: " " );
        }

        [DebuggerStepThrough]
        public static IList<MemberInfo> PropertyFunctionToPropertyInfo<TSource, TProperty>( Expression<Func<TSource, TProperty>> propertySelector, PropertySelectionType propertySelectionType = PropertySelectionType.FirstProperty ) {
            if ( propertySelector.Body is MemberExpression ) {
                return HandleMemberExpressionAndReturnMemberName( propertySelector.Body as MemberExpression, propertySelectionType );
            } else if ( propertySelector.Body is UnaryExpression && ( propertySelector.Body as UnaryExpression ).Operand is MemberExpression ) {
                return HandleMemberExpressionAndReturnMemberName( ( propertySelector.Body as UnaryExpression ).Operand as MemberExpression, propertySelectionType );
            } else {
                throw new NotImplementedException();
            }
        }

        [DebuggerStepThrough]
        private static IList<MemberInfo> HandleMemberExpressionAndReturnMemberName( MemberExpression memberPropertySelector, PropertySelectionType propertySelectionType, IList<MemberInfo> result = null ) {
            if ( result == null ) {
                result = new List<MemberInfo>();
            }
            if ( memberPropertySelector.Expression is MemberExpression ) {
                switch ( propertySelectionType ) {
                    case PropertySelectionType.FirstProperty:
                        return HandleMemberExpressionAndReturnMemberName( memberPropertySelector.Expression as MemberExpression, propertySelectionType, result );
                    case PropertySelectionType.LastProperty:
                        result.Add( memberPropertySelector.Member );
                        return result;
                    case PropertySelectionType.AllProperties:
                        HandleMemberExpressionAndReturnMemberName( memberPropertySelector.Expression as MemberExpression, propertySelectionType, result );
                        result.Add( memberPropertySelector.Member );
                        return result;
                    default:
                        throw new NotImplementedException( propertySelectionType.ToString() );
                }
            } else {
                result.Add( memberPropertySelector.Member );
                return result;
            }
        }

        [DebuggerStepThrough]
        public static Type PropertyFunctionToPropertyType<T>( Expression<Func<T, object>> propertySelector ) {
            return PropertyFunctionToPropertyType<T, object>( propertySelector );
        }

        [DebuggerStepThrough]
        public static Type PropertyFunctionToPropertyType<TSource, TProperty>( Expression<Func<TSource, TProperty>> propertySelector ) {
            if ( propertySelector.Body is MemberExpression ) {
                return HandleMemberExpressionAndReturnMemberType( propertySelector.Body as MemberExpression );
            } else if ( propertySelector.Body is UnaryExpression && ( propertySelector.Body as UnaryExpression ).Operand is MemberExpression ) {
                return HandleMemberExpressionAndReturnMemberType( ( propertySelector.Body as UnaryExpression ).Operand as MemberExpression );
            } else {
                //throw new NotImplementedException();
                return null;
            }
        }

        [DebuggerStepThrough]
        private static Type HandleMemberExpressionAndReturnMemberType( MemberExpression memberPropertySelector ) {
            if ( memberPropertySelector.Expression is MemberExpression ) {
                return HandleMemberExpressionAndReturnMemberType( memberPropertySelector.Expression as MemberExpression );
            } else {
                return ( memberPropertySelector.Member as PropertyInfo ).PropertyType;
            }
        }
    }
}
