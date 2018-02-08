using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataSets
{
    public class CustomerDS
    {
        public void WorkingWithDataSets()
        {
            Console.WriteLine("***** Fun with DataSets *****\n");

            // Create the DataSet object and add a few properties.
            DataSet carsInventoryDS = new DataSet("Car Inventory");
            carsInventoryDS.ExtendedProperties["TimeStamp"] = DateTime.Now;
            carsInventoryDS.ExtendedProperties["DataSetID"] = Guid.NewGuid();
            carsInventoryDS.ExtendedProperties["Company"] = "Mikko’s Hot Tub Super Store";

            Console.ReadLine();
        }

        public void FillDataSet(DataSet ds)
        {
            DataColumn carIDColumn = new DataColumn("CarID", typeof(int));
            carIDColumn.Caption = "Car ID";
            carIDColumn.ReadOnly = true;
            carIDColumn.AllowDBNull = false;
            carIDColumn.Unique = true;
            carIDColumn.AutoIncrement = true;
            carIDColumn.AutoIncrementSeed = 0;
            carIDColumn.AutoIncrementStep = 1;

            DataColumn carMakeColumn = new DataColumn("Make", typeof(string));
            DataColumn carColorColumn = new DataColumn("Color", typeof(string));
            DataColumn carPetNameColumn = new DataColumn("PetName", typeof(string));

            carPetNameColumn.Caption = "Pet Name";

            DataTable inventoryTable = new DataTable("Inventory");
            inventoryTable.Columns.AddRange(new DataColumn[] { carIDColumn, carMakeColumn, carColorColumn, carPetNameColumn });

            DataRow carRow = inventoryTable.NewRow();
            carRow["Make"] = "BMW";
            carRow["Color"] = "Black";
            carRow["PetName"] = "Hamlet";
            inventoryTable.Rows.Add(carRow);
            carRow = inventoryTable.NewRow();

            // Column 0 is the autoincremented ID field,
            // so start at 1.
            carRow[1] = "Saab";
            carRow[2] = "Red";
            carRow[3] = "Sea Breeze";
            inventoryTable.Rows.Add(carRow);

            ds.Tables.Add(inventoryTable);
        }

        public void PrintDataSet(DataSet ds)
        {
            Console.WriteLine("DataSet is named: {0}", ds.DataSetName);

            foreach (System.Collections.DictionaryEntry de in ds.ExtendedProperties)
            {
                Console.WriteLine("Key = {0}, Value = {1}", de.Key, de.Value);
            }

            Console.WriteLine();

            foreach (DataTable dt in ds.Tables)
            {
                Console.WriteLine("=> {0} Table:", dt.TableName);
                // Print out the column names.
                for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
                {
                    Console.Write(dt.Columns[curCol].ColumnName + "\t");
                }

                Console.WriteLine("\n----------------------------------");

                for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
                {
                    for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
                    {
                        Console.Write(dt.Rows[curRow][curCol].ToString() + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }

        public void ManipulateDataRowState()
        {
            // Create a temp DataTable for testing.
            DataTable temp = new DataTable("Temp");
            temp.Columns.Add(new DataColumn("TempColumn", typeof(int)));

            // RowState = Detached (i.e., not part of a DataTable yet).
            DataRow row = temp.NewRow();
            Console.WriteLine("After calling NewRow(): {0}", row.RowState);

            // RowState = Added.
            temp.Rows.Add(row);
            Console.WriteLine("After calling Rows.Add(): {0}", row.RowState);

            // RowState = Added.
            row["TempColumn"] = 10;
            Console.WriteLine("After first assignment: {0}", row.RowState);

            // RowState = Unchanged.
            temp.AcceptChanges();
            Console.WriteLine("After calling AcceptChanges: {0}", row.RowState);
            // RowState = Modified.
            row["TempColumn"] = 11;
            Console.WriteLine("After first assignment: {0}", row.RowState);

            // RowState = Deleted.
            temp.Rows[0].Delete();
            Console.WriteLine("After calling Delete: {0}", row.RowState);
        }

    }
}
