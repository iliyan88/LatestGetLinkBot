using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobTheBot.Kernel
{
    public enum ErrorType
    {
        NONE,
        /// <summary>
        /// "Missing required parameter '{0}'."
		/// (<see cref="string"/> parameterName)
		/// </summary>
        MissingRequiredParameter,

        /// <summary>
        /// "{0} with parameter '{1}' was not found"
        /// (<see cref="string"/> fieldName,
        /// <see cref="T"/> value)
        /// </summary>
        NotFound,

        /// <summary>
        /// "The '{0}' has no value."
		/// (<see cref="string"/> fieldName)
		/// </summary>
        NullReference,
    }
}
