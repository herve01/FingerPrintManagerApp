using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Employe;
using libzkfpcsharp;

namespace FingerPrintManagerApp.Dao.Employe
{
    class Util
    {
        public static ActeType ToActeNominationType(string value)
        {

            switch (value)
            {
                case "Affectation":
                    return ActeType.Affectation;
                case "Arrêté":
                    return ActeType.Arrêté;
                case "Décret":
                    return ActeType.Décret;
                case "Engagement":
                    return ActeType.Engagement;
                case "Notification":
                    return ActeType.Notification;
                case "Ordonnance":
                    return ActeType.Ordonnance;

                default:
                    return ActeType.Engagement;
            }
        }

        public static Fingers ToFingers(string value)
        {
            switch (value)
            {
                case "LL":
                case "Auriculaire gauche":
                    return Fingers.LL;
                case "LR":
                case "Annulaire gauche":
                    return Fingers.LR;
                case "LM":
                case "Majeur gauche":
                    return Fingers.LM;
                case "LI":
                case "Index gauche":
                    return Fingers.LI;
                case "LT":
                case "Pouce gauche":
                    return Fingers.LT;
                case "RT":
                case "Pouce droit":
                    return Fingers.RT;
                case "RI":
                case "Index droit":
                    return Fingers.RI;
                case "RM":
                case "Majeur droit":
                    return Fingers.RM;
                case "RR":
                case "Annulaire droit":
                    return Fingers.RR;
                case "RL":
                case "Auriculaire droit":
                    return Fingers.RL;
                default:
                    return Fingers.RT;
            }
        }

        public static ExtensionFile ToExtensionFile(string value)
        {

            switch (value)
            {
                case "IMAGE":
                    return ExtensionFile.IMAGE;

                case "PDF":
                    return ExtensionFile.PDF;

                default:
                    return ExtensionFile.IMAGE;
            }
        }

        public static EntiteType ToEntiteType(string value)
        {

            switch (value)
            {
                case "Agence":
                    return EntiteType.Agence;

                case "Antenne":
                    return EntiteType.Antenne;

                default:
                    return EntiteType.Agence;
            }
        }

        public static UniteType ToUniteType(string value)
        {

            switch (value.Trim())
            {
                case "Direction":
                    return UniteType.Direction;

                case "Division":
                    return UniteType.Division;

                case "Bureau":
                    return UniteType.Bureau;

                default:
                    return UniteType.Bureau;
            }
        }

        public static FonctionEmployeType ToFonctionEmployeType(string value)
        {

            switch (value.Trim())
            {
                case "Officiel":
                    return FonctionEmployeType.Officiel;

                case "Interim":
                    return FonctionEmployeType.Interim;

                default:
                    return FonctionEmployeType.Officiel;
            }
        }

        public static FonctionState ToFonctionState(string value)
        {

            switch (value.Trim())
            {
                case "Running":
                    return FonctionState.Running;

                case "Pause":
                    return FonctionState.Pause;

                default:
                    return FonctionState.Running;
            }
        }

        public static GradeEmployeType ToGradeEmployeType(string value)
        {
            switch (value.Trim())
            {
                case "Officiel":
                    return GradeEmployeType.Officiel;

                case "Commissionnement":
                    return GradeEmployeType.Commissionnement;

                default:
                    return GradeEmployeType.Commissionnement;
            }
        }

        public static Sex ToSexeType(string value)
        {
            
            switch (value.Trim())
            {
                case "Femme":
                case "Féminin":
                    return Sex.Femme;

                case "Homme":
                case "Masculin":
                    return Sex.Homme;

                default:
                    return Sex.Homme;
            }
        }

        public static MecanisationType ToMecanisationType(string value)
        {

            switch (value)
            {
                case "Salaire":
                    return MecanisationType.Salaire;

                case "Prime":
                    return MecanisationType.Prime;

                default:
                    return MecanisationType.Prime;
            }
        }

        public static float FingerPrintsMatchingScore(byte[] fp1, byte[] fp2)
        {
            try
            {
                return zkfp2.DBMatch(new System.IntPtr(0x18193fb8), fp1, fp2);
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public static PositionType ToPositionType(string value)
        {

            switch (value.Trim())
            {
                case "Formation":
                    return PositionType.Formation;

                case "Alitement":
                    return PositionType.Alitement;

                case "Suspension":
                    return PositionType.Suspension;

                case "Detachement":
                    return PositionType.Detachement;

                case "Disponibilité":
                    return PositionType.Disponibilité;

                default:
                    return PositionType.Formation;
            }
        }

        public static SuspensionType ToSuspensionType(string value)
        {

            switch (value.Trim())
            {
                case "Formelle":
                    return SuspensionType.Formelle;

                case "Circonstancielle":
                    return SuspensionType.Circonstancielle;

                default:
                    return SuspensionType.Formelle;
            }
        }
    }
}
