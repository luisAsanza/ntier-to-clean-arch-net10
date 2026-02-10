using Microsoft.AspNetCore.Mvc.Rendering;

namespace CleanCRUDSolution.Web.Extensions
{
    /// <summary>
    /// Provides helper extension methods for converting enums to web-friendly representations.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts all values of the specified enum type to a read-only list of <see cref="SelectListItem"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to convert.</typeparam>
        /// <param name="labelProvider">A function that provides the display label for each enum value.</param>
        /// <returns>A read-only list of <see cref="SelectListItem"/> where <c>Value</c> is the enum name and <c>Text</c> is the provided label.</returns>
        public static IReadOnlyList<SelectListItem> ToSelectListItem<TEnum>(
            Func<TEnum, string> labelProvider)
            where TEnum : struct, Enum
        {
            var values = Enum.GetValues<TEnum>();
            return values.Select(e => new SelectListItem()
            {
                Value = e.ToString(),
                Text = labelProvider(e)
            }).ToList();
        }
        
        /// <summary>
        /// Converts all values of the specified enum type to a read-only list of <see cref="SelectListItem"/>,
        /// using the enum name as the display label.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to convert.</typeparam>
        /// <returns>A read-only list of <see cref="SelectListItem"/> where both <c>Value</c> and <c>Text</c> are the enum name.</returns>
        public static IReadOnlyList<SelectListItem> ToSelectListItem<TEnum>() where TEnum : struct, Enum
        {            
            return ToSelectListItem<TEnum>(e => e.ToString()); // Default to enum name if no label provider is given
        }
    }
}

