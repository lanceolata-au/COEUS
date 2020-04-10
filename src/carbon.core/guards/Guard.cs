using System;
using System.Globalization;
using IdentityServer4.Extensions;

namespace carbon.core.guards
{

    public class Guard
    {
        private static decimal _decimalEpsilon = (decimal) (1 / Math.Pow(10, 28));
        
        public static void Against<T>(T value, GuardType type)
        {
            switch (value)
            {
                //INTEGER VALUES
                case byte byteTest:
                    switch (type)
                    {
                        case GuardType.Zero when byteTest == 0:
                            throw new GuardException(type,"The value " + byteTest + " is Zero");
                    
                        case GuardType.NotZero when byteTest != 0:
                            throw new GuardException(type,"The value " + byteTest + " is not Zero");
                    
                        case GuardType.Default when byteTest == default:
                            throw new GuardException(type,"The value " + byteTest + " is default");
                        
                        case GuardType.NullOrEmpty:
                            break;
                    }
                    break;
                
                case int intTest:
                    switch (type)
                    {
                        case GuardType.Zero when intTest == 0:
                            throw new GuardException(type,"The value " + intTest + " is Zero");
                    
                        case GuardType.NotZero when intTest != 0:
                            throw new GuardException(type,"The value " + intTest + " is not Zero");
                    
                        case GuardType.Default when intTest == default:
                            throw new GuardException(type,"The value " + intTest + " is default");
                        
                        case GuardType.NullOrEmpty:
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
                    
                        case GuardType.Default when longTest == default:
                            throw new GuardException(type,"The value " + longTest + " is default");

                        case GuardType.NullOrEmpty:
                            break;
                    }
                    break;
                
                
                //FLOATING POINT VALUES
                case float floatTest:
                    switch (type)
                    {
                        case GuardType.Zero when Math.Abs(floatTest) < float.Epsilon:
                            throw new GuardException(type,"The value " + floatTest + " is Zero");
        
                        case GuardType.NotZero when Math.Abs(floatTest) > float.Epsilon:
                            throw new GuardException(type,"The value " + floatTest + " is not Zero");
        
                        case GuardType.Default when floatTest == default:
                            throw new GuardException(type,"The value " + floatTest + " is default");

                        case GuardType.NullOrEmpty:
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
                        
                        case GuardType.Default when doubleTest == default:
                            throw new GuardException(type,"The value " + doubleTest + " is default");

                        case GuardType.NullOrEmpty:
                            break;

                    }
                    break;
                
                case decimal decimalTest:
                    switch (type)
                    {
                        case GuardType.Zero when Math.Abs(decimalTest) < _decimalEpsilon:
                            throw new GuardException(type,"The value " + decimalTest + " is Zero");
                        
                        case GuardType.NotZero when Math.Abs(decimalTest) > _decimalEpsilon:
                            throw new GuardException(type,"The value " + decimalTest + " is not Zero");
                        
                        case GuardType.Default when decimalTest == default:
                            throw new GuardException(type,"The value " + decimalTest + " is default");

                        case GuardType.NullOrEmpty:
                            break;

                    }
                    break;

                //STRING VALUES
                case string stringTest:
                    switch (type)
                    {
                        case GuardType.NullOrEmpty when stringTest.IsNullOrEmpty():
                            throw new GuardException(type,"The value " + stringTest + " is NullOrEmpty");
                        
                        case GuardType.Default:
                            throw new GuardException(type,"The value " + stringTest + " cannot be Default");
                        
                        case GuardType.Zero:
                            throw new GuardException(type,"The value " + stringTest + " cannot be Zero");
                        
                        case GuardType.NotZero:
                            throw new GuardException(type,"The value " + stringTest + " cannot be NotZero");

                    }
                    break;
                
            }
        }
    }
}