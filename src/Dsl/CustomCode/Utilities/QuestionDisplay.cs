namespace Sawczyn.EFDesigner
{
   /// <summary>
   /// This helps keep UI interaction out of our DSL project proper. DslPackage calls RegisterDisplayHandler with a method that shows the MessageBox
   /// (or other UI-related method) properly using the Visual Studio service provider.
   /// </summary>
   public static class QuestionDisplay
   {

      /// <summary>a display handler to call when a question needs to be asked. Set from the DslPackage assembly.</summary>
      ///
      /// <param name="message">The question to be asked.</param>
      ///
      /// <returns>A bool.</returns>
      public delegate bool QuestionVisualizer(string message);
      private static QuestionVisualizer QuestionVisualizerMethod;


      /// <summary>Displays a yes/no question, returning the response.</summary>
      ///
      /// <param name="message">The question to ask.</param>
      ///
      /// <returns>User's response. May return null depending on how the display handler works.</returns>
      public static bool? Show(string message)
      {
         if (QuestionVisualizerMethod != null)
         {
            try
            {
               return QuestionVisualizerMethod(message);
            }
            catch
            {
               return null;
            }
         }
         return null;
      }


      /// <summary>Registers a display handler to call when a question needs to be asked. This keeps this assembly UI-independent.</summary>
      ///
      /// <param name="method">The method to call to render the UI</param>
      public static void RegisterDisplayHandler(QuestionVisualizer method)
      {
         QuestionVisualizerMethod = method;
      }
   }
}