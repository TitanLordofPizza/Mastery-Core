using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System;

namespace Mastery.Core.Utility
{
    public static class ClassCopy
    {
        public static T CopyClass<T>(T obj)
        {
            var objectCopy = ObjectCopy(obj);
            return (T)objectCopy;
        }

        private static object ObjectCopy(object obj)
        {
            var type = obj.GetType();

            var objectCopy = Activator.CreateInstance(type);

            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                var value = field.GetValue(obj);

                if (value == null) continue; // If Value is Null Skip.

                if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>)) //Is the Value a List?
                {
                    var listType = field.FieldType;

                    var listCopy = (IList)Activator.CreateInstance(listType);

                    foreach (var item in (IEnumerable)value)
                    {
                        var copiedItem = item;

                        if (item != null && item.GetType().IsClass && item.GetType() != typeof(string))
                        {
                            copiedItem = CopyClass(item);
                        }

                        listCopy.Add(item != null ? Convert.ChangeType(copiedItem, copiedItem.GetType()) : null);
                    }

                    value = listCopy;
                }
                else if (field.FieldType.IsArray) //Is the Value a Array?
                {
                    Type elementType = field.FieldType.GetElementType();

                    var array = value as Array;

                    if (array != null)
                    {
                        var arrayCopy = Array.CreateInstance(elementType, array.Length);

                        for (int i = 0; i < array.Length; i++)
                        {
                            var copiedItem = array.GetValue(i);

                            if (elementType.IsClass && elementType != typeof(string))
                            {
                                copiedItem = CopyClass(copiedItem);
                            }

                            arrayCopy.SetValue(copiedItem, i);
                        }

                        value = arrayCopy;
                    }
                }
                else if (HasDefaultConstructor(field.FieldType) == false && field.FieldType.IsClass
                    && field.FieldType != typeof(string) && field.FieldType != typeof(UnityEngine.Texture2D)
                    && field.FieldType != typeof(Type) && field.FieldType != typeof(Verse.ModContentPack)
                    || field.FieldType == typeof(Verse.VerbProperties))
                {
                    value = CopyClass(value);
                }
                else if (field.FieldType == typeof(UtilityCurve))
                {
                    var oldCurve = (UtilityCurve)value;

                    var newCurve = new UtilityCurve()
                    {
                        Curve = new Verse.SimpleCurve(),
                        Percentage = oldCurve.Percentage
                    };

                    foreach (var point in oldCurve.Curve.Points)
                    {
                        newCurve.Curve.Add(point.x, point.y);
                    }

                    value = newCurve;
                }

                field.SetValue(objectCopy, value);
            }

            return objectCopy;
        }

        private static bool HasDefaultConstructor(Type type)
        {
            return !type.IsAbstract && type.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}