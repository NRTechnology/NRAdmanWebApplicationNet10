using System.ComponentModel.DataAnnotations;
using System.Net;

namespace NRAdmanWebApplicationNet10.Attributes
{
    public class IpAddressValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;

            var input = value.ToString();

            if (string.IsNullOrWhiteSpace(input))
                return false;

            var parts = input.Split('/');

            // Validate IP
            if (!IPAddress.TryParse(parts[0], out _))
                return false;

            // Validate CIDR
            if (parts.Length == 2)
            {
                if (!int.TryParse(parts[1], out int cidr))
                    return false;

                return cidr >= 0 && cidr <= 32;
            }

            return parts.Length == 1;
        }
    }
}
