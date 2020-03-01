using System;
using System.Globalization;

namespace carbon.core.guards
{
    public class Guard
    {
        public static void Against<T>(T value, GuardType type)
        {
            if (value is int intTest)
            {
                
                if (type == GuardType.Zero && intTest == 0)
                {
                    throw new GuardException(type,"The value " + intTest + " is Zero");
                }
                
                if (type == GuardType.NotZero && intTest != 0)
                {
                    throw new GuardException(type,"The value " + intTest + " is not Zero");
                }
                
                if (type == GuardType.NullOrDefault && intTest == default)
                {
                    throw new GuardException(type,"The value " + intTest + " is default");
                }
                
            }
            
            if (value is long longTest)
            {
                
                if (type == GuardType.Zero && longTest == 0)
                {
                    throw new GuardException(type,"The value " + longTest + " is Zero");
                }
                
                if (type == GuardType.NotZero && longTest != 0)
                {
                    throw new GuardException(type,"The value " + longTest + " is not Zero");
                }
                
            }

            if (value is double doubleTest)
            {
                
                if (type == GuardType.Zero && Math.Abs(doubleTest) < double.Epsilon)
                {
                    throw new GuardException(type,"The value " + doubleTest + " is Zero");
                }
                
                if (type == GuardType.NotZero && Math.Abs(doubleTest) > double.Epsilon)
                {
                    throw new GuardException(type,"The value " + doubleTest + " is not Zero");
                }
                
            }
            
        }
    }
}