using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace File_Manager
{
    public class ListViewColumnSorter : IComparer
    {
        private int ColumnToSort;
        private SortOrder OrderOfSort;
        private CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorter()
        {
            ColumnToSort = 0;
            OrderOfSort = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Compare the two items based on the column being sorted
            switch (ColumnToSort)
            {
                case 0: // Column 0 (Name)
                    compareResult = ObjectCompare.Compare(listviewX.Text, listviewY.Text);
                    break;
                case 1: // Column 1 (Size)
                    long xSize = long.TryParse(listviewX.SubItems[1].Text, out xSize) ? xSize : 0;
                    long ySize = long.TryParse(listviewY.SubItems[1].Text, out ySize) ? ySize : 0;
                    compareResult = xSize.CompareTo(ySize);
                    break;
                case 2: // Column 2 (Type)
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[2].Text, listviewY.SubItems[2].Text);
                    break;
                case 3: // Column 3 (Date Modified)
                    DateTime xDate = DateTime.TryParse(listviewX.SubItems[3].Text, out xDate) ? xDate : DateTime.MinValue;
                    DateTime yDate = DateTime.TryParse(listviewY.SubItems[3].Text, out yDate) ? yDate : DateTime.MinValue;
                    compareResult = xDate.CompareTo(yDate);
                    break;
                default:
                    compareResult = 0;
                    break;
            }

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Descending)
            {
                compareResult = -compareResult; // Reverse sort order for descending
            }

            return compareResult;
        }

        public int SortColumn
        {
            set { ColumnToSort = value; }
            get { return ColumnToSort; }
        }

        public SortOrder Order
        {
            set { OrderOfSort = value; }
            get { return OrderOfSort; }
        }
    }

}
