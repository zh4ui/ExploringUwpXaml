/*
Inspired by the DependencyObjectClassHierarchy sample from Charles Petzold's Programming Windows 6th.
Project Template from
https://marketplace.visualstudio.com/items?itemName=AndrewWhitechapelMSFT.ConsoleAppUniversal&ssr=false
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml;

// This example code shows how you could implement the required main function for a 
// Console UWP Application. You can replace all the code inside Main with your own custom code.

// You should also change the Alias value in the AppExecutionAlias Extension in the 
// Package.appxmanifest to a value that you define. To edit this file manually, right-click
// it in Solution Explorer and select View Code, or open it with the XML Editor.

namespace PrintDepObjSubs
{
  class ClassDerivatives
  {
    public ClassDerivatives(Type classType)
    {
      this.Type = classType;
      this.Derivatives = new List<ClassDerivatives>();
    }

    public Type Type { protected set; get; }
    public List<ClassDerivatives> Derivatives { protected set; get; }
  }


  class Program
  {
    static List<Type> descendants = new List<Type>();

    static void AddDerivatives(ClassDerivatives level)
    {
      foreach (Type type in descendants)
      {
        Type baseType = type.GetTypeInfo().BaseType;

        if (baseType == level.Type)
        {
          ClassDerivatives sublevel = new ClassDerivatives(type);
          level.Derivatives.Add(sublevel);
          AddDerivatives(sublevel);
        }
      }
    }

    static void PrintDerivaties(ClassDerivatives level, int indent = 0)
    {
      TypeInfo typeInfo = level.Type.GetTypeInfo();
      Console.Write(new String(' ', indent * 2));
      Console.WriteLine(typeInfo.Name);

      foreach (ClassDerivatives sublevel in level.Derivatives)
      {
        PrintDerivaties(sublevel, indent + 1);
      }
    }

    static void Main(string[] args)
    {
      Type rootType = typeof(DependencyObject);
      TypeInfo rootTypeInfo = rootType.GetTypeInfo();

      foreach (Type type in rootType.Assembly.ExportedTypes)
      {
        TypeInfo typeInfo = type.GetTypeInfo();

        if (typeInfo.IsPublic && rootTypeInfo.IsAssignableFrom(typeInfo))
          descendants.Add(type);
      }

      descendants.Sort((t1, t2) =>
          {
            return String.Compare(t1.GetTypeInfo().Name, t2.GetTypeInfo().Name);
          });

      ClassDerivatives toplevel = new ClassDerivatives(rootType);
      AddDerivatives(toplevel);

      PrintDerivaties(toplevel);

      Console.WriteLine("Press a key to continue: ");
      Console.ReadLine();
    }
  }
}
