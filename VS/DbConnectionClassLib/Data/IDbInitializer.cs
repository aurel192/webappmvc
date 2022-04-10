using System;
using System.Collections.Generic;
using System.Text;

namespace DbConnectionClassLib.Data
{
    public interface IDbInitializer
    {
        void Initialize();

        void InitializeUsersAndRoles();
    }
}
