using FingerPrintManagerApp.Model.Presence;

namespace FingerPrintManagerApp.Dao.Presence
{
    public class Util
    {
        public static ModePointage ToModePointage(string mode)
        {
            switch (mode)
            {
                case "Utilisateur":
                    return ModePointage.Utilisateur;

                case "Empreinte":
                    return ModePointage.Empreinte;

                case "Smart_card":
                    return ModePointage.Smart_card;

                case "QrCode":
                    return ModePointage.QrCode;

                case "RFID":
                    return ModePointage.RFID;
                default:
                    return ModePointage.Utilisateur;
            }
        }

    }
}
