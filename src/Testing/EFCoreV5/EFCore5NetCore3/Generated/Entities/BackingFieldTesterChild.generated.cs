//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor v2.1.0.0
//     Source:                    https://github.com/msawczyn/EFDesigner
//     Visual Studio Marketplace: https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner
//     Documentation:             https://msawczyn.github.io/EFDesigner/
//     License (MIT):             https://github.com/msawczyn/EFDesigner/blob/master/LICENSE
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
   public partial class BackingFieldTesterChild
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected BackingFieldTesterChild()
      {
         Init();
      }

      /// <summary>
      /// Replaces default constructor, since it's protected. Caller assumes responsibility for setting all required values before saving.
      /// </summary>
      public static BackingFieldTesterChild CreateBackingFieldTesterChildUnsafe()
      {
         return new BackingFieldTesterChild();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="_backingfieldtester0"></param>
      /// <param name="_backingfieldtester1"></param>
      /// <param name="_backingfieldtester2"></param>
      public BackingFieldTesterChild(global::Testing.BackingFieldTester _backingfieldtester0, global::Testing.BackingFieldTester _backingfieldtester1, global::Testing.BackingFieldTester _backingfieldtester2)
      {
         if (_backingfieldtester0 == null) throw new ArgumentNullException(nameof(_backingfieldtester0));
         _backingfieldtester0.ToBackingFieldOnConstruction = this;

         if (_backingfieldtester1 == null) throw new ArgumentNullException(nameof(_backingfieldtester1));
         _backingfieldtester1.ToBackingFieldAlways = this;

         if (_backingfieldtester2 == null) throw new ArgumentNullException(nameof(_backingfieldtester2));
         _backingfieldtester2.ToProperty = this;


         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="_backingfieldtester0"></param>
      /// <param name="_backingfieldtester1"></param>
      /// <param name="_backingfieldtester2"></param>
      public static BackingFieldTesterChild Create(global::Testing.BackingFieldTester _backingfieldtester0, global::Testing.BackingFieldTester _backingfieldtester1, global::Testing.BackingFieldTester _backingfieldtester2)
      {
         return new BackingFieldTesterChild(_backingfieldtester0, _backingfieldtester1, _backingfieldtester2);
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Identity, Indexed, Required
      /// </summary>
      [Key]
      [Required]
      public int Id { get; protected set; }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

   }
}

