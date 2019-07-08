using System;
using System.Runtime.InteropServices;
using carbon.core.dtos.account;

namespace carbon.api.Features
{
    public class AccessAttributes : Attribute
    {
        void HasGlobalAccess([In][Out] ref AccessEnum accessEnum)
        {
            //TODO I know this is possible but not enough about attributes at the moment to make this work
        }
        
    }
}