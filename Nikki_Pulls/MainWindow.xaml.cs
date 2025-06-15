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
using System.Xml;

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
            MessageBox.Show("Please select the number of pulls and the star rating.");
            return;
        }

        if (!int.TryParse(txtAmountPulling.Text, out int amount) || amount <= 0)
        {
            MessageBox.Show("Please enter a valid number of pulls.");
            return;
        }

        if (!int.TryParse(cmbStars.SelectedItem?.ToString(), out int userInputStars) || userInputStars < 3 || userInputStars > 5)
        {
            MessageBox.Show("Please select a valid star amount (3, 4, or 5).");
            return;
        }

        string xmlPath = "PullsData.xml";
        var doc = new XmlDocument();

        try
        {
            doc.Load(xmlPath);
        }
        catch
        {
            var root = doc.CreateElement("Pulls");
            doc.AppendChild(root);
        }

        // pity of last 5⭐ pulls
        int pityCount = 0;
        var pulls = doc.SelectNodes("//Pull");

        if (pulls != null && pulls.Count > 0)
        {
            for (int i = pulls.Count - 1; i >= 0; i--)
            {
                var pull = pulls[i];
                if (int.TryParse(pull.SelectSingleNode("Stars")?.InnerText, out int stars))
                {
                    if (stars == 5)
                    {
                        break;
                    }
                    pityCount++;
                }
            }
        }

        
        pityCount++; // start since the next pull will be the first after the last 5⭐

        // count pulls since last 4⭐
        int pullsSinceLast4 = 0;
        for (int i = pulls.Count - 1; i >= 0; i--)
        {
            var pull = pulls[i];
            if (int.TryParse(pull.SelectSingleNode("Stars")?.InnerText, out int stars))
            {
                if (stars == 4 || stars == 5)
                    break;

                pullsSinceLast4++;
            }
        }

        // start pulls
        for (int i = 0; i < amount; i++)
        {
            int pullStars = userInputStars;

            // Pity logic
            if (pityCount >= 20)
            {
                pullStars = 5;
            }
            else if (pullsSinceLast4 >= 9) // next 10 should be 4
            {
                pullStars = 4;
            }

            // make node xml
            var pullElem = doc.CreateElement("Pull");

            var starsElem = doc.CreateElement("Stars");
            starsElem.InnerText = pullStars.ToString();
            pullElem.AppendChild(starsElem);

            var dateElem = doc.CreateElement("Date");
            dateElem.InnerText = DateTime.Now.ToString("dd-MM-yyyy");
            pullElem.AppendChild(dateElem);

            var pityElem = doc.CreateElement("Pity");
            pityElem.InnerText = pityCount.ToString();
            pullElem.AppendChild(pityElem);

            doc.DocumentElement.AppendChild(pullElem);

            // update counters
            if (pullStars == 5)
            {
                pityCount = 1;
                pullsSinceLast4 = 0;
            }
            else
            {
                pityCount++;
                if (pullStars == 4)
                    pullsSinceLast4 = 0;
                else
                    pullsSinceLast4++;
            }
        }

        // save xml
        try
        {
            doc.Save(xmlPath);
            MessageBox.Show("Pulls correctly saved.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving XML: {ex.Message}");
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




