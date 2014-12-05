using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoordinateTransformation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Enter example input
            string sampleInput = "";
            sampleInput += ";Use the ';' character to add comments to the input\r\n";
            sampleInput += String.Format("{0,-30} ;The number of common points\r\n", 3);
            sampleInput += String.Format("{0,-15}{1,-15} ;The common points for the coordinate system the unknown points are being transformed to\r\n", 1049422.40, 51089.20);
            sampleInput += String.Format("{0,-15}{1,-15}\r\n{2,-15}{3,-15}\r\n", 1049413.95, 49659.30, 1049244.95, 49884.95);
            sampleInput += String.Format("{0,-15}{1,-15} ;The common points for the coordinate system that the unknown points are being transformed from\r\n", 121.622, -128.066);
            sampleInput += String.Format("{0,-15}{1,-15}\r\n{2,-15}{3,-15}\r\n", 141.228, 187.718, 175.802, 135.728);
            sampleInput += String.Format("{0,-30} ;The number of unknown points for which to solve\r\n", 4);
            sampleInput += String.Format("{0,-15}{1,-15} ;The ponts to transform\r\n", 174.148, -120.262);
            sampleInput += String.Format("{0,-15}{1,-15}\r\n{2,-15}{3,-15}\r\n{4,-15}{5,-15}", 513.520, -192.130, 754.444, -67.706, 972.788, 120.994);

            // Comment to test git
            inputTextBox.Text = sampleInput;
        }

        private void openButtonClick(object sender, RoutedEventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "dat files (*.dat)|*.dat|txt files (*.txt)|*.txt";
            openFileDialog1.DefaultExt = ".dat";
            openFileDialog1.RestoreDirectory = true;

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDialog1.ShowDialog();
            if (result == true)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                        
                            string fileName = openFileDialog1.FileName;
                            string fileText = File.ReadAllText(fileName);
                            inputTextBox.Text = fileText;
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void saveButtonClicked(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            StreamWriter myStreamWriter;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string textToSave;
            string selectedButton = (sender as Button).Content.ToString();

            if (selectedButton == "Save Input")
            {
                textToSave = inputTextBox.Text;
            }
            else
            {
                textToSave = outputTextBox.Text;
            }

            saveFileDialog1.Filter = "dat files (*.dat)|*.dat|txt files (*.txt)|*.txt";
            saveFileDialog1.DefaultExt = ".dat";
            saveFileDialog1.RestoreDirectory = true;
           
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = saveFileDialog1.ShowDialog();
            
            if (result == true)
            {
                using (myStream = File.Open(saveFileDialog1.FileName,FileMode.Create))
                using (myStreamWriter = new StreamWriter(myStream))
                {
                    myStreamWriter.Write(textToSave);
                }
            }
        }

        private void runButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                outputTextBox.Text = "";
                string input = inputTextBox.Text;

                // Remove all comments (marked by a ;), start with an array of all lines of text
                var lines = input.Split(new[] { "\r\n" }, StringSplitOptions.None);
                var sb = new StringBuilder();
                foreach (var line in lines.Select(t => t.Trim())
                                          .Where(line => (line != string.Empty) && !Regex.IsMatch(line, @"^\s*;(.*)$")))
                {
                    sb.AppendLine(Regex.IsMatch(line, @"^(.*);(.*)$") ? line.Substring(0, line.IndexOf(';')).Trim() : line);
                }
                input = sb.ToString();

                string[] inputArray = input.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
                int commonCoordinates = int.Parse(inputArray[0]);

                if (commonCoordinates <= 1)
                {
                    throw new InvalidOperationException("MUST PROVIDE A MINIMUM OF TWO COMMON COORDINATES");
                }
                int unknownCoordinates = int.Parse(inputArray[(4 * commonCoordinates) + 1]);
                string lMatrixString = "", aMatrixString = "",
                    unknownCoordinatesString = "", xyCoordinatePairs = "",
                    uvCoordinatePairs = "", unknownCoordinatePairs = "";
                double currentX, currentY;
                // Build the l matrix string
                for (int index = 1; index <= (2 * commonCoordinates); index++)
                {
                    lMatrixString += inputArray[index] + "\r\n";
                    xyCoordinatePairs += String.Format("{0,15:0.000}", double.Parse(inputArray[index]));
                    if (index % 2 == 0)
                    {
                        xyCoordinatePairs += "\r\n";
                    }
                }

                // Build the a matrix string
                for (int index = (2 * commonCoordinates) + 1; index <= (4 * commonCoordinates); index += 2)
                {
                    currentX = double.Parse(inputArray[index]);
                    currentY = double.Parse(inputArray[index + 1]);
                    aMatrixString += currentX.ToString() + " " + (-currentY).ToString() + " 1 0\r\n";
                    aMatrixString += currentY.ToString() + " " + currentX.ToString() + " 0 1\r\n";
                    uvCoordinatePairs += String.Format("{0,15:0.000}{1,15:0.000}\r\n", currentX, currentY);
                }

                // Build the unknown coordinates matrix string
                for (int index = (4 * commonCoordinates) + 2; index < inputArray.Length; index += 2)
                {
                    currentX = double.Parse(inputArray[index]);
                    currentY = double.Parse(inputArray[index + 1]);
                    unknownCoordinatesString += currentX.ToString() + " " + (-currentY).ToString() + " 1 0\r\n";
                    unknownCoordinatesString += currentY.ToString() + " " + currentX.ToString() + " 0 1\r\n";
                }

                // Build the A L and unknownCoordinate matrices
                Matrix aMatrix = Matrix.Parse(aMatrixString);
                Matrix lMatrix = Matrix.Parse(lMatrixString);
                Matrix unknownCoordinateMatrix = Matrix.Parse(unknownCoordinatesString);

                // inverseN = (AT*A)-1
                Matrix inverseN = (Matrix.Transpose(aMatrix) * aMatrix).Invert();

                // X = inverseN * AT * L
                Matrix xMatrix = inverseN * Matrix.Transpose(aMatrix) * lMatrix;

                // V = A*X - L
                Matrix vMatrix = (aMatrix * xMatrix) - lMatrix;

                // Transformed unknown coordinates = unknownCoordinateMatrix * X
                Matrix converted = unknownCoordinateMatrix * xMatrix;

                // Build the unknown coordinate pair string
                for (int index = 0; index < unknownCoordinateMatrix.rows; index += 2)
                {
                   // unknownCoordinatePairs += String.Format("{0:0.000}", unknownCoordinateMatrix[index, 0]) + "\t" + String.Format("{0:0.000}", unknownCoordinateMatrix[index + 1, 0]) + "\t" +
                   //     String.Format("{0:0.000}", converted[index, 0]) + "\t" + String.Format("{0:0.000}", converted[index + 1, 0]) + "\n";
                    unknownCoordinatePairs += String.Format("{0, -15:0.000}{1, -15:0.000}{2, -15:0.000}{3, -15:0.000}\r\n", unknownCoordinateMatrix[index, 0], unknownCoordinateMatrix[index + 1, 0], converted[index, 0], converted[index + 1, 0]);
                }

                double theta, a = xMatrix[0, 0], b = xMatrix[1, 0];

                // Calculate theta
                if (a < 0)
                {
                    theta = Math.Atan(b / a) * 180.0 / 3.14159265 + 180.0;
                }
                else if (b < 0)
                {
                    theta = 360.0 - Math.Atan(b / a) * 180.0 / 3.14159265;
                }
                else
                {
                    theta = Math.Atan(b / a) * 180.0 / 3.14159265;
                }
                double scaleFactor = a / Math.Cos(theta * 3.14159265 / 180.0);
                double variance = (Matrix.Transpose(vMatrix) * vMatrix)[0, 0] / 2;
                double standardDeviation = Math.Sqrt(variance);
                double errorA = standardDeviation * Math.Sqrt(inverseN[0, 0]);
                double errorB = standardDeviation * Math.Sqrt(inverseN[1, 1]);
                double errorTa = standardDeviation * Math.Sqrt(inverseN[2, 2]);
                double errorTb = standardDeviation * Math.Sqrt(inverseN[3, 3]);

                // Generate the output
                outputTextBox.Text += String.Format("THE NUMBER OF COMMON PAIRS IS {0}\r\n\r\n", commonCoordinates);
                outputTextBox.Text += "THE X AND Y COORDINATE PAIRS ARE:\r\n" + xyCoordinatePairs + "\r\n";
                outputTextBox.Text += "THE U AND V COORDINATE PAIRS ARE:\r\n" + uvCoordinatePairs + "\r\n";
                outputTextBox.Text += "THE RESIDUAL MATRIX IS:\r\n" + vMatrix.ToString() + "\r\n";
                outputTextBox.Text += "TRANSFORMATION PARAMETERS:\r\n\r\n";

                outputTextBox.Text += String.Format("THE a VALUE EQUALS: {0:0.0000} \u00B1 {1:0.000e+00}\r\n", a, errorA);
                outputTextBox.Text += String.Format("THE b VALUE EQUALS: {0:0.0000} \u00B1 {1:0.000e+00}\r\n", b, errorB);
                outputTextBox.Text += String.Format("THE TX VALUE EQUALS: {0:0.0000} \u00B1 {1:0.000e+00}\r\n", xMatrix[2, 0], errorTa);
                outputTextBox.Text += String.Format("THE TY VALUE EQUALS: {0:0.0000} \u00B1 {1:0.000e+00}\r\n\r\n", xMatrix[3, 0], errorTb);
                outputTextBox.Text += String.Format("THE SCALE VALUE EQUALS: {0:0.000000000}\r\n", scaleFactor);
                outputTextBox.Text += String.Format("THE THETA VALUE EQUALS: {0:0.000000000} DEGREES\r\n\r\n", theta);

                outputTextBox.Text += String.Format("THE VARIANCE EQUALS: {0:0.000}\r\n", variance);
                outputTextBox.Text += String.Format("THE STANDARD DEVIATION EQUALS: {0:0.000}\r\n\r\n", standardDeviation);
                outputTextBox.Text += String.Format("THE NEW COORDINATES OF THE TRANSFORMED POINTS ARE: \r\n\r\n{0,-15}{1,-15}{2,-15}{3,-15}\r\n", "U", "V", "X", "Y");
                outputTextBox.Text += unknownCoordinatePairs;
            }
            catch (FormatException)
            {
                outputTextBox.Text = "INVALID INPUT";
            }
            catch (IndexOutOfRangeException)
            {
                outputTextBox.Text = "INVALID INPUT";
            }
            catch (InvalidOperationException iOEx)
            {
                outputTextBox.Text = iOEx.Message;
            }
            catch (Exception)
            {
                outputTextBox.Text = "UNIDENTIFIED ERROR";
            }
        }


    }
}
