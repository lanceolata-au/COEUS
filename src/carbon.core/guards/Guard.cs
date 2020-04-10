using System;
using System.Globalization;

namespace carbon.core.guards
{
    public class Guard
    {
        public static void Against<T>(T value, GuardType type)
        {
            switch (value)
            {
                case int intTest:
                    switch (type)
                    {
                        case GuardType.Zero when intTest == 0:
                            throw new GuardException(type,"The value " + intTest + " is Zero");
                    
                        case GuardType.NotZero when intTest != 0:
                            throw new GuardException(type,"The value " + intTest + " is not Zero");
                    
                        case GuardType.NullOrDefault when intTest == default:
                            throw new GuardException(type,"The value " + intTest + " is default");
                        
                        case GuardType.Empty:
                            break;

                    }

                    break;
                
                case long longTest:
                    switch (type)
                    {
                        case GuardType.Zero when longTest == 0:
                            throw new GuardException(type,"The value " + longTest + " is Zero");
                    
                        case GuardType.NotZero when longTest != 0:
                            throw new GuardException(type,"The value " + longTest + " is not Zero");
                    
                        case GuardType.NullOrDefault when longTest == default:
                            throw new GuardException(type,"The value " + longTest + " is default");

                        case GuardType.Empty:
                            break;
                    }

                    break;
                
                case double doubleTest:
                    switch (type)
                    {
                        case GuardType.Zero when Math.Abs(doubleTest) < double.Epsilon:
                            throw new GuardException(type,"The value " + doubleTest + " is Zero");
                        
                        case GuardType.NotZero when Math.Abs(doubleTest) > double.Epsilon:
                            throw new GuardException(type,"The value " + doubleTest + " is not Zero");
                        
                        case GuardType.NullOrDefault when doubleTest == default:
                            throw new GuardException(type,"The value " + doubleTest + " is default");

                        case GuardType.Empty:
                            break;

                    }

                    break;
            }
        }
    }
}