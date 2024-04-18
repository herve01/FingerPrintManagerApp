using FingerPrintManagerApp.Model.Admin;

namespace FingerPrintManagerApp.Dao.Admin
{
    public class Util
    {
        public static UserState ToUserState(string state)
        {
            switch (state)
            {
                case "Bloqué":
                    return UserState.Bloqué;

                case "Fonctionnel":
                    return UserState.Fonctionnel;

                default:
                    return UserState.Fonctionnel;
            }
        }

        public static UserType ToUserType(string type)
        {
            switch (type)
            {
                case "ADMIN":
                    return UserType.ADMIN;

                case "USER":
                    return UserType.USER;

                default:
                    return UserType.USER;
            }
        }
    }
}
