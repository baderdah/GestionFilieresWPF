using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetGestionFilieres.Model
{
   
    public class ManagerDB
    {
       public static DataClasses1DataContext cl = new DataClasses1DataContext();
        const string LOGIN = "admin";
        const string PASSWORD = "admin";

        public static Boolean checkLogin(string login, string pass)
        {
            if (login == LOGIN && pass == PASSWORD)
                return true;
            return false;
        }

        public static string getResponsable(string nomFil)
        {
            var x = (from f in cl.Filieres
                    where f.Nom_filiere == nomFil
                    select f.Responsable).FirstOrDefault();
            return x;
        }

        public static int getNbEtds()
        {
            int c = 0;
            var x = from etd in cl.etudiants
                    select etd;
            foreach (etudiant etd in x)
                c++;
            return c;
        }

        public static int nbEtdParFil(int idFil)
        {
            int c = 0;
            var x = (from etd in cl.etudiants
                     where etd.id_fil == idFil
                     select etd);
            foreach (etudiant etd in x)
                c++;
            return c;
        }
    }
}
