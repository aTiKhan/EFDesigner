﻿//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Sawczyn.EFDesigner.EFModel
//{
//   class Int32NullableTypeConverter : TypeConverterBase
//   {
//       /// <summary>
//      ///    Returns whether this converter can convert an object of the given type to the type of this converter, using
//      ///    the specified context.
//      /// </summary>
//      /// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
//      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
//      /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from. </param>
//      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
//      {
//         return sourceType == typeof(string);
//      }

//      /// <summary>Returns whether this converter can convert the object to the specified type, using the specified context.</summary>
//      /// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
//      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
//      /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to. </param>
//      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
//      {
//         return destinationType == typeof(Int32Nullable);
//      }

//      /// <summary>Converts the given object to the type of this converter, using the specified context and culture information.</summary>
//      /// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
//      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
//      /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
//      /// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
//      /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
//      public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
//      {
//         if (value is string s)
//         {
//            return int.TryParse(s, out int val)
//                      ? new Int32Nullable(val)
//                      : null;
//         }

//         return base.ConvertFrom(context, culture, value);
//      }

//      /// <summary>
//      ///    Returns whether this object supports a standard set of values that can be picked from a list, using the
//      ///    specified context.
//      /// </summary>
//      /// <returns>
//      ///    true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> should be called to find a
//      ///    common set of values the object supports; otherwise, false.
//      /// </returns>
//      /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
//      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
//      {
//         return false;
//      }

//   }
//}
