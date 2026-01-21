using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CleanCRUDSolution.Web.Extensions
{
    public static class EnumExtensions
    {
        public static IReadOnlyList<SelectListItem> ToSelectListItem<TEnum>() where TEnum : struct, Enum
        {
            var values = Enum.GetValues<TEnum>();
            return values.Select(e => new SelectListItem()
            {
                Value = e.ToString(),
                Text = GetDisplayName(e)
            }).ToList();
        }

        private static string GetDisplayName(Enum value)
        {
            var type = value.GetType();
            var member = type.GetMember(value.ToString()).First();
            var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? value.ToString();
        }
    }
}
