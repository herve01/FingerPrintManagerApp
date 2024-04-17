using System.Collections.Generic;

namespace FingerPrintManagerApp.Model.Admin.Util
{
    public class DbUtil
    {
        #region Roles utilisateurs
        public enum Entity
        {
            Entite,
            Direction_Provinciale,
            Utilisateur,
            Acte_Nomination,
            Domaine_Etude,
            Niveau_Etude,
            Direction,
            Bureau,
            Division,
            Grade,
            Fonction,
            Taux,
            Employé,
            Application,
            Mécanisation,
            Recensement,
            Suspension,
            Formation,
            Détachement,
            Nomination_Grade,
            Promotion,
            Décès,
            Retraite,
            Affectation,
            Alitement,
            Disponibilite,
            Reprise_Changement,
            Presence
        }

        public enum Objet
        {
            SHOP,
            LOCAL,
            CABINET,
            SHELF,
            DRAWER,
            FOLDER,
            DOCUMENT,
            TOUS_ARCHIVE,
            DIFFUSION,
            TOUS

        }

        public enum Package
        {
            TOUS,
            DIFFUSION,
            ARCHIVE
        }

        public enum Privilege
        {
            AJOUTER,
            MODIFIER,
            SUPPRIMER,
            RESTAURER,
            IMPRIMER,
            SCANNER,
            VISUALISER,
            DEPLACER,
            OUVRIR,
            LIRE_BLOQUES,
            SUPPRIMER_BLOQUES,
            EXPORTER,
            BLOQUER,
            TOUS
        }

        private static SortedList<int, Package> PackageMapList = new SortedList<int, Package>()
        {
            { 1, Package.ARCHIVE },
            { 2, Package.DIFFUSION },
            { 3, Package.TOUS }
        };

        public static SortedList<int, Privilege> PrivilegeMapList = new SortedList<int, Privilege>()
        {
            { 1, Privilege.AJOUTER },
            { 2, Privilege.MODIFIER },
            { 3, Privilege.SUPPRIMER },
            { 4, Privilege.VISUALISER },
            { 5, Privilege.IMPRIMER },
            { 6, Privilege.DEPLACER },
            { 7, Privilege.SCANNER },
            { 8, Privilege.TOUS },
            { 9, Privilege.OUVRIR },
            { 10, Privilege.EXPORTER },
            { 11, Privilege.BLOQUER },
            { 12, Privilege.LIRE_BLOQUES },
            { 13, Privilege.SUPPRIMER_BLOQUES },
        };

        private static SortedList<int, Objet> ObjetMapList = new SortedList<int, Objet>()
        {
            { 1, Objet.LOCAL },
            { 2, Objet.CABINET },
            { 3, Objet.SHELF },
            { 4, Objet.DRAWER },
            { 5, Objet.FOLDER },
            { 6, Objet.DOCUMENT },
            { 7, Objet.TOUS_ARCHIVE },
            { 8, Objet.TOUS },
            { 9, Objet.DIFFUSION }
        };
        
        #endregion
    }
}
