using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace D6SpellCreator.Persistence
{
    interface ISQLLiteDB
    {
        SQLiteAsyncConnection GetConnection();
    }
}
