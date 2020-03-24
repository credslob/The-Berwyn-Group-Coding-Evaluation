using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

//Connor Redslob
//The Berwyn Group Coding Evaluation


namespace BerwynCodingProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get test.csv file path
            var reader = new StreamReader(@"test.csv");

            //Iniitalize variables
            List<string> GUID = new List<string>();
            var GUIDSet = new HashSet<string>();
            List<string> IsDuplicateGUID = new List<string>();
            List<string> Val1Val2SumList = new List<string>();
            List<string> Val3 = new List<string>();
            var recordCount = 0;
            var maxVal1Val2Sum = 0;
            var maxVal1Val2SumGUID = "GUID";
            var val3LengthSum = 0;
            var averageVal3Length = 0;

            //Remove Title values from test.csv
            //bufferValues[0] = "GUID", bufferValues[1] = "Val1", bufferValues[2] = "Val2", bufferValues[3] = "Val3"
            var bufferLine = reader.ReadLine();
            var bufferValues = bufferLine.Split(',');

            //Read until the end of test.csv
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                //Detect and display if the current GUID is a duplicate
                GUID.Add(values[0]);
                if (!GUIDSet.Add(values[0]))
                {
                    IsDuplicateGUID.Add("Y");
                    Console.WriteLine("Duplicate GUID Found: " + values[0]);
                }
                else 
                {
                    IsDuplicateGUID.Add("N");
                }

                //Calculate the current sum of Val1 and Val2
                var val1Val2Sum = int.Parse(values[1].Trim(' ', '"')) + int.Parse(values[2].Trim(' ', '"'));
                Val1Val2SumList.Add(val1Val2Sum.ToString());

                //Check if the current sum of Val1 and Val2 is the greatest calculated so far
                if (val1Val2Sum > maxVal1Val2Sum)
                {
                    maxVal1Val2Sum = val1Val2Sum;
                    maxVal1Val2SumGUID = values[0];
                }

                //Calcluate the sum of the lengths of Val3 to use to calculate the average length
                Val3.Add(values[3]);
                val3LengthSum = val3LengthSum + values[3].Length;

                //Increment the number of records counted
                recordCount = recordCount + 1;
            }

            //Display the requested values
            Console.WriteLine("Total Number of Records: " + recordCount);
            Console.WriteLine("Largest Sum of Val1 and Val2: " + maxVal1Val2Sum);
            Console.WriteLine("GUID of Largest Sum of Val1 and Val2: " + maxVal1Val2SumGUID);

            //Calculate and display the average length of Val3
            averageVal3Length = val3LengthSum / recordCount;
            Console.WriteLine("Average Length of Val3: " + averageVal3Length);

            //Initialize the output file
            string outputFile = @"output.csv";

            //Initialize a stringBuilder to use for the output
            StringBuilder output = new StringBuilder();

            //For each Val3, check and record if its length is greater than the average length of Val3
            for (int i = 0; i < Val3.Count; i++)
            {
                string val3result = "N";
                if (Val3[i].Length > averageVal3Length)
                {
                    val3result = "Y";
                }

                //Add a line of values that need to be posted to the output file
                output.AppendLine(GUID[i] + ", " + Val1Val2SumList[i] + ", " + IsDuplicateGUID[i] + ", " + val3result);
            }

            //Print the title values for the columns in the output file
            File.WriteAllText(outputFile, "GUID, Val1+Val2, IsDuplicateGUID, Val3Length > AverageVal3Length");
            File.AppendAllText(outputFile, Environment.NewLine);

            //Print the stringBuilder of all the lines of result data to the output file
            File.AppendAllText(outputFile, output.ToString());
        }
    }
}
