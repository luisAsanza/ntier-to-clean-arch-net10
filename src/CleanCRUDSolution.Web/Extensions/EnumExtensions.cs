using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CleanCRUDSolution.Web.Extensions
{
    /// <summary>
    /// Helper extensions for working with enums in Razor pages and view models.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts an enum type to a list of <see cref="SelectListItem"/> for use in select inputs.
        /// </summary>
        public static IReadOnlyList<SelectListItem> ToSelectListItem<TEnum>() where TEnum : struct, Enum
        {
            var values = Enum.GetValues<TEnum>();
            return values.Select(e => new SelectListItem()
            {
                Value = e.ToString(),
                Text = GetDisplayName(e)
            }).ToList();
        }

        /// <summary>
        /// Resolves the friendly display name of an enum value using the <see cref="DisplayAttribute"/>,
        /// falling back to the enum's name if not present.
        /// </summary>
        private static string GetDisplayName(Enum value)
        {
            var type = value.GetType();
            var member = type.GetMember(value.ToString()).First();
            var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? value.ToString();
        }
    }
}
