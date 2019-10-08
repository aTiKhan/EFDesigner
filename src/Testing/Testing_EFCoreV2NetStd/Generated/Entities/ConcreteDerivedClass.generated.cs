//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Testing
{
   public partial class ConcreteDerivedClass: global::Testing.AbstractBaseClass
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected ConcreteDerivedClass(): base()
      {
         Init();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="property0"></param>
      public ConcreteDerivedClass(string property0)
      {
         if (string.IsNullOrEmpty(property0)) throw new ArgumentNullException(nameof(property0));
         this.Property0 = property0;
         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="property0"></param>
      public static new ConcreteDerivedClass Create(string property0)
      {
         return new ConcreteDerivedClass(property0);
      }

      /*************************************************************************
       * Persistent properties
       *************************************************************************/

      /// <summary>
      /// Backing field for Property1
      /// </summary>
      protected string _Property1;
      /// <summary>
      /// When provided in a partial class, allows value of Property1 to be changed before setting.
      /// </summary>
      partial void SetProperty1(string oldValue, ref string newValue);
      /// <summary>
      /// When provided in a partial class, allows value of Property1 to be changed before returning.
      /// </summary>
      partial void GetProperty1(ref string result);

      public string Property1
      {
         get
         {
            string value = _Property1;
            GetProperty1(ref value);
            return (_Property1 = value);
         }
         set
         {
            string oldValue = _Property1;
            SetProperty1(oldValue, ref value);
            if (oldValue != value)
            {
               _Property1 = value;
            }
         }
      }

      /// <summary>
      /// Backing field for PropertyInChild
      /// </summary>
      protected string _PropertyInChild;
      /// <summary>
      /// When provided in a partial class, allows value of PropertyInChild to be changed before setting.
      /// </summary>
      partial void SetPropertyInChild(string oldValue, ref string newValue);
      /// <summary>
      /// When provided in a partial class, allows value of PropertyInChild to be changed before returning.
      /// </summary>
      partial void GetPropertyInChild(ref string result);

      public string PropertyInChild
      {
         get
         {
            string value = _PropertyInChild;
            GetPropertyInChild(ref value);
            return (_PropertyInChild = value);
         }
         set
         {
            string oldValue = _PropertyInChild;
            SetPropertyInChild(oldValue, ref value);
            if (oldValue != value)
            {
               _PropertyInChild = value;
            }
         }
      }

   }
}

