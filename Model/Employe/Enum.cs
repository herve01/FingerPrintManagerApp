namespace FingerPrintManagerApp.Model.Employe
{
    public enum GradeType
    {
        Agent,
        Cadre,
        Haut_cadre
    }

    public enum UniteType
    {
        Direction,
        Division,
        Bureau
    }

    public enum FonctionEmployeType
    {
        Officiel,
        Interim
    }

    public enum GradeEmployeType
    {
        Commissionnement,
        Officiel
    }

    public enum MecanisationType
    {
        Salaire,
        Prime
    }

    public enum BureauType
    {
        Bureau,
        Sécretariat
    }

    public enum EntiteType
    {
        Agence,
        Antenne
    }

    public enum ActeType
    {
        Engagement,
        Arrêté,
        Décret,
        Ordonnance,
        Notification,
        Affectation
    }

    public enum ExtensionFile
    {
        PDF,
        IMAGE
    }

    public enum Fingers
    {
        LL,
        LR,
        LM,
        LI,
        LT,
        RT,
        RI,
        RM,
        RR,
        RL
    }

    public enum PositionType
    {
        Formation,
        Alitement,
        Suspension,
        Detachement,
        Disponibilité
    }

    public enum SuspensionType
    {
        Formelle,
        Circonstancielle
    }

    public enum FonctionState
    {
        Running,
        Pause
    }
}
