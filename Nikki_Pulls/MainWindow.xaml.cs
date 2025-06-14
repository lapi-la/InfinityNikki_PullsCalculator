using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nikki_Pulls;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
   

    public MainWindow()
    {
        InitializeComponent();
        //the init of the combo boxes.
        cmbItemsCount.Items.Clear();
        cmbItemsCount.Items.Add(9);
        cmbItemsCount.Items.Add(10);
        cmbItemsCount.Items.Add(11);

        cmbStars.Items.Clear();
        cmbStars.Items.Add(3);
        cmbStars.Items.Add(4);
        cmbStars.Items.Add(5);

        cmbStars.SelectedIndex = -1;
        cmbItemsCount.SelectedIndex = -1;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void btnHistory_Click(object sender, RoutedEventArgs e)
    {
        var historyWindow = new History(); //history of pulls 
        historyWindow.ShowDialog();
    }

    private void btnMakePulls_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtAmountPulling.Text) || cmbStars.SelectedIndex == -1)
        {
            MessageBox.Show("Please select the number of pulls and the star rating.");  //Error messages if the user does not select the amount of pulls , the star rating or valid numbers also cmb valid star.
            return;
        }

        if (!int.TryParse(txtAmountPulling.Text, out int amount) || amount <= 0)
        {
            MessageBox.Show("Please enter a valid number of pulls.");
            return;
        }

        if (!int.TryParse(cmbStars.SelectedItem?.ToString(), out int stars) || (stars < 3 || stars > 5))
        {
            MessageBox.Show("Please select a valid star amount (3, 4, or 5).");
            return;
        }

        var xmlPath = "PullsData.xml"; // Where data is saved for the history of pulls.
        var doc = new System.Xml.XmlDocument();
        try
        {
            doc.Load(xmlPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading the XML file: {ex.Message}");
            return;
        }

        int pullsSinceLast4 = 0;
        int pullsSinceLast5 = 0;
        int pullsToNext5After4 = -1; // This means theres no 4-star pity active

        // Read Pity History 
        var pulls = doc.SelectNodes("//Pull");
        if (pulls != null)
        {
            for (int i = pulls.Count - 1; i >= 0; i--)
            {
                var pull = pulls[i];
                var starsNode = pull.SelectSingleNode("Stars");
                if (starsNode != null && int.TryParse(starsNode.InnerText, out int prevStars))
                {
                    if (prevStars == 5)
                    {
                        pullsSinceLast5 = 0;
                        pullsSinceLast4 = 0;
                        pullsToNext5After4 = -1;
                        break; // Pity Resets after 5 stars
                    }
                    else
                    {
                        pullsSinceLast5++;
                        if (prevStars == 4)
                        {
                            pullsSinceLast4 = 0;
                            pullsToNext5After4 = 10; // after 4 stars there's a 5-star pity in 10 pulls
                        }
                        else
                        {
                            pullsSinceLast4++;
                            if (pullsToNext5After4 > -1)
                                pullsToNext5After4--;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < amount; i++)
        {
            pullsSinceLast4++;
            pullsSinceLast5++;

            int pullStars = stars;

            // force 5 stars if pity conditions are met ( if user insert 4 stars manually )
            if (pullsToNext5After4 == 0)
            {
                pullStars = 5;
                pullsSinceLast5 = 0;
                pullsSinceLast4 = 0;
                pullsToNext5After4 = -1;
            }
            // make sure 5 stars every 20 pulls
            else if (pullsSinceLast5 >= 20)
            {
                pullStars = 5;
                pullsSinceLast5 = 0;
                pullsSinceLast4 = 0;
                pullsToNext5After4 = -1;
            }
            // make sure 4 stars every 10 pulls
            else if (pullsSinceLast4 >= 10)
            {
                pullStars = 4;
                pullsSinceLast4 = 0;
                pullsToNext5After4 = 10; // after 4 stars, the next 5 stars will be in 10 pulls
            }
            // if user selects 3 stars but pity conditions are met , force 4 or 5 stars
            else if (stars == 3)
            {
                if (pullsSinceLast5 == 20)
                {
                    pullStars = 5;
                    pullsSinceLast5 = 0;
                    pullsSinceLast4 = 0;
                    pullsToNext5After4 = -1;
                }
                else if (pullsSinceLast4 == 10)
                {
                    pullStars = 4;
                    pullsSinceLast4 = 0;
                    pullsToNext5After4 = 10;
                }
            }

           //this are for the user if does stuff manually just in case.
            if (stars == 4 && pullsToNext5After4 == -1)
            {
                pullsToNext5After4 = 10;
            }

            var pullElem = doc.CreateElement("Pull");

            var starsElem = doc.CreateElement("Stars");
            starsElem.InnerText = pullStars.ToString();
            pullElem.AppendChild(starsElem);

            var dateElem = doc.CreateElement("Date");
            dateElem.InnerText = DateTime.Now.ToString("dd-MM-yyyy");
            pullElem.AppendChild(dateElem);

            var pityElem = doc.CreateElement("Pity");
            pityElem.InnerText = (i + 1).ToString();
            pullElem.AppendChild(pityElem);

            doc.DocumentElement.AppendChild(pullElem);
        }

        try
        {
            doc.Save(xmlPath);
            MessageBox.Show("Pulls Correctly Saved.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading the XML file:: {ex.Message}");
        }
    }

  

    private void cmbItemsCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cmbItemsCount.SelectedItem == null)
        {
            txtDiamondsPullsNeed.Text = string.Empty;
            return;
        }

        if (int.TryParse(cmbItemsCount.SelectedItem.ToString(), out int items))
        {
            switch (items)
            {
                case 9:
                    txtDiamondsPullsNeed.Text = "180";  //this are the values of each outfit 9 pieces = 180 pulls maximum
                    break;
                case 10:
                    txtDiamondsPullsNeed.Text = "200"; //this are the values of each outfit 10 pieces = 200 pulls maximum
                    break;
                case 11:
                    txtDiamondsPullsNeed.Text = "220"; //this are the values of each outfit 11 pieces = 220 pulls maximum
                    break;
                default:
                    txtDiamondsPullsNeed.Text = string.Empty;
                    break;
            }
        }
        else
        {
            txtDiamondsPullsNeed.Text = string.Empty;
            txtDiamondsPulls.Text = "Updated";
        }

      
    }
 

    private void txtDiamondsPullsNeed_TextChanged(object sender, TextChangedEventArgs e)
    {
        // make controls validated and initialized before using them
        if (txtDiamonds == null || txtCrystals == null || txtStellarite == null ||
            txtDiamondsPulls == null || txtCrystalsPulls == null || txtStellaritePulls == null ||
            txtTotalPulls == null || txtDiamondsPullsNeed == null || txtTotalPullsNeeded == null)
            return;

        int diamonds = 0, crystals = 0, stellarite = 0;
        int diamondsPulls = 0, crystalsPulls = 0, stellaritePulls = 0;

        // read and calculate pulls from diamonds
        if (int.TryParse(txtDiamonds.Text, out diamonds) && diamonds >= 0)
            diamondsPulls = diamonds / 120;

        // read and calculate pulls from crystals
        if (int.TryParse(txtCrystals.Text, out crystals) && crystals >= 0)
            crystalsPulls = crystals;

        // read and calculate pulls from Sterlarite
        if (int.TryParse(txtStellarite.Text, out stellarite) && stellarite >= 0)
            stellaritePulls = stellarite / 120;

        // Calculate all pulls
        int totalPulls = diamondsPulls + crystalsPulls + stellaritePulls;

        // Calculate needed pulls 
        int pullsNeeded = 0;
        int.TryParse(txtDiamondsPullsNeed.Text, out pullsNeeded);
        int pullsLeft = pullsNeeded - totalPulls;
        txtTotalPullsNeeded.Text = pullsLeft.ToString();
    }

    private void ResourcePulls_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Reused code too lazy to change it sorry 
        if (txtDiamonds == null || txtCrystals == null || txtStellarite == null ||
            txtDiamondsPulls == null || txtCrystalsPulls == null || txtStellaritePulls == null ||
            txtTotalPulls == null || txtDiamondsPullsNeed == null || txtTotalPullsNeeded == null)
            return;

        int diamonds = 0, crystals = 0, stellarite = 0;
        int diamondsPulls = 0, crystalsPulls = 0, stellaritePulls = 0;

       
        if (int.TryParse(txtDiamonds.Text, out diamonds) && diamonds >= 0)
            diamondsPulls = diamonds / 120;

    
        if (int.TryParse(txtCrystals.Text, out crystals) && crystals >= 0)
            crystalsPulls = crystals;

        if (int.TryParse(txtStellarite.Text, out stellarite) && stellarite >= 0)
            stellaritePulls = stellarite / 120;

        // show individual pulls 
        txtDiamondsPulls.Text = diamondsPulls > 0 ? diamondsPulls.ToString() : "0";
        txtCrystalsPulls.Text = crystalsPulls > 0 ? crystalsPulls.ToString() : "0";
        txtStellaritePulls.Text = stellaritePulls > 0 ? stellaritePulls.ToString() : "0";

        int totalPulls = diamondsPulls + crystalsPulls + stellaritePulls;
        txtTotalPulls.Text = totalPulls.ToString();

 
        int pullsNeeded = 0;
        int.TryParse(txtDiamondsPullsNeed.Text, out pullsNeeded);
        int pullsLeft = pullsNeeded - totalPulls;
        txtTotalPullsNeeded.Text = pullsLeft.ToString();
    }

    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        txtDiamondsPulls.Text = null;
        txtCrystalsPulls.Text = null;
        txtStellaritePulls.Text = null;
        txtTotalPulls.Text = null;
        txtDiamonds.Text = null;
        txtCrystals.Text = null;
        txtStellarite.Text = null;
        txtDiamondsPullsNeed.Text = null;
        cmbStars.SelectedIndex = -1;
       
    }

    private void btnhelp_Click(object sender, RoutedEventArgs e)
    {
        var helpForm = new Help(); //helpform
        helpForm.ShowDialog();
    }
}




