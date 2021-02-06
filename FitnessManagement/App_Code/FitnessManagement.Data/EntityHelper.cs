using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EntityHelper
/// </summary>
public static class EntityHelper
{
    public static void SetAuditFieldForInsert(dynamic entity, string userName)
    {
        entity.ChangedWhen = DateTime.Now;
        entity.ChangedWho = userName;
        entity.CreatedWhen = DateTime.Now;
        entity.CreatedWho = userName;        
    }


    public static void SetAuditFieldForUpdate(dynamic entity, string userName)
    {
        entity.ChangedWhen = DateTime.Now;
        entity.ChangedWho = userName;
    }
}