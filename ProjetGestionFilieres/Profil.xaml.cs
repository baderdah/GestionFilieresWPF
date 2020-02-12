using ProjetGestionFilieres.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Charting;
using Telerik.Windows.Controls;

namespace ProjetGestionFilieres
{
    /// <summary>
    /// Interaction logic for Profil.xaml
    /// </summary>
    public partial class Profil : Window
    {
        DataClasses1DataContext cl;
        ObservableCollection<etudiant> collectionStudents;
        ObservableCollection<Filiere> collectionFilieres;
        public Profil()
        {
            InitializeComponent();

        }


        private void profilLoaded(object sender, RoutedEventArgs e)
        {
            cl = ManagerDB.cl;
            collectionStudents = new ObservableCollection<etudiant>(cl.etudiants.ToList());

            collectionFilieres = new ObservableCollection<Filiere>(cl.Filieres.ToList());
            studentsListView.ItemsSource = collectionStudents;
            foreach (Filiere f in collectionFilieres)
            {
                filiereComboBox.Items.Add(f.Nom_filiere);
            }

            carouselFiliere.ItemsSource = collectionFilieres;
            carouselFiliere.SelectedItem = carouselFiliere.Items[0];
            fillChart();

        }

        private void filiereComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFil = filiereComboBox.Items[filiereComboBox.SelectedIndex].ToString();
            studentsListView.ItemsSource = collectionStudents.Where(etd => etd.Filiere.Nom_filiere == selectedFil);
            nomFil.Content = selectedFil;
            responsableFil.Content = ManagerDB.getResponsable(selectedFil);

        }

        private void filiereView(object sender, RoutedEventArgs e)
        {
            studentView.Visibility = Visibility.Hidden;
            statistiquesPane.Visibility = Visibility.Hidden;
            viewFiliere.Visibility = Visibility.Visible;
        }

        private void PageStudents_Click(object sender, RoutedEventArgs e)
        {
            viewFiliere.Visibility = Visibility.Hidden;
            statistiquesPane.Visibility = Visibility.Hidden;
            studentView.Visibility = Visibility.Visible;

        }

        private void Satistiques_Click(object sender, RoutedEventArgs e)
        {
            studentView.Visibility = Visibility.Hidden;
            viewFiliere.Visibility = Visibility.Hidden;
            statistiquesPane.Visibility = Visibility.Visible;
        }

        private void menuAddFiliere_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            (sender as RadRadialMenuItem).IsSelected = false;

            formNewFiliere.Visibility = Visibility.Visible;
        }

        private void menuUpdateFiliere_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            (sender as RadRadialMenuItem).IsSelected = false;

            formUpdateFiliere.Visibility = Visibility.Visible;
            Filiere f = carouselFiliere.SelectedItem as Filiere;
            filiereUpdatedName.Text = f.Nom_filiere;
            filiereUpdatedManager.Text = f.Responsable;
        }

        private void menuDeleteFiliere_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            (sender as RadRadialMenuItem).IsSelected = false;

            Filiere f = carouselFiliere.SelectedItem as Filiere;

            if (f != null)
            {
                Filiere rf = (from fil in cl.Filieres
                              where fil.Id_filiere == f.Id_filiere
                              select fil).FirstOrDefault();
                cl.Filieres.DeleteOnSubmit(rf);
                cl.SubmitChanges();
                collectionFilieres.Remove(f);
            }
            else
            {
                MessageBox.Show(this, "Veuillez selectionnez une filière");
            }

        }

        private void submitNewFiliere_Click(object sender, RoutedEventArgs e)
        {
            Filiere f = new Filiere { Nom_filiere = newFiliereName.Text, Responsable = newFiliereManager.Text };
            cl.Filieres.InsertOnSubmit(f);
            cl.SubmitChanges();
            collectionFilieres.Add(f);
            formNewFiliere.Visibility = Visibility.Hidden;
        }

        private void resetNewFiliere_Click(object sender, RoutedEventArgs e)
        {
            newFiliereName.Text = "";
            newFiliereManager.Text = "";
            formNewFiliere.Visibility = Visibility.Hidden;

        }

        private void updateFiliere_Click(object sender, RoutedEventArgs e)
        {
            Filiere f = carouselFiliere.SelectedItem as Filiere;

            if (f != null)
            {
                Filiere rf = (from fil in cl.Filieres
                              where fil.Id_filiere == f.Id_filiere
                              select fil).FirstOrDefault();
                rf.Nom_filiere = filiereUpdatedName.Text;
                rf.Responsable = filiereUpdatedManager.Text;
                cl.SubmitChanges();
                collectionFilieres.Remove(f);
            }
            else
            {
                MessageBox.Show(this, "Veuillez selectionnez une filière");
            }
            formUpdateFiliere.Visibility = Visibility.Hidden;
            
            carouselFiliere.ItemsSource = collectionFilieres;
        }

        private void resetUpdateFiliere_Click(object sender, RoutedEventArgs e)
        {
            filiereUpdatedName.Text = "";
            filiereUpdatedManager.Text = "";
            formUpdateFiliere.Visibility = Visibility.Hidden;
        }
        private void deleteStudent(object sender, RoutedEventArgs e)
        {
            etudiant etd = studentsListView.SelectedItem as etudiant;
            if (etd != null)
            {
                
                cl.etudiants.DeleteOnSubmit(etd);
                collectionStudents.Remove(etd);
                cl.SubmitChanges();
                MessageBox.Show("Etudiant supprimé");
                collectionStudents.Remove(etd);
                studentsListView.ItemsSource = collectionStudents;
            }
            else
            {
                MessageBox.Show(this, "Veuillez selectionnez un étudiant");
            }
            

        }
        private void updateStudent(object sender, RoutedEventArgs e)
        {
            etudiant etd = studentsListView.SelectedItem as etudiant;
            
            if (etd != null)
            {
                
                updateStudentForm.Visibility = Visibility.Visible;
                newstudentCne.Text = etd.cne.ToString();
                newstudentName.Text = etd.nom;
                newstudentLastName.Text = etd.prenom;
                newdate_naissStudent.DisplayDate = etd.date_naiss.Value; 
            }
            else
            {
                MessageBox.Show(this, "Veuillez selectionnez un étudiant");
            }
            

        }

        private void addStudent(object sender, RoutedEventArgs e)
        {
            addStudentForm.Visibility = Visibility.Visible;
            
        }
        private void saveStudent(object sender, RoutedEventArgs e) {
            string nomfil = filiereComboBox.SelectedItem.ToString();
            int idfil = (from f in cl.Filieres where nomfil.Equals(f.Nom_filiere) select f.Id_filiere).SingleOrDefault();
            int cne =Int32.Parse(studentCne.Text);
            string nom = studentName.Text;
            string prenom = studentLastName.Text;
            string sexe = sexeStudent.SelectedItem.ToString();
            char sexeEt ='F';
            if (sexe.Equals("F"))
            {
                sexeEt = 'F';
            }
            else if (sexe.Equals("M"))
            {
                sexeEt = 'M';
            }
            
            string date = date_naissStudent.SelectedDate.ToString();
            etudiant etudiant = new etudiant();
            etudiant.id_fil = idfil;
            etudiant.cne = cne;
            etudiant.nom = nom;
            etudiant.prenom = prenom;
            etudiant.sexe =sexeEt;
            etudiant.date_naiss = DateTime.Parse(date);
            
            cl.etudiants.InsertOnSubmit(etudiant);
            cl.SubmitChanges();
            collectionStudents.Add(etudiant);
            studentsListView.ItemsSource = collectionStudents;
            addStudentForm.Visibility = Visibility.Hidden;

        }
        private void fillChart()
        {
            int nbEtds = ManagerDB.getNbEtds();
            ObservableCollection<Plotinfo> plotinfos = new ObservableCollection<Plotinfo>();

            foreach (Filiere f in cl.Filieres)
            {
                string x = f.Nom_filiere;
                int y = ManagerDB.nbEtdParFil(f.Id_filiere);
                double z = Convert.ToDouble(y) / nbEtds;
                plotinfos.Add(new Plotinfo(x, y, z * 100));
            }

            barSeries.ItemsSource = plotinfos;
        }

        private void ResetProfil_Click(object sender, RoutedEventArgs e)
        {
            updateStudentForm.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addStudentForm.Visibility = Visibility.Hidden;
        }
    }
}
