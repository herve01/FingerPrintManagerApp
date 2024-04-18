using System;

namespace FingerPrintManagerApp.Dao.Admin
{
    public class LogUtil
    {
        private static EntryLogDao entryDao = null;

        private static EntryLogDao GetEntryLogDao()
        {
            if (entryDao == null)
                entryDao = new EntryLogDao();

            return entryDao;
        }

        public static void AddEntry(Model.Admin.User user, string entity, string ev)
        {
            var dao = GetEntryLogDao();
            if (dao != null)
                dao.Add(new Model.Admin.EntryLog()
                {
                    User = user,
                    Entity = entity,
                    Event = ev,
                    EntryDate = DateTime.Now,
                    IPAddress = Model.Util.AppUtil.GetClientIPAddress(),
                    MachineName = Model.Util.AppUtil.GetClientMachineName()
                });
        }
    }
}
