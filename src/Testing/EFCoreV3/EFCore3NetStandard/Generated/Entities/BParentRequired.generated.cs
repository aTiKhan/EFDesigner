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
   public partial class BParentRequired
   {
      partial void Init();

      /// <summary>
      /// Default constructor
      /// </summary>
      public BParentRequired()
      {
         BChildCollection = new System.Collections.Generic.HashSet<global::Testing.BChild>();

         Init();
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Backing field for Id
      /// </summary>
      internal int _id;
      /// <summary>
      /// When provided in a partial class, allows value of Id to be changed before setting.
      /// </summary>
      partial void SetId(int oldValue, ref int newValue);
      /// <summary>
      /// When provided in a partial class, allows value of Id to be changed before returning.
      /// </summary>
      partial void GetId(ref int result);

      /// <summary>
      /// Identity, Indexed, Required
      /// </summary>
      [Key]
      [Required]
      public int Id
      {
         get
         {
            int value = _id;
            GetId(ref value);
            return (_id = value);
         }
         protected set
         {
            int oldValue = _id;
            SetId(oldValue, ref value);
            if (oldValue != value)
            {
               _id = value;
            }
         }
      }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

      protected global::Testing.BChild _bChildOptional;
      partial void SetBChildOptional(global::Testing.BChild oldValue, ref global::Testing.BChild newValue);
      partial void GetBChildOptional(ref global::Testing.BChild result);

      public virtual global::Testing.BChild BChildOptional
      {
         get
         {
            global::Testing.BChild value = _bChildOptional;
            GetBChildOptional(ref value);
            return (_bChildOptional = value);
         }
         set
         {
            global::Testing.BChild oldValue = _bChildOptional;
            SetBChildOptional(oldValue, ref value);
            if (oldValue != value)
            {
               _bChildOptional = value;
            }
         }
      }

      protected global::Testing.BChild _bChildRequired;
      partial void SetBChildRequired(global::Testing.BChild oldValue, ref global::Testing.BChild newValue);
      partial void GetBChildRequired(ref global::Testing.BChild result);

      /// <summary>
      /// Required
      /// </summary>
      public virtual global::Testing.BChild BChildRequired
      {
         get
         {
            global::Testing.BChild value = _bChildRequired;
            GetBChildRequired(ref value);
            return (_bChildRequired = value);
         }
         set
         {
            global::Testing.BChild oldValue = _bChildRequired;
            SetBChildRequired(oldValue, ref value);
            if (oldValue != value)
            {
               _bChildRequired = value;
            }
         }
      }

      public virtual ICollection<global::Testing.BChild> BChildCollection { get; protected set; }

   }
}

