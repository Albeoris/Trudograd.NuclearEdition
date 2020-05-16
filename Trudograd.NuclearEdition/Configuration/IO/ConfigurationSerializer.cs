using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace Trudograd.NuclearEdition
{
    public sealed class ConfigurationSerializer
    {
        public static void Read(RootConfiguration root, StreamReader sr)
        {
            Type rootType = root.GetType();
            String errorMessage = String.Empty;

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                try
                {

                    if (String.IsNullOrEmpty(line))
                        continue;

                    if (line.StartsWith("//"))
                        continue;

                    String[] parts = line.Split('=');
                    if (parts.Length != 2)
                    {
                        errorMessage = $"line.Split('=').Length != 2";
                        goto onError;
                    }

                    String key = parts[0].Trim();
                    String value = parts[1].Trim();

                    parts = key.Split('.');

                    Type type = rootType;
                    Object instance = root;
                    PropertyInfo property = null;
                    for (Int32 i = 0; i < parts.Length; i++)
                    {
                        property = type.GetProperty(parts[i]);
                        if (property?.CanRead != true)
                        {
                            errorMessage = $"type.GetProperty(parts[i])?.CanRead != true [type: {type.FullName}, parts[i]: {parts[i]}, property: {property}";
                            goto onError;
                        }

                        if (i < parts.Length - 1)
                        {
                            instance = property.GetValue(instance);
                            if (instance is null)
                            {
                                errorMessage = $"property.GetValue(instance) is null";
                                goto onError;
                            }

                            type = property.PropertyType;
                        }
                    }

                    if (property?.CanWrite != true)
                    {
                        errorMessage = $"property?.CanWrite != true";
                        goto onError;
                    }

                    Object convertedValue;
                    if (property.PropertyType.IsEnum)
                    {
                        Int64 number = Int64.Parse(value, CultureInfo.InvariantCulture);
                        convertedValue = Enum.ToObject(property.PropertyType, number);
                    }
                    else
                    {
                        convertedValue = Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture);
                    }

                    property.SetValue(instance, convertedValue);
                    continue;
                }
                catch (Exception ex)
                {
                    errorMessage = ex.ToString();
                }

                onError:
                Debug.LogWarning($"{nameof(NuclearEdition)} Invalid configuration line: {line}. Error: {errorMessage}");
            }
        }

        public static void Write(RootConfiguration root, StreamWriter sw)
        {
            WriteSection(String.Empty, root, sw);
        }

        private static void WriteSection(String parentKey, Object instance, StreamWriter sw)
        {
            Type type = instance.GetType();
            foreach (var property in type.GetProperties())
            {
                if (!property.CanRead)
                    continue;

                Object value = property.GetValue(instance);
                String key = parentKey == String.Empty ? property.Name : $"{parentKey}.{property.Name}";

                if (property.CanWrite)
                {
                    WriteValueDescription(property, sw);
                    WriteValue(key, value, sw);
                }
                else
                {
                    WriteSectionDescription(key, property, sw);
                    WriteSection(key, value, sw);
                }
            }

            sw.WriteLine();
            sw.WriteLine();
        }

        private static void WriteSectionDescription(String key, PropertyInfo sectionProperty, StreamWriter sw)
        {
            Type sectionType = sectionProperty.PropertyType;
            ConfigurationSectionAttribute sectionAttribute = sectionType.GetCustomAttribute<ConfigurationSectionAttribute>();
            if (sectionAttribute is null)
                throw new NotSupportedException(sectionType.FullName);

            sw.WriteLine("// ----------------------------------------------------------");
            sw.WriteLine($"// {key} - {sectionAttribute.Description}");
            sw.WriteLine("// ----------------------------------------------------------");
            sw.WriteLine();
        }

        private static void WriteValue(String key, Object value, StreamWriter sw)
        {
            String formattedValue;
            if (value is Boolean b)
                formattedValue = b ? "true" : "false";
            else if (value is Enum e)
                formattedValue = Convert.ToInt64(e).ToString(CultureInfo.InvariantCulture);
            else
                formattedValue = Convert.ToString(value, CultureInfo.InvariantCulture);

            sw.Write(key);
            sw.Write(" = ");
            sw.WriteLine(formattedValue);
            sw.WriteLine();
        }

        private static void WriteValueDescription(PropertyInfo valueProperty, StreamWriter sw)
        {
            IEnumerable<ConfigurationValueAttribute> valueAttribute = valueProperty.GetCustomAttributes<ConfigurationValueAttribute>();
            foreach (var attr in valueAttribute)
            {
                sw.Write("// ");
                sw.Write(attr.Value);
                sw.Write(" - ");
                sw.WriteLine(attr.Description);
            }
        }
    }
}